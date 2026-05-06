using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.QueryFilters
{
    public class PaginationQueryFilter
    {
        /// <summary>
        /// Cantidad de registros por pagina
        /// </summary>
        [SwaggerSchema("Cantidad de registros por pagina", Nullable = false)]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Numero de pagina a mostrar
        /// </summary>
        [SwaggerSchema("Numero de pagina a mostrar", Nullable = false)]
        public int PageNumber { get; set; }
    }
}
