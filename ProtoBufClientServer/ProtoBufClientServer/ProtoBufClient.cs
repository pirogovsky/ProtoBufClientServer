using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net;
using static ProtoBufClientServer.DoubleValues.Types;
using System.Linq;
using Google.Protobuf;

namespace ProtoBufClient
{
    class ProtoBufClient
    {
      HttpClient m_client;

      public ProtoBufClient()
      {
         m_client = new HttpClient();
         var uriString = "http://localhost:8081/";
         var uri = new Uri(uriString);
         m_client.BaseAddress = uri;
      }

      public HttpStatusCode SendValues(Dictionary<DateTime, Double> valuesWithTimeStamps)
      {         
         var doubleValues = new ProtoBufClientServer.DoubleValues
         {
            Name = "Super Awesome Point"            
         };
         doubleValues.Values.AddRange(valuesWithTimeStamps.Select(p => new valueWithTimestamp { Time = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(p.Key.ToUniversalTime()), Value = p.Value }));
         //Serializing now
         var outputStream = new System.IO.MemoryStream();
         doubleValues.WriteTo(outputStream);
         var request = new HttpRequestMessage(HttpMethod.Post, "");
         outputStream.Position = 0;
         request.Content = new StreamContent(outputStream);                 
         var sendTask = m_client.SendAsync(request);
         sendTask.Wait();
         var response = sendTask.Result;
         return response.StatusCode;
         
      }

    }
}
