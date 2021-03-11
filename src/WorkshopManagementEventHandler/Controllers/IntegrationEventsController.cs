using System;
using System.Threading.Tasks;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pitstop.WorkshopManagementEventHandler.DataAccess;
using Pitstop.WorkshopManagementEventHandler.Events;
using Pitstop.WorkshopManagementEventHandler.Model;
using Serilog;

namespace Pitstop.WorkshopManagementEventHandler.Controllers
{
    [ApiController]
    public class IntegrationEventsController : ControllerBase
    {
        private readonly WorkshopManagementDBContext _dbContext;
        private const string DaprSubName = "pubsub";

        public IntegrationEventsController(WorkshopManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("CustomerRegistered")]
        [Topic(DaprSubName,"CustomerRegistered")]
        public async Task HandleAsync(CustomerRegistered e)
        {
            Log.Error("Customer registered");
            if (e.Id != Guid.Empty)
            {
                Log.Information("Register Customer: {CustomerId}, {Name}, {TelephoneNumber}",
                    e.CustomerId, e.Name, e.TelephoneNumber);

                try
                {
                    await _dbContext.Customers.AddAsync(new Customer
                    {
                        CustomerId = e.CustomerId,
                        Name = e.Name,
                        TelephoneNumber = e.TelephoneNumber
                    });
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    Log.Warning("Skipped adding customer with customer id {CustomerId}.", e.CustomerId);
                }
            }
            else
            {
                throw new Exception("TODO");
            }
        }

        [HttpPost("VehicleRegistered")]
        [Topic(DaprSubName, "VehicleRegistered")]
        public async Task HandleAsync(VehicleRegistered e)
        {
            Log.Information("Register Vehicle: {LicenseNumber}, {Brand}, {Type}, Owner Id: {OwnerId}",
                e.LicenseNumber, e.Brand, e.Type, e.OwnerId);
            Log.Error("Vehicle registered");

            try
            {
                await _dbContext.Vehicles.AddAsync(new Vehicle
                {
                    LicenseNumber = e.LicenseNumber,
                    Brand = e.Brand,
                    Type = e.Type,
                    OwnerId = e.OwnerId
                });
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                Console.WriteLine($"Skipped adding vehicle with license number {e.LicenseNumber}.");
            }
        }

        [HttpPost("MaintenanceJobPlanned")]
        [Topic(DaprSubName, "MaintenanceJobPlanned")]
        public async Task HandleAsync(MaintenanceJobPlanned e)
        {
            Log.Information(
                "Register Maintenance Job: {JobId}, {StartTime}, {EndTime}, {CustomerName}, {LicenseNumber}",
                e.JobId, e.StartTime, e.EndTime, e.CustomerInfo.Name, e.VehicleInfo.LicenseNumber);
            Log.Error("Maintenance registered");

            try
            {
                // determine customer
                Customer customer =
                    await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == e.CustomerInfo.Id);
                if (customer == null)
                {
                    customer = new Customer
                    {
                        CustomerId = e.CustomerInfo.Id,
                        Name = e.CustomerInfo.Name,
                        TelephoneNumber = e.CustomerInfo.TelephoneNumber
                    };
                }

                // determine vehicle
                Vehicle vehicle =
                    await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.LicenseNumber == e.VehicleInfo.LicenseNumber);
                if (vehicle == null)
                {
                    vehicle = new Vehicle
                    {
                        LicenseNumber = e.VehicleInfo.LicenseNumber,
                        Brand = e.VehicleInfo.Brand,
                        Type = e.VehicleInfo.Type,
                        OwnerId = customer.CustomerId
                    };
                }

                // insert maintetancejob
                await _dbContext.MaintenanceJobs.AddAsync(new MaintenanceJob
                {
                    Id = e.JobId,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Customer = customer,
                    Vehicle = vehicle,
                    WorkshopPlanningDate = e.StartTime.Date,
                    Description = e.Description
                });
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                Log.Warning("Skipped adding maintenance job with id {JobId}.", e.JobId);
            }
        }

        [HttpPost("MaintenanceJobFinished")]
        [Topic(DaprSubName, "MaintenanceJobFinished")]
        public async Task HandleAsync(MaintenanceJobFinished e)
        {
            Log.Information("Finish Maintenance job: {JobId}, {ActualStartTime}, {EndTime}",
                e.JobId, e.StartTime, e.EndTime);
            Log.Error("MaintenanceJob finished");

            try
            {
                // insert maintetancejob
                var job = await _dbContext.MaintenanceJobs.FirstOrDefaultAsync(j => j.Id == e.JobId);
                job.ActualStartTime = e.StartTime;
                job.ActualEndTime = e.EndTime;
                job.Notes = e.Notes;
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                Log.Warning("Skipped adding maintenance job with id {JobId}.", e.JobId);
            }
        }
    }
}