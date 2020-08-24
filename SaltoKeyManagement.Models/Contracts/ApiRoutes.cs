using System;
using System.Collections.Generic;
using System.Text;

namespace SaltoKeyManagement.Models.Contracts
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Doors
        {
            public const string GetAll = Base + "/doors/{pageNumber}/{pageSize}";
            public const string Create = Base + "/doors";
            public const string Get = Base + "/doors/{doorId}";
            public const string Update = Base + "/doors/{doorId}";
            public const string Patch = Base + "/doors/{doorId}";
            public const string Delete = Base + "/doors/{doorId}";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
            public const string Refresh = Base + "/identity/refresh";
        }

        public static class Entrances
        {
            public const string OpenGuestDoorForUser = Base + "/entrances/" + 
                ClearanceClaims.User.Guest + "/{doorId}";

            public const string OpenEmployeeDoorForUser = Base + "/entrances/" + 
                ClearanceClaims.User.Employee + "/{doorId}";

            public const string OpenMaintenanceDoorForUser = Base + "/entrances/" +
                ClearanceClaims.User.Maintenance + "/{doorId}";

            public const string OpenServerStorageDoorForUser = Base + "/entrances/" +
                ClearanceClaims.User.ServerStorage + "/{doorId}";

            public const string OpenAdministrationDoorForUser = Base + "/entrances/" +
                ClearanceClaims.User.Administration + "/{doorId}";

            public const string OpenDocumentArchiveDoorForUser = Base + "/entrances/" +
                ClearanceClaims.User.DocumentArchive + "/{doorId}";

            public const string OpenManagementDoorForUser = Base + "/entrances/" +
                ClearanceClaims.User.Management + "/{doorId}";
        }
    }
}
