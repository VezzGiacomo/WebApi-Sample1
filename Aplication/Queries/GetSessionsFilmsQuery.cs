using Aplication.Queries.ViewModels;
using MediatR;
using System;

namespace Aplication.Queries
{
    public class GetSessionsFilmsQuery : IRequest<QueryResponse<SessionFilmsViewModel[]>>
    {
        public bool? IsPublished { get; set; }
        public DateTime? ReferenceDate { get; set; }
    }
}
