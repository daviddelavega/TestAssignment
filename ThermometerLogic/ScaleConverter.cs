namespace TemperatureAlertSystem.ThermometerLogic
{
    /* Author: David DLV
     * Date:5/18/2023
     * This class has a single responsibility to convert Celcius to Fahrenheit
     * Note that my design really only needs to convert Celsius -> Fahrenheit only in this direction because
     * the data provider is using scale Celsius. That said I did provide both conversions for testing purposes.
     * The mathematical equations are with order of operations:
     * F = (C * 9 / 5) + 32
     * C = (F - 32) * 5 / 9
     */
    public class ScaleConverter
    {
        public static float ConvertToFahrenheit(float celciusTemperature)
        {
            return (celciusTemperature * 9 / 5) + 32;
        }

        public static float ConvertToCelsius(float fahrenheitTemperature)
        {
            return (fahrenheitTemperature - 32) * 5 / 9;
        }
    }
}
