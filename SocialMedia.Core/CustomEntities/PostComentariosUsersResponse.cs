using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.CustomEntities
{
    public class PostComentariosUsersResponse
    {
        public int PostId { get; set; }
        public string Description { get; set; }
        public int TotalComentarios { get; set; }
    }
}
