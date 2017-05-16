using System;
using System.Windows;

namespace FSMProject.FSMGraph
{
    public class VertexMoveEventArg:EventArgs
    {

        public Vector Offset { get; set; }

        public VertexMoveEventArg(Vector offset)
        {
            Offset = offset;
        }
    }
}
