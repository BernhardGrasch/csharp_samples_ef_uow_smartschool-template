using SmartSchool.Core.Entities;
using System.Collections.Generic;

namespace SmartSchool.Core.Contracts
{
    public interface IMeasurementRepository
    {
        void AddRange(Measurement[] measurements);

        Measurement[] GetAllMeasurementsByLocationAndName(string location, string name);

        IEnumerable<Measurement> GetValidCo2MeasurementsInOffice(string location, int min, int max);
    }
}
