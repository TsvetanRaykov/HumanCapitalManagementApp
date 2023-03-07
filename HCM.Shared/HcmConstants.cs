namespace HCM.Shared
{
    public static class HcmConstants
    {
        public static class SupportedCustomOidcScopes
        {
            public const string HcmApiScope = "https://human-capital-management.com/api";
            public const string HcmUmsScope = "https://human-capital-management.com/ums";
        }

        public static class ApiEndpoints
        {
            public const string Jobs = "/api/v1/jobs";
            public const string Users = "/api/v1/users";
            public const string Employees = "/api/v1/employees";
            public const string Departments = "/api/v1/departments";
        }
    }
}