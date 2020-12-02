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
            foreach( JObject o in j["regions"])
            {
                Region region = new Region { regionID = (string)o["regionID"] };
                List<Reading> readings = new List<Reading>();
                foreach(JObject r in o["readings"])
                {
                    Reading reading = new Reading { date = (string)r["date"], readingID = (string)r["readingID"], reading = (r["reading"] as JArray).Select(a => (int)a).ToArray()};
                    readings.Add(reading);
                    //Console.WriteLine(reading.findWater());
                }
                region.readings = readings;
                regions.Add(region);
            }

            int max = 0;
            Region maxR = null;
            foreach(Region r in regions)
            {
                if(r.highestDiff() > max)
                {
                    max = r.highestDiff();
                    maxR = r;
                }
            }
            Console.WriteLine("Max = " + max + " RegionID = " + maxR.regionID);
            // regionID is 9SECC9, needs to be converted?

        }
    }
}

class Region
{
    public string regionID;
    public List<Reading> readings;

    public int highestDiff()
    {
        int max = 0;
        int prev = 0;
        bool first = true;
        foreach(Reading r in readings)
        {
            if (first)
            {
                prev = r.findWater();
                first = false;
            }
            else
            {
                int curr = r.findWater();
                if (Math.Abs(curr - prev) > max) max = Math.Abs(curr - prev);
                prev = curr;
            }
        }

        return max;
    }
}

class Reading
{
    public string readingID;
    public int[] reading;
    public string date;

    // Function from https://www.geeksforgeeks.org/trapping-rain-water/
    public int findWater()
    {
        int[] arr = reading;
        int n = arr.Length;
        // initialize output
        int result = 0;

        // maximum element on left and right
        int left_max = 0, right_max = 0;

        // indices to traverse the array
        int lo = 0, hi = n - 1;

        while (lo <= hi)
        {
            if (arr[lo] < arr[hi])
            {
                if (arr[lo] > left_max)

                    // update max in left
                    left_max = arr[lo];
                else

                    // water on curr element =
                    // max - curr
                    result += left_max - arr[lo];
                lo++;
            }
            else
            {
                if (arr[hi] > right_max)

                    // update right maximum
                    right_max = arr[hi];

                else
                    result += right_max - arr[hi];
                hi--;
            }
        }

        return result;
    }
}