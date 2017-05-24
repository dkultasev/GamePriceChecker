using System.Net;
using GamesPriceChecker;
using Moq;
using NLog;
using NUnit.Framework;

namespace GamePriceChecker.Tests
{
    public class DatabaseClientTests
    {
        Mock<ILogger> _logger;
        Mock<IWebBrowserOperations> _browserOperationMock;
        private WebClientMockable _browser;
        WebBrowserOperations _operations;
        PsPriceClient _client;

        [SetUp]
        public void Init()
        {
            _logger = new Mock<ILogger>();
            _browser = new WebClientMockable(new WebClient());
            _operations = new WebBrowserOperations(_logger.Object, _browser);
            _client = new PsPriceClient(_operations);
            _browserOperationMock = new Mock<IWebBrowserOperations>();
        }

        [Test]
        public void test()
        {
            var games = _client.GetAllGames();
            new DatabaseClient().UpsertRecordsFromPsPrice(games);
        }
    }
}
