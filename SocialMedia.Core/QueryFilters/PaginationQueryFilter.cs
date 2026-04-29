using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.QueryFilters
{
    public class PaginationQueryFilter
    {
        //Cantidad de registros por pagina
        public int PageSize { get; set; } = 10;

        //Numero de pagina a mostrar
        public int PageNumber { get; set; }
    }
}
