using SmartSchool.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SmartSchool.Core.Contracts
{
    public interface ISensorRepository
    {
        void AddRange(List<Sensor> sensors);
        Sensor GetSensorByLocationAndName(string location, string name);
        IEnumerable<Sensor> GetAllSensors();
    }
}
