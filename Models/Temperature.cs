namespace TemperatureAlertSystem.Models
{
    /* Author: David DLV
     * Date:5/18/2023
     * This class is the GraphQL model definition.
     * It defines the schema of the TemperatureAlertSystem GraphQL API.
     */
    public class Temperature
    {
        public float Celsius { get; set; }

        public float Fahrenheit { get; set; }

        public string Message { get; set; }
    }
}
