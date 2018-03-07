using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using static ProtoBufClientServer.DoubleValues.Types;

namespace ProtoBufServer
{
    class ProtoBufServer
    {      
      HttpListener m_Listener;


      public ProtoBufServer()
      {
         var uriString = "http://*:8081/";
         m_Listener = new HttpListener();
         m_Listener.Prefixes.Add(uriString);
         m_Listener.Start();
         Console.WriteLine($"Listening for traffic on {uriString}");
      }

      ~ProtoBufServer()
      {
         m_Listener.Stop();
         m_Listener.Close();
      }

      public void GetAndPrintNextRequest()
      {
         var context = m_Listener.GetContext();
         System.IO.Stream requestStream = context.Request.InputStream;
         var doubleVals = ProtoBufClientServer.DoubleValues.Parser.ParseFrom(requestStream);
         Console.WriteLine($"received {doubleVals.Values.Count} values from client");
         foreach (var val in doubleVals.Values)
         {
            Console.WriteLine($"Time: {val.Time} Value: {val.Value}");
         }
         var response = context.Response;
         response.StatusCode = (int)HttpStatusCode.OK;
         // Construct a response.
         string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
         byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
         // Get a response stream and write the response to it.
         response.ContentLength64 = buffer.Length;
         System.IO.Stream output = response.OutputStream;
         output.Write(buffer, 0, buffer.Length);
         // You must close the output stream.
         output.Close();
         response.Close();         
      }

      public bool IsAlive()
      {
         return m_Listener.IsListening;
      }



    }
}
