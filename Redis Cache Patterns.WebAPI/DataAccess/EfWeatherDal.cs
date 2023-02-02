using Castle.Core.Resource;
using LazyLoading_Pattern_WebAPI.DatabaseFirst;

namespace LazyLoading_Pattern_WebAPI.DataAccess
{
    public class EfWeatherDal : EF_EntityRepositoryBase<WeatherInformation, WeatherContext>, IWeatherDal
    {
    }
}
