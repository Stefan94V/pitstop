using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.Models;
using Microsoft.AspNetCore.Hosting;
using Refit;
using WebApp.Commands;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System;
using Dapr.Client;

namespace WebApp.RESTClients
{
    public class VehicleManagementAPI : IVehicleManagementAPI
    {
        private readonly DaprClient _daprClient;
        private const string VehicleManagmentApi_AppId = "vehiclemanagement-api";
        private const string VehicleManagmentApi_Path = "/api/vehicles";

        public VehicleManagementAPI(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task<List<Vehicle>> GetVehicles()
        {
            return await _daprClient.InvokeMethodAsync<List<Vehicle>>(
                HttpMethod.Get,
                VehicleManagmentApi_AppId,
                VehicleManagmentApi_Path);
        }

        public async Task<Vehicle> GetVehicleByLicenseNumber(string licenseNumber)
        {
            try
            {
                return await _daprClient.InvokeMethodAsync<Vehicle>(
                    HttpMethod.Get,
                    VehicleManagmentApi_AppId,
                    $"{VehicleManagmentApi_Path}/{licenseNumber}");
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task RegisterVehicle(RegisterVehicle command)
        {
            await _daprClient.InvokeMethodAsync(
                HttpMethod.Post,
                VehicleManagmentApi_AppId,
                VehicleManagmentApi_Path,
                new
                {
                    command.MessageId,
                    command.Brand,
                    command.Type,
                    command.LicenseNumber,
                    command.OwnerId,
                });
        }
    }
}