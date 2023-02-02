using Castle.Core.Resource;
using LazyLoading_Pattern_WebAPI.DatabaseFirst;

namespace LazyLoading_Pattern_WebAPI.DataAccess
{
    public interface IWeatherDal : IEntityRepository<WeatherInformation>
    {
    }
}
