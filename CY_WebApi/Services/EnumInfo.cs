using CY_BM;

namespace CY_WebApi.Services
{
    public class EnumInfo
    {
        public static int GetEnumNumber(Type enumType, string name)
        {
            // بررسی ورودی‌ها
            if (enumType == null || string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("enumType or name cannot be null or empty.");
            }

            // بررسی اینکه نوع ارائه‌شده یک Enum باشد
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Provided type must be an Enum.");
            }

            // بررسی اینکه مقدار با نام ارائه‌شده در Enum وجود داشته باشد
            if (!Enum.IsDefined(enumType, name))
            {
                throw new ArgumentException($"The name '{name}' is not defined in the enum type '{enumType.Name}'.");
            }

            // تبدیل نام به مقدار Enum و سپس به عدد صحیح
            var enumValue = Enum.Parse(enumType, name);
            return Convert.ToInt32(enumValue);
        }
    }
}
