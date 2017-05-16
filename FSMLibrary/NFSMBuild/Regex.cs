using System.Collections.Generic;

namespace FSMLibrary.NFSMBuild
{
    public class Regex
    {
        public Node Root { get; set; }

        
        
        
        public Regex(string regex)
        {
            BuildRegexTree(regex);
        }

        private void BuildRegexTree(string regex)
        {
            var stack=new Stack<char>();
            var nodes=new Stack<Node>();
            Node currentRoot=null;
          
            foreach (var symbol in regex)
            {
                switch (symbol)
                {
                    case '(':
                        stack.Push(symbol);
                        break;
                    case ')':
                        while (stack.Peek()!='(')
                        {
                            
                            var temp = stack.Pop();
                            currentRoot = new Node(new Symbol(temp));
                            if (temp == '*')
                            {
                                currentRoot.LeftChild = nodes.Pop();
                            }
                            else
                            {
                                
                                currentRoot.RightChild = nodes.Pop();
                                currentRoot.LeftChild = nodes.Pop();
                            }
                            nodes.Push(currentRoot);
                        }
                        stack.Pop();
                        break;
                    case '|': 
                    case '+':
                        if (stack.Count != 0)
                        {
                            if (stack.Peek() == '*')
                            {
                                currentRoot = new Node(new Symbol(stack.Pop()));
                                currentRoot.LeftChild = nodes.Pop();
                                nodes.Push(currentRoot);
                            }
                        }
                        stack.Push(symbol);
                        break;
                    case '*':
                        stack.Push(symbol);
                        break;
                    default:
                        nodes.Push(new Node(new Symbol(symbol)));
                        break;


                }
            }
          
            
            while (stack.Count>0)
            {
               
                var temp = stack.Pop();
                if (temp != ')')
                {
                    currentRoot = new Node(new Symbol(temp));
                    if (temp == '*')
                    {
                        currentRoot.LeftChild = nodes.Pop();
                    }
                    else
                    {

                        currentRoot.RightChild = nodes.Pop();
                        currentRoot.LeftChild = nodes.Pop();
                    }
                    nodes.Push(currentRoot);
                }
            }
            Root = nodes.Pop();
           

        }
    }
}
