using TemperatureAlertSystem.Enums;
using TemperatureAlertSystem.Models;

namespace TemperatureAlertSystem.ThermometerLogic
{
   /* Author: David DLV
    * Date:5/18/2023
    * This class acts as the brain of the thermometer, the logic within defines exacly all the conditions of when to alert a caller, and when
    * not to alert a caller.
    * The FluxCapacitor is the key to the logic of the Temperature alert System Algorithm I developed. The FluxCapacitor, is essentially calculating the
    * Temperature Flux i.e. the Temperature changes over time, the flux capacitor tracks the amount of flux since the last time Marty(a caller) was alerted
    * the Flux Capacitor is there as code logic to account for a temperature data set that blips along at a low rate. It is 
    * essential to the algorithm I created because the thermometer reading could +/- at a rate lower than the insignificant value, adding up to a big Flux.
    * The Flux Capacitor is also a Key ingredient to Time Travel....
    */
    public class TemperatureAlertLogic
    {
        private static float FluxCapacitor;
        private static bool AlertMarty;
        public static List<Temperature> CheckTemperature(int consumerId, List<float> temperatures, Criterion criterion)
        {         
            var temperatureList = new List<Temperature>();

            float previousTemperature = temperatures[0];

            Console.WriteLine($"Processing Temperature: {previousTemperature}°C");
           
            AlertMarty = false;
            FluxCapacitor = 0f;

            for (int i = 1; i < temperatures.Count; i++)
            {
                float currentTemperature = temperatures[i];

                Console.WriteLine($"Processing Temperature: {currentTemperature}°C");
                
                if (criterion.Direction == Direction.Rising)
                {                    
                    if (currentTemperature > previousTemperature)
                    {
                        AlertMarty = FluxCapacitor_Activated(currentTemperature, previousTemperature, criterion);
                    }
                    else
                    {
                        AlertMarty = false;
                    }
                }
                else if (criterion.Direction == Direction.Falling) 
                {
                    if (currentTemperature < previousTemperature)
                    {
                        AlertMarty = FluxCapacitor_Activated(currentTemperature, previousTemperature, criterion);
                    }
                    else
                    {
                        AlertMarty = false;
                    }                    
                }             
                else if (criterion.Direction == Direction.None)
                {
                    AlertMarty = FluxCapacitor_Activated(currentTemperature, previousTemperature, criterion);
                }

                if (AlertMarty)
                {
                    Console.WriteLine($"\n***************\n" +
                        $"Consumer{consumerId}'s Threshold Breached!!: " +
                        $"\nArbitraryThreshold:{criterion.ArbitraryThreshold}°C" +
                        $"\nInsignificantFluctuation:+/-{criterion.InsignificantFluctuation}°C" +
                        $"\nDirection:{criterion.Direction}" +
                        $"\nPreviousTemperature:{previousTemperature}°C" +
                        $"\nCurrentTemperature:{currentTemperature}°C");

                    var temperatureModel = new Temperature();
                    temperatureModel.Fahrenheit = ThermometerAlertSystem.GetProducerThread().GetFahrenheit(currentTemperature);
                    temperatureModel.Celsius = currentTemperature;
                    temperatureModel.Message =
                            $"Temperature Alert for Consumer#{consumerId}'s arbitraryThreshold. " +
                            $"The System is Notifying Consumer#{consumerId}...";

                    temperatureList.Add(temperatureModel);

                    FluxCapacitor = 0;                    
                }
                //For the GraphQL Query bonus feature
                ThermometerAlertSystem.GetProducerThread().SetCelsius(currentTemperature);

                previousTemperature = currentTemperature;                
            }         

            return temperatureList;
        }

        private static bool FluxCapacitor_Activated(float currentTemperature, float previousTemperature,  Criterion criterion)
        {
            var abs_insignificantFluctuation = Math.Abs(criterion.InsignificantFluctuation);
            var abs_previousTemperature = Math.Abs(previousTemperature); 
            bool threshold_breached = false;

            if (!AlertMarty && (abs_previousTemperature > abs_insignificantFluctuation))
            {
                FluxCapacitor += Math.Abs(previousTemperature);
            }

            bool abs_insignificant_breached = (FluxCapacitor > abs_insignificantFluctuation);

            if (currentTemperature == criterion.ArbitraryThreshold && currentTemperature != previousTemperature)
            {
                threshold_breached = true;
            }

            bool preliminaryResult = InExclusiveRange(currentTemperature, previousTemperature, criterion.ArbitraryThreshold);
            AlertMarty = threshold_breached || preliminaryResult && abs_insignificant_breached;

            return AlertMarty;
        }

        private static bool InExclusiveRange(float currentTemperature, float previousTemperature, float arbitraryThreshold)
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
