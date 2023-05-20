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

        private void ConsumeTemperature(object state)
        {
            while (true)
            {
                producerThread.GetConsumerEvent(consumerIndex).WaitOne(); // Wait for notification

                lock (producerThread)
                {
                    Console.WriteLine($"Consumer#{consumerIndex}'s Has Received an Alert from the Producer: Processing Temperature Data...");
                    Console.WriteLine($"***************");
                    Console.WriteLine($"Celsius: {producerThread.getCelsius()}°C -> Consumer#{consumerIndex}");
                    Console.WriteLine($"Fahrenheit: {producerThread.getFahrenheit()}°F -> Consumer#{consumerIndex}");                   
                }                
            }
        }
    }
}