using Api.Infrastructure.Versioning;
using Aplication.Queries;
using Aplication.Queries.ViewModels;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Api.Controllers.Scheduling
{
    [Version1]
    [RoutePrefix("api/v{version:apiVersion}/SessionsType")]
    public class SessionsTypeController : ApiController
    {
        private readonly IMediator _mediator;

        public SessionsTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/SessionsType

        [HttpGet]
        [Route]
        [ResponseType(typeof(SessionTypeViewModel[]))]
        public async Task<IHttpActionResult> GetCinemas()
        {
            var response = await _mediator.Send(new GetSessionsTypeQuery
            {
                IsPublished = null,
                ReferenceDate = null
            });
            return Ok(response.Data);
        }

    }


}
