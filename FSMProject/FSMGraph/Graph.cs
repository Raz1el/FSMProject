using System.Collections.Generic;

namespace FSMProject.FSMGraph
{
    public class Graph
    {
        public List<Vertex> Vertices { get; set; }
        public List<Edge> Edges { get; set; }


        public Graph()
        {
            Vertices=new List<Vertex>();
            Edges=new List<Edge>();
        }
        public void AddVertex(Vertex v)
        { Vertices.Add(v); }
        public void AddEdge(Edge v)
        { Edges.Add(v); }
    }
}
