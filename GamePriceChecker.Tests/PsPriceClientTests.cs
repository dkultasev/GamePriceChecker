using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using FluentAssertions;
using GamesPriceChecker;
using Moq;
using NUnit.Framework;
using NLog;

namespace GamePriceChecker.Tests
{
    public class PsPriceClientTests
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
        public void Test_GetRatingFromString()
        {
            var res = @"


                4.43
                6023


        ";
            var result = _client.GetRatingFromString(res);

            result[0].Should().Be(4.43);
            result[1].Should().Be(6023);
        }

        [Test]
        public void Test_GetRatingFromGameWithoutRatingString()
        {
            var res = "asd";
            var result = _client.GetRatingFromString(res);

            result[0].Should().Be(0);
            result[1].Should().Be(0);
        }

        [Test]
        public void Test_Get2PricesFromString()
        {
            var res = @"
            
                UAH499 &rarr;
            
            
                UAH219
                
            
        ";

            var result = _client.GetPricesFromString(res);
            result[0].Should().Be(499);
            result[1].Should().Be(219);

        }

        [Test]
        public void Test_Get3PricesFromString()
        {
            var res = @"
            
                UAH749 &rarr;
            
            
                UAH459
                
                    &nbsp; UAH309.20
                
            
        ";

            var result = _client.GetPricesFromString(res);
            result[0].Should().Be(749);
            result[1].Should().Be(459);
            result[2].Should().Be(309.2);

        }

        [Test]
        public void Test_GetPricesWithoutDiscountButWithSubscriptionFromString()
        {
            var res = @"
            
            
                UAH249
                
                    &nbsp; UAH199.20
                
            
        ";

            var result = _client.GetPricesFromString(res);
            result[0].Should().Be(249);
            result[1].Should().Be(249);
            result[2].Should().Be(199.20);

        }

        [Test]
        public void Test_GetPricesWithFreePriceFromString()
        {
            var res = @"


            
                UAH142 &rarr;
            
            
                Бесплатно
                
            
";

            var result = _client.GetPricesFromString(res);
            result[0].Should().Be(142);
            result[1].Should().Be(142);
            result[2].Should().Be(0);

        }

        [Test]
        public void Test_GetDateTimeFromString()
        {
            var text = "закончится 2017-06-09T22:59:00Z";
            var result = _client.GetDateTimeFromString(text);

            result.Should().Be(new DateTime(2017, 6, 9, 22, 59, 0));

        }

        [Test]
        public void Test_GetPagesCount()
        {
            _client.GetPageCount(HelpClass.GetPageSource()).Should().Be(17);
        }

        [Test]
        public void test()
        {
            _client.GetAllGames();
        }
    }
}
