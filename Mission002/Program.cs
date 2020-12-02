using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mission002
{
    class Program
    {
        static void Main(string[] args)
        {
            var  j = JArray.Parse(File.ReadAllText("Output.txt"));
            List<Day> days = new List<Day>();
            foreach(JObject o in j)
            {
                Day d = new Day { date = o.First.First.ToString() };
                //Console.WriteLine(o.First.First.ToString());
                days.Add(d);
            }
            
            // Convert initial input from binary
            //string[] data = _input.Split(" ");
            //byte[] bData = data.Select(d => Convert.ToByte(d,2)).ToArray();
            //string s = "";
            //foreach (byte b in bData) {
            //    var x = Convert.ToChar(b);
            //    //Console.Write(x);
            //    s += x;
            //}
            //File.WriteAllText("Output.txt", s);
        }
    }
}

class Day
{
    public string date;
    List<Reading> readings;
}

class Reading
{
    string id;
    int time;
    List<Contaminant> contaminants;
}

class Contaminant
{
    string id;
    int amount;
}