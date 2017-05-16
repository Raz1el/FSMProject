using System;
using System.Collections.Generic;
using System.Linq;
using FSMLibrary.NFSMBuild;

namespace FSMLibrary.DFSMBuild
{
    public class DetermMachineBuilder
    {
        private FiniteStateMachine _nfsm;
        private HashSet<Symbol> _alphabet;
        private Dictionary<State, int> _dict;
        private Dictionary<int, State> _reverseDict;   

        class MultyState
        {
            private State[] _states;
            public int Count
            {
                get { return _states.Length; }
            }

            public MultyState(IEnumerable<State> states)
            {
                this._states = states.ToArray();
            }

            public MultyState(State state)
            {
                this._states = Parse(state.ToString());
            }

            public static State[] Parse(string str)
            {
                var temp = str.Split(new[] {' ', '{', '}', ','}, StringSplitOptions.RemoveEmptyEntries);
                var states = new State[temp.Length];
                for (int i = 0; i < temp.Length; i++)
                {
                    states[i] = temp[i];
                }
                return states;
            }

            public State ToState()
            {
                return this.ToString();
            }

            public override string ToString()
            {
                if (_states.Length == 0)
                {
                    return "";
                }
                if (_states.Length == 1)
                {
                    return _states[0].ToString();
                }
                string res = "{";
                foreach (var st in _states)
                {
                    res += st + ",";
                }
                res=res.Remove(res.Length - 1);
                res += "}";
                return res;
            }

            public void Sort(Dictionary<State, int> dict,Dictionary<int,State> reverseDict )
            {


                if (_states[0] != "#")
                {
                    var tempStates = new int[_states.Length];
                    for (int i = 0; i < _states.Length; i++)
                    {
                        tempStates[i] = dict[_states[i]];
                    }
                    var list = tempStates.ToList();
                    list.Sort();
                    tempStates = list.ToArray();

                    for (int i = 0; i < _states.Length; i++)
                    {
                        _states[i] = reverseDict[tempStates[i]];
                    }
                }

            }
            public State this[int ind]
            {
                get { return _states[ind]; }
            }
        }


        public FiniteStateMachine Build(FiniteStateMachine nfsm)
        {
            _nfsm = nfsm;

            _dict = new Dictionary<State, int>();
            _reverseDict = new Dictionary<int, State>();
            for (int i = 0; i < _nfsm.States.Count; i++)
            {
                _dict.Add(_nfsm.States[i], i);
                _reverseDict.Add(i, _nfsm.States[i]);
            }

            var newStartState = GetNewStartState();

            _alphabet = GenerateAlphabet();

            var states = new List<State>();
            states.Add(newStartState);

            var transitions = new List<Transition>();

            var queue = new Queue<State>();

           

            foreach (var symbol in _alphabet)
            {
                var temp1 = GenerateNewTransition(newStartState, symbol);
                transitions.Add(temp1);
                var temp = temp1.NextState;
                if (!states.Contains(temp))
                    queue.Enqueue(temp);
            }

            while (true)
            {
                if (queue.Count == 0)
                {
                    break;
                }
                var nowState = queue.Dequeue();

                var tempMult = new MultyState(nowState);
                tempMult.Sort(_dict, _reverseDict);
                nowState = tempMult.ToState();

                if (!states.Contains(nowState))
                    states.Add(nowState);

                foreach (var symbol in _alphabet)
                {
                    var temp1 = GenerateNewTransition(nowState, symbol);

                    if (temp1.NextState == "#")
                    {
                        if (transitions.Contains(temp1))
                        {
                            break;
                        }
                    }
                    transitions.Add(temp1);
                    var temp = temp1.NextState;


                    if (!states.Contains(temp) && !queue.Contains(temp))
                        queue.Enqueue(temp);
                }

            }


            var finalStates = GetFinalStates(states);

            states.Remove("#");
            transitions.RemoveAll(a => (a.CurrentState == "#" || a.NextState == "#"));
            finalStates.RemoveAll(a => (a == "#"));
            

            return new FiniteStateMachine(states,transitions,finalStates,newStartState);
        }


        private State GetNewStartState()
        {
            var multState = new MultyState(GetEpsillonTransitionStates(_nfsm.StartState));
            multState.Sort(_dict,_reverseDict);
            return multState.ToState();
        }

        private List<State> GetFinalStates(List<State> allStates)
        {
            var finalStates = new List<State>();
            
            foreach (var st in allStates)
            {
                var mult = new MultyState(st);
                for (int i = 0; i < mult.Count; i++)
                {
                    if (_nfsm.FinalStates.Contains(mult[i]))
                    {
                        finalStates.Add(st);
                        break;
                    }
                }
            }
            return finalStates;
        } 

        private HashSet<Symbol> GenerateAlphabet()
        {
            var alphabet=new HashSet<Symbol>();
            foreach (var tr in _nfsm.Transitions)
            {
                alphabet.Add(tr.Symbol);
            }
            alphabet.Remove('#');
            return alphabet;
        } 

        private Transition GenerateNewTransition(State state,Symbol symbol )
        {
            var transitions = _nfsm.Transitions;
            var multyState=new MultyState(state);
            var newMultyStateTemp=new HashSet<State>();
            for (int i = 0; i < multyState.Count; i++)
            {
                var temp = transitions.FindAll(a => (multyState[i] == a.CurrentState && symbol == a.Symbol));
                if (temp.Count != 0)
                {
                    foreach (var tr in temp)
                    {
                        var func = GetEpsillonTransitionStates(tr.NextState);
                        foreach (var st in func)
                        {
                            newMultyStateTemp.Add(st);
                        }
                    }
                }
            }
            var oldState = new State(multyState.ToString());
            var newMultyState=new MultyState(newMultyStateTemp);
          
            if(newMultyStateTemp.Count==0)
                return new Transition(oldState,new State("#"), symbol);
            newMultyState.Sort(_dict, _reverseDict);
            var newState=new State(newMultyState.ToString());
            
            return new Transition(oldState,newState, symbol);
        }

        private List<State> GetEpsillonTransitionStates(State state)
        {
            var states = new List<State>();
            var transitions = _nfsm.Transitions;

            var queue = new Queue<State>();
            queue.Enqueue(state);

            while (queue.Count != 0)
            {
                var nowState = queue.Dequeue();
                if(!states.Contains(nowState))
                states.Add(nowState);
                var temp = transitions.FindAll((a) => (a.CurrentState == nowState && a.Symbol.IsEpsilon()));
                foreach (var tr in temp)
                {
                    if (!states.Contains(tr.NextState))
                        queue.Enqueue(tr.NextState);
                }

            }

            return states;
        }


    }
}
