using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace GamesPriceChecker
{
    public class DatabaseClient
    {
        private string _connection = "Data Source=localhost;initial catalog=GamesPriceChecker;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=True";

        private void UpsertSingleRecordFromPsPrice(GamePrice game)
        {
            using (var objconnection = new SqlConnection(_connection))
            {
                objconnection.Open();
                using (var objcmd = new SqlCommand("dbo.p_UpsertGamePrice", objconnection))
                {
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.Parameters.Add("@offerFinishesAt", SqlDbType.DateTime2, 8);
                    objcmd.Parameters["@offerFinishesAt"].Value = game.OfferFinishesAt;
                    objcmd.Parameters.Add("@psPriceRating", SqlDbType.Decimal, 5);
                    objcmd.Parameters["@psPriceRating"].Value = game.PsPriceRating;
                    objcmd.Parameters.Add("@actualPrice", SqlDbType.Decimal, 5);
                    objcmd.Parameters["@actualPrice"].Value = game.ActualPrice;
                    objcmd.Parameters.Add("@originalPrice", SqlDbType.Decimal, 5);
                    objcmd.Parameters["@originalPrice"].Value = game.OriginalPrice;
                    objcmd.Parameters.Add("@subscriptionPrice", SqlDbType.Decimal, 5);
                    objcmd.Parameters["@subscriptionPrice"].Value = game.SubscriptionPrice;
                    objcmd.Parameters.Add("@psPriceRatingQty", SqlDbType.Int, 4);
                    objcmd.Parameters["@psPriceRatingQty"].Value = game.PsPriceRatingQty;
                    objcmd.Parameters.Add("@name", SqlDbType.NVarChar, 1000);
                    objcmd.Parameters["@name"].Value = game.Name;
                    objcmd.Parameters.Add("@platform", SqlDbType.VarChar, 50);
                    objcmd.Parameters["@platform"].Value = game.Platform;
                    objcmd.ExecuteReader();
                }
            }
        }

        public void UpsertRecordsFromPsPrice(List<GamePrice> games)
        {
            foreach (var game in games)
            {
                UpsertSingleRecordFromPsPrice(game);
            }
        }
    }
}
