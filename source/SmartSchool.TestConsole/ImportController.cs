using System;
using System.Collections.Generic;
using SmartSchool.Core.Entities;
using Utils;
using System.Linq;
using System.IO;
using System.Text;

namespace SmartSchool.TestConsole
{
    public class ImportController
    {
        const string Filename = "measurements.csv";

        /// <summary>
        /// Liefert die Messwerte mit den dazugehörigen Sensoren
        /// </summary>
        public static IEnumerable<Measurement> ReadFromCsv()
        {
            bool skipRow = true;
            IList<Measurement> measurements = new List<Measurement>();
            IDictionary<string, Sensor> sensors = new Dictionary<string, Sensor>();
            string filePath = MyFile.GetFullNameInApplicationTree(Filename);
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            foreach (var item in lines)
            {
                if(skipRow == false)
                {
                    string[] splitParts = item.Split(";");
                    string location = splitParts[2].Split("_")[0];
                    string name = splitParts[2].Split("_")[1];
                    DateTime dateTime = DateTime.Parse($"{splitParts[0]} {splitParts[1]}");

                    Measurement measurement = new Measurement() { Time = dateTime, Value = Convert.ToDouble(splitParts[3]) };

                    if (sensors.TryGetValue(splitParts[2], out var item2) != true)
                    {
                        Sensor newSensor = new Sensor()
                        {
                            Name = name,
                            Location = location
                        };
                        measurement.Sensor = newSensor;
                        newSensor.Measurements.Add(measurement);
                        sensors.Add(splitParts[2], newSensor);
                    }
                    else
                    {
                        Sensor sensor = sensors
                            .Values
                            .SingleOrDefault(s => s.Name.Equals(name) && s.Location.Equals(location));
                        measurement.Sensor = sensor;
                        sensor.Measurements.Add(measurement);
                    }
                    measurements.Add(measurement);
                }
                if(skipRow == true)
                {
                    skipRow = false;
                }
                
            }

            return measurements;
        }

    }
}
