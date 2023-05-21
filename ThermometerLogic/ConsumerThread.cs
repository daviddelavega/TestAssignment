namespace TemperatureAlertSystem.ThermometerLogic
{
   /* Author: David DLV
    * Date:5/18/2023
    * This class acts as the caller/consumer of Thermometer Temperature Alerts. The Producer thread will invoke the consumer thread only when
    * the alert criteria is met.
    */
    public class ConsumerThread
    {
        private readonly int consumerIndex;
        private readonly ProducerThread producerThread;

        public ConsumerThread(int index, ProducerThread producer)
        {
            consumerIndex = index;
            producerThread = producer;
        }

        public void Start()
        {
            ThreadPool.QueueUserWorkItem(ConsumeTemperature);
        }

        private async void ConsumeTemperature(object state)
        {
            while (true)
            {
                producerThread.GetConsumerEvent(consumerIndex).WaitOne(); // Wait for notification

                lock (producerThread)
                {
                    var temperatureResults = ThermometerAlertSystem.GetTemperatureResults();

                    temperatureResults.ForEach(temperature =>
                        Console.WriteLine(
                            $"***************" +
                            $"\nConsumer#{consumerIndex} Processing Temperature Results Data:" +
                            $"\nCelsius:{temperature.Celsius}°C -> Consumer#{consumerIndex}" +
                            $"\nFahrenheit:{temperature.Fahrenheit}°F -> Consumer#{consumerIndex}" +
                            $"\n***************")
                    );                               
                }                
            }
        }
    }
}