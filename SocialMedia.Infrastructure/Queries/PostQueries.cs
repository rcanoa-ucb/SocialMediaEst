using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Queries
{
    public static class PostQueries
    {
        public static string PostQuerySqlServer = @"
                        select Id, UserId, Date, Description, Imagen 
                        from post 
                        order by Date desc
                        OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;";
        public static string PostQueryMySQl = @"
                        select Id, UserId, Date, Description, Imagen 
                        from post 
                        order by Date desc
                        LIMIT @Limit
                    ";
        public static string PostComentadosUsuariosActivos = @"
                        SELECT 
                        p.Id AS PostId,
                        p.Description,
                        COUNT(c.Id) AS TotalComentarios
                    FROM Post p
                    JOIN Comment c ON p.Id = c.PostId
                    JOIN User u ON c.UserId = u.Id
                    WHERE u.IsActive = 1        
                    GROUP BY p.Id, p.Description
                    HAVING COUNT(c.Id) > 2
                    ORDER BY TotalComentarios DESC;            
                    ";
    }
}
