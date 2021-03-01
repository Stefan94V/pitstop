using Pitstop.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Commands;

namespace WebApp.RESTClients
{
    public interface IWorkshopManagementAPI
    {
        Task<WorkshopPlanning> GetWorkshopPlanning(string planningDate);

        Task<MaintenanceJob> GetMaintenanceJob(string planningDate, string jobId);

        Task RegisterPlanning(string planningDate, RegisterPlanning cmd);

        Task PlanMaintenanceJob(string planningDate, PlanMaintenanceJob cmd);

        Task FinishMaintenanceJob(string planningDate, string jobId, FinishMaintenanceJob cmd);

        Task<List<Customer>> GetCustomers();

        Task<Customer> GetCustomerById(string id);

        Task<List<Vehicle>> GetVehicles();

        Task<Vehicle> GetVehicleByLicenseNumber(string licenseNumber);
    }
}
