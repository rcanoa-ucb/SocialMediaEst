using FluentValidation;
using SocialMedia.Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Validators
{
    public class PostDtoValidator : AbstractValidator<PostDto>
    {
        public PostDtoValidator() 
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("El IdUser debe ser mayor que 0");
        }
    }
}
