using Metrix.Core.Application.Persistence;
using Metrix.Core.Domain.Metrics;

namespace Metrix.Core.Application.Queries.Metrics.GetAll;

public class GetAllMetricsQueryExecutor : IQueryExecutor<IMetric[]>
{
  private readonly GetAllMetricsQuery _command;

  public GetAllMetricsQueryExecutor(GetAllMetricsQuery command)
  {
    _command = command;
  }

  public async Task<IMetric[]> Execute(IRepository repository)
  {
    IMetric[] allMetrics = await repository.GetAllMetrics();
    return allMetrics.OrderByDescending(m => m.LastMeasurementDate).ToArray();
  }
}
