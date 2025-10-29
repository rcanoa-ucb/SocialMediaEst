namespace SocialMedia.Core.QueryFilters
{
    public abstract class PaginationQueryFilter
    {
        /// <summary>
        /// Cantidad de registros por pagina
        /// </summary>
        public int PageSize { get; set; }


        /// <summary>
        /// Numero de pagina a mostrar
        /// </summary>
        public int PageNumber { get; set; }
    }
}
