using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mission003
{
    class Program
    {
        static void Main(string[] args)
        {
            var j = JObject.Parse(File.ReadAllText("Input.txt"));
            /*
             * Format:
             *  regions
             *      regionID
             *      readings[26] A-Z
             *          readingID
             *          reading[200]
             *          date
             */
            List<Region> regions = new List<Region>();
            foreach( JObject o in j.GetValue("regions"))
            {
                Region region = new Region { regionID = (string)o.GetValue("regionID") };
                List<Reading> readings = new List<Reading>();
                foreach(JObject r in o.GetValue("readings"))
                {
                    Reading reading = new Reading { date = (string)r.GetValue("date"), readingID = (string)r.GetValue("readingID"), reading = (r.GetValue("reading") as JArray).Select(a => (int)a).ToArray()};
                    readings.Add(reading);
                }
                region.readings = readings;
                regions.Add(region);
            }
            Console.WriteLine("Hello World!");
        }
    }
}

class Region
{
    public string regionID;
    public List<Reading> readings;
}

class Reading
{
    public string readingID;
    public int[] reading;
    public string date;
}