CREATE PROCEDURE [dbo].[p_UpsertGamePrice]
    @name NVARCHAR(500) = NULL,
    @platform VARCHAR(50) = NULL,
    @actualPrice DECIMAL(6, 2) = NULL,
    @originalPrice DECIMAL(6, 2) = NULL,
    @subscriptionPrice DECIMAL(6, 2) = NULL,
    @offerFinishesAt DATETIME2(7) = NULL,
    @psPriceRating DECIMAL(2, 1) = NULL,
    @psPriceRatingQty INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT *
        FROM dbo.GamePrice
        WHERE Platform = @platform
              AND Name = @name
    )
    BEGIN
        INSERT INTO dbo.GamePrice
        (
            Name,
            Platform,
            ActualPrice,
            OriginalPrice,
            SubscriptionPrice,
            OfferFinishesAt,
            PsPriceRating,
            PsPriceRatingQty
        )
        SELECT @name,
            @platform,
            @actualPrice,
            @originalPrice,
            @subscriptionPrice,
            @offerFinishesAt,
            @psPriceRating,
            @psPriceRatingQty;

    END;

    ELSE
    BEGIN

        UPDATE dbo.GamePrice
        SET ActualPrice = @actualPrice,
            OriginalPrice = @originalPrice,
            SubscriptionPrice = @subscriptionPrice,
            OfferFinishesAt = @offerFinishesAt,
            PsPriceRating = @psPriceRating,
            PsPriceRatingQty = @psPriceRatingQty
        WHERE Platform = @platform
              AND Name = @name
              AND
              (
                  COALESCE(ActualPrice, '-1') <> COALESCE(@actualPrice, '-1')
                  OR COALESCE(OriginalPrice, '-1') <> COALESCE(@originalPrice, '-1')
                  OR COALESCE(SubscriptionPrice, '-1') <> COALESCE(@subscriptionPrice, '-1')
                  OR COALESCE(OfferFinishesAt, '1900.01.02') <> COALESCE(@offerFinishesAt, '1900.01.02')
                  OR COALESCE(PsPriceRating, '-1') <> COALESCE(@psPriceRating, '-1')
                  OR COALESCE(PsPriceRatingQty, '-1') <> COALESCE(@psPriceRatingQty, '-1')
              );
    END;
END;
