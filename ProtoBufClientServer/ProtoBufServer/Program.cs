using System;

namespace ProtoBufServer
{
    class Program
    {
      static void Main(string[] args)
      {
         var server = new ProtoBufServer();
         while (server.IsAlive())
         {
            server.GetAndPrintNextRequest();
         }
      }
    }
}
