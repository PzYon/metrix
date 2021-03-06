namespace Metrix.Persistence.Mongo;

public interface IMongoRepositorySettings
{
  string MongoDbConnectionString { get; }
  string DatabaseName { get; }
  string MetricsCollectionName { get; }
  string MeasurementsCollectionName { get; }
  string UsersCollectionName { get; }
}
