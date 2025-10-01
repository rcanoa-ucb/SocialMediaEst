using FluentValidation;
using SocialMedia.Infrastructure.DTOs;
using System.Globalization;

namespace SocialMedia.Infrastructure.Validators
{
    public class PostDtoValidator : AbstractValidator<PostDto>
    {
        public PostDtoValidator() 
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("El IdUser debe ser mayor que 0"); RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es requerida")
                .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres")
                .MinimumLength(10).WithMessage("La descripción debe tener al menos 10 caracteres");

            RuleFor(x => x.Imagen)
                .Must(UrlImagenValida).When(x => !string.IsNullOrEmpty(x.Imagen))
                .WithMessage("La URL de la imagen no es válida")
                .MaximumLength(1000).WithMessage("La URL de la imagen es demasiado larga");

            // Validar que la fecha pueda ser parseada desde el formato dd-MM-yyyy
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("La fecha es requerida")
                .Must(BeValidDateFormat).WithMessage("La fecha debe tener el formato dd-MM-yyyy")
                .Must(BeValidDate).WithMessage("La fecha no es válida");
        }

        private bool UrlImagenValida(string url)
        {
            if (string.IsNullOrEmpty(url)) return true;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp
                       || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        private bool BeValidDateFormat(string fecha)
        {
            if (string.IsNullOrEmpty(fecha))
                return false;

            return DateTime.TryParseExact(fecha, "dd-MM-yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        private bool BeValidDate(string fecha)
        {
            if (DateTime.TryParseExact(fecha, "dd-MM-yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result != default(DateTime) &&
                       result >= new DateTime(1900, 1, 1) &&
                       result <= new DateTime(2100, 12, 31);
            }
            return false;
        }
    }
}
