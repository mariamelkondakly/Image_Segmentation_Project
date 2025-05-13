using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        List<(int weight, int s, int d)> blue_adj_list = new List<(int, int, int)>();
        List<(int weight, int s, int d)> red_adj_list = new List<(int, int, int)>();

        List<(int weight, int s, int d)> green_adj_list = new List<(int, int, int)>();

        int posithoin_encoding (int row ,int col ,int widht)

        {

            //ENCODING 
            int pos = row * widht + col; 
            return pos;

            

        }
        (int x ,int y) posithon_decoding(int pos , int width)
        {
              int x = pos % width; // col ==j
             int y = pos / width; // row =i

            return (x, y);
        }

        int k;

        public (List<(int, int, int)> red, List<(int, int, int)> green, List<(int, int, int)> blue) pixels_graph(RGBPixel[,] ImageMatrix , int maskSize)
        {
            k = maskSize;

            red_adj_list.Clear();
            green_adj_list.Clear();
            blue_adj_list.Clear();

            int red_weight;
            int green_weight;
            int blue_weight;
            int Src;
            int dis;


            // List<List<RGBPixel>> adj_list = new List<List<RGBPixel>>();
            int length = ImageMatrix.GetLength(0);
            int width = ImageMatrix.GetLength(1);//hwa n 
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
                        Src = posithoin_encoding(i, j, width);

                        dis = posithoin_encoding(i + 1, j, width);
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
                        Src = posithoin_encoding(i, j, width);
                        dis = posithoin_encoding(i, j + 1, width);
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
                        Src = posithoin_encoding(i, j, width);
                        dis = posithoin_encoding(i+1, j + 1, width);
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
                        Src = posithoin_encoding(i, j, width);
                        dis = posithoin_encoding(i+1, j - 1, width);
                        red_adj_list.Add((red_weight, Src, dis));
                        green_adj_list.Add((green_weight, Src, dis));
                        blue_adj_list.Add((blue_weight, Src, dis));

                    }



                }
            }

            return (red_adj_list, green_adj_list, blue_adj_list);

        }

        //Mariam matnsesh t sort al 3 arraysssss

        private Dictionary<int, int> mappingToParents = new Dictionary<int, int>(); // key is pixel id and value is root id
        private Dictionary<int, (int,int)> sizeOfSetAndMaxEdge = new Dictionary<int, (int, int)>(); // key is root node id and value is size of comp and max edge in it 
    
        int findParent(int pixelID) // lazm?? 
        {
            //find the parent of the node
            if (mappingToParents[pixelID] != pixelID)
                mappingToParents[pixelID] = findParent(mappingToParents[pixelID]);
            return mappingToParents[pixelID];
        }
        void initializeFistPixel(int pixelID)
        {
            mappingToParents[pixelID] = pixelID; // lwa7dha fel region 
            sizeOfSetAndMaxEdge[pixelID] = (1, 0);
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

            var(size1, maxEdge1) = sizeOfSetAndMaxEdge[root1];
            var (size2, maxEdge2) = sizeOfSetAndMaxEdge[root2];
            int pixel1threshold = maxEdge1 + (k / size1);
            int pixel2threshold = maxEdge2 + (k / size2);
            int mint = Math.Min(pixel1threshold, pixel2threshold);
            // menna check han merge wla la
            if (edgeWeight > mint)
                return false;

            // lw han merge 
            mappingToParents[root2] = root1; // now pixel1ID is the root          
            foreach (var key in mappingToParents.Keys)
            {          
                if (mappingToParents[key] == root2)
                {
                    mappingToParents[key] = root1;
                }
            }



            sizeOfSetAndMaxEdge.Remove(root2);
            sizeOfSetAndMaxEdge[root1] = (size1 + size2, Math.Max(Math.Max(maxEdge1, maxEdge2), edgeWeight)); // update size and max edge


            return true;
        }


    }
}
