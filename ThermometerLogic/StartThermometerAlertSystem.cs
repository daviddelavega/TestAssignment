using TemperatureAlertSystem.Queries;
using TemperatureAlertSystem.Utilities;

namespace TemperatureAlertSystem.ThermometerLogic
{
    public class StartThermometerAlertSystem
    {
        List<float> InputTemperatures;

        public StartThermometerAlertSystem(List<float> _inputTemperatures) 
        {
            this.InputTemperatures = _inputTemperatures;
        }
        public void Start()
        {
            var criteriaMap = AlertCriteriaModelReader.ReadAlertCriteriaData();

            var producer = new ProducerThread(InputTemperatures, criteriaMap);        

            var consumers = new List<ConsumerThread>();

            for (int i = 0; i < criteriaMap.Keys.Count; i++)
            {
                var consumerThread = new ConsumerThread(i, producer);
                consumers.Add(consumerThread);
                consumerThread.Start();
            }

            producer.Start();

            CurrentTemperatureQuery.SetProducerThread(producer);
        }
    }
}
