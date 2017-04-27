using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddStop(string tripName, Stop newStop)
        {
            var theTrip = GetTripByName(tripName);
            // Need to calculate the value for Order (the highest existing stop +1 )
            // PJS NOTE:  I added the code to check if count = 0 (no existing stops) as the Max method throws an exception in this case
            if (theTrip.Stops.Count == 0 )
            {
                newStop.Order = 1;
            }
            else
            {
                newStop.Order = theTrip.Stops.Max(s => s.Order) + 1;
            }
            // This puts the new stop into the stops collection for the trip.
            // Without this line, the "TripId" value in the new stop is set to null (basically an orphan)
            theTrip.Stops.Add(newStop);
            _context.Stops.Add(newStop);
        }

        public void AddTrip(Trip newTrip)
        {
            // This might be a good place to add default properties
            _context.Add(newTrip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            try
            {
                return _context.Trips.OrderBy(t => t.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips from database", ex);
                return null;
            }
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return _context.Trips
                    .Include(t => t.Stops)
                    .OrderBy(t => t.Name)
                    .ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }

        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips.Include(t => t.Stops)
                .Where(t => t.Name == tripName)
                .FirstOrDefault();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
