using System.ComponentModel.DataAnnotations;

namespace bepensa_socio_selecto_models.Validators;

public class FechaNacimientoValidaAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateOnly fechaNacimiento)
        {
            DateOnly fechaMinima = DateOnly.FromDateTime(DateTime.Today.AddYears(-90));
            DateOnly fechaMaxima = DateOnly.FromDateTime(DateTime.Today.AddYears(-18));

            if (fechaNacimiento < fechaMinima || fechaNacimiento > fechaMaxima)
            {
                return new ValidationResult("La fecha de nacimiento proporcionada es inválida.");
            }
        }

        return ValidationResult.Success;
    }
}
