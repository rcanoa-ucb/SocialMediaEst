using SocialMedia.Core.Entities;
using SocialMedia.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.DTOs
{
    public class SecurityDto
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }

        public RoleType? Role { get; set; }
    }
}
