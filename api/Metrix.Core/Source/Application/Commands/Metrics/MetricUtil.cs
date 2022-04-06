﻿using Metrix.Core.Application.Commands.Measurements.Add;
using Metrix.Core.Application.Persistence;
using Metrix.Core.Domain.Metrics;

namespace Metrix.Core.Application.Commands.Metrics;

public static class MetricUtil
{
  public static Metric LoadAndValidateMetric(IDb db, ICommand command, string metricKey)
  {
    if (string.IsNullOrEmpty(metricKey))
    {
      throw new InvalidCommandException(command, $"A {nameof(BaseAddMeasurementCommand.MetricKey)} must be specified.");
    }

    Metric? metric = db.Metrics.FirstOrDefault(m => m.Key == metricKey);

    if (metric == null)
    {
      throw new InvalidCommandException(command, $"A metric with key \"{metricKey}\" does not exist.");
    }

    return metric;
  }
}
