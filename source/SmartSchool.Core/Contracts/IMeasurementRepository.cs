using SmartSchool.Core.Entities;
using System.Collections.Generic;

namespace SmartSchool.Core.Contracts
{
    public interface IMeasurementRepository
    {
        void AddRange(Measurement[] measurements);

        IEnumerable<Measurement> GetAllMeasurementsByLocationAndName(string location, string name);

    }
}
