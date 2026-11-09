namespace CY_WebApi.Models
{
    public class DomainConfig
    {
        public static bool IsDomainEnabled = true;

        //public static bool IsDomainEnabled = false;

        public static string Domain => IsDomainEnabled ? ".chipyab.com" : null;
    }
}
