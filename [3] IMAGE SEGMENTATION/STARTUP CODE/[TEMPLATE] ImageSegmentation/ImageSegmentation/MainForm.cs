using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace ImageTemplate
{
    public partial class MainForm : Form
    {
        HashSet<int> selected = new HashSet<int>();

        GraghRepresentation graphs;
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;
        public static Stopwatch stopwatch;
        RGBPixel[,] original;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            original = ImageMatrix;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            stopwatch = new Stopwatch();
            stopwatch.Start();
            // mike
            graphs = new GraghRepresentation();
            //ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            graphs.pixels_graph(ImageMatrix, maskSize, textBox1.Text, 0);
            graphs.pixels_graph(ImageMatrix, maskSize, textBox1.Text, 1);
            RGBPixel[,] result = graphs.pixels_graph(ImageMatrix, maskSize, textBox1.Text, 2);
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            ImageOperations.DisplayImage(result, pictureBox2);
            MessageBox.Show("Elapsed time: "+ elapsedTime.ToString());

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }




        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = e.Location;
            int col = p.X;
            int row = p.Y;

            if (row >= graphs.length || col >= graphs.width || row < 0 || col < 0)
                return;

            //GraghRepresentation g = new GraghRepresentation(); 
            int pos = graphs.position_encoding(row, col, graphs.width);
            // RGBPixel pix = ImageMatrix[row, col];
            int redroot = graphs.findParent(pos, graphs.redparent);
            int greenroot = graphs.findParent(pos, graphs.greenparent);
            int blueroot = graphs.findParent(pos, graphs.blueparent);
            //mark it
            ImageMatrix[row, col] = new RGBPixel { red = 0, green = 0, blue = 255 };

            int id = graphs.intersectionOfColors[(redroot, greenroot, blueroot)];
            selected.Add(id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int firstregion = selected.First();

            var firstregionroot = graphs.intersectionOfColors.First(kv => kv.Value == firstregion).Key;
            foreach (int id in selected)
            {
                foreach (var regionid in graphs.intersectionOfColors)
                {

                    if (regionid.Value != id)
                    {
                        continue;
                    }

                    for (int i = 0; i < graphs.length; i++)
                    {
                        for (int j = 0; j < graphs.width; j++)
                        {

                            var key = regionid.Key;

                            int pos = graphs.position_encoding(i, j, graphs.width);

                            int redP = graphs.findParent(pos, graphs.redparent);
                            int greenP = graphs.findParent(pos, graphs.greenparent);
                            int blueP = graphs.findParent(pos, graphs.blueparent);

                            if ((redP, greenP, blueP) == key)
                            {
                                // Merge 
                                graphs.redparent[pos] = firstregionroot.Item1;
                                graphs.greenparent[pos] = firstregionroot.Item2;
                                graphs.blueparent[pos] = firstregionroot.Item3;

                                ImageMatrix[i, j] = original[i, j];
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < graphs.length; i++)
            {
                for (int j = 0; j < graphs.width; j++)
                {
                    int pos = graphs.position_encoding(i, j, graphs.width);
                    int redP = graphs.findParent(pos, graphs.redparent);
                    int greenP = graphs.findParent(pos, graphs.greenparent);
                    int blueP = graphs.findParent(pos, graphs.blueparent);

                    var key = (redP, greenP, blueP);

                    if (graphs.intersectionOfColors.TryGetValue(key, out int regionId))
                    {
                        if (!selected.Contains(regionId))
                        {
                            ImageMatrix[i, j].red = 255;
                            ImageMatrix[i, j].green = 255;
                            ImageMatrix[i, j].blue = 255;
                        }
                    }
                }
            }
            selected.Clear();
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }
    }
}