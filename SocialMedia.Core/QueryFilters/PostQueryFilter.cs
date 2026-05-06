using Swashbuckle.AspNetCore.Annotations;

namespace SocialMedia.Core.QueryFilters
{
    public class PostQueryFilter : PaginationQueryFilter
    {
        /// <summary>
        /// Identificador del usuario que crea el post
        /// </summary>
        [SwaggerSchema("Identificador del usuario que crea el post", Nullable = true)]
        public int? UserId { get; set; }


        /// <summary>
        /// Fecha de creacion del post
        /// </summary>
        [SwaggerSchema("Fecha de creacion del post",
            Format = "date-time", Nullable = true)]
        public string? Date { get; set; }

        /// <summary>
        /// Decripcion del post para la búsqueda por texto
        /// Solo aceptar valores con tamaño entre 2 y 100
        /// </summary>
        [SwaggerSchema("Decripcion del post para la búsqueda por texto",
             Nullable = true)]
        public string Description { get; set; }
    }
}
