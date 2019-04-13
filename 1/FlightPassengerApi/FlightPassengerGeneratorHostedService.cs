using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlightPassengerHttpClient;
using Microsoft.Extensions.Logging;
using Bogus;
using Bogus.DataSets;

namespace FlightPassengerApi
{
    public class FlightPassengerGeneratorHostedService : BackgroundService
    { 
        //private System.Timers.Timer _timer = new System.Timers.Timer();
        private readonly DataBase _db;
        public FlightPassengerGeneratorHostedService(DataBase db)
        {
            _db = db;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var fakePassports = new Faker<Passport>()
                .StrictMode(true)
                .RuleFor(x => x.Guid, f => f.Random.Guid())
                .RuleFor(x => x.DateOfBirth, f => f.Person.DateOfBirth)
                .RuleFor(x => x.GivenNames, f => f.Person.FirstName)
                .RuleFor(x => x.Surname, f => f.Person.LastName)
                .RuleFor(x => x.Nationality, f => f.Address.Country())
                .RuleFor(x => x.Sex, f => f.Random.Enum<Sex>());
            var generator = new Faker<FlightPassenger>()
                .RuleFor(x => x.Passport, () => fakePassports)
                .RuleFor(x => x.BaggageWeight, f => f.Random.UInt(0, 100))
                .RuleFor(x => x.TypeOfFood, f => f.Random.Enum<TypeOfFood>());
            /*
            _timer.Elapsed += delegate {
                var flightPassenger = generator.Generate();
                flightPassenger.Start();
            };
            _timer.Interval = 5000;
            _timer.Start();
            */
            while (!stoppingToken.IsCancellationRequested)
            {
                _ = Task.Run(() =>
                {
                    var flightPassenger = generator.Generate();
                    _db.flightPassengers.Add(flightPassenger);
                    flightPassenger.Start();
                });
                await Task.Delay(1000);
            } 
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            //_timer.Stop();
            return base.StopAsync(cancellationToken);
        }
        public override void Dispose()
        {
            //_timer?.Dispose();
            base.Dispose();
        }
    }
}
