using App.Metrics;
using App.Metrics.Counter;

namespace DShop.Services.Discounts.Metrics
{
    public class MetricsRegistry : IMetricsRegistry
    {
        private readonly IMetricsRoot _metricsRoot;
        private readonly CounterOptions _findDiscountsQueries = new CounterOptions { Name = "find-discounts" };

        public MetricsRegistry(IMetricsRoot metricsRoot)
            => _metricsRoot = metricsRoot;
        
        public void IncrementFindDiscountsQuery()
            => _metricsRoot.Measure.Counter.Increment(_findDiscountsQueries);
    }
}