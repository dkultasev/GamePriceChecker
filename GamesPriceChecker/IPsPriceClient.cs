using System;
using System.Collections.Generic;

namespace GamesPriceChecker
{
    public interface IPsPriceClient
    {
        List<GamePrice> GetAllGames();
        int GetPageCount(string source);
        List<GamePrice> GetGamesFromPage(string page);
        DateTime GetDateTimeFromString(string dateString);
        double[] GetRatingFromString(string rating);
        double[] GetPricesFromString(string prices);
    }
}