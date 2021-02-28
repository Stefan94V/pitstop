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
using System.Threading;
using Dapr.Client;
using Newtonsoft.Json;

namespace WebApp.RESTClients
{
    public class CustomerManagementAPI : ICustomerManagementAPI
    {
        private readonly DaprClient _daprClient;
        private const string CustomerManagementApi_AppId = "customermanagement-api";
        private const string CustomerManagementApi_Path = "/api/customers";

        public CustomerManagementAPI(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await _daprClient.InvokeMethodAsync<List<Customer>>(
                HttpMethod.Get,
                CustomerManagementApi_AppId,
                CustomerManagementApi_Path
            );
        }

        public async Task<Customer> GetCustomerById(string customerId)
        {
            try
            {
                return await _daprClient.InvokeMethodAsync<Customer>(
                    HttpMethod.Get,
                    CustomerManagementApi_AppId,
                    $"{CustomerManagementApi_Path}/{customerId}");
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

        public async Task RegisterCustomer(RegisterCustomer command)
        {
            await _daprClient.InvokeMethodAsync(
                HttpMethod.Post,
                CustomerManagementApi_AppId,
                CustomerManagementApi_Path,
                new
                {
                    command.CustomerId,
                    command.Address,
                    command.City,
                    command.Name,
                    command.EmailAddress,
                    command.PostalCode,
                    command.TelephoneNumber,
                    command.MessageId
                });
        }
    }
}