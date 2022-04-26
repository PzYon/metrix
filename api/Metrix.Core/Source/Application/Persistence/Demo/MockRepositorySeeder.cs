﻿using System.Text;
using Metrix.Core.Application.Commands.Measurements.Add;
using Metrix.Core.Application.Commands.Measurements.Add.Counter;
using Metrix.Core.Application.Commands.Measurements.Add.Gauge;
using Metrix.Core.Application.Commands.Measurements.Add.Timer.End;
using Metrix.Core.Application.Commands.Measurements.Add.Timer.Start;
using Metrix.Core.Application.Commands.Metrics.Add;
using Metrix.Core.Application.Commands.Metrics.Edit;
using Metrix.Core.Domain.Measurements;
using Metrix.Core.Domain.Metrics;

namespace Metrix.Core.Application.Persistence.Demo;

public class MockRepositorySeeder
{
  private readonly IRepository _repository;

  public MockRepositorySeeder(IRepository repository)
  {
    _repository = repository;
  }

  public async Task Seed()
  {
    await CreateRandomMetricsAndMeasurements();
    AddSpecificCases();
  }

  private async Task CreateRandomMetricsAndMeasurements()
  {
    foreach (int metricIndex in Enumerable.Range(0, Random.Shared.Next(5, 30)))
    {
      string metricKey = "key" + metricIndex;
      var command = new AddMetricCommand
      {
        Key = metricKey,
        Description = LoremIpsum(0, 12, 1, 3),
        Name = LoremIpsum(1, 3, 1, 1),
        Type = GetRandomMetricType()
      };

      var dateService = new SelfIncrementingDateService();

      await command.CreateExecutor().Execute(_repository, dateService);

      IMetric? metric = await _repository.GetMetric(metricKey);

      await AddMeasurements(metric!, dateService);
    }
  }

  private async Task AddMeasurements(IMetric metric, IDateService dateService)
  {
    switch (metric)
    {
      case CounterMetric counterMetric:
        await AddMeasurements(counterMetric, dateService);
        break;
      case GaugeMetric gaugeMetric:
        await AddMeasurements(gaugeMetric, dateService);
        break;
      case TimerMetric timerMetric:
        await AddMeasurements(timerMetric, dateService.UtcNow);
        break;
      default:
        throw new Exception($"Metric type \"{metric.Type}\" is not yet supported.");
    }
  }

  private async Task AddMeasurements(CounterMetric metric, IDateService dateService)
  {
    foreach (int _ in Enumerable.Range(0, Random.Shared.Next(0, 30)))
    {
      var command = new AddCounterMeasurementCommand
      {
        MetricKey = metric.Key
      };

      await new AddCounterMeasurementCommandExecutor(command).Execute(_repository, dateService);
    }
  }

  private async Task AddMeasurements(GaugeMetric metric, IDateService dateService)
  {
    foreach (int _ in Enumerable.Range(0, Random.Shared.Next(0, 30)))
    {
      var command = new AddGaugeMeasurementCommand
      {
        MetricKey = metric.Key,
        Value = Random.Shared.Next(0, Random.Shared.Next(5, 150))
      };

      await new AddGaugeMeasurementCommandExecutor(command).Execute(_repository, dateService);
    }
  }

  private async Task AddMeasurements(TimerMetric metric, DateTime metricDate)
  {
    FakeDateService dateService = new FakeDateService(metricDate);

    int[] count = Enumerable.Range(0, Random.Shared.Next(0, 30)).ToArray();

    foreach (int measurementIndex in count)
    {
      int remainingSteps = (count.Length - measurementIndex) * 2;

      dateService.SetNext(remainingSteps);

      var startTimerCommand = new StartTimerMeasurementCommand { MetricKey = metric.Key };
      await new StartTimerMeasurementCommandExecutor(startTimerCommand).Execute(_repository, dateService);

      dateService.SetNext(remainingSteps);

      var endTimerCommand = new EndTimerMeasurementCommand { MetricKey = metric.Key };
      await new EndTimerMeasurementCommandExecutor(endTimerCommand).Execute(_repository, dateService);
    }
  }

  private void AddSpecificCases()
  {
    AddSpecificCase(SpecificCases.GetMigraineMedicineCase());
    AddSpecificCase(SpecificCases.GetOffByOneEdgeCase());
  }

  private void AddSpecificCase(SpecificCase specificCase)
  {
    var dateService = new SelfIncrementingDateService();
    IMetric metric = specificCase.Metric;
    string metricKey = metric.Key;

    new AddMetricCommand
      {
        Key = metricKey,
        Description = metric.Description,
        Name = metric.Name,
        Type = metric.Type,
      }
      .CreateExecutor()
      .Execute(_repository, dateService);

    if (metric.Flags.Any())
    {
      new EditMetricCommand
        {
          MetricKey = metricKey,
          Flags = metric.Flags,
          Description = metric.Description,
          Name = metric.Name
        }
        .CreateExecutor()
        .Execute(_repository, dateService);
    }

    foreach (IMeasurement measurement in specificCase.Measurements)
    {
      BaseAddMeasurementCommand command;

      switch (measurement)
      {
        case CounterMeasurement:
          command = new AddCounterMeasurementCommand();
          break;
        case GaugeMeasurement gaugeMeasurement:
          command = new AddGaugeMeasurementCommand { Value = gaugeMeasurement.Value };
          break;
        case TimerMeasurement:
          throw new NotImplementedException();
        default:
          throw new ArgumentOutOfRangeException(nameof(measurement));
      }

      command.Notes = measurement.Notes;
      command.MetricKey = metricKey;
      command.MetricFlagKey = measurement.MetricFlagKey;

      IDateService measurementDateService = measurement.DateTime != null
        ? new FakeDateService(measurement.DateTime.Value)
        : (IDateService)dateService;

      command.CreateExecutor().Execute(_repository, measurementDateService);
    }
  }

  private static MetricType GetRandomMetricType()
  {
    return (MetricType)Random.Shared.Next(0, Enum.GetNames(typeof(MetricType)).Length);
  }

  // https://stackoverflow.com/questions/4286487/is-there-any-lorem-ipsum-generator-in-c
  private static string LoremIpsum(int minWords, int maxWords, int minSentences, int maxSentences)
  {
    var words = new[]
    {
      "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
      "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
      "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"
    };

    int numSentences = Random.Shared.Next(minSentences, maxSentences);
    int numWords = Random.Shared.Next(minWords, maxWords);

    if (numWords == 0 || numSentences == 0)
    {
      return string.Empty;
    }

    var result = new StringBuilder();

    for (var s = 0; s < numSentences; s++)
    {
      for (var w = 0; w < numWords; w++)
      {
        if (w > 0)
        {
          result.Append(" ");
        }

        result.Append(words[Random.Shared.Next(words.Length)]);
      }

      result.Append(". ");
    }

    return result.ToString();
  }
}