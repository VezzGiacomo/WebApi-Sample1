using System;
using System.Collections.Generic;

namespace Api.BindingModels
{
    public class GetSessionsFreeBindingModel
    {
        public bool? IsPublished { get; set; }

        public int? NumberOfFreeSeats { get; set; }

        public int? FilmId { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public int? CinemaId { get; set; }

        public string[] SessionsType { get; set; }
    }
}
