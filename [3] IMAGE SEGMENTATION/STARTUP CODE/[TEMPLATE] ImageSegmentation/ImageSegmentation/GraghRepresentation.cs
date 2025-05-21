using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace ImageTemplate
{
    public class GraghRepresentation
    {
        //tuple(int weight,pair<int,int> src, pair<int,int> dest);
        // int v;
        /*List<Dictionary<(int, int), int>> blue_adj_list = new List<Dictionary<(int, int), int>>();
        List<Dictionary<(int, int), int>> green_adj_list = new List<Dictionary<(int, int), int>>();
        List<Dictionary<(int, int), int>> red_adj_list = new List<Dictionary<(int, int), int>>();*/

        //list <tuple> ,pair (source ),pair (distination ) ,boolen use as a id for pixel 
        // 
        public int width, length;


        //List<(int weight, int s, int d)> blue_adj_list = new List<(int, int, int)>();
        //List<(int weight, int s, int d)> red_adj_list = new List<(int, int, int)>();

        //List<(int weight, int s, int d)> green_adj_list = new List<(int, int, int)>();

        //List<(int s, int d, int redWeight, int greenWeight, int blueWeight)> adj;
        public List<(int s, int d, int weight)> redAdj;
        public List<(int s, int d, int weight)> greenAdj;
        public List<(int s, int d, int weight)> blueAdj;
        public Dictionary<(int, int, int), int> intersectionOfColors ;
        public Dictionary<int, int> regionSizes;


        //private Dictionary<int, int> mappingToParents = new Dictionary<int, int>(); // key is pixel id and value is root id
        //private List<Dictionary<int, int>> MaxEdge = new List<Dictionary<int, int>> { //key is the root pixel, max edge
        //    new Dictionary<int, int>(), // Red
        //    new Dictionary<int, int>(), // Green
        //    new Dictionary<int, int>()  // Blue
        //};
        //private Dictionary<int, int> clusterSize = new Dictionary<int, int>(); //key is the root pixel, clusterSize
        public int[] redparent, greenparent, blueparent;
        // int[] parent;     // Union-Find root mapping
        public int[] redSize, greenSize, blueSize;
        public int[] redMaxEdge, greenMaxEdge, blueMaxEdge;
        private int nSize;

        public int position_encoding(int row, int col, int width)
        {
            //ENCODING 
            int pos = row * width + col;
            return pos;
        }
        public (int row, int col) position_decoding(int pos)
        {
            int col = pos % width;
            int row = pos / width;
            return (row, col);
        }

        int k = 1;

        public RGBPixel[,] pixels_graph(RGBPixel[,] ImageMatrix, int maskSize, string kString, int channel)
        {
            k = int.Parse(kString);
            length = ImageMatrix.GetLength(0);
            width = ImageMatrix.GetLength(1);
            int numPixels = length * width;
            int src;
            int dis;
            if (channel == 0)
            {
                int red_weight;
                redparent = new int[numPixels];
                redSize = new int[numPixels];
                redMaxEdge = new int[numPixels];


                for (int i = 0; i < numPixels; i++)
                {
                    redparent[i] = i;

                    redSize[i] = 1;

                    redMaxEdge[i] = 0;
                }
                redAdj = new List<(int, int, int)>(length * width);

                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < width; j++)//j == col ==> width  , i == row ==> lenght 
                    {
                        //Console.WriteLine("at " + i + " " + j + " : " + adj.Count); 
                        src = position_encoding(i, j, width);
                        //down
                        if (i + 1 < length)
                        {

                            red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j].red);
                           

                            dis = position_encoding(i + 1, j, width);
                            //red_adj_list.Add((red_weight, Src, dis));
                            //green_adj_list.Add((green_weight, Src, dis));
                            //blue_adj_list.Add((blue_weight, Src, dis));
                            //if (red_weight == 0)
                            //{
                            //    //Console.WriteLine("shortcutting");
                            //    unionSet(src, dis, red_weight, redparent, redSize, redMaxEdge);

                            //}
                            //else
                            //{
                            //    redAdj.Add((src, dis, red_weight));
                            //}
                            redAdj.Add((src, dis, red_weight));
                        }
                        //Console.WriteLine("at " + i + " " + j + " : " + adj.Count);

                        // right 
                        if (j + 1 < width)//3
                        {
                            red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i, j + 1].red);
                            dis = position_encoding(i, j + 1, width);
                            //red_adj_list.Add((red_weight, Src, dis));
                            //green_adj_list.Add((green_weight, Src, dis));
                            //blue_adj_list.Add((blue_weight, Src, dis));
                            //if (red_weight == 0)
                            //{
                            //    //Console.WriteLine("shortcutting");
                            //    unionSet(src, dis, red_weight, redparent, redSize, redMaxEdge);
                            //}
                            //else
                            //{
                            //  redAdj.Add((src, dis, red_weight));
                            //}
                            redAdj.Add((src, dis, red_weight));
                        }

                        // Diagonal down-right(x + 1, y + 1)
                        if (j + 1 < width && i + 1 < length) //2
                        {
                            red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j + 1].red);
                            dis = position_encoding(i + 1, j + 1, width);
                            //red_adj_list.Add((red_weight, Src, dis));
                            //green_adj_list.Add((green_weight, Src, dis));
                            //blue_adj_list.Add((blue_weight, Src, dis));
                            //if (red_weight == 0)
                            //{
                            //    //Console.WriteLine("shortcutting");
                            //    unionSet(src, dis, red_weight, redparent, redSize, redMaxEdge);
                            //}
                            //else
                            //{
                            //    redAdj.Add((src, dis, red_weight));
                            //}
                            redAdj.Add((src, dis, red_weight));
                        }
                        //Console.WriteLine("at " + i + " " + j + " : " + adj.Count);

                        //Diagonal down-left(x - 1, y + 1) //1
                        if (j - 1 >= 0 && i + 1 < length)
                        {
                            red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j - 1].red);
                            dis = position_encoding(i + 1, j - 1, width);
                            //red_adj_list.Add((red_weight, Src, dis));
                            //green_adj_list.Add((green_weight, Src, dis));
                            //blue_adj_list.Add((blue_weight, Src, dis));
                            //if (red_weight == 0)
                            //{
                            //    //Console.WriteLine("shortcutting");
                            //    unionSet(src, dis, red_weight, redparent, redSize, redMaxEdge);
                            //}
                            //else
                            //{
                            //    redAdj.Add((src, dis, red_weight));
                            //}
                            redAdj.Add((src, dis, red_weight));

                        }

                    }

                }
                nSize=redAdj.Count;
                MST(redparent, redSize, redMaxEdge, 0);

            }
            if (channel == 1)
            {
                int green_weight;
                greenparent = new int[numPixels];
                greenSize = new int[numPixels];
                greenMaxEdge = new int[numPixels];


                for (int i = 0; i < numPixels; i++)
                {
                    greenparent[i] = i;

                    greenSize[i] = 1;

                    greenMaxEdge[i] = 0;
                }
                greenAdj = new List<(int, int, int)>(length * width);

                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < width; j++)//j == col ==> width  , i == row ==> lenght 
                    {
                        //Console.WriteLine("at " + i + " " + j + " : " + adj.Count); 
                        src = position_encoding(i, j, width);
                        //down
                        if (i + 1 < length)
                        {

                            green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j].green);


                            dis = position_encoding(i + 1, j, width);
                            //red_adj_list.Add((red_weight, Src, dis));
                            //green_adj_list.Add((green_weight, Src, dis));
                            //blue_adj_list.Add((blue_weight, Src, dis));
                            //if (green_weight == 0)
                            //{
                            //    //Console.WriteLine("shortcutting");
                            //    unionSet(src, dis, green_weight, greenparent, greenSize, greenMaxEdge);

                            //}
                            //else
                            //{
                            //    greenAdj.Add((src, dis, green_weight));
                            //}
                            greenAdj.Add((src, dis, green_weight));

                        }
                        //Console.WriteLine("at " + i + " " + j + " : " + adj.Count);

                        // right 
                        if (j + 1 < width)//3
                        {
                            green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i, j + 1].green);
                            dis = position_encoding(i, j + 1, width);
                            //red_adj_list.Add((red_weight, Src, dis));
                            //green_adj_list.Add((green_weight, Src, dis));
                            //blue_adj_list.Add((blue_weight, Src, dis));
                            //if (green_weight == 0)
                            //{
                            //    //Console.WriteLine("shortcutting");
                            //    unionSet(src, dis, green_weight, greenparent, greenSize, greenMaxEdge);
                            //}
                            //else
                            //{
                            //    greenAdj.Add((src, dis, green_weight));
                            //}
                            greenAdj.Add((src, dis, green_weight));

                        }

                        // Diagonal down-right(x + 1, y + 1)
                        if (j + 1 < width && i + 1 < length) //2
                        {
                            green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j + 1].green);
                            dis = position_encoding(i + 1, j + 1, width);
                            //red_adj_list.Add((red_weight, Src, dis));
                            //green_adj_list.Add((green_weight, Src, dis));
                            //blue_adj_list.Add((blue_weight, Src, dis));
                            //if (green_weight == 0)
                            //{
                            //    //Console.WriteLine("shortcutting");
                            //    unionSet(src, dis, green_weight, greenparent, greenSize, greenMaxEdge);
                            //}
                            //else
                            //{
                            //    redAdj.Add((src, dis, green_weight));
                            //}
                            greenAdj.Add((src, dis, green_weight));
                        }
                        //Console.WriteLine("at " + i + " " + j + " : " + adj.Count);

                        //Diagonal down-left(x - 1, y + 1) //1
                        if (j - 1 >= 0 && i + 1 < length)
                        {
                            green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j - 1].green);
                            dis = position_encoding(i + 1, j - 1, width);
                            //red_adj_list.Add((red_weight, Src, dis));
                            //green_adj_list.Add((green_weight, Src, dis));
                            //blue_adj_list.Add((blue_weight, Src, dis));
                            //if (green_weight == 0)
                            //{
                            //    //Console.WriteLine("shortcutting");
                            //    unionSet(src, dis, green_weight, greenparent, greenSize, greenMaxEdge);
                            //}
                            //else
                            //{
                            //    greenAdj.Add((src, dis, green_weight));
                            //}
                            greenAdj.Add((src, dis, green_weight));
                        }

                    }
                }
                MST(greenparent, greenSize, greenMaxEdge, 1);
                int greenSegs = greenparent.Select((_, i) => findParent(i, greenparent)).Distinct().Count();
                Console.WriteLine($"Green Segments: {greenSegs}");

            }
            if (channel == 2)
            {
                int blue_weight;
                blueparent = new int[numPixels];
                blueSize = new int[numPixels];
                blueMaxEdge = new int[numPixels];


                for (int i = 0; i < numPixels; i++)
                {
                    blueparent[i] = i;

                    blueSize[i] = 1;

                    blueMaxEdge[i] = 0;
                }
                blueAdj = new List<(int, int, int)>(length * width);

                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < width; j++)//j == col ==> width  , i == row ==> lenght 
                    {
                        //Console.WriteLine("at " + i + " " + j + " : " + adj.Count); 
                        src = position_encoding(i, j, width);
                        //down
                        if (i + 1 < length)
                        {

                            blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j].blue);


                            dis = position_encoding(i + 1, j, width);
                            //red_adj_list.Add((red_weight, Src, dis));
                            //green_adj_list.Add((green_weight, Src, dis));
                            //blue_adj_list.Add((blue_weight, Src, dis));
                            //if (blue_weight == 0)
                            //{
                            //    //Console.WriteLine("shortcutting");
                            //    unionSet(src, dis, blue_weight, blueparent, blueSize, blueMaxEdge);

                            //}
                            //else
                            //{
                            //    blueAdj.Add((src, dis, blue_weight));
                            //}
                            blueAdj.Add((src, dis, blue_weight));

                        }
                        //Console.WriteLine("at " + i + " " + j + " : " + adj.Count);

                        // right 
                        if (j + 1 < width)//3
                        {
                            blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i, j + 1].blue);
                            dis = position_encoding(i, j + 1, width);
                            //red_adj_list.Add((red_weight, Src, dis));
                            //green_adj_list.Add((green_weight, Src, dis));
                            //blue_adj_list.Add((blue_weight, Src, dis));
                            //if (blue_weight == 0)
                            //{
                            //    //Console.WriteLine("shortcutting");
                            //    unionSet(src, dis, blue_weight, greenparent, greenSize, greenMaxEdge);
                            //}
                            //else
                            //{
                            //    redAdj.Add((src, dis, green_weight));
                            //}
                            blueAdj.Add((src, dis, blue_weight));

                        }

                        // Diagonal down-right(x + 1, y + 1)
                        if (j + 1 < width && i + 1 < length) //2
                        {
                            blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j + 1].blue);
                            dis = position_encoding(i + 1, j + 1, width);
                            //red_adj_list.Add((red_weight, Src, dis));
                            //green_adj_list.Add((green_weight, Src, dis));
                            //blue_adj_list.Add((blue_weight, Src, dis));
                            //if (green_weight == 0)
                            //{
                            //    //Console.WriteLine("shortcutting");
                            //    unionSet(src, dis, green_weight, redparent, redSize, redMaxEdge);
                            //}
                            //else
                            //{
                            //    redAdj.Add((src, dis, green_weight));
                            //}
                            blueAdj.Add((src, dis, blue_weight));

                        }
                        //Console.WriteLine("at " + i + " " + j + " : " + adj.Count);

                        //Diagonal down-left(x - 1, y + 1) //1
                        if (j - 1 >= 0 && i + 1 < length)
                        {
                            blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j - 1].blue);
                            dis = position_encoding(i + 1, j - 1, width);
                            //red_adj_list.Add((red_weight, Src, dis));
                            //green_adj_list.Add((green_weight, Src, dis));
                            //blue_adj_list.Add((blue_weight, Src, dis));
                            //if (green_weight == 0)
                            //{
                            //    //Console.WriteLine("shortcutting");
                            //    unionSet(src, dis, green_weight, redparent, redSize, redMaxEdge);
                            //}
                            //else
                            //{
                            //    redAdj.Add((src, dis, green_weight));
                            //}
                            blueAdj.Add((src, dis, blue_weight));

                        }

                    }
                }
                MST(blueparent, blueSize, blueMaxEdge, 2);

                RGBPixel[,] result = ColourImage(ImageMatrix);
                int blueSegs = blueparent.Select((_, i) => findParent(i, blueparent)).Distinct().Count();
                //Console.WriteLine($"Blue Segments: {blueSegs}");

                return result;



            }



            int redSegs = redparent.Select((_, i) => findParent(i, redparent)).Distinct().Count();




            //WriteInFile();
            Console.WriteLine($"Red Segments: {redSegs}");
            //SaveSegmentedImage(redparent, "redSegmented.png");
            //SaveSegmentedImage(greenparent, "greenSegmented.png");
            //SaveSegmentedImage(blueparent, "blueSegmented.png");
            return null;
        }

        bool IsInside(RGBPixel[,] img, int i, int j)
        {
            return i >= 0 && i < img.GetLength(0) && j >= 0 && j < img.GetLength(1);
        }

        RGBPixel[,] ColourImage(RGBPixel[,] imageMatrix)
        {
            Random rand = new Random();
            int numPixels = length * width;

            intersectionOfColors = new Dictionary<(int, int, int), int>();
            regionSizes = new Dictionary<int, int>();
            Dictionary<int, RGBPixel> regionColors = new Dictionary<int, RGBPixel>();

            int[,] regionMap = new int[length, width]; // stores final region id per pixel
            bool[,] visited = new bool[length, width];

            int regionID = 1;

            int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (visited[i, j]) continue;

                    int pos = position_encoding(i, j, width);
                    var regionKey = (
                        findParent(pos, redparent),
                        findParent(pos, greenparent),
                        findParent(pos, blueparent)
                    );

                    // Start BFS
                    Queue<(int x, int y)> q = new Queue<(int x, int y)>();
                    q.Enqueue((i, j));
                    visited[i, j] = true;
                    regionMap[i, j] = regionID;
                    int regionSize = 1;

                    while (q.Count > 0)
                    {
                        var (x, y) = q.Dequeue();

                        for (int d = 0; d < 8; d++)
                        {
                            int nx = x + dx[d];
                            int ny = y + dy[d];

                            if (nx >= 0 && nx < length && ny >= 0 && ny < width && !visited[nx, ny])
                            {
                                int npos = position_encoding(nx, ny, width);
                                var neighborKey = (
                                    findParent(npos, redparent),
                                    findParent(npos, greenparent),
                                    findParent(npos, blueparent)
                                );

                                if (neighborKey == regionKey)
                                {
                                    visited[nx, ny] = true;
                                    regionMap[nx, ny] = regionID;
                                    q.Enqueue((nx, ny));
                                    regionSize++;
                                }
                            }
                        }
                    }

                    // Assign color and size
                    regionSizes[regionID] = regionSize;
                    regionColors[regionID] = new RGBPixel
                    {
                        red = (byte)rand.Next(0, 256),
                        green = (byte)rand.Next(0, 256),
                        blue = (byte)rand.Next(0, 256)
                    };

                    // Just map a key for interactive merging support
                    intersectionOfColors[regionKey] = regionID;
                    regionID++;
                }
            }

            // Color image by regionMap
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int id = regionMap[i, j];
                    imageMatrix[i, j] = regionColors[id];
                }
            }

            // Save region stats to file
            string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string path = Path.Combine(projectDir, "ImageSegmentation", "numOfSegmentsAndSizes.txt");

            var sizes = regionSizes.Values.ToList();
            sizes.Sort((a, b) => b.CompareTo(a)); // Descending

            string content = $"Number of regions : {sizes.Count}\n";
            foreach (int s in sizes)
                content += $"region size : {s}\n";

            File.WriteAllText(path, content);

            return imageMatrix;
        }


        //Mariam matnsesh t sort al 3 arraysssss

        void MST(int[] parentcolor, int[] colorsize, int[] colormaxedge, int channel)
        {
            if (channel==0)
            {
                redAdj.Sort((a, b) => a.weight.CompareTo(b.weight));
                int weight = 0;
                int maxEdges = (length * width) - 1;
                for (int i = 0; i < nSize; i++)
                {
                    // List<int> weights = new List<int> { blue_adj_list[i].weight, red_adj_list[i].weight, green_adj_list[i].weight };
                    weight = redAdj[i].weight;

                    // unionSet(red_adj_list[i].s, red_adj_list[i].d, weights);
                    unionSet(redAdj[i].s, redAdj[i].d, weight, parentcolor, colorsize, colormaxedge);
                }
            }
            else if (channel == 1)
            {
                greenAdj.Sort((a, b) => a.weight.CompareTo(b.weight));
                int weight = 0;
                int maxEdges = (length * width) - 1;
                for (int i = 0; i < nSize; i++)
                {
                    // List<int> weights = new List<int> { blue_adj_list[i].weight, red_adj_list[i].weight, green_adj_list[i].weight };
                    weight = greenAdj[i].weight;

                    // unionSet(red_adj_list[i].s, red_adj_list[i].d, weights);
                    unionSet(greenAdj[i].s, greenAdj[i].d, weight, parentcolor, colorsize, colormaxedge);
                }
            }
            else if (channel == 2)
            {
                blueAdj.Sort((a, b) => a.weight.CompareTo(b.weight));
                int weight = 0;
                int maxEdges = (length * width) - 1;
                for (int i = 0; i < nSize; i++)
                {
                    // List<int> weights = new List<int> { blue_adj_list[i].weight, red_adj_list[i].weight, green_adj_list[i].weight };
                    weight = blueAdj[i].weight;

                    unionSet(blueAdj[i].s, blueAdj[i].d, weight, parentcolor, colorsize, colormaxedge);
                }
            }
        }
        public int findParent(int pixelID, int[] parentcolor) // lazm?? 
        {
            //find the parent of the node
            if (parentcolor[pixelID] != pixelID)
                parentcolor[pixelID] = findParent(parentcolor[pixelID], parentcolor);  // path compression
            return parentcolor[pixelID];
        }

        //void initializeFistPixel(int pixelID)
        //{
        //    mappingToParents.Add(pixelID,pixelID); // lwa7dha fel region 
        //    for(int i=0; i<3;i++)
        //        MaxEdge[i][pixelID] = (0);
        //    clusterSize[pixelID] = 1;
        //}
        bool unionSet(int pixel1ID, int pixel2ID, int weight, int[] parent, int[] size, int[] maxEdge)
        {
            int root1 = findParent(pixel1ID, parent);
            int root2 = findParent(pixel2ID, parent);

            if (root1 == root2)
                return false;

            double threshold1 = maxEdge[root1] + (k / (double)size[root1]);
            double threshold2 = maxEdge[root2] + (k / (double)size[root2]);

            if (weight > Math.Min(threshold1, threshold2))
                return false;

            // Union by size
            if (size[root1] < size[root2])
            {
                parent[root1] = root2;
                size[root2] += size[root1];
                maxEdge[root2] = Math.Max(Math.Max(maxEdge[root1], maxEdge[root2]), weight);
            }
            else
            {
                parent[root2] = root1;
                size[root1] += size[root2];
                maxEdge[root1] = Math.Max(Math.Max(maxEdge[root1], maxEdge[root2]), weight);
            }

            return true;
        }
        void SaveSegmentedImage(int[] parent, string outputPath)
        {
            RGBPixel[,] segmentedImage = new RGBPixel[length, width];
            Dictionary<int, RGBPixel> regionColors = new Dictionary<int, RGBPixel>();
            Random rand = new Random();

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int id = position_encoding(i, j, width);
                    int root = findParent(id, parent);

                    if (!regionColors.ContainsKey(root))
                    {
                        regionColors[root] = new RGBPixel
                        {
                            red = (byte)rand.Next(256),
                            green = (byte)rand.Next(256),
                            blue = (byte)rand.Next(256)
                        };
                    }

                    segmentedImage[i, j] = regionColors[root];
                }
            }

            string projectDir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            string fullPath = Path.Combine(projectDir, "ImageSegmentation", outputPath);
            //ImageOperations.DisplayImage(segmentedImage, new PictureBox()); // Optional for GUI
            SaveImage(segmentedImage, fullPath);
        }
        public static void SaveImage(RGBPixel[,] ImageMatrix, string FilePath)
        {
            int height = ImageMatrix.GetLength(0);
            int width = ImageMatrix.GetLength(1);
            Bitmap bmp = new Bitmap(width, height);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    RGBPixel pixel = ImageMatrix[i, j];
                    Color color = Color.FromArgb(pixel.red, pixel.green, pixel.blue);
                    bmp.SetPixel(j, i, color);
                }
            }

            bmp.Save(FilePath); // Automatically infers format from extension
        }

        
        //bool unionSet(int pixel1ID, int pixel2ID, List<int> weights)
        //{
        //    //if two nodes are in the same set (have the same parent) don't merge, return false
        //    //else calculate everything and check
        //    //if the internal difference is less than the threshold, return false
        //    //else merge the two sets and return true
        //    //union the two sets
        //    int rootP1 = findParent(pixel1ID);
        //    int rootP2 = findParent(pixel2ID);
        //    if (rootP1 == rootP2)
        //        return false;

        //    for (int i = 0; i < 3; i++)
        //    {
        //        int thresholdP1 = maxEdge[i][rootP1] + (k / size[rootP1]);
        //        int thresholdP2 = maxEdge[i][rootP2] + (k / size[rootP2]);

        //        if (weights[i] > Math.Min(thresholdP1, thresholdP2))
        //        {
        //            return false;
        //        }
        //        // Union by size   
        //    }
        //    if (size[rootP1] < size[rootP2])
        //    {
        //        parent[rootP1] = rootP2;
        //        size[rootP2] += size[rootP1];

        //        for (int i = 0; i < 3; i++) { 
        //        maxEdge[i][rootP1] = Math.Max(Math.Max(maxEdge[i][rootP1], maxEdge[i][rootP2]), weights[i]);
        //        }
        //    }
        //    else
        //    {
        //        parent[rootP2] = rootP1;
        //        size[rootP1] += size[rootP2];
        //        for (int i = 0; i < 3; i++)
        //        {
        //            maxEdge[i][rootP2] = Math.Max(Math.Max(maxEdge[i][rootP1], maxEdge[i][rootP2]), weights[i]);
        //        }
        //    }
        //    return true;
        //}

        // menna check han merge wla la

        //void merge(int pixel1ID, int pixel2ID, List<int> weights)
        //{


        //    if (!mappingToParents.ContainsKey(pixel1ID))
        //        initializeFistPixel(pixel1ID);
        //    if (!mappingToParents.ContainsKey(pixel2ID))
        //        initializeFistPixel(pixel2ID);
        //    int root1 = mappingToParents[pixel1ID];
        //    int root2 = mappingToParents[pixel2ID];


        //    mappingToParents[root2] = root1; // now pixel1ID is the root          
        //    foreach (var key in mappingToParents.Keys.ToList())
        //    {
        //        if (mappingToParents[key] == root2)
        //        {
        //            mappingToParents[key] = root1;
        //        }
        //    }

        //    int size1 = clusterSize[root1];
        //    int size2 = clusterSize[root2];
        //    clusterSize.Remove(root2);
        //    clusterSize[root1] = size1 + size2;

        //    for (int i = 0; i < 3; i++)
        //    {
        //        int maxEdge1 = MaxEdge[i][root1];
        //        int maxEdge2 = MaxEdge[i][root2];

        //        MaxEdge[i].Remove(root2);
        //        MaxEdge[i][root1] = Math.Max(Math.Max(maxEdge1, maxEdge2), weights[i]); // update size and max edge
        //    }
        //}

    }
}