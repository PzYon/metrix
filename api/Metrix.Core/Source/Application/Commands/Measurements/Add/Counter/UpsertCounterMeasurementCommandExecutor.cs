﻿using Metrix.Core.Domain.Measurements;
using Metrix.Core.Domain.Metrics;

namespace Metrix.Core.Application.Commands.Measurements.Add.Counter;

public class UpsertCounterMeasurementCommandExecutor : BaseUpsertMeasurementCommandExecutor<
  UpsertCounterMeasurementCommand,
  CounterMeasurement,
  CounterMetric
>
{
  public UpsertCounterMeasurementCommandExecutor(UpsertCounterMeasurementCommand command) : base(command) { }

  protected override CounterMeasurement CreateMeasurement(IDateService dateService)
  {
    return new CounterMeasurement();
  }
}