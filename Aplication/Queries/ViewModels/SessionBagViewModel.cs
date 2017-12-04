using System.Collections.Generic;

namespace Aplication.Queries.ViewModels
{
    public class SessionBagViewModel
    {
        public CinemaViewModel[] Cinemas { get; set; }
        public SessionTypeViewModel[] SessionsType { get; set; }
        public SessionFilmsViewModel[] SessionFilms { get; set; }
    }
}
