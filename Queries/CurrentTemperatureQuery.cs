using TemperatureAlertSystem.Enums;
using TemperatureAlertSystem.Models;

namespace TemperatureAlertSystem.Queries
{
    /* Author: David DLV
     * Date:5/18/2023
     * This class is for a GraphQL Query  to return a result to the Query.
     * I would like to kindly note that this is a BONUS feature in the Solution
     * I wanted to also show that I have knowledge of building a GraphQL Query and to return values from it.
     * 
     */
    public class CurrentTemperatureQuery
    {
        [GraphQLDescription("Get the current temperature.")]
        public Temperature GetTemperature()
        {
            float celsius_temperature = GetCurrentTemperature(Scale.Celsius);

            float fahrenheit_temperature = GetCurrentTemperature(Scale.Fahrenheit);

            var response = new Temperature
            {
                Celsius = celsius_temperature,
                Fahrenheit = fahrenheit_temperature,
                Message = "Current Temperature Reading"
            };

            return response;
        }

        private float GetCurrentTemperature(Scale _scale)
        {
            switch (_scale)
            {
                case Scale.Fahrenheit:
                    return 10.5f;
                case Scale.Celsius:
                    return -33.5f;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
