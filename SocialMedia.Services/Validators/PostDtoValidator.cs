using FluentValidation;
using SocialMedia.Core.DTOs;

namespace SocialMedia.Services.Validators
{
    public class PostDtoValidator 
        : AbstractValidator<PostDto>
    {
        public PostDtoValidator()
        {
            //Validacion para UserId
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El Id Usuario es obligatorio y debe ser mayor a 0")
                .NotEmpty().WithMessage("El id usuario no puede ser vacío");

            //Validar la fecha Date
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("La fecha es obligatoria")
                .Must(BeAValidDate).WithMessage("La fecha debe tener un formato valido ej: dd-MM-yyyy o similar")
                .Must(BeNotFutureDate).WithMessage("La fecha no puede ser futura");

            //Validacion de la descripcion
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripcion es obligatoria")
                .MinimumLength(5).WithMessage("La descripcion debe tener al menos 5 caracteres")
                .MaximumLength(1000).WithMessage("La descripcion no puede exceder los 1000 caracteres");

            //Validacion para Image (Opcional)
            RuleFor(x => x.Image)
                .Must(BeAValidUrl).When(x => !string.IsNullOrEmpty(x.Image))
                .WithMessage("La url de la imagen no es valida, debe comenzar con http: o https");
        }


        // Métodos de validación personalizados
        private bool BeAValidDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }

        private bool BeNotFutureDate(string date)
        {
            if (DateTime.TryParse(date, out var parsedDate))
            {
                return parsedDate <= DateTime.Now;
            }
            return true; // Si no es una fecha válida, la otra validación se encargará
        }

        private bool BeAValidUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return true;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
