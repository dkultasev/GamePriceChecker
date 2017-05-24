using System;

namespace GamesPriceChecker
{
    public interface IWebClientMockable
    {
        string DownloadString(string address);
        string DownloadString(Uri address);
    }
}