using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.MDS;
using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace ProcessDependency
{
    public partial class Form : System.Windows.Forms.Form
    {
        private List<string> Services = new List<string>
        {
        };


        public Form()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 
            //var allServices = ServiceController.GetServices().Where(ser => Services.Contains(ser.ServiceName));
            foreach (ServiceController serviceController in ServiceController.GetServices())
            {
                var service = $"{serviceController.DisplayName} ({serviceController.ServiceName})";
                graph.AddNode(service);
                foreach (var dependentService in serviceController.DependentServices)
                {
                    string dependent = $"{dependentService.DisplayName} ({dependentService.ServiceName})";
                    graph.AddNode(dependent);
                    graph.AddEdge(dependent, service);
                }
            }
            //bind the graph to the viewer 
            graph.Attr.LayerDirection = LayerDirection.None;
            graph.Attr.OptimizeLabelPositions = true;
            graph.CreateLayoutSettings();
            graph.LayoutAlgorithmSettings = new MdsLayoutSettings();
            viewer.Graph = graph;
            SuspendLayout();
            viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            Controls.Add(viewer);
            ResumeLayout();

        }
    }
}
