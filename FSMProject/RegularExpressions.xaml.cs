using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FSMLibrary.DFSMBuild;
using FSMLibrary.NFSMBuild;
using FSMProject.FSMGraph;

namespace FSMProject
{
    /// <summary>
    /// Interaction logic for RegularExpressions.xaml
    /// </summary>
    public partial class RegularExpressions : Window
    {
        public FiniteStateMachine DetermFSM;
        public Graph Graph { get; set; }
        public RegularExpressions()
        {
            InitializeComponent();

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

        private void InitiateGrammarTextBox()
        {
            var grammar = DetermFSM.GetAsRegularGrammar();
            for (int j = 0; j < grammar.Count; j++)
            {
                var gr = grammar[j];            
                var split = gr.Split(new string[] {"->", "|"}, StringSplitOptions.RemoveEmptyEntries);

                var list=new List<string>();
                list.Add(split[0]);
                list.Add("->");
                for (int i = 1; i < split.Length; i++)
                {
                    list.Add(split[i]);
                    if (split.Length - 1 != i)
                    {
                        list.Add("|");
                    }
                }

                var paragraph = new Paragraph();
                for (int i = 0; i < list.Count; i++)
                {
                    var run=new Run(list[i]);
                    run.FontSize = 15;
                    if (gr == grammar[0])
                    {
                       
                        paragraph.Inlines.Add(new Underline(new Bold(run)));
                    }
                    else
                    paragraph.Inlines.Add(new Bold(run));
                }

                GrammarRichTextBox.Document.Blocks.Add(paragraph);
            }
            

        }

        private void CheckWordButton_OnClick(object sender, RoutedEventArgs e)
        {


                if (CheckWordTempTextBox.Text == string.Empty)
                {
                    if (DetermFSM.IsFinalState(DetermFSM.StartState))
                    {
                        MessageBox.Show("Данная строка входит в язык, порождаеммым регулярным выражением.");
                    }
                    else
                    {
                        MessageBox.Show("Данная строка НЕ входит в язык, порождаеммым регулярным выражением.");
                    }
                }
                else
                {

                    var content = CheckWordTempTextBox.Text.ToLower();
                    CheckWordTempTextBox.Visibility = Visibility.Hidden;
                    CheckWordSymbolRun.Text = content[0].ToString();
                    CheckWordPartRun.Text = content.Substring(1);
                    CheckWordButton.Visibility = Visibility.Hidden;
                    CheckWordStateLabel.Content = DetermFSM.StartState;

                    var vertex = Graph.Vertices.Find(a => a.State == DetermFSM.StartState);
                    vertex.ChangeColor(Color.FromRgb(200, 0, 100));

                    RegexTextBox.IsEnabled = false;
                    RegexButton.IsEnabled = false;
                    CheckImage.Source = null;
                }

                
            
        }

        private void CheckWordTempTextBox_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (CheckWordTempTextBox.Visibility == Visibility.Visible)
            {
                CheckWordTextBox.Visibility = Visibility.Hidden;
                CheckWordTempTextBox.Text = String.Empty;

                if(!object.Equals(DetermFSM, null))
                if (DetermFSM.IsFinalState(DetermFSM.StartState)  )
                {
                    CheckImage.Source = new BitmapImage(new Uri("Images/true.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    CheckImage.Source = new BitmapImage(new Uri("Images/wrong.png", UriKind.RelativeOrAbsolute));
                }
            }
            else
            {
                CheckWordTextBox.Visibility = Visibility.Visible;
                CheckImage.Source = null;
               
                
            }
        }

        private void CheckWordButton_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (CheckWordButton.Visibility == Visibility.Hidden)
            {
                CheckWordIterationButton.Visibility = Visibility.Visible;
            }
            else
            {
                CheckWordIterationButton.Visibility = Visibility.Hidden;
            }
        }

        private void CheckWordIterationButton_OnClick(object sender, RoutedEventArgs e)
        {       
            var state=new State(CheckWordStateLabel.Content.ToString());

           

            var symbol = CheckWordSymbolRun.Text[0];
            var content = CheckWordPartRun.Text;

            if (state != State.Error)
            {

                var edges = from a in Graph.Edges where a.Target.State == state select a;
                foreach (var edge in edges)
                {
                    edge.ReturnColorToDefault();
                    edge.UnBlock();
                }

                var vertex = Graph.Vertices.Find(a => a.State == state);

                vertex.ReturnDefaultColor();

                var newState = DetermFSM.GetNextState(state, symbol);

                if (newState == null)
                {
                    CheckWordStateLabel.Content = State.Error;
                }
                else
                {
                    CheckWordStateLabel.Content = newState;

                    var newVertex = Graph.Vertices.Find(a => a.State == newState);
                    newVertex.ChangeColor(Color.FromRgb(200, 0, 100));

                    var newEdge = Graph.Edges.Find(a => a.Source.State == state && a.Target.State == newState);
                    newEdge.ChangeColor(Color.FromRgb(200, 0, 100));
                    newEdge.Block();

                }

            }

            if (content.Length != 0)
            {
                var newSymbol = content[0];
                var newContent = content.Substring(1);
                CheckWordPartRun.Text = newContent;
                CheckWordSymbolRun.Text = newSymbol.ToString();
            }
            else
            {

                var nowState= new State(CheckWordStateLabel.Content.ToString());


                var newVertex = Graph.Vertices.Find(a => a.State == nowState);

                var edges = from a in Graph.Edges where a.Target.State == nowState select a;
              

                if (DetermFSM.IsFinalState(nowState))
                {
                    MessageBox.Show("Данная строка входит в язык, порождаеммым регулярным выражением.");
                }
                else
                {
                    MessageBox.Show("Данная строка НЕ входит в язык, порождаеммым регулярным выражением.");
                }
                if (newVertex != null)
                {
                    newVertex.ReturnDefaultColor();
                }
                foreach (var edge in edges)
                {
                    edge.ReturnColorToDefault();
                    edge.UnBlock();
                }
                CheckWordStateLabel.Content = string.Empty;
                CheckWordButton.Visibility = Visibility.Visible;
                CheckWordTempTextBox.Visibility = Visibility.Visible;
                RegexTextBox.IsEnabled = true;
                RegexButton.IsEnabled = true;
            }
        }

        private void CheckWordTempTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (DetermFSM.CheckWord(CheckWordTempTextBox.Text) )
            {
                CheckImage.Source= new BitmapImage(new Uri("Images/true.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                CheckImage.Source = new BitmapImage(new Uri("Images/wrong.png", UriKind.RelativeOrAbsolute));
            }
            if (String.IsNullOrEmpty(CheckWordTempTextBox.Text))
            {
                CheckImage.Source = null;
            }
            if (CheckWordTempTextBox.Text == string.Empty)
            {
                if (DetermFSM.IsFinalState(DetermFSM.StartState))
                {
                    CheckImage.Source = new BitmapImage(new Uri("Images/true.png", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    CheckImage.Source = new BitmapImage(new Uri("Images/wrong.png", UriKind.RelativeOrAbsolute));
                }
            }
        }

        private void Rich_OnMouseEnter(object sender, MouseEventArgs e)
        {
            GrammarRichTextBox.Visibility=Visibility.Visible;

        }

        private void Rich_OnMouseLeave(object sender, MouseEventArgs e)
        {
            GrammarRichTextBox.Visibility = Visibility.Hidden;
        }

        private void RegexButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(RegexTextBox.Text))
                {
                    MessageBox.Show("Строка пустая!");
                }
                else
                {
                    GrammarRichTextBox.Document.Blocks.Clear();
                    MainPanel.Children.Clear();
                    Regex rgx = new Regex(RegexTextBox.Text);
                    var builder = new MachineBuilder();
                    var detBuilder = new DetermMachineBuilder();
                    var FSM = detBuilder.Build(builder.Build(rgx));
                    DetermFSM = FSM;

                    GraphBuilder graphBuilder = new GraphBuilder(MainPanel, 50, 50);
                    Graph = graphBuilder.BuildGraph(FSM, 300, 300, 200);
                    var startState = Graph.Vertices.First(v => v.State == FSM.StartState);
                    ScrollViewer.ScrollToHorizontalOffset(Canvas.GetTop(startState) - (Width - ScrollViewer.Margin.Right) / 2);
                    ScrollViewer.ScrollToVerticalOffset(Canvas.GetLeft(startState) - Height / 2);

                    if (DetermFSM.IsFinalState(DetermFSM.StartState))
                    {
                        CheckImage.Source = new BitmapImage(new Uri("Images/true.png", UriKind.RelativeOrAbsolute));
                    }
                    else
                    {
                        CheckImage.Source = new BitmapImage(new Uri("Images/wrong.png", UriKind.RelativeOrAbsolute));
                    }
                    InitiateGrammarTextBox();

                    CheckWordButton.IsEnabled = true;
                    CheckWordTempTextBox.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
