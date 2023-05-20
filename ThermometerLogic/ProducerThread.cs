namespace TemperatureAlertSystem.ThermometerLogic
{
   /* Author: David DLV
    * Date:5/18/2023
    * This class facilitates as the Temperature Alert distributor. It is a producer thread that will invoke the exact client thread
    * that has met the alert criteria.
    */
    public class ProducerThread
    {
        private readonly int SLEEP = 5000;
        public List<float> Temperatures { get; }
        private readonly Dictionary<int, Criterion> AlertCriteriaMap;
        private readonly AutoResetEvent[] consumerEvents;
        protected float Celsius { get; set; }       
        public float PreviousTemperature {get; set;}
        public float CurrentTemperature { get; set;}
        private bool DisplayEndingMessage = true;
    

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
                    if (Temperatures.Count != 0)
                    {
                        DisplayEndingMessage = true;

                        PreviousTemperature = Celsius;

                        Celsius = Temperatures[0];
                        Temperatures.RemoveAt(0);

                        Console.WriteLine($"Producer thread processing Temperature: {Celsius}°C");

                        for (int i = 0; i < consumerEvents.Length; i++)
                        {
                            Criterion criterion = AlertCriteriaMap[i];
                            bool alertConsumer = TemperatureAlertLogic.TemperatureUpdate(i, Celsius, PreviousTemperature, criterion);

                            if (alertConsumer)
                            {
                                Console.WriteLine($"Temperature {Celsius}°C has triggered an Alert for Consumer#{i}'s threshold. The Producer is Notifying Consumer#{i}...");
                                consumerEvents[i].Set();
                            }
                        }

                        Thread.Sleep(SLEEP);
                    }  
                    else
                    {
                        DisplayEndOfDataMessage();
                        DisplayEndingMessage = false;                                          
                    }
                }                
            }
        }

        private void DisplayEndOfDataMessage()
        {
            if (DisplayEndingMessage)
            {
                Console.WriteLine($"All Temperature Data successfully Processed. Final temperature processed was {Celsius}°C");
                Console.WriteLine($"Please Load more temperature data in Celsius °C scale for additional processing...");
            }
        }

        public AutoResetEvent GetConsumerEvent(int consumerIndex)
        {
            return consumerEvents[consumerIndex];
        }
    }
}
