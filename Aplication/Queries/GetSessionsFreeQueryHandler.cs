using System.Linq;
using System.Threading.Tasks;
using Aplication.Queries.Infrastructure;
using Aplication.Queries.ViewModels;
using MediatR;
using Dapper;

namespace Aplication.Queries
{
    public class GetSessionsFreeQueryHandler : IAsyncRequestHandler<GetSessionsFreeQuery, QueryResponse<SessionFreeViewModel[]>>
    {
        private readonly IConnectionProvider _connectionProvider;

        public GetSessionsFreeQueryHandler(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<QueryResponse<SessionFreeViewModel[]>> Handle(GetSessionsFreeQuery message)
        {
            using (var conn = _connectionProvider.CreateConnection())
            {
                const string sql = @"
SELECT Sessions.Id As SessionsId
      ,Sessions.FilmId
      ,Sessions.[Start]
      ,Sessions.IsPublished
	  ,Screens.Id As ScreensId
	  ,Screens.[Name] As ScreenName
      ,Screens.CinemaId
	  ,FreeSeats.NumberOfFreeSeats
  FROM session.Sessions
  INNER JOIN cine.Screens ON Sessions.ScreenId = Screens.Id
  INNER JOIN 
  (
	  SELECT SessionId
			,COUNT(*) As NumberOfFreeSeats
	  FROM session.SessionSeats
	  WHERE Sold = 0 
	  GROUP BY SessionId
  ) As FreeSeats ON session.Sessions.Id = FreeSeats.SessionId";
                string sqlWhere = string.Empty;
                if (message.IsPublished != null)
                    sqlWhere = string.Format(" {0} {1} {2}", sqlWhere, (sqlWhere.Length == 0 ? "WHERE" : "AND"), "Sessions.IsPublished = @IsPublished");
                if (message.NumberOfFreeSeats != null)
                    sqlWhere = string.Format(" {0} {1} {2}", sqlWhere, (sqlWhere.Length == 0 ? "WHERE" : "AND"), "FreeSeats.NumberOfFreeSeats >= @NumberOfFreeSeats");
                if (message.FilmId != null)
                    sqlWhere = string.Format(" {0} {1} {2}", sqlWhere, (sqlWhere.Length == 0 ? "WHERE" : "AND"), "Sessions.FilmId = @FilmId");

                if (message.Start != null && message.End != null)
                    sqlWhere = string.Format(" {0} {1} {2}", sqlWhere, (sqlWhere.Length == 0 ? "WHERE" : "AND"), "Sessions.[Start] BETWEEN @Start AND @End");
                if (message.Start != null && message.End == null)
                    sqlWhere = string.Format(" {0} {1} {2}", sqlWhere, (sqlWhere.Length == 0 ? "WHERE" : "AND"), "Sessions.[Start] >= @Start");
                if (message.Start == null && message.End != null)
                    sqlWhere = string.Format(" {0} {1} {2}", sqlWhere, (sqlWhere.Length == 0 ? "WHERE" : "AND"), "Sessions.[Start] <= @End");

                if (message.CinemaId != null)
                    sqlWhere = string.Format(" {0} {1} {2}", sqlWhere, (sqlWhere.Length == 0 ? "WHERE" : "AND"), "Screens.CinemaId = @CinemaId");
                if (message.SessionsType != null)
                    sqlWhere = string.Format(" {0} {1} {2}", sqlWhere, (sqlWhere.Length == 0 ? "WHERE" : "AND"), @"
                CASE 
                    WHEN CAST(Sessions.[Start] AS time) < '14:00' THEN 'M'
                    WHEN CAST(Sessions.[Start] AS time) BETWEEN '14:00' AND '19:30' THEN 'T'
                    WHEN CAST(Sessions.[Start] AS time) > '19:30' THEN 'N'
                END IN @SessionsType");

                var sessiones = await conn.QueryAsync<SessionFreeViewModel>(string.Format("{0} {1}", sql, sqlWhere), new
                {
                    IsPublished = message.IsPublished,
                    NumberOfFreeSeats = message.NumberOfFreeSeats,
                    FilmId = message.FilmId,
                    Start = message.Start,
                    End = message.End,
                    CinemaId = message.CinemaId,
                    SessionsType = message.SessionsType
                });
                return new QueryResponse<SessionFreeViewModel[]>
                {
                    Data = sessiones.ToArray()
                };
            }
        }
    }

}
