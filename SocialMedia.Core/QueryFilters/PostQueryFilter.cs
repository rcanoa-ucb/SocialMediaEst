using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.QueryFilters
{
     /// <summary>
     /// Filtra los parametros de post
     /// </summary>
    public class PostQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// Id del usuario
        /// </summary>
        [SwaggerSchema("Id del usuario")]
        public int? userId { get; set; }
        /// <summary>
        /// Fecha de publicacion
        /// </summary>
        [SwaggerSchema("Fecha de publicacion")]
        public DateTime? Date { get; set; }
        /// <summary>
        /// Descripcion del post
        /// </summary>
        [SwaggerSchema("Descripcion del post")]
        public string Description { get; set; }
    }
}
