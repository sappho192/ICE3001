using GibbsPhenomenonTest.Core;
using InteractiveDataDisplay.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GibbsPhenomenonTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApproxTest test;
        private const int FROM = 0;
        private const int TO = 2;
        double[] dis_t;
        double[] x_t;
        double[] xN_t;
        double[] eN_t;

        private LineGraph graph_x_t;
        private LineGraph graph_xN_t;

        public MainWindow()
        {
            test = new ApproxTest(10, 0.005);
            dis_t = Approx.GetDiscreteT(FROM, TO, test.Delta_t);

            InitializeComponent();
            tbN.Text = test.N.ToString();

            InitializeApproxTab();
            InitializeErrorTab();
            UpdateAbsErr();
        }

        private void UpdateAbsErr()
        {
            /*
             * eN_t :  -1, +0.4, -0.2, +2
             * eN_t.Min() == -1 => +1
             * eN_t.Max() == +2 => +2
             * 각각의 절댓값을 구하면?
             */
            var absMin = Math.Abs(eN_t.Min());
            var absMax = Math.Abs(eN_t.Max());
            tbAbsError.Text = (absMin > absMax ? absMin : absMax).ToString();
        }

        private void InitializeErrorTab()
        {
            eN_t = x_t.Zip(xN_t, (a, b) => a - b).ToArray();
            eNGraph.Plot(dis_t, eN_t);
        }

        private void InitializeApproxTab()
        {
            x_t = Approx.SetOf(test.x_t, FROM, TO, test.Delta_t);
            xN_t = Approx.SetOf(test.xN_t, FROM, TO, test.Delta_t, test.N);

            //printBox(string.Join(",", x_t));
            graph_x_t = new LineGraph();
            Graphs.Children.Add(graph_x_t);
            graph_x_t.Description = "x(t)";
            graph_x_t.Stroke = Brushes.Black;
            graph_x_t.StrokeThickness = 2;
            graph_x_t.Plot(dis_t, x_t);

            //printBox("xN(t)");
            //printBox(string.Join(",", xN_t));
            graph_xN_t = new LineGraph();
            Graphs.Children.Add(graph_xN_t);
            graph_xN_t.Description = "xN(t)";
            graph_xN_t.Stroke = Brushes.Gray;
            graph_xN_t.StrokeThickness = 2;
            graph_xN_t.Plot(dis_t, xN_t);
        }

        private void printBox(string message)
        {
            PrintBlock.Text += $"{message}{System.Environment.NewLine}";
        }

        private void BtApply_Click(object sender, RoutedEventArgs e)
        {
            if(uint.TryParse(tbN.Text, out uint result) && result != 0)
            {
                test.N = result;
                RefreshGraphs();
            }
            else
            {
                tbN.Text = test.N.ToString();
            }
        }

        private void RefreshGraphs()
        {
            RefreshApproxGraphs();
            RefreshErrorGraphs();
        }

        private void RefreshErrorGraphs()
        {
            eN_t = x_t.Zip(xN_t, (a, b) => a - b).ToArray();
            eNGraph.Plot(dis_t, eN_t);
        }

        private void RefreshApproxGraphs()
        {
            dis_t = Approx.GetDiscreteT(FROM, TO, test.Delta_t);
            x_t = Approx.SetOf(test.x_t, FROM, TO, test.Delta_t);
            xN_t = Approx.SetOf(test.xN_t, FROM, TO, test.Delta_t, test.N);
            graph_x_t.Plot(dis_t, x_t);
            graph_xN_t.Plot(dis_t, xN_t);
        }
    }
}
