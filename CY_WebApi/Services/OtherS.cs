using CY_BM;

namespace CY_WebApi.Services
{
    public class OtherS
    {


    }


    public static class UserTypeExtensions
    {
        public static string GetRoleName(this UserType userType)
        {
            return userType switch
            {
                UserType.GroupManager => "GroupManager",
                UserType.GroupExpert => "GroupExpert",
                UserType.DepartmentManager => "DepartmentManager",
                UserType.GeneralManager => "GeneralManager",
                UserType.PurchasingExpert => "PurchasingExpert",
                UserType.PurchasingManager => "PurchasingManager",
                UserType.CustomerAdmin => "CustomerAdmin",
                UserType.Reporter => "Reporter",
                UserType.SysAdmin => "SysAdmin",
                _ => userType.ToString()
            };
        }
    }


    public static class UserTypeExtensionsB
    {
        public static UserType GetRoleType(string userClaimRole)
        {
            UserType userType = userClaimRole switch
            {
                "GroupExpert" => UserType.GroupExpert,
                "GroupManager" => UserType.GroupManager,
                "DepartmentManager" => UserType.DepartmentManager,
                "GeneralManager" => UserType.GeneralManager,
                "PurchasingExpert" => UserType.PurchasingExpert,
                "PurchasingManager" => UserType.PurchasingManager,
                "CustomerAdmin" => UserType.CustomerAdmin,
                "Reporter" => UserType.Reporter,
                "SysAdmin" => UserType.SysAdmin,
                _ => UserType.NewUser
            };

            return userType;
        }
    }


    public static class IntToCurency
    {
        public static Currency GetIntCurncy(int input)
        {
            Currency curency = input switch
            {
                1=>Currency.Yuan,
                2=>Currency.Rial,
                3=>Currency.Dollor,

            };

            return curency;
        }
    }


    public static class CurencyToInt
    {
        public static int GetCurencyType(this Currency input)
        {
            return input switch
            {
                Currency.Yuan => 1 ,
                Currency.Rial => 2 ,
                Currency.Dollor => 3 ,

            };
        }
    }

}
;