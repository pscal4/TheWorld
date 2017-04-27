using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using AutoMapper;
using TheWorld.ViewModels;
using TheWorld.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld.Controllers.Api
{
    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<TripController> _logger;
        private CoordService _coordService;

        public StopController(IWorldRepository repository, ILogger<TripController> logger, CoordService coordService)
        {
            _repository = repository;
            _logger = logger;
            _coordService = coordService;
        }
        // GET: /<controller>/
        [HttpGet("")]
        public JsonResult Get(string tripName)
        {
            try
            {
                var results = _repository.GetTripByName(tripName);
                // Above will return null if not found
                if (results == null)
                {
                    return Json(null);
                }
                return Json(Mapper.Map<IEnumerable<StopViewModel>>(results.Stops.OrderBy(s => s.Order)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get stops for trip {tripName}",ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occured finding trip name");
            }
        }

        [HttpPost("")]
        public async Task<JsonResult> Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Map to the Stop Entity
                    var newStop = Mapper.Map<Stop>(vm);
                    // Looking up Geocoordinates
                    var coordResult = await _coordService.Lookup(newStop.Name);
                    if (!coordResult.Success)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(coordResult.Message);
                    }

                    newStop.Latitude = coordResult.Latitude;
                    newStop.Longitude = coordResult.Longitude;

                    // Save to the database
                    _repository.AddStop(tripName, newStop);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<StopViewModel>(newStop));
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new stop",ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Failed to save new stop");


            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new stop");

        }
    }
}
