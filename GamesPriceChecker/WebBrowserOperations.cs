using System;
using NLog;

namespace GamesPriceChecker
{
    public class WebBrowserOperations : IWebBrowserOperations
    {
        private readonly ILogger _logger;
        public IWebClientMockable WebClient { get; }

        public WebBrowserOperations(ILogger logger, IWebClientMockable webClient)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (webClient == null) throw new ArgumentNullException(nameof(webClient));

            WebClient = webClient;
            _logger = logger;
        }

        public string GetSourceByUrl(string url)
        {
            _logger.Info($"Go to {url} page");
            return WebClient.DownloadString(url);
        }

    }
}
