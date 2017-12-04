using Api.Infrastructure;
using Api.Infrastructure.Versioning;
using Aplication.Queries;
using Aplication.Queries.ViewModels;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Api.Controllers.Scheduling
{

    [Version1]
    [RoutePrefix("api/v{version:apiVersion}/SessionsBag")]
    public class SessionsBagController : ApiController
    {
        private readonly IMediator _mediator;

        public SessionsBagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/SessionsBag

        [HttpGet]
        [Route]
        [ResponseType(typeof(SessionBagViewModel[]))]
        public async Task<IHttpActionResult> GetSessionsBag()
        {
            return Ok(await GetSessionsBagData(null,null));
        }


        // GET: api/v1/SessionsBag/2017/10/31

        [HttpGet]
        [Route("{year:int}/{month:range(1,12)}/{day:range(1,31)}")]
        [Route("{year:int}/{month:range(1,12)}/{day:range(1,31)}/Published/{isPublished:bool?}")]
        [ResponseType(typeof(SessionBagViewModel[]))]
        public async Task<IHttpActionResult> GetSessionsBag(int year, int month, int day, bool? isPublished = null)
        {
            if (DateBuilder.TryBuildFrom(year, month, day, out DateTime date) == false)
            {
                return BadRequest();
            }
            return Ok(await GetSessionsBagData(isPublished, new DateTime(year, month, day)));
        }

        private async Task<SessionBagViewModel> GetSessionsBagData(bool? isPublished, DateTime? referenceDate)
        {
            SessionBagViewModel ret = new SessionBagViewModel();
            //
            // Recupera los cinemas 
            //
            var qrCinemas = await _mediator.Send(new GetCinemasQuery());
            ret.Cinemas = qrCinemas.Data;

            //
            // Franjas del día (Mañana Tarde Noche) abiertas por cinemas
            //
            var getSessionsTypeQuery = new GetSessionsTypeQuery { IsPublished = isPublished, ReferenceDate = referenceDate };
            var qrSessionsType = await _mediator.Send(getSessionsTypeQuery);
            ret.SessionsType = qrSessionsType.Data;

            //
            // Films en cartelera
            //
            var getSessionsFilmsQuery = new GetSessionsFilmsQuery { IsPublished = isPublished, ReferenceDate = referenceDate };
            var qrSessionsFilms = await _mediator.Send(getSessionsFilmsQuery);
            ret.SessionFilms = qrSessionsFilms.Data;

            return ret;
        }


    }

}
