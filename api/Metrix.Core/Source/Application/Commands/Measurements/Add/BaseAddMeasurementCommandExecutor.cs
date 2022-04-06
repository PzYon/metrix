using Metrix.Core.Application.Commands.Metrics;
using Metrix.Core.Application.Persistence;
using Metrix.Core.Domain.Measurements;
using Metrix.Core.Domain.Metrics;

namespace Metrix.Core.Application.Commands.Measurements.Add;

public abstract class BaseAddMeasurementCommandExecutor<TCommand, TMeasurement> : ICommandExecutor
  where TCommand : BaseAddMeasurementCommand
  where TMeasurement : IMeasurement
{
  protected TCommand Command { get; }

  protected abstract TMeasurement CreateMeasurement();

  protected virtual void PerformAdditionalValidation(IDb db, Metric metric) { }

  protected BaseAddMeasurementCommandExecutor(TCommand command)
  {
    Command = command;
  }

  public void Execute(IDb db)
  {
    Metric metric = MetricUtil.LoadAndValidateMetric(db, Command, Command.MetricKey);

    EnsureCompatibleMetricType(metric);
    ValidateMetricFlag(metric);
    
    PerformAdditionalValidation(db, metric);

    IMeasurement measurement = CreateMeasurement();
    measurement.MetricKey = Command.MetricKey;
    measurement.Notes = Command.Notes;
    measurement.DateTime = DateTime.UtcNow;
    measurement.MetricFlagKey = Command.MetricFlagKey;

    db.Measurements.Add(measurement);
  }

  private void EnsureCompatibleMetricType(Metric metric)
  {
    if (metric.Type != Command.GetSupportedMetricType())
    {
      throw CreateInvalidCommandException(
        $"Command with metric type \"{Command.GetSupportedMetricType()}\" is not compatible with metric of type \"{metric.Type}\"."
      );
    }
  }

  private void ValidateMetricFlag(Metric metric)
  {
    if (!string.IsNullOrEmpty(Command.MetricFlagKey) && !metric.Flags.ContainsKey(Command.MetricFlagKey))
    {
      throw CreateInvalidCommandException($"Flag \"{Command.MetricFlagKey}\" does not exist on metric.");
    }
  }

  protected InvalidCommandException CreateInvalidCommandException(string message)
  {
    return new InvalidCommandException(Command, message);
  }
}
