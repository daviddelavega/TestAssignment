using TemperatureAlertSystem.Mutators;
using TemperatureAlertSystem.Queries;

namespace TemperatureAlertSystem.ThermometerLogic
{
    public class ThermometerAlertSystem
    {
        private static ProducerThread producerThread;       

        public static ProducerThread GetProducerThread() 
        {
            return producerThread; 
        }

        public static void Start()
        {
            producerThread = new ProducerThread();
            producerThread.Start();
            CurrentTemperatureQuery.SetProducerThread(producerThread);
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
    }
}
