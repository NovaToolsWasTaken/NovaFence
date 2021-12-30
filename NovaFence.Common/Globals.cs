using NovaFence.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NovaFence.Common
{
    public static class Globals
    {
        public static readonly HttpClient client = new HttpClient(new HttpClientHandler
        {
            UseProxy = false,
            Proxy = null
        });

        public static List<About> AboutList = new List<About>(); 
    }
}
