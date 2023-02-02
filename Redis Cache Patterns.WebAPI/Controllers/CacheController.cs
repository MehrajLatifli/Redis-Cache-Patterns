using LazyLoading_Pattern_WebAPI.DataAccess;
using LazyLoading_Pattern_WebAPI.DatabaseFirst;
using Microsoft.AspNetCore.Mvc;
using Redis_Cache_Patterns.WebAPI.Services;

namespace Redis_Cache_Patterns.WebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CacheController : ControllerBase
    {


        private readonly CacheService _cache;
        IWeatherDal _weatherDal;

        public CacheController(IWeatherDal weatherDal, CacheService cache)
        {
            _cache = cache;
            _weatherDal = weatherDal;


        }



        [HttpGet("Get/{idWeather}")]
        public async Task<IActionResult> Get(int idWeather)
        {
            var value = _cache.Get(idWeather);


            var item = _weatherDal.Get(p => p.IdWeather == Convert.ToInt32(idWeather));

            try
            {
                if (item == null)
                {

                    return StatusCode(StatusCodes.Status404NotFound);
                }

                else
                {
                    if (value == null)
                    {


                        _cache.Add(item);

                        value = _cache.Get(idWeather);

                        return Ok(_weatherDal.GetList().Where(o => o.IdWeather == Convert.ToInt32(value.IdWeather)));
                    }
                    else
                    {

                        return Ok(_weatherDal.GetList().Where(o => o.IdWeather == Convert.ToInt32(value.IdWeather)));
                    }
                }
            }
            catch (Exception)
            {

            }




            return BadRequest();

        }


        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] WeatherInformation weatherInformation)
        {
            try
            {
                _cache.Add(weatherInformation);

                var value = _cache.Get(weatherInformation.IdWeather);

                _weatherDal.Add(value);

                return Ok(_weatherDal.GetList().Where(o => o.IdWeather == Convert.ToInt32(value.IdWeather)));

            }
            catch (Exception)
            {


            }
            return BadRequest();

        }


        [HttpDelete("Delete/{idWeather}")]
        public async Task<IActionResult> Delete(int idWeather)
        {
            var value = _cache.Get(idWeather);
            try
            {
                if (_weatherDal.GetList().Any(i => i.IdWeather == idWeather))
                {

                    if (value != null)
                    {

                        var item = _weatherDal.Get(p => p.IdWeather == Convert.ToInt32(idWeather));


                        value = _cache.Get(item.IdWeather);

                        _cache.Delete(item.IdWeather);


                        _weatherDal.Delete(new WeatherInformation { IdWeather = value.IdWeather });

                        return StatusCode(StatusCodes.Status204NoContent);
                    }

                    else
                    {

                        var item = _weatherDal.Get(p => p.IdWeather == Convert.ToInt32(idWeather));

                        _cache.Add(item);

                        value = _cache.Get(idWeather);


                        _weatherDal.Delete(new WeatherInformation { IdWeather = value.IdWeather });

                        _cache.Delete(item.IdWeather);

                        return StatusCode(StatusCodes.Status204NoContent);

                    }
                }

                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

            }
            catch (Exception)
            {


            }
            return BadRequest();

        }


        [HttpPut("Update")]
        public async Task<IActionResult> PutCustomer([FromBody] WeatherInformation weatherInformation)
        {
            try
            {
                if (weatherInformation != null)
                {



                    _cache.Update(weatherInformation);


                    var value = _cache.Get(weatherInformation.IdWeather);


                    _weatherDal.Update(value);


                    //return StatusCode(StatusCodes.Status200OK);
                    return StatusCode(StatusCodes.Status204NoContent);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }


            }
            catch (Exception)
            {


            }
            return BadRequest();

        }
    }
}

