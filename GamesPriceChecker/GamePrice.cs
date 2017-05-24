using System;

namespace GamesPriceChecker
{
    public class GamePrice
    {
        public int Id { get; set; }
        public string Platform { get; set; }
        public string Name { get; set; }
        public double ActualPrice { get; set; }
        public double OriginalPrice { get; set; }
        public double SubscriptionPrice { get; set; }
        public DateTime OfferFinishesAt { get; set; }
        public double PsPriceRating { get; set; }
        public int PsPriceRatingQty { get; set; }
    }
}
