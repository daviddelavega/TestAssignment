using TemperatureAlertSystem.Enums;

namespace TemperatureAlertSystem.ThermometerLogic
{
    public class TemperatureAlertLogic
    {       
             
        public static bool TemperatureUpdate(int consumerId, float currentTemperature, float previousTemperature, Criterion criterion)
        {            
            bool alertConsumer = false;   
            float arbitraryThreshold = criterion.ArbitraryThreshold;
            float insignificantFluctuation = criterion.InsignificantFluctuation;
            Direction direction = criterion.Direction;          

            if (direction != Direction.None)
            {
                if (direction == Direction.Rising && previousTemperature < arbitraryThreshold && currentTemperature >= arbitraryThreshold)
                {
                    alertConsumer = true;                    
                }
                else if (direction == Direction.Falling && previousTemperature > arbitraryThreshold && currentTemperature <= arbitraryThreshold)
                {
                    alertConsumer = true;                  
                }                
            }
            else if (Math.Abs(currentTemperature - arbitraryThreshold) > insignificantFluctuation)
            {
                alertConsumer = true;               
            }      

            if (alertConsumer) 
            {
                Console.WriteLine($"\n***************\n" +
                        $"Consumer{consumerId}'s Threshold Breached!!: " +
                        $"\nArbitraryThreshold:{arbitraryThreshold}°C" +
                        $"\ninsignificantFluctuation:+/-{insignificantFluctuation}°C" +
                        $"\ndirection:{direction}" +
                        $"\nPreviousTemperature:{previousTemperature}°C" +
                        $"\nCurrentTemperature:{currentTemperature}°C");               
            }
            else
            {
                Console.WriteLine($"\n***************\n" +
                        $"Consumer{consumerId}'s Threshold NOT! breached: " +
                        $"\nArbitraryThreshold:{arbitraryThreshold}°C" +
                        $"\ninsignificantFluctuation:+/-{insignificantFluctuation}°C" +
                        $"\ndirection:{direction}" +
                        $"\nPreviousTemperature:{previousTemperature}°C" +
                        $"\nCurrentTemperature:{currentTemperature}°C");
            }

            return alertConsumer;
            
        }
    }   
}
