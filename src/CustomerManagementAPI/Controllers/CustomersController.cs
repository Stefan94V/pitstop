﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Pitstop.CustomerManagementAPI.DataAccess;
using Pitstop.CustomerManagementAPI.Model;
using Pitstop.Infrastructure.Messaging;
using Pitstop.CustomerManagementAPI.Events;
using Pitstop.CustomerManagementAPI.Commands;
using Pitstop.CustomerManagementAPI.Mappers;
using Serilog;
using System;
using System.IO;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Pitstop.Application.CustomerManagementAPI.Controllers
{
    [Route("/api/[controller]")]
    public class CustomersController : Controller
    {
        IMessagePublisher _messagePublisher;
        private readonly ILogger<CustomersController> _logger;
        CustomerManagementDBContext _dbContext;

        public CustomersController(
            CustomerManagementDBContext dbContext,
            IMessagePublisher messagePublisher,
            ILogger<CustomersController> logger)
        {
            _dbContext = dbContext;
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _dbContext.Customers.ToListAsync());
        }

        [HttpGet]
        [Route("{customerId}", Name = "GetByCustomerId")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterCustomer command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Polly seems to create multiple when retrying
                    if (await _dbContext.Customers.AnyAsync(x => x.EmailAddress == command.EmailAddress))
                        return BadRequest("Email already has an account");
                    
                    // insert customer
                    Customer customer = command.MapToCustomer();

                    _dbContext.Customers.Add(customer);
                    await _dbContext.SaveChangesAsync();

                    // send event
                    CustomerRegistered e = command.MapToCustomerRegistered();
                    try
                    {
                        await _messagePublisher.PublishMessageAsync(e);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Could not publish");
                    }

                    // return result
                    return CreatedAtRoute("GetByCustomerId", new {customerId = customer.CustomerId}, customer);
                }

                return BadRequest();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                                             "Try again, and if the problem persists " +
                                             "see your system administrator.");
                return StatusCode(StatusCodes.Status500InternalServerError);
                throw;
            }
        }
    }
}