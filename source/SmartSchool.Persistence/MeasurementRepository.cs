using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SmartSchool.Persistence
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private ApplicationDbContext _dbContext;
        
        public MeasurementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  void AddRange(Measurement[] measurements)
        {
            _dbContext.Measurements.AddRange(measurements);
        }

        public Measurement[] GetAllMeasurementsByLocationAndName(string location, string name)
        {
            return _dbContext.Measurements
                .Include(s => s.Sensor)
                .Where(s => s.Sensor.Location.Equals(location) && s.Sensor.Name.Equals(name))
                .OrderByDescending(m => m.Value)
                .ThenByDescending(m => m.Time)
                .Take(3)
                .ToArray();
        }

        public IEnumerable<Measurement> GetValidCo2MeasurementsInOffice(string location, int min, int max)
        {
            return _dbContext.Measurements
                .Include(s => s.Sensor)
                .Where(m => m.Sensor.Location.Equals(location))
                .Where(m => m.Sensor.Name == "co2")
                .Where(m => m.Value <= max)
                .Where(m => m.Value >= min);
        }
    }
}