CREATE TABLE [dbo].[GamePrice]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	[Name] NVARCHAR(500) NOT NULL,
    [Platform] VARCHAR(50) NOT NULL, 
    [ActualPrice] DECIMAL(6, 2) NOT NULL, 
    [OriginalPrice] DECIMAL(6, 2) NOT NULL, 
    [SubscriptionPrice] DECIMAL(6, 2) NOT NULL, 
    [OfferFinishesAt] DATETIME2 NOT NULL, 
    [PsPriceRating] DECIMAL(2, 1) NULL, 
    [PsPriceRatingQty] INT NULL,

)

GO

CREATE INDEX [IX_GamePrice_NamePlatform] ON [dbo].[GamePrice] ([Name], [Platform])
