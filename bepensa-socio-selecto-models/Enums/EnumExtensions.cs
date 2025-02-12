using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace bepensa_socio_selecto_models.Enums
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr?.Name ?? enu.ToString();
        }

        public static string GetDescription(this Enum enu)
        {
            var attr = GetDisplayAttribute(enu);
            return attr?.Description ?? enu.ToString();
        }

        private static DisplayAttribute? GetDisplayAttribute(object value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("Tipo {0} no es un enum", type));
            }

            var field = type.GetField(value.ToString());
            return field?.GetCustomAttribute<DisplayAttribute>();
        }
    }
}
