using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GamesPriceChecker
{
    public class PsPriceClient
    {
        private readonly IWebBrowserOperations _browserOperations;
        private const string BaseUrl = "https://psprices.com/region-ua/discounts?sort=date&platform=PS4";

        public PsPriceClient(IWebBrowserOperations browserOperations)
        {
            if (browserOperations == null) throw new ArgumentNullException(nameof(browserOperations));
            _browserOperations = browserOperations;
        }

        public List<Game> GetAllGames()
        {
            var gamesList = new List<Game>();
            var page = _browserOperations.GetSourceByUrl(GetUrlByPage(1));
            int pagesCount = GetPageCount(page);
            for (var i = 0; i < pagesCount; i++)
            {
                try
                {
                    var gamesFromPage = GetGamesFromPage(_browserOperations.GetSourceByUrl(GetUrlByPage(i + 1)));
                    gamesList.AddRange(gamesFromPage);
                    //gamesList.AddRange(GetGamesFromPage(_browserOperations.GetSourceByUrl(GetUrlByPage(17))));
                }
                catch (Exception e)
                {
                    int a = 0;
                }
            }
            return gamesList;
        }

        public int GetPageCount(string source)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(source);

            var test = doc.DocumentNode.SelectNodes("//*[@id=\"pjax-container\"]/div[3]/div/div[4]/ul/li[*]");

            return int.Parse(test.Last().InnerText);
        }

        public List<Game> GetGamesFromPage(string page)
        {
            List<Game> games = new List<Game>();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(page);
            var test2 = doc.DocumentNode.SelectNodes("//*[@id=\"pjax-container\"]/div[2]/div/div[*]/div");
            foreach (var game in test2)
            {
                if (game.ChildNodes.Count == 7) continue;

                var i = 0;
                var newGame = new Game() {Platform = "PS4"};
                foreach (var gameChildNode in game.ChildNodes)
                {
                    var temp = gameChildNode.InnerText;
                    if (i == 3)
                    {
                        newGame.Name = temp;
                    }

                    if (i == 5)
                    {
                        var rate = GetRatingFromString(temp);
                        newGame.PsPriceRating = rate[0];
                        newGame.PsPriceRatingQty = (int)rate[1];

                        if ((int)rate[1] == 0) i+=2;
                    }

                    if (i == 11)
                    {
                        if (!temp.Contains(newGame.Platform))
                        {
                            newGame.OfferFinishesAt = GetDateTimeFromString(temp);
                        }
                        else
                        {
                            i += 2;
                        }
                    }

                    if (i == 15)
                    {
                        var prices = GetPricesFromString(temp);
                        newGame.ActualPrice = prices[1];
                        newGame.OriginalPrice = prices[0];
                        if (prices.Length == 3)
                        {
                            newGame.SubscriptionPrice = prices[2];
                        }
                    }

                    i++;
                }
                games.Add(newGame);
                
            }
            return games;
        }

        public DateTime GetDateTimeFromString(string dateString)
        {
            var temp = dateString.Replace("закончится ","" );

            return Convert.ToDateTime(temp).ToUniversalTime();
        }

        public double[] GetRatingFromString(string rating)
        {
            var temp = rating.Trim().Split();
            try
            {
                return new double[2] {double.Parse(temp.First()), double.Parse(temp.Last())};
            }
            catch
            {
                return new double[2];
            }
        }

        private string GetUrlByPage(int page)
        {
            return $"{BaseUrl}&page={page}";
        }

        public double[] GetPricesFromString(string prices)
        {
            string[] temp;
            var result = new double[3];

            if (prices.Contains("nbsp") || prices.Contains("Бесплатно"))
            {
                double a = 0;
                temp = prices.Trim().Replace("Бесплатно","0").Replace("UAH", "").Replace(" &rarr;", "").Replace("&nbsp; ", "").Split();
                var newList = temp.Where(x => double.TryParse(x, out a)).ToList();

                result[0] = double.Parse(newList[0]);
                result[1] = prices.Contains("rarr") && !prices.Contains("Бесплатно") ? double.Parse(newList[1]) : double.Parse(newList[0]);
                result[2] = double.Parse(newList.Last());
            }
            else
            {
                temp = prices.Trim().Replace("UAH", "").Replace(" &rarr;", "").Split();
                result[0] = double.Parse(temp.First());
                result[1] = double.Parse(temp.Last());
                result[2] = double.Parse(temp.Last());
            }

            return result;
        }
    }
}
