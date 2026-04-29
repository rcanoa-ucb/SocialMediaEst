using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.CustomEntities
{
    public class PagedList<T> : List<T>
    {
        #region Atributos
        //Pagina Actual
        public int CurrentPage { get; set; }

        //Total de paginas
        public int TotalPages { get; set; }

        //Cantidad de registros en una pagina
        public int PageSize { get; set; }

        //Cantidad de registros
        public int TotalCount { get; set; }
        #endregion

        #region Propiedades
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public int? NextPageNumber => HasNextPage ? CurrentPage + 1 : null;
        public int? PreviousPageNumber => HasPreviousPage ? CurrentPage - 1 : null; 
        #endregion

        public PagedList(List<T> items,
            int count,
            int pageNumber,
            int pageSize) 
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> Create
            (IEnumerable<T> source,
            int pageNumber,
            int pageSize)
        {
           var  count = source.Count();
            var items = source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();

            return new PagedList<T>
                (items, count, pageNumber, pageSize);
        }
    }
}
