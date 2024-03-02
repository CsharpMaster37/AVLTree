using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using CustomAVLTree;

namespace RealizeAVLTree
{
    internal class Program
    {
        public static void Shuffle(int[] arr)
        {
            Random rand = new Random();
            for (int i = arr.Length - 1; i >= 1; i--)
            {
                int j = rand.Next(i + 1);

                int tmp = arr[j];
                arr[j] = arr[i];
                arr[i] = tmp;
            }
        }
        static void Main(string[] args)
        {
            StreamReader sr = new StreamReader("input.txt");
            //int[] digits = new int[10000];
            //for (int i = 0; i < 10000; i++)
            //    digits[i] = i;
            //Shuffle(digits);
            //Stopwatch stopwatch = new Stopwatch();

            //stopwatch.Start();
            //AVLTree<int, int> AVL = new AVLTree<int, int>();
            //for (int i = 0; i < 10000; i++)
            //    AVL.Add(digits[i], 1);
            //for (int i = 5000; i < 7000; i++)
            //    AVL.Delete(digits[i]);
            //for (int i = 0; i < 10000; i++)
            //    AVL.Find(digits[i]);
            //stopwatch.Stop();
            //Console.WriteLine($"Время AVL: {stopwatch.ElapsedMilliseconds}");

            //stopwatch.Restart();
            //SortedDictionary<int, int> sortDict = new SortedDictionary<int, int>();
            //for (int i = 0; i < 10000; i++)
            //    sortDict.Add(digits[i], 1);
            //for (int i = 5000; i < 7000; i++)
            //    sortDict.Remove(digits[i]);
            //for (int i = 0; i < 10000; i++)
            //    sortDict.ContainsKey(digits[i]);
            //stopwatch.Stop();
            //Console.WriteLine($"Время SortedDictionary: {stopwatch.ElapsedMilliseconds}");

            AVLTree<int, int> AVL = new AVLTree<int, int>();
            AVL.Add(30, 1);
            AVL.Add(25, 1);
            AVL.Add(35, 1);
            AVL.Add(33, 1);
            AVL.Add(36, 1);
            AVL.Add(37, 1);
            Console.WriteLine();
        }
    }
}
