﻿using Metrix.Core.Domain.Measurements;
using Metrix.Core.Domain.Metrics;

namespace Metrix.Core.Application.Persistence;

public class SpecificCase
{
  public Metric Metric { get; set; } = null!;

  public List<IMeasurement> Measurements { get; } = new();
}

public static class SpecificCases
{
  public static SpecificCase GetOffByOneEdgeCase()
  {
    return new SpecificCase
    {
      Metric = new Metric
      {
        Key = "edge-case",
        Name = "Date Edge Case",
        Type = MetricType.Counter
      },
      Measurements =
      {
        new CounterMeasurement
        {
          DateTime = new DateTime(2022, 3, 31, 21, 23, 0, DateTimeKind.Utc)
        },
        new CounterMeasurement
        {
          DateTime = new DateTime(2022, 4, 1, 2, 1, 0, DateTimeKind.Utc)
        }
      }
    };
  }

  public static SpecificCase GetMigraineMedicineCase()
  {
    const string irfenKey = "irf";
    const string eletriptanKey = "ele";
    const string imigranKey = "imi";

    return new SpecificCase
    {
      Metric = new Metric
      {
        Key = "migraine-medicine",
        Name = "Migraine Medicine",
        Description = "How many migraine medicines have been taken.",
        Type = MetricType.Counter,
        Flags = new Dictionary<string, string>
        {
          { irfenKey, "Irfen" },
          { eletriptanKey, "Eletriptan" },
          { imigranKey, "Imigran" }
        }
      },
      Measurements =
      {
        new CounterMeasurement
        {
          DateTime = DateTime.UtcNow.AddDays(-30),
          MetricFlagKey = eletriptanKey
        },
        new CounterMeasurement
        {
          DateTime = DateTime.UtcNow.AddDays(-30),
          MetricFlagKey = irfenKey
        },
        new CounterMeasurement
        {
          DateTime = DateTime.UtcNow.AddDays(-30),
          MetricFlagKey = imigranKey
        },
        new CounterMeasurement
        {
          DateTime = DateTime.UtcNow.AddDays(-30),
          MetricFlagKey = eletriptanKey
        },
        new CounterMeasurement
        {
          DateTime = DateTime.UtcNow.AddDays(-28),
          MetricFlagKey = irfenKey
        },
        new CounterMeasurement
        {
          DateTime = DateTime.UtcNow.AddDays(-10),
          MetricFlagKey = eletriptanKey
        },
        new CounterMeasurement
        {
          DateTime = DateTime.UtcNow.AddDays(-5),
          MetricFlagKey = eletriptanKey
        },
        new CounterMeasurement
        {
          DateTime = DateTime.UtcNow.AddDays(-3),
          MetricFlagKey = imigranKey
        },
        new CounterMeasurement
        {
          DateTime = DateTime.UtcNow.AddDays(-3),
          MetricFlagKey = irfenKey
        }
      }
    };
  }
}
