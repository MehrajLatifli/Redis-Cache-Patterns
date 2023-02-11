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


            WeatherInformation item;



            try
            {
                
                if (value == null)
                {
                        item = _weatherDal.Get(p => p.IdWeather == Convert.ToInt32(idWeather));
                    if (item != null)
                    {

                        _cache.Add(item);

                        return Ok(_weatherDal.GetList().Where(o => o.IdWeather == Convert.ToInt32(item.IdWeather)));
                    }
                }

                else
                {

                    item = _weatherDal.Get(p => p.IdWeather == Convert.ToInt32(value.IdWeather));

                    if (item != null)
                    {

                        return Ok(_weatherDal.GetList().Where(o => o.IdWeather == Convert.ToInt32(item.IdWeather)));
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
                _weatherDal.Add(weatherInformation);


                _cache.Add(weatherInformation);



                return Ok(_weatherDal.GetList().Where(o => o.IdWeather == Convert.ToInt32(weatherInformation.IdWeather)));

            }
            catch (Exception)
            {


            }
            return BadRequest();

        }


        [HttpDelete("Delete/{idWeather}")]
        public async Task<IActionResult> Delete(int idWeather)
        {
            if (idWeather != null)
            {

                var item = _weatherDal.Get(p => p.IdWeather == Convert.ToInt32(idWeather));

                _weatherDal.Delete(item);

                _cache.Delete(item.IdWeather);

                return StatusCode(StatusCodes.Status204NoContent);

            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
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


                    _weatherDal.Update(weatherInformation);

                    _cache.Update(weatherInformation);






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

