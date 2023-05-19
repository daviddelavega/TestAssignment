namespace TemperatureAlertSystem.ThermometerLogic
{
    //Producer Thread
    public class ProducerThread
    {
        private readonly int SLEEP = 2000;
        public List<float> Temperatures { get; }
        private readonly Dictionary<int, Criterion> AlertCriteriaMap;
        private readonly AutoResetEvent[] consumerEvents;
        protected float Celsius { get; set; }
        private float Fahrenheit; 
    

        public ProducerThread(List<float> _temperatures, Dictionary<int, Criterion> _alertCriteriaMap)
        {
            Temperatures = _temperatures;
            AlertCriteriaMap = _alertCriteriaMap;
            consumerEvents = new AutoResetEvent[AlertCriteriaMap.Count];
            InitializeConsumerEvents();
        }

        private void InitializeConsumerEvents()
        {
            for (int i = 0; i < consumerEvents.Length; i++)
            {
                consumerEvents[i] = new AutoResetEvent(false);
            }
        }

        public float getCelsius()
        {
            return Celsius;
        }

        public float getFahrenheit()
        {
            return ScaleConverter.ConvertToFahrenheit(Celsius);
        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem(ProcessTemperature);
        }

        private void ProcessTemperature(object state)
        {
            while (true)
            {               
                lock (Temperatures)
                {
                    if (Temperatures.Count == 0)
                    {
                        Console.WriteLine("Temperatures list is empty. Exiting the producer thread.");
                        break;
                    }

                    Celsius = Temperatures[0];
                    Temperatures.RemoveAt(0);
                }

                Console.WriteLine($"Producer thread processing Temperature: {Celsius}°C");

                for (int i = 0; i < consumerEvents.Length; i++)
                {
                    Criterion criterion = AlertCriteriaMap[i];
                    bool alertConsumer = TemperatureAlertLogic.TemperatureUpdate(Celsius, criterion);

                    if (alertConsumer)
                    {
                        Console.WriteLine($"Temperature {Celsius}°C has triggered an Alert for Consumer#{i}'s threshold. The Producer is Notifying Consumer#{i}...");
                        consumerEvents[i].Set();
                    }
                }

                Thread.Sleep(SLEEP);
            }
        }

        public AutoResetEvent GetConsumerEvent(int consumerIndex)
        {
            return consumerEvents[consumerIndex];
        }
    }
}
