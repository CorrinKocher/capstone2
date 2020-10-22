using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    /// <summary>
    /// methods to make: SeeAllSpacesForVenue, 
    /// </summary>
    class Space
    { 
        public int SpaceId { get; set; }

        public string Name { get; set; }

        public int OpenDate { get; set; }

        public int CloseDate { get; set; }

        public int MaximumOccupancy { get; set; }

        public bool WheelChairAcceability { get; set; }

        public decimal DailyRate { get; set; }
    }
}
