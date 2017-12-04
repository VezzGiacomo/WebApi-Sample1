using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Queries.ViewModels
{
    public class SessionFreeViewModel
    {
        public int SessionsId { get; set; }
        public int FilmId { get; set; }
        public DateTime Start { get; set; }
        public bool IsPublished { get; set; }
        public int ScreensId { get; set; }
        public string ScreenName { get; set; }
        public int CinemaId { get; set; }
        public int NumberOfFreeSeats { get; set; }
    }
}
