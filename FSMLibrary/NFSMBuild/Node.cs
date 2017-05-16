namespace FSMLibrary.NFSMBuild
{
    public class Node
    {
        public Symbol Data { get; set; }
        public Node LeftChild { get; set; }
        public Node RightChild { get; set; }
     
        
        
        public Node(Symbol data)
        {
            Data = data;
        }
        
        
        
        public override string ToString()
        {
            return Data.ToString();
        }
    }
}
