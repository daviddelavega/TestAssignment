using TemperatureAlertSystem.Models;
using TemperatureAlertSystem.Mutators;
using TemperatureAlertSystem.Queries;

namespace TemperatureAlertSystem.ThermometerLogic
{
    public class ThermometerAlertSystem
    {
        private static ProducerThread ProducerThread;

        private static List<Temperature> TemperatureResults;

        public static ProducerThread GetProducerThread() 
        {
            return ProducerThread; 
        }

        public static void Start()
        {
            ProducerThread = new ProducerThread();
            ProducerThread.Start();
            CurrentTemperatureQuery.SetProducerThread(ProducerThread);
            Console.WriteLine("Producer Thread Started");
        }

        public static void AddConsumer(List<AlertCriteriaModel> alertCriteria)
        {
            var alertCriteriaMap = new Dictionary<int, Criterion>();            

            alertCriteria.ForEach(ac => {
                Console.WriteLine($"Creating Consumer#{ac.id} with Alert Criteria:" +
                $"\n\tArbitraryThreshold:{ac.arbitraryThreshold}" +
                $"\n\tInsignificantFluctuation:{ac.insignificantFluctuation}" +
                $"\n\tDirection:{ac.direction}");

                var criterion = new Criterion(ac.arbitraryThreshold, ac.insignificantFluctuation, ac.direction);
                alertCriteriaMap.Add(ac.id, criterion);         

            });

            alertCriteria.ForEach(ac =>
            {
                new ConsumerThread(ac.id, ThermometerAlertSystem.GetProducerThread()).Start();
            });

            ThermometerAlertSystem.GetProducerThread().SetAlertCriteriaMap(alertCriteriaMap);
        }

        public static void SetTemperatureResults(List<Temperature> _temperatureResults)
        {
            TemperatureResults = _temperatureResults;
        }

        public static List<Temperature> GetTemperatureResults()
        {
            return TemperatureResults;
        }
    }
}
