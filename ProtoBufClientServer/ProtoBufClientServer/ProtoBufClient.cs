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

      public HttpStatusCode SendValues<T>(Dictionary<DateTime, T> valuesWithTimeStamps, string pointName = "Super Awesome Point")
      {
         var message = new ProtoBufClientServer.ValuesForPoint
         {
            PointName = pointName            
         };
         
         switch (Type.GetTypeCode(typeof(T)))
         {
            case TypeCode.Double:
               message.DataType = ProtoBufClientServer.ValuesForPoint.Types.Type.Double;
               break;
            case TypeCode.Int64:
               message.DataType = ProtoBufClientServer.ValuesForPoint.Types.Type.Int;
               break;
            case TypeCode.String:
               message.DataType = ProtoBufClientServer.ValuesForPoint.Types.Type.String;
               break;
            case TypeCode.Boolean:
               message.DataType = ProtoBufClientServer.ValuesForPoint.Types.Type.Bool;
               break;
            case TypeCode.Object:
               if(Type.GetTypeCode(typeof(T).GetElementType()) == TypeCode.Byte)
               {
                  message.DataType = ProtoBufClientServer.ValuesForPoint.Types.Type.Blob;
                  break;
               }
               else
               {
                  throw new ArgumentOutOfRangeException($"Dude....not a valid point type {typeof(T)}");
               }
            default:
               throw new ArgumentOutOfRangeException($"Dude....not a valid point type {typeof(T)}");
         }         


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
