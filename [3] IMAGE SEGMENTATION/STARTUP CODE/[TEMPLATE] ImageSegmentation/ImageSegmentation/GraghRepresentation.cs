using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

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

        int width, length;
        List<(int weight, int s, int d)> blue_adj_list = new List<(int, int, int)>();
        List<(int weight, int s, int d)> red_adj_list = new List<(int, int, int)>();

        List<(int weight, int s, int d)> green_adj_list = new List<(int, int, int)>();

        private Dictionary<int, int> mappingToParents = new Dictionary<int, int>(); // key is pixel id and value is root id
        private List<Dictionary<int, int>> MaxEdge = new List<Dictionary<int, int>> { //key is the root pixel, max edge
            new Dictionary<int, int>(), // Red
            new Dictionary<int, int>(), // Green
            new Dictionary<int, int>()  // Blue
        };
        private Dictionary<int, int> clusterSize = new Dictionary<int, int>(); //key is the root pixel, clusterSize

        private int nSize;

        int position_encoding (int row ,int col ,int width)
        {
            //ENCODING 
            int pos = row * width + col;
            return pos;
        }
        (int x ,int y) position_decoding(int pos)
        {
              int x = pos % width; // col ==j
             int y = pos / width; // row =i

            return (y, x);
        }

        int k=1;

        public RGBPixel[,] pixels_graph(RGBPixel[,] ImageMatrix , int maskSize)
        {

            red_adj_list.Clear();
            green_adj_list.Clear();
            blue_adj_list.Clear();

            int red_weight;
            int green_weight;
            int blue_weight;
            int Src;
            int dis;


            // List<List<RGBPixel>> adj_list = new List<List<RGBPixel>>();
            length = ImageMatrix.GetLength(0);
            width = ImageMatrix.GetLength(1);//hwa n 
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)//j == col ==> width  , i == row ==> lenght 
                {
                   

                    //down
                    if (i + 1 < length)
                    {
                        red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j].red);
                        green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j].green);
                        blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j].blue);
                        Src = position_encoding(i, j, width);

                        dis = position_encoding(i + 1, j, width);
                        red_adj_list.Add((red_weight, Src, dis));
                        green_adj_list.Add((green_weight, Src, dis));
                        blue_adj_list.Add((blue_weight, Src, dis));
                    }
                    // right 
                    if (j + 1 < width)
                    {
                        red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i, j + 1].red);
                        green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i, j + 1].green);
                        blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i, j + 1].blue);
                        Src = position_encoding(i, j, width);
                        dis = position_encoding(i, j + 1, width);
                        red_adj_list.Add((red_weight, Src, dis));
                        green_adj_list.Add((green_weight, Src, dis));
                        blue_adj_list.Add((blue_weight, Src, dis));


                    }
                    // Diagonal down-right(x + 1, y + 1)
                    if (j + 1 < width && i+1<length)
                    {
                        red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i+1, j + 1].red);
                        green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j + 1].green);
                        blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j + 1].blue);
                        Src = position_encoding(i, j, width);
                        dis = position_encoding(i+1, j + 1, width);
                        red_adj_list.Add((red_weight, Src, dis));
                        green_adj_list.Add((green_weight, Src, dis));
                        blue_adj_list.Add((blue_weight, Src, dis));

                    }


                    //Diagonal down-left(x - 1, y + 1)
                    if ( j-1 >=0 && i+1<length)
                    {
                        red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j - 1].red);
                        green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j - 1].green);
                        blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j - 1].blue);
                        Src = position_encoding(i, j, width);
                        dis = position_encoding(i+1, j - 1, width);
                        red_adj_list.Add((red_weight, Src, dis));
                        green_adj_list.Add((green_weight, Src, dis));
                        blue_adj_list.Add((blue_weight, Src, dis));

                    }



                }
            }
            nSize = red_adj_list.Count();
            MST();
            return ColourImage(ImageMatrix);

        }

        bool IsInside(RGBPixel[,] img, int i, int j)
        {
            return i >= 0 && i < img.GetLength(0) && j >= 0 && j < img.GetLength(1);
        }

        RGBPixel[,] ColourImage(RGBPixel[,] imageMatrix)
        {
            foreach (int key in mappingToParents.Keys)
            {
                (int x, int y) = position_decoding(key);
                (int xParent, int yParent) = position_decoding(mappingToParents[key]);

                if (IsInside(imageMatrix, x, y) && IsInside(imageMatrix, xParent, yParent))
                {
                    imageMatrix[x, y].red = imageMatrix[xParent, yParent].red;
                    imageMatrix[x, y].green = imageMatrix[xParent, yParent].green;
                    imageMatrix[x, y].blue = imageMatrix[xParent, yParent].blue;
                }

            }
            return imageMatrix;
        }



        //Mariam matnsesh t sort al 3 arraysssss

        void MST()
        {
            red_adj_list.Sort();
            blue_adj_list.Sort();
            green_adj_list.Sort();


            int noOfEdges = 0;
            for (int i = 0; i < nSize && noOfEdges != nSize - 1; i++)
            {
                bool union = unionSet(red_adj_list[i].s, red_adj_list[i].d, red_adj_list[i].weight);



                if (union)
                {
                    List<int> weights = new List<int> { blue_adj_list[i].weight, red_adj_list[i].weight, green_adj_list[i].weight };
                    noOfEdges++;
                    merge(blue_adj_list[i].s, blue_adj_list[i].d, weights);

                }
            }
        }
        int findParent(int pixelID) // lazm?? 
        {
            //find the parent of the node
            if (mappingToParents[pixelID] != pixelID)
                mappingToParents[pixelID] = findParent(mappingToParents[pixelID]);
            return mappingToParents[pixelID];
        }

        void initializeFistPixel(int pixelID)
        {
            mappingToParents.Add(pixelID,pixelID); // lwa7dha fel region 
            for(int i=0; i<3;i++)
                MaxEdge[i][pixelID] = (0);
            clusterSize[pixelID] = 1;
        }
       // (int,int) findSizeAndInternalDiffernece(int pixelID) //return the biggest edge in the tree,
                                     //check if there is only one pixel in
                                     //the tree, return 0
      //  {
            //find the internal difference of the node
        //    int parentId = findParent(pixelID);    
       //     return (sizeOfSetAndMaxEdge[parentId]);           
      //  }
       
        bool unionSet(int pixel1ID, int pixel2ID, int edgeWeight)
        {
            //if two nodes are in the same set (have the same parent) don't merge, return false
            //else calculate everything and check
            //if the internal difference is less than the threshold, return false
            //else merge the two sets and return true
            //union the two sets
            if (!mappingToParents.ContainsKey(pixel1ID)) 
                initializeFistPixel(pixel1ID);           
            if (!mappingToParents.ContainsKey(pixel2ID)) 
                initializeFistPixel(pixel2ID);

          //  int root1 = findParent(pixel1ID);
           // int root2 = findParent(pixel2ID);
            int root1 = mappingToParents[pixel1ID];
            int root2 = mappingToParents[pixel2ID];
            if (root1 == root2)
                return false;
            int size1 = clusterSize[root1];
            int size2 = clusterSize[root2];
            if (size1 == 0)
                size1 = 1; // or handle differently depending on your logic
            if (size2 == 0)
                size2 = 1; // or handle differently depending on your logic
            Console.WriteLine("sizeee: "+size1 + " " + size2);

            for (int i = 0; i < 3; i++) {
                int maxEdge1=MaxEdge[i][root1]; //size, max edge
                int maxEdge2 = MaxEdge[i][root2]; //size, max edge

                int pixel1threshold = maxEdge1 + (k / size1);
                int pixel2threshold = maxEdge2 + (k / size2);
                int mint = Math.Min(pixel1threshold, pixel2threshold);
                if (edgeWeight > mint)
                    return false;

            }
            // menna check han merge wla la


            // lw han merge 

            return true;
        }
        void merge(int pixel1ID, int pixel2ID, List<int>weights)
        {
            if (!mappingToParents.ContainsKey(pixel1ID))
                initializeFistPixel(pixel1ID);
            if (!mappingToParents.ContainsKey(pixel2ID))
                initializeFistPixel(pixel2ID);
            int root1 = mappingToParents[pixel1ID];
            int root2 = mappingToParents[pixel2ID];


            mappingToParents[root2] = root1; // now pixel1ID is the root          
            foreach (var key in mappingToParents.Keys.ToList())
            {
                if (mappingToParents[key] == root2)
                {
                    mappingToParents[key] = root1;
                }
            }

            int size1 = clusterSize[root1];
            int size2 = clusterSize[root2];
            clusterSize.Remove(root2);
            clusterSize[root1]= size1 + size2;

            for (int i = 0; i < 3; i++)
            {
                int maxEdge1 = MaxEdge[i][root1];
                int maxEdge2 = MaxEdge[i][root2];

                MaxEdge[i].Remove(root2);
                MaxEdge[i][root1] = Math.Max(Math.Max(maxEdge1, maxEdge2), weights[i]); // update size and max edge
            }
        }

    }
}
