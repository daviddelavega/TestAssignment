﻿using TemperatureAlertSystem.Enums;

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
    }   
}
