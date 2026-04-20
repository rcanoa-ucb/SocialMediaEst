using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Queries
{
    public static class Primero
    {
        public static string unoMySql = @"
        select Id, UserId, Date, Description, Imagen
        from post
        order by date desc
        LIMIT @Limit;";

        public static string unoSql = @"
        select Id, UserId, Date, Description, Imagen
        from post
        order by date desc
        OFFSET 0 ROWS FETCH NEXT @Limit ROWS ONLY;
        ";
    }
}
