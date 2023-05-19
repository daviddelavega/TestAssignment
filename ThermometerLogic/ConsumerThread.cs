namespace TemperatureAlertSystem.ThermometerLogic
{
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
                    if (producerThread.Temperatures.Count == 0)
                    {
                        Console.WriteLine($"Consumer {consumerIndex}: Temperature Data is empty. Exiting the consumer thread.");
                        break;
                    }
                }

                Console.WriteLine($"Consumer#{consumerIndex}'s Has Received an Alert from the Producer: Processing Temperature Data...");
                Console.WriteLine($"Celcius: {producerThread.getCelsius()}°C");
                Console.WriteLine($"Fahrenheit: {producerThread.getFahrenheit()}°F");
            }
        }
    }
}