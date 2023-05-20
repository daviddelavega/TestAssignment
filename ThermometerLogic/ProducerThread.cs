using TemperatureAlertSystem.Models;

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
        private List<float> Temperatures { get; set; }
        private Dictionary<int, Criterion> AlertCriteriaMap { get; set; }
        private AutoResetEvent[] consumerEvents;
        protected float Celsius { get; set; }       
        public float PreviousTemperature {get; set;}
        public float CurrentTemperature { get; set;}
        private bool DisplayEndingMessage = true;        

        public ProducerThread()
        {
            Temperatures = new();
            AlertCriteriaMap = new();
        }       

        public ProducerThread SetTemperatures(List<float> _temperatures) 
        {
            this.Temperatures = _temperatures;
            return this;
        }

        public ProducerThread SetAlertCriteriaMap(Dictionary<int, Criterion> _alertCriteriaMap)
        {
            this.AlertCriteriaMap = _alertCriteriaMap;            
            InitializeConsumerEvents();
            return this;
        }

        private void InitializeConsumerEvents()
        {
            consumerEvents = new AutoResetEvent[AlertCriteriaMap.Count];

            foreach(KeyValuePair<int, Criterion> kvp in AlertCriteriaMap)
            {
                consumerEvents[kvp.Key] = new AutoResetEvent(false);
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
                    if (consumerEvents != null && Temperatures.Count != 0 && AlertCriteriaMap.Count != 0)
                    {
                        DisplayEndingMessage = true;

                        PreviousTemperature = Celsius;

                        Celsius = Temperatures[0];
                        Temperatures.RemoveAt(0);

                        Console.WriteLine($"Producer thread processing Temperature: {Celsius}°C");

                        foreach (KeyValuePair<int, Criterion> kvp in AlertCriteriaMap)
                        {
                            Criterion criterion = kvp.Value;

                            bool alertConsumer = TemperatureAlertLogic.TemperatureUpdate(kvp.Key, Celsius, PreviousTemperature, criterion);

                            if (alertConsumer)
                            {
                                Console.WriteLine($"Temperature {Celsius}°C has triggered an Alert for Consumer#{kvp.Key}'s threshold. The Producer is Notifying Consumer#{kvp.Key}...");
                                consumerEvents[kvp.Key].Set();
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
