using System.ComponentModel.DataAnnotations;

namespace Common
{
    public static class ValidationHelper
    {
        // Validiert ein DTO anhand seiner DataAnnotations-Attribute
        // ([Required], [Range], [StringLength], ...) und gibt eine
        // lesbare Fehlerliste zurück, falls etwas ungültig ist.
        public static List<string> Validate<T>(T instance) where T : notnull
        {
            var context = new ValidationContext(instance);
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(instance, context, results, validateAllProperties: true);

            return results
                .Select(r => r.ErrorMessage ?? "Ungültiger Wert")
                .ToList();
        }
    }
}
