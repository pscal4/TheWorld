using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    public class TripController : Controller

    {
        private IWorldRepository _repository;
        private ILogger<TripController> _logger;

        public TripController(IWorldRepository repository, ILogger<TripController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet("")]
        public JsonResult Get()
        {
            var results = Mapper.Map<IEnumerable<TripViewModel>>(_repository.GetAllTripsWithStops());
            return Json(results);
        }
        [HttpPost("")]
        public JsonResult Post([FromBody]TripViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTrip = Mapper.Map<Trip>(vm);

                    // Save to the Databae
                    _logger.LogInformation("Attempting to save new trip");

                    _repository.AddTrip(newTrip);

                    if (_repository.SaveAll())
                    {

                    }

                    Response.StatusCode = (int)HttpStatusCode.Created;
                    // Now map the model back to the view model
                    return Json(Mapper.Map<TripViewModel>(newTrip));
                }
            }
            catch (Exception ex)
            {

                // Log the exception
                _logger.LogError("Failed to save new trip", ex);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message});
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }
    }
}
