﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// methods: bookReservation,
    /// </summary>
    class Reservation
    {
        public int ReservationId { get; set; }

        public string ReservedFor { get; set; }

        public int NumberOfAttendees { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


    }
}
