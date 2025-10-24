using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.CustomEntities
{
    public class PostComentariosUsersResponse
    {
        public string PostId { get; set; }
        public string Description { get; set; }
        public string TotalComentarios { get; set; }
    }
}
