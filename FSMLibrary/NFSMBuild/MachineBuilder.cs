using System.Collections.Generic;

namespace FSMLibrary.NFSMBuild
{

    public class MachineBuilder
    {
        public FiniteStateMachine GetFromConcatenation(FiniteStateMachine firstMachine, FiniteStateMachine secondMachine)
        {
            var result =new FiniteStateMachine();
            result.StartState = firstMachine.StartState;
            result.FinalStates.AddRange(secondMachine.FinalStates);
            result.States = CombineCollections(firstMachine.States, secondMachine.States);
            result.Transitions = CombineCollections(firstMachine.Transitions, secondMachine.Transitions);
            AddNewTransitions(firstMachine.FinalStates,result.Transitions,secondMachine.StartState,new Symbol('#'));
            return result;
        }
        public FiniteStateMachine GetFromUnion(FiniteStateMachine firstMachine, FiniteStateMachine secondMachine)
        {
            var result = new FiniteStateMachine();
            result.StartState = new State(Id.GetId().ToString());
            result.FinalStates = CombineCollections(secondMachine.FinalStates, firstMachine.FinalStates);
            result.States = CombineCollections(firstMachine.States, secondMachine.States);
            result.States.Add(result.StartState);
            result.Transitions = CombineCollections(firstMachine.Transitions, secondMachine.Transitions);
            AddNewTransition(result.StartState,firstMachine.StartState,result.Transitions,new Symbol('#'));
            AddNewTransition(result.StartState, secondMachine.StartState, result.Transitions, new Symbol('#'));
            return result;
        }
        public FiniteStateMachine GetFromKleeneClosure(FiniteStateMachine machine)
        {
            var result = new FiniteStateMachine();
            result.StartState = new State(Id.GetId().ToString());
            result.FinalStates=CombineCollections(machine.FinalStates, result.StartState);
            result.States=CombineCollections(machine.States, result.StartState);
            AddNewTransitions(machine.Transitions,result.Transitions);
            AddNewTransition(result.StartState, machine.StartState, result.Transitions, new Symbol('#'));
            AddNewTransitions(machine.FinalStates, result.Transitions, machine.StartState, new Symbol('#'));
            return result;
        }

        public FiniteStateMachine GetMachineFromLetter(Symbol symbol)
        {
            var result = new FiniteStateMachine();
            var startState=new State(Id.GetId().ToString());
            var finalState=new State(Id.GetId().ToString());
            result.StartState = startState;
            result.FinalStates.Add(finalState);
            result.States.Add(startState);
            result.States.Add(finalState);
            AddNewTransition(startState,finalState,result.Transitions,symbol);
            return result;
        }

       
       
        
        public FiniteStateMachine Build(Regex regex)
        {
            return Build(regex.Root);
        }

        public FiniteStateMachine Build(Node node)
        {
            if (node.Data.Type == SymbolType.KleeneStar)
            {
                return GetFromKleeneClosure(Build(node.LeftChild));
            }
            if (node.Data.Type == SymbolType.Union)
            {
                return GetFromUnion(Build(node.LeftChild),Build(node.RightChild));
            }
            if (node.Data.Type == SymbolType.Concatenation)
            {
                return GetFromConcatenation(Build(node.LeftChild), Build(node.RightChild));
            }

            return GetMachineFromLetter(node.Data);
        }





        List<T> CombineCollections<T>(List<T> firstCollection,List<T> secondCollection )
        {
            var result=new List<T>(firstCollection);
            result.AddRange(secondCollection);
            return result;
        }
        List<T> CombineCollections<T>(List<T> firstCollection, T item)
        {
            var result = new List<T>(firstCollection);
            result.Add(item);
            return result;
        }
        void AddNewTransitions(List<State> sourceCollection,List<Transition> outputCollection,State state,Symbol symbol)
        {
            foreach (var finalState in sourceCollection)
            {
                AddNewTransition(finalState,state,outputCollection,symbol);
            }
        }
        void AddNewTransitions(List<Transition> sourceCollection, List<Transition> outputCollection)
        {
            outputCollection.AddRange(sourceCollection);
        }
        void AddNewTransition(State sourceState,State state, List<Transition> outputCollection, Symbol symbol)
        {
            outputCollection.Add(new Transition(sourceState, state, symbol));
        }  
    }
}
