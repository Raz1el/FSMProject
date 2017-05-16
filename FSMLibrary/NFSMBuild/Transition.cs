namespace FSMLibrary.NFSMBuild
{
    public class Transition
    {
        public State CurrentState { get; set; }
        public State NextState { get; set; }
        public Symbol Symbol { get; set; }

     
        
        
        public Transition(State current, State next, Symbol symbol)
        {
            CurrentState = current;
            NextState = next;
            Symbol = symbol;
        }
        public static bool operator ==(Transition left, Transition right)
        {
            if ((object) left == null && (object) right == null)
                return true;
            if ((object) left == null || (object) right == null)
                return false;
            return (left.CurrentState == right.CurrentState && left.NextState == right.NextState&&left.Symbol==right.Symbol);
        }

        public static bool operator !=(Transition left, Transition right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals((Transition)obj);
        }

        protected bool Equals(Transition other)
        {
            return this == other;
        }
        public override string ToString()
        {
            return CurrentState.ToString()+Symbol.ToString()+NextState.ToString();
        }
    }
}
