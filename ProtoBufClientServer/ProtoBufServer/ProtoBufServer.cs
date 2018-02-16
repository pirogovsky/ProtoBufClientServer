using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;

namespace ProtoBufServer
{
    class ProtoBufServer
    {
      Uri m_uri;
      HttpClient m_httpClient;


      public ProtoBufServer()
      {
         var uriBuilder = new UriBuilder("http", "127.0.0.1", 81);
         m_uri = uriBuilder.Uri;

      }

      ~ProtoBufServer()
      {

      }

    }
}
