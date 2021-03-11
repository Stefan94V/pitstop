using Pitstop.Application.VehicleManagement.Commands;
using Pitstop.Infrastructure.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.Application.VehicleManagement.Events
{
    public class VehicleRegistered : Event
    {
        public string LicenseNumber  { get; private set; }
        public string Brand  { get; private set; }
        public string Type  { get; private set; }
        public string OwnerId  { get; private set; }
        
        public VehicleRegistered(string licenseNumber, string brand, string type, string ownerId) 
        {
            LicenseNumber = licenseNumber;
            Brand = brand;
            Type = type;
            OwnerId = ownerId;
        }

        public static VehicleRegistered FromCommand(RegisterVehicle command)
        {
            return new VehicleRegistered(
                command.LicenseNumber,
                command.Brand,
                command.Type,
                command.OwnerId
            );
        }
    }
}
