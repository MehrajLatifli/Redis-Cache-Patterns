using LazyLoading_Pattern_WebAPI.DatabaseFirst;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Redis_Cache_Patterns.WebAPI.Services
{
    public class CacheService
    {
        private readonly Lazy<ConnectionMultiplexer> _connection;
        private readonly IDatabase _database;

        public CacheService()
        {
            _connection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect("localhost");
            });
            _database = _connection.Value.GetDatabase();
        }

        public WeatherInformation Get(int id)
        {

            var value = _database.StringGet(id.ToString());
            return value.HasValue ? JsonConvert.DeserializeObject<WeatherInformation>(value) : null;
        }

        public void Add(WeatherInformation item)
        {
            _database.StringSet(item.IdWeather.ToString(), JsonConvert.SerializeObject(item, Formatting.Indented));
        }

        public void Update(WeatherInformation item)
        {
            _database.StringSet(item.IdWeather.ToString(), JsonConvert.SerializeObject(item, Formatting.Indented));
        }

        public void Delete(int id)
        {
            _database.KeyDelete(id.ToString());
        }
    }
}
