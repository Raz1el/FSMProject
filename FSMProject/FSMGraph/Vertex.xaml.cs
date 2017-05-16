using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FSMLibrary.NFSMBuild;

namespace FSMProject.FSMGraph
{
    /// <summary>
    /// Interaction logic for Vertex.xaml
    /// </summary>
    public enum StateTypes{Start,Final,Normal,StartAndFinal}

    public partial class Vertex : UserControl
    {
        public State State { get; set; }
        public StateTypes Type { get; set; }
        public event EventHandler<VertexMoveEventArg> PositionChanged;

        private bool _isDragging;
        private Point _previousMousePosition;

        public void Init(State state, StateTypes type)
        {
            State = state;
            Type = type;
            switch (type)
            {
                case StateTypes.Start:
                    StartStateElement.Visibility = Visibility.Visible;
                    BackgroundEllipse.Fill = new SolidColorBrush(Colors.DeepSkyBlue);
                    break;
                case StateTypes.Final:
                    FinalStateElement.Visibility = Visibility.Visible;
                    BackgroundEllipse.Fill = new SolidColorBrush(Colors.SlateBlue);
                    break;
                case StateTypes.Normal:
                    BackgroundEllipse.Fill = new SolidColorBrush(Colors.SteelBlue);
                    break;
                case StateTypes.StartAndFinal:
                    BackgroundEllipse.Fill = new SolidColorBrush(Colors.DeepSkyBlue);
                    StartStateElement.Visibility = Visibility.Visible;
                    FinalStateElement.Visibility = Visibility.Visible;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            DataLabel.Content = State.ToString();
        }

        public Vertex()
        {
            InitializeComponent();
            MouseLeftButtonDown += new MouseButtonEventHandler(Control_MouseLeftButtonDown);
            MouseLeftButtonUp += new MouseButtonEventHandler(Control_MouseLeftButtonUp);
            MouseMove += new MouseEventHandler(Control_MouseMove);
            LostMouseCapture += (sender, e) => { _isDragging = false; };

        }

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            var draggableControl = sender as UserControl;
            _previousMousePosition = e.GetPosition(Parent as UIElement);
            draggableControl.CaptureMouse();
        }

        private void Control_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            _isDragging = false;
            var draggable = sender as UserControl;
            draggable.ReleaseMouseCapture();
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            var draggableControl = sender as UserControl;

            if (_isDragging && draggableControl != null)
            {
                Point currentMousePosition = e.GetPosition(this.Parent as UIElement);
                var currentVertexPosition = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
                var offset = currentMousePosition - _previousMousePosition;
                Canvas.SetLeft(this, currentVertexPosition.X + offset.X);
                Canvas.SetTop(this, currentVertexPosition.Y + offset.Y);
                _previousMousePosition = currentMousePosition;
                PositionChanged(this, new VertexMoveEventArg(offset));
            }
        }

        public void ChangeColor(Color color)
        {
            BackgroundEllipse.Fill = new SolidColorBrush(color);
        }
       

        public void ReturnDefaultColor()
        {
            switch (Type)
            {
                case StateTypes.Start:
                    BackgroundEllipse.Fill = new SolidColorBrush(Colors.DeepSkyBlue);
                    break;
                case StateTypes.Final:
                    BackgroundEllipse.Fill = new SolidColorBrush(Colors.SlateBlue);
                    break;
                case StateTypes.Normal:
                    BackgroundEllipse.Fill = new SolidColorBrush(Colors.SteelBlue);
                    break;
                case StateTypes.StartAndFinal:
                    BackgroundEllipse.Fill = new SolidColorBrush(Colors.DeepSkyBlue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }
    }
}
