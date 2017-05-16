using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;

using System.Windows.Controls;
using FSMLibrary.NFSMBuild;



namespace FSMProject.FSMGraph
{
    class GraphBuilder
    {
        private Canvas _canvas;

        public int VertexWidth { get; set; }
        public int VertexHeight { get; set; }

        public GraphBuilder(Canvas canvas,int vertexWidth,int vertexHeigt)
        {
            _canvas = canvas;
            VertexHeight = vertexHeigt;
            VertexWidth = vertexWidth;

        }

      
      

       

      

      

      
       

        public Graph BuildGraph(FiniteStateMachine machine, int centerX,int centerY,int radius)
        {
            var tmp = new Dictionary<State,Vertex>();
            var result=new Graph();
         
            
            for (var i = 0; i < machine.States.Count; i++)
            {
                var state = machine.States[i];
                var vertex = InitVertex(state, machine);
                _canvas.Children.Add(vertex);
                tmp.Add(state, vertex);
                result.AddVertex(vertex);


                DrawVertexOnCircle(vertex, i, machine.States.Count, centerX, centerY, radius);
            }
           
            
            foreach (var transition in machine.Transitions)
            {
                var edgeMark = new StringBuilder();
                machine.Transitions.Where(
                        (t => t.CurrentState == transition.CurrentState && t.NextState == transition.NextState))
                        .Select(t => t.Symbol.Value)
                        .Aggregate(edgeMark, (res, ch) => edgeMark.Append(ch+","));
                var edge = new Edge(tmp[transition.CurrentState], tmp[transition.NextState],
                    edgeMark.Remove(edgeMark.Length-1,1).ToString());
                result.AddEdge(edge);
 
                _canvas.Children.Insert(0,edge.EdgeTag);
                _canvas.Children.Insert(0, edge.Arrow);
                if (edge.SelfLoopPath!=null)
                {
                    _canvas.Children.Insert(0, edge.SelfLoopPath);
                }

            }

            return result;

        }





       

        private void DrawVertexOnCircle(Vertex vertex, int vertexIndex, int verticesCount, int centerX, int centerY, int radius)
        {
            var x = radius * Math.Cos(2 * Math.PI * vertexIndex / verticesCount ) + centerX;
            var y = radius * Math.Sin(2 * Math.PI * vertexIndex / verticesCount ) + centerY;
            Canvas.SetTop(vertex, y);
            Canvas.SetLeft(vertex, x);
        }
       

        public Vertex InitVertex(State state,FiniteStateMachine machine)
        {
            var vertex=new Vertex();
            if (state == machine.StartState)
            {
                if (machine.FinalStates.Contains(state))
                {
                    vertex.Init(state.Data, StateTypes.StartAndFinal);
                }
                else
                {
                    vertex.Init(state.Data, StateTypes.Start);
                }
            }
            else if (machine.FinalStates.Contains(state))
            {
                vertex.Init(state.ToString(), StateTypes.Final);
            }
            else
            {
                vertex.Init(state.Data, StateTypes.Normal);
            }
            vertex.Width = VertexWidth;
            vertex.Height = VertexHeight;
            return vertex;
        }

 




    }
}
