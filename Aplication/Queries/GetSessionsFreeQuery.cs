using System;
using Aplication.Queries.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace Aplication.Queries
{
   
    public class GetSessionsFreeQuery : IRequest<QueryResponse<SessionFreeViewModel[]>>
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
