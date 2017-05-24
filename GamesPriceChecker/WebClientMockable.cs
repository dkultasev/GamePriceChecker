using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GamesPriceChecker
{
    public class WebClientMockable : IWebClientMockable
    {
        private readonly WebClient _client;

        public WebClientMockable(WebClient client)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            client.Encoding = Encoding.UTF8;
            _client = client;
        }

        public string DownloadString(string address)
        {
            return _client.DownloadString(address);
        }
        public string DownloadString(Uri address)
        {
            return _client.DownloadString(address);
        }


    }
}
