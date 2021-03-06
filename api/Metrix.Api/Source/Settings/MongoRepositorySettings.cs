using Metrix.Persistence.Mongo;

namespace Metrix.Api.Settings;

public class MongoRepositorySettings : IMongoRepositorySettings
{
  public MongoRepositorySettings(string connectionString)
  {
    MongoDbConnectionString = connectionString;
  }

  public string MongoDbConnectionString { get; }

  public string DatabaseName => "metrix_test";
  public string MetricsCollectionName => "metrics";
  public string MeasurementsCollectionName => "measurements";
  public string UsersCollectionName => "users";
}
