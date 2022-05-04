﻿namespace Metrix.Core.Application.Commands.Measurements.Add.Timer.End;

public class EndTimerMeasurementCommand : ICommand
{
  public string MetricId { get; set; } = null!;

  // what about Notes?

  public ICommandExecutor CreateExecutor()
  {
    return new EndTimerMeasurementCommandExecutor(this);
  }
}
