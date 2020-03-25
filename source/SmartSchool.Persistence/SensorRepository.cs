using SmartSchool.Core.Contracts;
using SmartSchool.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SmartSchool.Persistence
{
    public class SensorRepository : ISensorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SensorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddRange(List<Sensor> sensors) => _dbContext.AddRange(sensors);
        

        public IEnumerable<Sensor> GetAllSensors()
        {
            return _dbContext.Sensors
                .Include(s => s.Measurements);
        }

        public Sensor GetSensorByLocationAndName(string location, string name)
        {
            return _dbContext.Sensors
                .Include(s => s.Measurements)
                .Where(s => s.Location.Equals(location))
                .Where(s => s.Name.Equals(name))
                .SingleOrDefault();
        }
    }
}