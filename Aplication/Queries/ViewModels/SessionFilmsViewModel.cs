using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Queries.ViewModels
{
    public class SessionFilmsViewModel
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public int DurationInMinutes { get; private set; }
    }
}
