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

            foreach(Region r in regions)
            {
                r.highestDiff();               
            }
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
            int s = r.maxWater();
            if (first)
            {
                prev = s;
                first = false;
            }
            else
            {
                int curr = s;
                if (Math.Abs(curr - prev) > max) max = Math.Abs(curr - prev);
                if (Math.Abs(curr - prev) > 1000) Console.WriteLine("regionID = " + regionID + " date = " + r.date + " readingID = " + r.readingID);
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

    // Function from https://www.geeksforgeeks.org/trapping-rain-water/
    // Gets different answer from above, something to look into
    public int maxWater()
    {
        int[] arr = reading;
        int n = arr.Length;

        // To store the maximum water
        // that can be stored
        int res = 0;

        // For every element of the array
        // except first and last element
        for (int i = 1; i < n - 1; i++)
        {

            // Find maximum element on its left
            int left = arr[i];
            for (int j = 0; j < i; j++)
            {
                left = Math.Max(left, arr[j]);
            }

            // Find maximum element on its right
            int right = arr[i];
            for (int j = i + 1; j < n; j++)
            {
                right = Math.Max(right, arr[j]);
            }

            // Update maximum water value
            res += Math.Min(left, right) - arr[i];
        }
        return res;
    }
}