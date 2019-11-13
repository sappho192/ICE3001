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
        private readonly ApproxTest test;
        private const int FROM = 0;
        private const int TO = 2;
        private double[] dis_t;
        private double[] x_t;
        private double[] xN_t;
        private double[] eN_t;

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
            UpdateErrors();
        }

        private void UpdateErrors()
        {
            UpdateAbsErr();
            UpdateRMSErr();
        }

        private void UpdateRMSErr()
        {
            double RMSE =
                Math.Sqrt(
                    eN_t.Sum((e) => Math.Pow(e, 2))
                    / eN_t.Length
                );
            tbRMSError.Text = RMSE.ToString();
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
            UpdateErrorGraphs();
        }

        private void InitializeApproxTab()
        {
            x_t = Approx.SetOf(test.x_t, FROM, TO, test.Delta_t);
            xN_t = Approx.SetOf(test.xN_t, FROM, TO, test.Delta_t, test.N);

            graph_x_t = new LineGraph();
            Graphs.Children.Add(graph_x_t);
            graph_x_t.Description = "x(t)";
            graph_x_t.Stroke = Brushes.Black;
            graph_x_t.StrokeThickness = 2;
            graph_x_t.Plot(dis_t, x_t);

            graph_xN_t = new LineGraph();
            Graphs.Children.Add(graph_xN_t);
            graph_xN_t.Description = "xN(t)";
            graph_xN_t.Stroke = Brushes.Gray;
            graph_xN_t.StrokeThickness = 2;
            graph_xN_t.Plot(dis_t, xN_t);
        }

        private void BtApply_Click(object sender, RoutedEventArgs e)
        {
            ApplyChanges();
        }

        private void ApplyChanges()
        {
            if (uint.TryParse(tbN.Text, out uint result) && result != 0)
            {
                test.N = result;
                UpdateGraphs();
                UpdateErrors();
            }
            else
            {
                tbN.Text = test.N.ToString();
            }
        }

        private void UpdateGraphs()
        {
            UpdateApproxGraphs();
            UpdateErrorGraphs();
        }

        private void UpdateErrorGraphs()
        {
            eN_t = x_t.Zip(xN_t, (a, b) => a - b).ToArray();

            // distance: t 축에서 1만큼의 거리가 delta_t 간격으로 얼마나 나뉘었는지
            uint distance = (uint)(1.0 / test.Delta_t);
            for (uint i = 0; i < eN_t.Length; i += distance)
            {// 시작점과 끝점이 정수이므로 불연속점을 알 수 있음
                eN_t[i] = 0; // 불연속점에서의 오차는 생략 처리함
            }

            eNGraph.Plot(dis_t, eN_t);
        }

        private void UpdateApproxGraphs()
        {
            dis_t = Approx.GetDiscreteT(FROM, TO, test.Delta_t);
            x_t = Approx.SetOf(test.x_t, FROM, TO, test.Delta_t);
            xN_t = Approx.SetOf(test.xN_t, FROM, TO, test.Delta_t, test.N);
            graph_x_t.Plot(dis_t, x_t);
            graph_xN_t.Plot(dis_t, xN_t);
        }

        private void TbN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ApplyChanges();
            }
        }
    }
}
