using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.BindingModels
{
    public class GetSessionsTypeQueryBindingModel
    {
        public bool? IsPublished { get; set; }

        public DateTime? ReferenceDate { get; set; }

    }
}
