namespace DShop.Services.Discounts.Metrics
{
    public interface IMetricsRegistry
    {
        void IncrementFindDiscountsQuery();
    }
}