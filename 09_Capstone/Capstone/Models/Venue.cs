using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// Methods: ViewAllVenues, SelectVenue, ReturnAllAvailableDatesForAVenue, DispayVenueDetails
    /// </summary>
    public class Venue
    {
        public int VenueId { get; set; }

        public string VenueName { get; set; }

        public int CityId { get; set; }

        public string State { get; set; }

        public string CityName { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        
    }
}
