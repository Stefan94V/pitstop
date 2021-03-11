﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pitstop.Application.VehicleManagement.Model;
using Pitstop.Application.VehicleManagement.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Pitstop.Infrastructure.Messaging;
using Pitstop.Application.VehicleManagement.Events;
using Pitstop.Application.VehicleManagement.Commands;
using Pitstop.VehicleManagementAPI.Mappers;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Pitstop.Application.VehicleManagement.Controllers
{
    [Route("/api/[controller]")]
    public class VehiclesController : Controller
    {
        private const string NUMBER_PATTERN = @"^((\d{1,3}|[a-z]{1,3})-){2}(\d{1,3}|[a-z]{1,3})$";
        IMessagePublisher _messagePublisher;
        private readonly ILogger<VehiclesController> _logger;
        VehicleManagementDBContext _dbContext;

        public VehiclesController(VehicleManagementDBContext dbContext, IMessagePublisher messagePublisher, ILogger<VehiclesController> _logger)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
            this._logger = _logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _dbContext.Vehicles.ToListAsync());
        }

        [HttpGet]
        [Route("{licenseNumber}", Name = "GetByLicenseNumber")]
        public async Task<IActionResult> GetByLicenseNumber(string licenseNumber)
        {
            var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.LicenseNumber == licenseNumber);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterVehicle command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // check invariants
                    if (!Regex.IsMatch(command.LicenseNumber, NUMBER_PATTERN, RegexOptions.IgnoreCase))
                    {
                        return BadRequest(
                            $"The specified license-number '{command.LicenseNumber}' was not in the correct format.");
                    }

                    // insert vehicle
                    Vehicle vehicle = command.MapToVehicle();
                    _dbContext.Vehicles.Add(vehicle);
                    await _dbContext.SaveChangesAsync();

                    try
                    {

                        // send event
                        var e = VehicleRegistered.FromCommand(command);
                        await _messagePublisher.PublishMessageAsync(e);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex, "Could not publish");
                        if(ex.InnerException != null)
                            _logger.LogCritical(ex.InnerException,"Could not publish in dapr" );
                    }

                    //return result
                    return CreatedAtRoute("GetByLicenseNumber", new {licenseNumber = vehicle.LicenseNumber}, vehicle);
                }

                return BadRequest();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists " +
                                             "see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}