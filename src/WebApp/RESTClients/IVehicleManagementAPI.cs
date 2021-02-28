using Pitstop.Models;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Commands;

namespace WebApp.RESTClients
{
    public interface IVehicleManagementAPI
    {
        Task<List<Vehicle>> GetVehicles();
        
        Task<Vehicle> GetVehicleByLicenseNumber(string licenseNumber);
        
        Task RegisterVehicle(RegisterVehicle command);
    }
}
