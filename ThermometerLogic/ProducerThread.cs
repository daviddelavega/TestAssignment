namespace TemperatureAlertSystem.ThermometerLogic
{
   /* Author: David DLV
    * Date:5/18/2023
    * This class facilitates as the Temperature Alert distributor. It is a producer thread that will invoke the exact client thread
    * that has met the alert criteria.
    */
    public class ProducerThread
    {
        private readonly int SLEEP = 1500;
        private List<float> Temperatures { get; set; }
        private Dictionary<int, Criterion> AlertCriteriaMap { get; set; }
        private AutoResetEvent[] consumerEvents;
        protected float Celsius { get; set; }       
        public float PreviousTemperature {get; set;}
        public float CurrentTemperature { get; set;}    

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

        public float GetCelsius()
        {
            return Celsius;
        }

        public void SetCelsius(float _celsius)
        {
            this.Celsius = _celsius;
        }

        public float GetFahrenheit()
        {
            return ScaleConverter.ConvertToFahrenheit(Celsius);
        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem(ProcessTemperature);
        }

        private async void ProcessTemperature(object state)
        {
            while (true)
            {               
                lock (Temperatures)
                {
                    if (consumerEvents != null && Temperatures.Count != 0 && AlertCriteriaMap.Count != 0)
                    {
                        foreach (KeyValuePair<int, Criterion> kvp in AlertCriteriaMap)
                        {
                            Criterion criterion = kvp.Value;

                            var temperatureResultsList = TemperatureAlertLogic.CheckTemperature(kvp.Key, Temperatures, criterion);
                          
                            if (temperatureResultsList.Count > 0)
                            {
                                ThermometerAlertSystem.SetTemperatureResults(temperatureResultsList);                               
                                consumerEvents[kvp.Key].Set();
                            }
                        }

                        Thread.Sleep(SLEEP);

                        Array.Clear(consumerEvents, 0, consumerEvents.Length);
                        Temperatures.Clear();
                        AlertCriteriaMap.Clear();

                        Console.WriteLine(
                           $"*************" +
                           $"\nAll Temperature Data is Successfully Processed." +
                           $"\nPlease send More Temperature Data and Add More Consumer(s) In Any Order" +
                           $"\n***********");
                    }                   
                }                
            }
        }       

        public AutoResetEvent GetConsumerEvent(int consumerIndex)
        {
            return consumerEvents[consumerIndex];
        }
    }
}
