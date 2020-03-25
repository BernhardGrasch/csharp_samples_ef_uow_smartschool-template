using System;
using System.Linq;
using SmartSchool.Core.Entities;
using SmartSchool.Persistence;

namespace SmartSchool.TestConsole
{
	class Program
	{
		static void Main()
		{
			Console.WriteLine("Import der Measurements und Sensors in die Datenbank");
			using (UnitOfWork unitOfWorkImport = new UnitOfWork())
			{
				Console.WriteLine("Datenbank löschen");
				unitOfWorkImport.DeleteDatabase();
				Console.WriteLine("Datenbank migrieren");
				unitOfWorkImport.MigrateDatabase();
				Console.WriteLine("Messwerte werden von measurements.csv eingelesen");
				var measurements = ImportController.ReadFromCsv().ToArray();
				if (measurements.Length == 0)
				{
					Console.WriteLine("!!! Es wurden keine Messwerte eingelesen");
					return;
				}

				Console.WriteLine(
						$"  Es wurden {measurements.Count()} Messwerte eingelesen, werden in Datenbank gespeichert ...");
				unitOfWorkImport.MeasurementRepository.AddRange(measurements);
				int countSensors = measurements.GroupBy(m => m.Sensor).Count();
				int savedRows = unitOfWorkImport.SaveChanges();
				Console.WriteLine(
						$"{countSensors} Sensoren und {savedRows - countSensors} Messwerte wurden in Datenbank gespeichert!");
				Console.WriteLine();
			}

			using (UnitOfWork unitOfWork = new UnitOfWork())
			{
				Console.WriteLine("Import beendet, Test der gespeicherten Daten");
				Console.WriteLine("--------------------------------------------");
				Console.WriteLine();

				var count = unitOfWork.MeasurementRepository
                    .GetAllMeasurementsByLocationAndName("livingromm", "temperature").Count();   // TODO

				Console.WriteLine($"Anzahl Messwerte für Sensor temperature in location livingroom: {count}");
				Console.WriteLine();



                var greatestmeasurements = unitOfWork.MeasurementRepository
                    .GetAllMeasurementsByLocationAndName("livingroom", "temperature");  // TODO

                Console.WriteLine("Letzte 3 höchste Temperaturmesswerte im Wohnzimmer");
				WriteMeasurements(greatestmeasurements);
				Console.WriteLine();



                var average = unitOfWork.MeasurementRepository
                    .GetValidCo2MeasurementsInOffice("office", 300, 5000)
                    .Average(m => m.Value);
				Console.WriteLine($"Durchschnitt der gültigen Co2-Werte (>300, <5000) im office: {average}");
				Console.WriteLine();
				Console.WriteLine("Alle Sensoren mit dem Durchschnitt der Messwerte");
                var allAverage = unitOfWork.SensorRepository
                    .GetAllSensors();

                foreach(var item in allAverage)
                {
                    Console.WriteLine($"{item.Location} {item.Name} {item.Measurements}");
                }
                    

			}

			Console.Write("Beenden mit Eingabetaste ...");
			Console.ReadLine();
		}

		private static void WriteMeasurements(Measurement[] measurements)
		{
			Console.WriteLine("Date       Time     Value");
			for (int i = 0; i < measurements.Length; i++)
			{
				Console.WriteLine($"{measurements[i].Time} {measurements[i].Value}°");
			}
		}
	}
}
