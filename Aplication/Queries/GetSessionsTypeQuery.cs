using Aplication.Queries.ViewModels;
using MediatR;
using System;

namespace Aplication.Queries
{
    public class GetSessionsTypeQuery : IRequest<QueryResponse<SessionTypeViewModel[]>>
    {
        public bool? IsPublished { get; set; }
        public DateTime? ReferenceDate { get; set; }
    }
}
