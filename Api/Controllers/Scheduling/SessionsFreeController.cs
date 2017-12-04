using Api.BindingModels;
using Api.Infrastructure;
using Api.Infrastructure.Versioning;
using Aplication.Queries;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Api.Controllers.Scheduling
{
    [Version1]
    [RoutePrefix("api/v{version:apiVersion}/SessionsFree")]
    public class SessionsFreeController : ApiController
    {
        private readonly IMediator _mediator;

        public SessionsFreeController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // GET: 
        //      api/v1/SessionsFree/2017/10/31/M,T,N/cinema/3/film/1
	    //      api/v1/SessionsFree/2017/10/31/M,T,N/cinema/3
	    //      api/v1/SessionsFree/2017/10/31/M,T,N/film/1
	    //      api/v1/SessionsFree/2017/10/31/M,T,N
        //      api/v1/SessionsFree/2017/10/31
        [HttpGet]
        [Route("{year:int}/{month:range(1,12)}/{day:range(1,31)}/{sessionsTypes?}")]
        [Route("{year:int}/{month:range(1,12)}/{day:range(1,31)}/{sessionsTypes?}/cinema/{cinemaId:int?}")]
        [Route("{year:int}/{month:range(1,12)}/{day:range(1,31)}/{sessionsTypes?}/film/{filmId:int?}")]
        [Route("{year:int}/{month:range(1,12)}/{day:range(1,31)}/{sessionsTypes?}/cinema/{cinemaId:int?}/film/{filmId:int?}")]
        [ResponseType(typeof(GetSessionsFreeQuery))]
        public async Task<IHttpActionResult> GetSessionsFreeAsync(int year, int month, int day, string sessionsTypes = null, int? cinemaId = null, int? filmId = null)
        {
            if (DateBuilder.TryBuildFrom(year, month, day, out DateTime date) == false)
            {
                return BadRequest();
            }
            GetSessionsFreeQuery filtros = new GetSessionsFreeQuery { Start = date, CinemaId = cinemaId, FilmId = filmId };
            if(!string.IsNullOrWhiteSpace(sessionsTypes))
                filtros.SessionsType = sessionsTypes.Split(',');
            
            var response = await _mediator.Send(filtros);
            return Ok(response.Data);
        }

    }
}
