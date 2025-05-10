using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTemplate
{
    public class GraghRepresentation
    {
        //tuple(int weight,pair<int,int> src, pair<int,int> dest);
        // int v;
        List<Dictionary<(int, int), int>> blue_adj_list = new List<Dictionary<(int, int), int>>();
        List<Dictionary<(int, int), int>> green_adj_list = new List<Dictionary<(int, int), int>>();
        List<Dictionary<(int, int), int>> red_adj_list = new List<Dictionary<(int, int), int>>();

        public (List<Dictionary<(int, int), int>> red, List<Dictionary<(int, int), int>> green, List<Dictionary<(int, int), int>> blue) pixels_graph(RGBPixel[,] ImageMatrix)
        {
            red_adj_list.Clear();
            green_adj_list.Clear();
            blue_adj_list.Clear();

            int red_weight;
            int green_weight;
            int blue_weight;
            // List<List<RGBPixel>> adj_list = new List<List<RGBPixel>>();
            int length = ImageMatrix.GetLength(0);
            int width = ImageMatrix.GetLength(1);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Dictionary<(int, int),int> blue_neighbors = new Dictionary<(int, int),int>();
                    Dictionary<(int, int),int> green_neighbors = new Dictionary<(int, int),int>();
                    Dictionary<(int, int),int> red_neighbors= new Dictionary<(int, int),int>();
 
                    //up
                    if (i > 0 )
                    {
                         red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i - 1, j].red);
                        green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i - 1, j].green);
                        blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i - 1, j].blue);
                        red_neighbors.Add((i - 1,j), red_weight);
                        green_neighbors.Add((i - 1, j), green_weight);
                        blue_neighbors.Add((i-1, j), blue_weight);
                        //blue_adj_list.push({blue_weight, (i, j), (i + 1, j)});
                    }
                    //down
                    if (i < length - 1)
                    {
                        red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j].red);
                        green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j].green);
                        blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j].blue);
                        red_neighbors.Add((i + 1, j), red_weight);
                        green_neighbors.Add((i + 1, j), green_weight);
                        blue_neighbors.Add((i + 1, j), blue_weight);
                    }
                    //left
                    if (j > 0)
                    {
                        red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i, j - 1].red);
                        green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i, j - 1].green);
                        blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i, j - 1].blue);
                        red_neighbors.Add((i, j - 1), red_weight);
                        green_neighbors.Add((i, j - 1), green_weight);
                        blue_neighbors.Add((i, j - 1), blue_weight);
                    }
                    //right
                    if (j < width - 1)
                    {
                        red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i, j + 1].red);
                        green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i, j + 1].green);
                        blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i, j + 1].blue);
                        red_neighbors.Add((i, j + 1), red_weight);
                        green_neighbors.Add((i, j + 1), green_weight);
                        blue_neighbors.Add((i, j + 1), blue_weight);
                    }
                    //up left
                    if (i > 0 && j > 0)
                    {
                        red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i - 1, j - 1].red);
                        green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i - 1, j - 1].green);
                        blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i - 1, j - 1].blue);
                        red_neighbors.Add((i - 1, j - 1), red_weight);
                        green_neighbors.Add((i - 1, j - 1), green_weight);
                        blue_neighbors.Add((i - 1, j - 1), blue_weight);
                    }
                    //up right
                    if (i > 0 && j < width - 1)
                    {
                        red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i - 1, j + 1].red);
                        green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i - 1, j + 1].green);
                        blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i - 1, j + 1].blue);
                        red_neighbors.Add((i - 1, j + 1), red_weight);
                        green_neighbors.Add((i - 1, j + 1), green_weight);
                        blue_neighbors.Add((i - 1, j + 1), blue_weight);
                    }
                    //down left
                    if (i < length - 1 && j > 0)
                    {
                        red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j - 1].red);
                        green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j - 1].green);
                        blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j - 1].blue);
                        red_neighbors.Add((i + 1, j - 1), red_weight);
                        green_neighbors.Add((i + 1, j - 1), green_weight);
                        blue_neighbors.Add((i + 1, j - 1), blue_weight);
                    }
                    //down right
                    if (i < length - 1 && j < width - 1)
                    {
                        red_weight = Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j + 1].red);
                        green_weight = Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j + 1].green);
                        blue_weight = Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j + 1].blue);
                        red_neighbors.Add((i + 1, j + 1), red_weight);
                        green_neighbors.Add((i + 1, j + 1), green_weight);
                        blue_neighbors.Add((i + 1, j + 1), blue_weight);
                    }
                    red_adj_list.Add(red_neighbors);
                    green_adj_list.Add(green_neighbors);
                    blue_adj_list.Add(blue_neighbors);

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
