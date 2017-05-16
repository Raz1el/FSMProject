namespace FSMLibrary.NFSMBuild
{
    public class State
    {

        public static State Error = new State("ERROR");

       
        
        public string Data { get; set; }

      
        
        public State(string data)
        {
            Data = data;
        }

        
        public static bool operator ==(State left, State right)
        {
            if ((object)left == null && (object)right == null)
                return true;
            if ((object)left == null || (object)right == null)
                return false;
            return left.Data == right.Data;
        }

        public static bool operator !=(State left, State right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as State);
        }

        protected bool Equals(State other)
        {
            return string.Equals(Data, other.Data);
        }

        public override int GetHashCode()
        {
            return (Data != null ? Data.GetHashCode() : 0);
        }

        public static implicit operator State(string value)
        {
            return new State(value);
        }

        public override string ToString()
        {
            return Data;
        }
    }
}
