using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// methods: bookReservation,
    /// </summary>
    public class Reservation
    {
        public int ReservationId { get; set; }

        public string ReservedFor { get; set; }

        public int NumberOfAttendees { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int SpaceId { get; set; }
        public string SpaceName { get; set; }

        public string VenueName { get; set; }
    }
}
