namespace Metrix.Core.Domain.Metrics;

public abstract class BaseMetric : IMetric
{
  public string? Id { get; set; }

  public string? UserId { get; set; }

  public string Name { get; set; } = null!;

  public string? Description { get; set; }

  public abstract MetricType Type { get; }

  public Dictionary<string, MetricAttribute> Attributes { get; set; } = new();

  public DateTime? LastMeasurementDate { get; set; }
}
