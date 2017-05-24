namespace GamesPriceChecker
{
    public interface IWebBrowserOperations
    {
        string GetSourceByUrl(string url);
        IWebClientMockable WebClient { get; }
    }
}