using System;
using System.Collections.Generic;
using System.Linq;
using FSMLibrary.DFSMBuild;
using FSMLibrary.NFSMBuild;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex rgx = new Regex("(a|b)*");
            var builder = new MachineBuilder();
            var fsm=builder.Build(rgx);


            var newFsm = GetDmk1kFSM();
            var detBuilder=new DetermMachineBuilder();
            var det = detBuilder.Build(newFsm);
            var det2 = detBuilder.Build(fsm);
            var another = GetAnotherFSM();
            var det1 = detBuilder.Build(another);
            Console.WriteLine(det.GetAsRegularGrammar());
            Console.WriteLine("abba is"+det2.CheckWord("abba"));
            Console.WriteLine("abbab is"+det2.CheckWord("abbab"));
            Console.ReadKey();
        }

        static FiniteStateMachine GetDmk1kFSM()
        {
            var states = new State[] { "q1", "q2", "q3", "q4", "q5" };
            var tr1 = new Transition("q1", "q2", '\0');
            var tr2 = new Transition("q1", "q3", 'b');
            var tr3 = new Transition("q2", "q3", '\0');
            var tr4 = new Transition("q2", "q1", 'a');
            var tr5 = new Transition("q2", "q5", 'a');
            var tr6 = new Transition("q2", "q4", '\0');
            var tr7 = new Transition("q3", "q5", 'b');
            var tr8 = new Transition("q4", "q5", 'a');
            var tr9 = new Transition("q5", "q4", '\0');

            var transitions = new Transition[] { tr1, tr2, tr3, tr4, tr5, tr6, tr7, tr8, tr9 };
            var finalState = new State[] { "q5" };
            var start = "q1";

            return new FiniteStateMachine(states, transitions, finalState, start);
        }

        static FiniteStateMachine GetAnotherFSM()
        {
            var states = new State[] {"q0", "q1" , "q2"  };
            var tr1=new Transition("q0","q2",'b');
            var tr2 = new Transition("q0", "q1", 'a');
            var tr3 = new Transition("q0", "q1", 'b');

            var tr4 = new Transition("q1", "q1", 'a');
            var tr5 = new Transition("q1", "q1", 'a');

            var tr6 = new Transition("q2", "q1", 'a');
            var tr7 = new Transition("q2", "q1", 'b');
            var transitions = new Transition[] {tr1, tr2, tr3, tr4, tr5,tr6,tr7};
            var finalState = new State[] { "q2" ,"q1"};
            var start = "q0";
            return new FiniteStateMachine(states,transitions,finalState,start);
        }
        
    }
}
