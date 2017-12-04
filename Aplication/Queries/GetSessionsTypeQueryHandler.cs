using System.Linq;
using System.Threading.Tasks;
using Aplication.Queries.Infrastructure;
using Aplication.Queries.ViewModels;
using MediatR;
using Dapper;

namespace Aplication.Queries
{
    public class GetSessionsTypeQueryHandler : IAsyncRequestHandler<GetSessionsTypeQuery, QueryResponse<SessionTypeViewModel[]>>
    {
        private readonly IConnectionProvider _connectionProvider;

        public GetSessionsTypeQueryHandler(IConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<QueryResponse<SessionTypeViewModel[]>> Handle(GetSessionsTypeQuery message)
        {
            using (var conn = _connectionProvider.CreateConnection())
            {
                string sqlWhere = string.Empty;
                if (message.IsPublished != null)
                    sqlWhere = string.Format(" {0} {1} {2}", sqlWhere, (sqlWhere.Length == 0 ? "WHERE" : "AND"), "Sessions.IsPublished = @IsPublished");
                if (message.ReferenceDate != null)
                    sqlWhere = string.Format(" {0} {1} {2}", sqlWhere, (sqlWhere.Length == 0 ? "WHERE" : "AND"), "Sessions.[Start] >= @ReferenceDate");

                string sql = string.Format(@"
SELECT Screens.CinemaId
 	,CASE 
		WHEN CAST(Sessions.Start AS time) < '14:00' THEN 'M'
		WHEN CAST(Sessions.Start AS time) BETWEEN '14:00' AND '19:30' THEN 'T'
		WHEN CAST(Sessions.Start AS time) > '19:30' THEN 'N'
	END AS SessionType

  FROM session.Sessions
  INNER JOIN cine.Screens ON Sessions.ScreenId = Screens.Id
  {0} 
  GROUP BY Screens.CinemaId
   ,CASE 
		WHEN CAST(Sessions.Start AS time) < '14:00' THEN 'M'
		WHEN CAST(Sessions.Start AS time) BETWEEN '14:00' AND '19:30' THEN 'T'
		WHEN CAST(Sessions.Start AS time) > '19:30' THEN 'N'
	END ", sqlWhere);

                var sessiones = await conn.QueryAsync<SessionTypeViewModel>(sql, new
                {
                    IsPublished = message.IsPublished,
                    ReferenceDate = message.ReferenceDate
                });
                return new QueryResponse<SessionTypeViewModel[]>
                {
                    Data = sessiones.ToArray()
                };
            }
        }
    }

}
