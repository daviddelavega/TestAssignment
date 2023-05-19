using TemperatureAlertSystem.Enums;

namespace TemperatureAlertSystem.Models
{
    /* Author: David DLV
     * Date:5/18/2023
     * This class is a POCO and is responsible modeling a Caller's Alert Criterial for when they might receive the Temperature data
     */
    public class AlertCriteriaModel
    {
        public float ArbitraryThreshold { get; set; }
        public float InsignificantFluctuation { get; set; }
        public Direction Direction { get; set; }
    }
}
