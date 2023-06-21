using System.ComponentModel.DataAnnotations;

namespace Models.InputModels
{
    public class NotPippoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext valCtx)
        {
            if (value == null || ((string)value).ToUpper() != "PIPPO" )
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage, new[] { valCtx.MemberName! });
        }
    }
}

/*
Con ValidationContext.GetService(typeof(nome_servizio)) as nome_servizio
si possono ottenere i servizi che non sono disponibili qui usando
la dependency injection
*/