using System.Collections.Generic;
using System.Threading.Tasks;
using Pitstop.Models;
using Microsoft.AspNetCore.Hosting;
using Refit;
using WebApp.Commands;
using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebApp.RESTClients
{
    public class WorkshopManagementAPI : IWorkshopManagementAPI
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<WorkshopManagementAPI> _logger;
        private const string WorkshopManagementApi_AppId = "workshopmanagement-api";
        private const string WorkshopManagementApi_Path = "/api/WorkshopPlanning";

        public WorkshopManagementAPI(DaprClient daprClient, ILogger<WorkshopManagementAPI> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }

        public async Task<WorkshopPlanning> GetWorkshopPlanning(string planningDate)
        {
            try
            {
                return await _daprClient.InvokeMethodAsync<WorkshopPlanning>(
                    HttpMethod.Get,
                    WorkshopManagementApi_AppId,
                    $"{WorkshopManagementApi_Path}/{planningDate}");
            }
            catch (InvocationException ex)
            {
                if (ex.InnerException is HttpRequestException httpEx)
                {
                    if (httpEx.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                }

                throw;
            }
        }

        public async Task<MaintenanceJob> GetMaintenanceJob(string planningDate, string jobId)
        {
            try
            {
                return await _daprClient.InvokeMethodAsync<MaintenanceJob>(
                    HttpMethod.Get,
                    WorkshopManagementApi_AppId,
                    $"{WorkshopManagementApi_Path}/{planningDate}/jobs/{jobId}"
                );
            }
            catch (InvocationException ex)
            {
                if (ex.InnerException is HttpRequestException httpEx)
                {
                    if (httpEx.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                }

                throw;
            }
        }

        public async Task RegisterPlanning(string planningDate, RegisterPlanning cmd)
        {
            await _daprClient.InvokeMethodAsync(
                HttpMethod.Post,
                WorkshopManagementApi_AppId,
                $"{WorkshopManagementApi_Path}/{planningDate}",
                new
                {
                    cmd.PlanningDate,
                    cmd.MessageId,
                    cmd.MessageType
                }
            );
        }

        public async Task PlanMaintenanceJob(string planningDate, PlanMaintenanceJob cmd)
        {

            var jsonData = new
            {
                cmd.Description,
                cmd.VehicleInfo.LicenseNumber,
                CustomerInfo = new {cmd.CustomerInfo.Id, cmd.CustomerInfo.Name, cmd.CustomerInfo.TelephoneNumber},
                cmd.EndTime,
                cmd.JobId,
                cmd.StartTime,
                VehicleInfo =  new {cmd.VehicleInfo.LicenseNumber, cmd.VehicleInfo.Brand, cmd.VehicleInfo.Type},
                cmd.MessageId,
                cmd.MessageType
            };

            var jsonString = JsonConvert.SerializeObject(jsonData);
            _logger.LogError("Converting PLANNING: {JsonString}", jsonString);
            
            await _daprClient.InvokeMethodAsync(
                HttpMethod.Post,
                WorkshopManagementApi_AppId,
                $"{WorkshopManagementApi_Path}/{planningDate}/jobs",
                new
                {
                    cmd.Description,
                    cmd.VehicleInfo.LicenseNumber,
                    CustomerInfo = new {cmd.CustomerInfo.Id, cmd.CustomerInfo.Name, cmd.CustomerInfo.TelephoneNumber},
                    cmd.EndTime,
                    cmd.JobId,
                    cmd.StartTime,
                    VehicleInfo =  new {cmd.VehicleInfo.LicenseNumber, cmd.VehicleInfo.Brand, cmd.VehicleInfo.Type},
                    cmd.MessageId,
                    cmd.MessageType
                }
            );
        }
        public async Task FinishMaintenanceJob(string planningDate, string jobId, FinishMaintenanceJob cmd)
        {
            await _daprClient.InvokeMethodAsync(
                HttpMethod.Put,
                WorkshopManagementApi_AppId,
                $"{WorkshopManagementApi_Path}/{planningDate}/jobs/{jobId}/finish",
                new
                {
                    cmd.Notes,
                    cmd.EndTime,
                    cmd.JobId,
                    cmd.StartTime,
                    cmd.MessageId,
                    cmd.MessageType
                }
            );
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await _daprClient.InvokeMethodAsync<List<Customer>>(
                HttpMethod.Get,
                WorkshopManagementApi_AppId,
                $"/api/refdata/customers"
            );
        }

        public async Task<Customer> GetCustomerById(string id)
        {
            try
            {
                return await _daprClient.InvokeMethodAsync<Customer>(
                    HttpMethod.Get,
                    WorkshopManagementApi_AppId,
                    $"/api/refdata/customers/{id}"
                );
            }
            catch (InvocationException ex)
            {
                if (ex.InnerException is HttpRequestException httpEx)
                {
                    if (httpEx.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                }

                throw;
            }
        }

        public async Task<List<Vehicle>> GetVehicles()
        {
            return await _daprClient.InvokeMethodAsync<List<Vehicle>>(
                HttpMethod.Get,
                WorkshopManagementApi_AppId,
                $"/api//refdata/vehicles"
            );
        }

        public async Task<Vehicle> GetVehicleByLicenseNumber(string licenseNumber)
        {
            try
            {
                return await _daprClient.InvokeMethodAsync<Vehicle>(
                    HttpMethod.Get,
                    WorkshopManagementApi_AppId,
                    $"/api/refdata/vehicles/{licenseNumber}"
                );
            }
            catch (HttpRequestException ex)
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
    }
}