using bepensa_models.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace bepensa_models.Enums
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

        public static string GetDescriptionFromValue<T>(int value) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                T enumValue = (T)(object)value;
                return enumValue.GetDisplayName();
            }
            return "Desconocido";
        }

        public static string GetCssClassFromValue<T>(int value) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), value))
            {
                T enumValue = (T)(object)value;
                return enumValue.GetCssClass();
            }
            return "Desconocido";
        }

        private static string GetCssClass<T>(this T value) where T : Enum
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<CssClassAttribute>();
            return attribute != null ? attribute.Name : string.Empty;
        }
    }
}
