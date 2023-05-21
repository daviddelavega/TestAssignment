using HotChocolate.Types;
using TemperatureAlertSystem.Enums;
using TemperatureAlertSystem.Models;

namespace TemperatureAlertSystem.ThermometerLogic
{
   /* Author: David DLV
    * Date:5/18/2023
    * This class acts as the brain of the thermometer, the logic within defines exacly all the conditions of when to alert a caller, and when
    * not to alert a caller.
    */
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
                        $"\nInsignificantFluctuation:+/-{insignificantFluctuation}°C" +
                        $"\nDirection:{direction}" +
                        $"\nPreviousTemperature:{previousTemperature}°C" +
                        $"\nCurrentTemperature:{currentTemperature}°C");               
            }
            else
            {
                Console.WriteLine($"\n***************\n" +
                        $"Consumer{consumerId}'s Threshold NOT! breached: " +
                        $"\nArbitraryThreshold:{arbitraryThreshold}°C" +
                        $"\nInsignificantFluctuation:+/-{insignificantFluctuation}°C" +
                        $"\nDirection:{direction}" +
                        $"\nPreviousTemperature:{previousTemperature}°C" +
                        $"\nCurrentTemperature:{currentTemperature}°C");
            }

            return alertConsumer;
            
        }

        public static List<Temperature> CheckTemperature(int consumerId, List<float> temperatures, Criterion criterion)
        {         
            var temperatureList = new List<Temperature>();

            float previousTemperature = temperatures[0];

            Console.WriteLine($"Processing Temperature: {previousTemperature}°C");

            bool threshold_breached = false;
            bool alertConsumer = false;
            var fluxCapacitor = 0f;

            for (int i = 1; i < temperatures.Count; i++)
            {
                float currentTemperature = temperatures[i];

                Console.WriteLine($"Processing Temperature: {currentTemperature}°C");

                if (criterion.Direction != Direction.None)
                {
                    if (criterion.Direction == Direction.Rising && previousTemperature < criterion.ArbitraryThreshold && currentTemperature >= criterion.ArbitraryThreshold)
                    {
                        alertConsumer = true;
                    }
                    else if (criterion.Direction == Direction.Falling && previousTemperature > criterion.ArbitraryThreshold && currentTemperature <= criterion.ArbitraryThreshold)
                    {
                        alertConsumer = true;
                    }
                }
                else
                {
                    var abs_insignificantFluctuation = Math.Abs(criterion.InsignificantFluctuation);
                    var abs_previousTemperature = Math.Abs(previousTemperature);

                    if (!alertConsumer && (abs_previousTemperature > abs_insignificantFluctuation))
                    {
                        fluxCapacitor += Math.Abs(previousTemperature);
                    }

                    bool abs_insignificant_breached = (fluxCapacitor > abs_insignificantFluctuation);

                    if (currentTemperature == criterion.ArbitraryThreshold)
                    {
                        threshold_breached = true;
                    }

                    bool preliminaryResult = CheckPreliminaryResult(currentTemperature, previousTemperature, criterion.ArbitraryThreshold);
                    alertConsumer = threshold_breached && preliminaryResult && abs_insignificant_breached;
                }

                if (alertConsumer)
                {
                    Console.WriteLine($"\n***************\n" +
                        $"Consumer{consumerId}'s Threshold Breached!!: " +
                        $"\nArbitraryThreshold:{criterion.ArbitraryThreshold}°C" +
                        $"\nInsignificantFluctuation:+/-{criterion.InsignificantFluctuation}°C" +
                        $"\nDirection:{criterion.Direction}" +
                        $"\nPreviousTemperature:{previousTemperature}°C" +
                        $"\nCurrentTemperature:{currentTemperature}°C");

                    var temperatureModel = new Temperature();
                    temperatureModel.Fahrenheit = ThermometerAlertSystem.GetProducerThread().GetFahrenheit();
                    temperatureModel.Celsius = currentTemperature;
                    temperatureModel.Message =
                            $"Temperature Alert for Consumer#{consumerId}'s arbitraryThreshold. " +
                            $"The System is Notifying Consumer#{consumerId}...";

                    temperatureList.Add(temperatureModel);

                    fluxCapacitor = 0;
                }

                    previousTemperature = currentTemperature;
                
            }         

            return temperatureList;
        }
        private static bool CheckPreliminaryResult(float currentTemperature, float previousTemperature, float arbitraryThreshold)
        {
            bool result = false;
            if ((currentTemperature >= arbitraryThreshold && previousTemperature < arbitraryThreshold)
                || (currentTemperature <= arbitraryThreshold && previousTemperature > arbitraryThreshold))
            {
                result = true;
            }

            return result;
        }
    }   
}
