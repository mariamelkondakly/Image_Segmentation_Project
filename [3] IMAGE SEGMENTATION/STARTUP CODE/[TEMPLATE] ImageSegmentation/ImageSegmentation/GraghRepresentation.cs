using System;
using System.Collections.Generic;
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

        public (List<(int, int, int)> red, List<(int, int, int)> green, List<(int, int, int)> blue) pixels_graph(RGBPixel[,] ImageMatrix)
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
        int findParent()
        {
            //find the parent of the node
            return 0;
        }
        int findInternalDiffernece() //return the biggest edge in the tree,
                                     //check if there is only one pixel in
                                     //the tree, return 0
        {
            //find the internal difference of the node
            return 0;
        }
        bool unionSet()
        {
            //if two nodes are in the same set (have the same parent) don't merge, return false
            //else calculate everything and check
            //if the internal difference is less than the threshold, return false
            //else merge the two sets and return true
            //union the two sets
            return true;
        }


    }
}
