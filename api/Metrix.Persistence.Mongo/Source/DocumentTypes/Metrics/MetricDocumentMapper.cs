using AutoMapper;
using Metrix.Core.Domain.Metrics;

namespace Metrix.Persistence.Mongo.DocumentTypes.Metrics;

public static class MetricDocumentMapper
{
  private static readonly IMapper Mapper;

  static MetricDocumentMapper()
  {
    var configuration = new MapperConfiguration(
      cfg =>
      {
        cfg.CreateMap<IMetric, MetricDocument>()
          .Include<CounterMetric, CounterMetricDocument>()
          .Include<GaugeMetric, GaugeMetricDocument>()
          .Include<TimerMetric, TimerMetricDocument>();

        cfg.CreateMap<CounterMetric, CounterMetricDocument>();
        cfg.CreateMap<GaugeMetric, GaugeMetricDocument>();
        cfg.CreateMap<TimerMetric, TimerMetricDocument>();

        cfg.CreateMap<MetricDocument, IMetric>()
          .Include<CounterMetricDocument, CounterMetric>()
          .Include<GaugeMetricDocument, GaugeMetric>()
          .Include<TimerMetricDocument, TimerMetric>();

        cfg.CreateMap<CounterMetricDocument, CounterMetric>();
        cfg.CreateMap<GaugeMetricDocument, GaugeMetric>();
        cfg.CreateMap<TimerMetricDocument, TimerMetric>();
      }
    );

    // configuration.AssertConfigurationIsValid();

    Mapper = configuration.CreateMapper();
  }

  public static MetricDocument ToDocument(IMetric metric)
  {
    return Mapper.Map<MetricDocument>(metric);
  }

  public static TMetric FromDocument<TMetric>(MetricDocument? document) where TMetric : class, IMetric
  {
    return document == null
      ? null!
      : (TMetric)Mapper.Map(document, document.GetType(), typeof(TMetric));
  }
}
