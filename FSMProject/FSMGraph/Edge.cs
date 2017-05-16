using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FSMProject.FSMGraph
{
    public class Edge
    { 
        #region const
        private const int MarkOffsetX = 10;
        private const int MarkOffsetY = 15;
        private const int MarkFontSize = 20;

        private const int ArrowWidth = 15;
        private const int ArrowHeight = 5;

        private const int ArcWidth = 25;
        private const int ArcwHeight = 20;

        private const int DefaultThickness = 2;

        private const int DefaultArcAngle = 90;
        private const int SelfLoopArcDx = 10;
        private const int SelfLoopArcDy = 2;

        private const double SelfLoopArrowDx1 = 12;
        private const double SelfLoopArrowDx2 = 11.3;
        private const double SelfLoopArrowDy1 = 1;
        private const double SelfLoopArrowDy2 = 2;
#endregion
       



        public Vertex Source { get; set; }
        public Vertex Target { get; set; }
        public Arrow Arrow { get; set; }
        public Path SelfLoopPath { get; set; }
        public Label EdgeTag { get; set; }
        private bool _isSelfLoop;


        public Edge(Vertex source,Vertex target,string tag)
        {
            Source = source;
            Target = target;
            EdgeTag = GetMark(tag);

            Connect(source,target);
            source.PositionChanged += Source_PositionChanged;
            target.PositionChanged += Target_PositionChanged;
            Arrow.MouseEnter += Arrow_MouseEnter;
            Arrow.MouseLeave += Arrow_MouseLeave;
            EdgeTag.MouseEnter += EdgeTag_MouseEnter;
            EdgeTag.MouseLeave += EdgeTag_MouseLeave;
            if (SelfLoopPath != null)
            {
                SelfLoopPath.MouseEnter += SelfLoopPath_MouseEnter;
                SelfLoopPath.MouseLeave += SelfLoopPath_MouseLeave;
            }
          

            _isSelfLoop = source.State == target.State;




        }

        void SelfLoopPath_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SelectionOff();
        }

        void SelfLoopPath_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SelectionOn();
        }

        void EdgeTag_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SelectionOff();
        }

        void EdgeTag_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SelectionOn();
        }

        void Arrow_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SelectionOff();
        }

        void Arrow_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            SelectionOn();
        }

        void SelectionOn()
        {
            EdgeTag.Foreground=Brushes.Red;
            Arrow.Stroke=Brushes.Red;
            Arrow.StrokeThickness = 2*DefaultThickness;
            Canvas.SetZIndex(Arrow, 1);
            if (SelfLoopPath != null)
            {
                SelfLoopPath.Stroke=Brushes.Red;
                SelfLoopPath.StrokeThickness = 2 * DefaultThickness;
                
            }
        }

        void SelectionOff()
        {
            EdgeTag.Foreground = Brushes.Black;
            Arrow.Stroke = Brushes.Black;
            Arrow.StrokeThickness =  DefaultThickness;
            Canvas.SetZIndex(Arrow, 0);
            if (SelfLoopPath != null)
            {
                SelfLoopPath.Stroke = Brushes.Black;
                SelfLoopPath.StrokeThickness =  DefaultThickness;

            }
        }

        public void Block()
        {
            Arrow.IsHitTestVisible = false;
            EdgeTag.IsHitTestVisible = false;
        }

        public void UnBlock()
        {
            Arrow.IsHitTestVisible = true;
            EdgeTag.IsHitTestVisible = true;
        }
        public void ChangeColor(Color color)
        {
            EdgeTag.Foreground = new SolidColorBrush(color);
            Arrow.Stroke = new SolidColorBrush(color);
            Arrow.StrokeThickness = 2 * DefaultThickness;
            Canvas.SetZIndex(Arrow, 1);
            if (SelfLoopPath != null)
            {
                SelfLoopPath.Stroke = new SolidColorBrush(color);
                SelfLoopPath.StrokeThickness = 2 * DefaultThickness;

            }
        }

        public void ReturnColorToDefault()
        {
            EdgeTag.Foreground = Brushes.Black;
            Arrow.Stroke = Brushes.Black;
            Arrow.StrokeThickness = DefaultThickness;
            Canvas.SetZIndex(Arrow, 0);
            if (SelfLoopPath != null)
            {
                SelfLoopPath.Stroke = Brushes.Black;
                SelfLoopPath.StrokeThickness = DefaultThickness;

            }
        }

        private void Target_PositionChanged(object sender, VertexMoveEventArg e)
        {
            if (!_isSelfLoop)
            {
                var p1 = GetPosition(Source);
                var p2 = GetPosition(Target);



                var offset = new Vector(p2.X - p1.X, p2.Y - p1.Y);
                offset.Normalize();
                offset = offset * Target.Width / 2;

                Arrow.X1 = p1.X + offset.X;
                Arrow.Y1 = p1.Y + offset.Y;
                Arrow.X2 = p2.X - offset.X;
                Arrow.Y2 = p2.Y - offset.Y;

                if (p1.X < p2.X)
                {
                    Canvas.SetTop(EdgeTag, (p1.Y + p2.Y) / 2);
                }
                else
                {
                    Canvas.SetTop(EdgeTag, (p1.Y + p2.Y) / 2-2*MarkOffsetY);
                }
                Canvas.SetLeft(EdgeTag, (p1.X + p2.X) / 2 - MarkOffsetY);
               
            }
          
            

        }

        private void Source_PositionChanged(object sender, VertexMoveEventArg e)
        {
            if (!_isSelfLoop)
            {
                var p1 = GetPosition(Source);
                var p2 = GetPosition(Target);



                var offset = new Vector(p2.X - p1.X, p2.Y - p1.Y);
                offset.Normalize();
                offset = offset * Source.Width / 2;

                Arrow.X1 = p1.X + offset.X;
                Arrow.Y1 = p1.Y + offset.Y;
                Arrow.X2 = p2.X - offset.X;
                Arrow.Y2 = p2.Y - offset.Y;


                if (p1.X < p2.X)
                {
                    Canvas.SetTop(EdgeTag, (p1.Y + p2.Y) / 2);
                }
                else
                {
                    Canvas.SetTop(EdgeTag, (p1.Y + p2.Y) / 2 - 2*MarkOffsetY);
                }
                Canvas.SetLeft(EdgeTag, (p1.X + p2.X) / 2 - MarkOffsetY);

            }
            else
            {
                Arrow.X1 += e.Offset.X;
                Arrow.Y1 += e.Offset.Y;
                Arrow.X2 += e.Offset.X;
                Arrow.Y2 += e.Offset.Y;

                var position = VisualTreeHelper.GetOffset(SelfLoopPath);
                Canvas.SetLeft(SelfLoopPath,position.X+e.Offset.X);
                Canvas.SetTop(SelfLoopPath, position.Y + e.Offset.Y);

                var markPosition = VisualTreeHelper.GetOffset(EdgeTag);
                Canvas.SetLeft(EdgeTag, markPosition.X + e.Offset.X);
                Canvas.SetTop(EdgeTag, markPosition.Y + e.Offset.Y);
            }
          
        }

        void Connect(Vertex source, Vertex target)
        {



            var p1 = GetPosition(source);
            var p2 = GetPosition(target);



            var offset = new Vector(p2.X - p1.X, p2.Y - p1.Y);
            offset.Normalize();
            offset = offset * target.Width / 2;





            if (source == target)
            {
                Canvas.SetLeft(EdgeTag,p1.X-MarkOffsetX);
                Canvas.SetTop(EdgeTag, p1.Y + source.Height + MarkOffsetY);
                SelfLoopPath = GetArcPath(p1.X, p2.Y + source.Height / 2, p1.X, p2.Y + source.Height / 2, ArcWidth, ArcwHeight);
                Arrow = GetArrow(p1.X + SelfLoopArrowDx1, p2.Y + source.Height / 2 - SelfLoopArrowDy1, p1.X + SelfLoopArrowDx2, p2.Y + source.Height / 2 - SelfLoopArrowDy2);
              

        
            }
            else
            {
                Arrow = GetArrow(p1.X + offset.X, p1.Y + offset.Y, p2.X - offset.X, p2.Y - offset.Y);

                if (p1.X < p2.X)
                {
                   
                    Canvas.SetTop(EdgeTag, (p1.Y + p2.Y)/2);
                }
                else
                {
                    Canvas.SetTop(EdgeTag, (p1.Y + p2.Y) / 2-2*MarkOffsetY);
                }
                Canvas.SetLeft(EdgeTag, (p1.X + p2.X) / 2-MarkOffsetY);
            }


        }




        Point GetPosition(Vertex v)
        {
            return new Point(Canvas.GetLeft(v) + v.Width / 2, Canvas.GetTop(v) + v.Height / 2);
        }
        Path GetArcPath(double startPosX, double startPosY, double endPosX, double endPosY, int width, int height)
        {
            var pthFigure = new PathFigure();
            var arcSeg = new ArcSegment();

            
            if (startPosX == endPosX && startPosY == startPosY)
            {
                pthFigure.StartPoint = new Point(startPosX - SelfLoopArcDx, startPosY-SelfLoopArcDy );
                arcSeg.Point = new Point(endPosX + SelfLoopArcDx, endPosY - SelfLoopArcDy);

            }
            else
            {
                pthFigure.StartPoint = new Point(startPosX, startPosY);
                arcSeg.Point = new Point(endPosX, endPosY);

            }

            arcSeg.Size = new Size(width, height);
            arcSeg.IsLargeArc = true;
            arcSeg.RotationAngle = DefaultArcAngle;
            var myPathSegmentCollection = new PathSegmentCollection {arcSeg};
            pthFigure.Segments = myPathSegmentCollection;
            var pthFigureCollection = new PathFigureCollection {pthFigure};
            var pthGeometry = new PathGeometry {Figures = pthFigureCollection};
            var arcPath = new Path{ Stroke = new SolidColorBrush(Colors.Black),StrokeThickness = DefaultThickness,Data = pthGeometry };


            return arcPath;
        }

        Arrow GetArrow(double startPosX, double startPosY, double endPosX, double endPosY)
        {
            Arrow arrow = new Arrow();
            arrow.StrokeThickness = DefaultThickness;
            arrow.Stroke = Brushes.Black;
            arrow.HeadHeight = ArrowHeight;
            arrow.HeadWidth = ArrowWidth;
          
            arrow.X1 = startPosX;
            arrow.Y1 = startPosY;
            arrow.X2 = endPosX;
            arrow.Y2 = endPosY;
            return arrow;
        }

        private Label GetMark(string value)
        {
            var mark = new Label();
            mark.Foreground = Brushes.Black;
            mark.FontSize = MarkFontSize;
            mark.Content = value;
            return mark;


        }
    }
}
