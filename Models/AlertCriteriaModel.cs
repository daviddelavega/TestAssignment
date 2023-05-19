using TemperatureAlertSystem.Enums;

namespace TemperatureAlertSystem.Models
{
    public class AlertCriteriaModel
    {
        public float ArbitraryThreshold { get; set; }
        public float InsignificantFluctuation { get; set; }
        public Direction Direction { get; set; }
    }
}
