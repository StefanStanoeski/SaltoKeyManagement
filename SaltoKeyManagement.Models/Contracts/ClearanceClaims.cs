using System;
using System.Collections.Generic;
using System.Text;

namespace SaltoKeyManagement.Models.Contracts
{
    public static class ClearanceClaims
    {
        public static class User
        {
            public const string Guest = "Guest";
            public const string Employee = "Employee";
            public const string Maintenance = "Maintenance";
            public const string ServerStorage = "ServerStorage";
            public const string Administration = "Administration";
            public const string DocumentArchive = "DocumentArchive";
            public const string Management = "Management";
        }

        public static class Policy
        {
            public const string GuestEntrance = "GuestEntrance";
            public const string EmployeeEntrance = "EmployeeEntrance";
            public const string MaintenanceEntrance = "MaintenanceEntrance";
            public const string ServerStorageEntrance = "ServerStorageEntrance";
            public const string AdministrationEntrance = "AdministrationEntrance";
            public const string DocumentArchiveEntrance = "DocumentArchiveEntrance";
            public const string ManagementEntrance = "ManagementEntrance";
        }
    }
}
