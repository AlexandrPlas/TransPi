using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ZedGraph;

namespace TransPi
{
    public partial class FormGraph : Form
    {
        public FormGraph()
        {
            InitializeComponent();

            DrawGraph();
        }
        public double[] Yar, Xar;

        private void FormGraph_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void DrawGraph()
        {
            
        }

        private void DrawSingleGraph(GraphPane pane, double[] mass)
        {
            if (mass != null)
            {
                pane.CurveList.Clear();

                PointPairList list = new PointPairList();

                int xmin = 0;
                int xmax = mass.Length;

                for (int x = xmin; x < xmax; x++)
                {
                    list.Add(x, mass[x]);
                }

                LineItem myCurve = pane.AddCurve("X Scale", list, Color.Blue, SymbolType.None);
            }
        }
        private void DrawSingleGraph1(GraphPane pane, double[] mass)
        {
            if (mass != null)
            {
                pane.CurveList.Clear();

                PointPairList list = new PointPairList();

                int xmin = 0;
                int xmax = mass.Length;

                for (int x = xmin; x < xmax; x++)
                {
                    list.Add(x, mass[x]);
                }

                LineItem myCurve = pane.AddCurve("YScale", list, Color.Red, SymbolType.None);
            }
        }

        private void zedGraph_Load(object sender, EventArgs e)
        {

        }

        private void zedGraph_Paint(object sender, PaintEventArgs e)
        {
            ZedGraph.MasterPane masterPane = zedGraph.MasterPane;

            masterPane.PaneList.Clear();

            GraphPane pane1 = new GraphPane()
                     ,pane2 = new GraphPane();

            DrawSingleGraph(pane1, Xar);
            DrawSingleGraph1(pane2, Yar);

            masterPane.Add(pane1);
            masterPane.Add(pane2);

            using (Graphics g = CreateGraphics())
            {
                masterPane.SetLayout(g, PaneLayout.SingleColumn);
            }

            zedGraph.AxisChange();
            zedGraph.Invalidate();
        }

    }
}
