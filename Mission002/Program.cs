using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace Mission002
{
    class Program
    {
        static void Main(string[] args)
        {
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


            var j = JArray.Parse(File.ReadAllText("Output.txt"));
            List<Day> days = new List<Day>();
            foreach(JObject o in j)
            {
                Day day = new Day { date = (string)o["date"] };
                var readings = o["readings"];
                List<Reading> readings1 = new List<Reading>();
                foreach(JObject r in readings)
                {
                    Reading reading = new Reading { id = (string)r["id"], time = (int)r["time"] };
                    var contam = r["contaminants"];
                    List<Contaminant> contams = new List<Contaminant>();
                    foreach(JProperty c in contam)
                    {
                        Contaminant contaminant = new Contaminant { id = c.Name, amount = (int)c.Value };
                        contams.Add(contaminant);
                    }
                    reading.contaminants = contams;
                    readings1.Add(reading);
                }
                day.readings = readings1;
                days.Add(day);
            }

            double max = 0;
            Day maxD = null;
            foreach(var d in days)
            {
                if (d.StdDev() > max)
                {
                    max = d.StdDev();
                    maxD = d;
                }
            }
            max = 0;
            Reading maxR = null;
            foreach(var r in maxD.readings)
            {
                if(r.Sum() > max)
                {
                    max = r.Sum();
                    maxR = r;
                }
            }
            Console.WriteLine(ConvertHex(maxR.id));
        }

        // Method from StackOverflow: https://stackoverflow.com/questions/5613279/c-sharp-hex-to-ascii
        public static string ConvertHex(string hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    string hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = Convert.ToUInt32(hs, 16);
                    char character = Convert.ToChar(decval);
                    ascii += character;

                }

                return ascii;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return string.Empty;
        }
    }

}

class Day
{
    public string date;
    public List<Reading> readings;

    public double Mean()
    {
        int sum = 0;
        foreach(var r in readings)
        {
            sum += r.Sum();
        }
        return sum / 24;
    }

    public double StdDev()
    {
        double m = Mean();
        double sum = 0;
        foreach(var r in readings)
        {
            sum += Math.Pow(r.Sum() - m,2);
        }
        return Math.Sqrt(sum / 24);
    }
}

class Reading
{
    public string id;
    public int time;
    public List<Contaminant> contaminants;

    public int Sum()
    {
        int sum = 0;
        foreach(var c in contaminants)
        {
            sum += c.amount;
        }
        return sum;
    }
}

class Contaminant
{
    public string id;
    public int amount;
}
