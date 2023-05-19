using TemperatureAlertSystem.Enums;

namespace TemperatureAlertSystem.ThermometerLogic
{
    public class TemperatureAlertLogic
    {
        private static float Temperature;
        private Dictionary<string, Criterion> AlertCriteria;

        public TemperatureAlertLogic()
        {
            Temperature = 0.0f;
            AlertCriteria = new Dictionary<string, Criterion>();
        }
      
        public static bool TemperatureUpdate(float _nextTemperature, Criterion criterion)
        { 
            float previousTemperature = Temperature;
            Temperature = _nextTemperature;            
            
            bool alertConsumer = false;   
            float arbitraryThreshold = criterion.ArbitraryThreshold;
            float insignificantFluctuation = criterion.InsignificantFluctuation;
            Direction direction = criterion.Direction;          

            if (direction != Direction.None)
            {
                if (direction == Direction.Rising && previousTemperature < arbitraryThreshold && Temperature >= arbitraryThreshold)
                {
                    alertConsumer = true;                    
                }
                else if (direction == Direction.Falling && previousTemperature > arbitraryThreshold && Temperature <= arbitraryThreshold)
                {
                    alertConsumer = true;                  
                }                
            }
            else if (Math.Abs(Temperature - arbitraryThreshold) > insignificantFluctuation)
            {
                alertConsumer = true;               
            }      

            if (alertConsumer) 
            {
                Console.WriteLine($"Threshold breached: ArbitraryThreshold:{arbitraryThreshold}°C" +
                        $"\ninsignificantFluctuation:{insignificantFluctuation}" +
                        $"\ndirection:{direction}" +
                        $"\nPreviousTemperature:{previousTemperature}°C" +
                        $"\nCurrentTemperature:{Temperature}");
                Console.WriteLine($"Alerting Consumer that Criterion is reached: ({arbitraryThreshold}°C)");
            }

            return alertConsumer;
            
        }
    }   
}
