using System;
using System.Collections.Generic;
using System.Linq;

namespace FSMLibrary.NFSMBuild
{
    public class FiniteStateMachine
    {
       

        public List<State> States { get; set; }
        public List<Transition> Transitions { get; set; } 
        public  List<State> FinalStates { get; set; }
        public  State StartState { get; set; }

        
        public FiniteStateMachine()
        {
            States=new List<State>();
            Transitions=new List<Transition>();
            FinalStates=new List<State>();
        }

        public FiniteStateMachine(IEnumerable<State> states, IEnumerable<Transition> transitions, 
            IEnumerable<State> finalStates, State startState)
        {
            States = states.ToList();
            Transitions = transitions.ToList();
            FinalStates = finalStates.ToList();
            StartState = startState;
            if(IsDetermial())
            ReduceStates();
        }
        public FiniteStateMachine(IEnumerable<State> states, IEnumerable<Transition> transitions,
            State finalState, State startState)
        {
            States = states.ToList();
            Transitions = transitions.ToList();
            FinalStates = new List<State>(new [] {finalState});
            StartState = startState;
            ReduceStates();
        }


        public bool IsDetermial()
        {
            foreach (var transition in Transitions)
            {
                if (transition.Symbol == '#')
                    return false;
            }

            foreach (var tr in Transitions)
            {
                var temp = from tran in Transitions
                    where (tran.CurrentState == tr.CurrentState && tran.Symbol == tr.Symbol)
                    select tran;
                if (temp.Count() != 1)
                    return false;
                if (temp.Count() == 1)
                {
                    if (temp.First().Symbol == '#')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public List<string> GetAsRegularGrammar()
        {
            var list = new List<String>();
            string temp = string.Empty;
            for (int i = 0; i < States.Count; i++)
            {
                var transitions = Transitions.FindAll(a => a.CurrentState == States[i]);
                if (transitions.Count!=0)
                {
                    temp += string.Format("{0} -> {1} {2}", transitions[0].CurrentState, transitions[0].Symbol,
                        transitions[0].NextState);
                    for (int j = 1; j < transitions.Count; j++)
                    {
                        temp += " | ";
                        temp += string.Format("{0} {1}", transitions[j].Symbol, transitions[j].NextState);
                    }
                    list.Add(temp);
                }
                temp = string.Empty;
            }
            return list;
        }

        public bool CheckWord(string word)
        {
            var currentState = StartState;
            for (int i = 0; i < word.Length; i++)
            {
                var currentLetter = word[i];
                var tr = Transitions.Find(a => a.Symbol == currentLetter && a.CurrentState==currentState);
                if (tr != null)
                {
                    currentState = tr.NextState;
                }
                else
                {
                    return false;
                }
            }
            return FinalStates.Contains(currentState);
        }

        public bool IsFinalState(State state)
        {
            return FinalStates.Contains(state);
        }
        

        public State GetNextState(State currentState,char a)
        {
            var tr = Transitions.Find(b => b.Symbol == a && b.CurrentState == currentState);
            if (tr != null)
            {
                return tr.NextState;
            }
            return null;
        }

        private void ReduceStates()
        {
            var newState = 'A';
            for (int i = 0; i < States.Count; i++)
            {
                if (StartState == States[i])
                {
                    StartState = newState.ToString();
                }

                if (FinalStates.Contains(States[i]))
                {
                    FinalStates[FinalStates.IndexOf(States[i])] = newState.ToString();
                }

                foreach (var tr in Transitions)
                {
                    if (tr.CurrentState == States[i])
                    {
                        tr.CurrentState = newState.ToString();
                    }

                    if (tr.NextState == States[i])
                    {
                        tr.NextState = newState.ToString();
                    }
                }

                States[i] = newState.ToString();
                newState++;
            }
        }
    }
}
