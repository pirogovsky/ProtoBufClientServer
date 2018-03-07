using System;
using System.Collections.Generic;

namespace ProtoBufClient
{
    class Program
    {
        static void Main(string[] args)
        {
         var client = new ProtoBufClient();
         var values = new Dictionary<DateTime, Double>();
         
         for(var i = 0; i < 10; ++i)
         {
            var now = DateTime.Now;
            values.Add(now.AddMinutes(i), (double)i);
         }
         Console.WriteLine($"Sending {values.Count} rows of data.");
         var result = client.SendValues(values);
         Console.WriteLine($"Received status of : {result}");
        }
    }
}
