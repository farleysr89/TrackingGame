using System;
using System.IO;

namespace TrackingGame
{
    class Program
    {
        static void Main(string[] args)
        {
            string _input = File.ReadAllText("Input.txt");

            for(int i = 0; i < _input.Length - 16; i++)
            {
                if (uniqueCharacters(_input.Substring(i, 16)))
                {
                    var data = Convert.FromBase64String(_input.Substring(i, 16));
                    var str = System.Text.Encoding.ASCII.GetString(data);
                    Console.WriteLine(str);
                    break;
                }
            }
        }

        static bool uniqueCharacters(String str)
        {
            char[] chArray = str.ToCharArray();

            // Using sorting
            Array.Sort(chArray);

            for (int i = 0; i < chArray.Length - 1; i++)
            {

                // if the adjacent elements are not
                // equal, move to next element
                if (chArray[i] != chArray[i + 1])
                    continue;

                // if at any time, 2 adjacent elements
                // become equal, return false
                else
                    return false;
            }

            return true;
        }
    }
}
