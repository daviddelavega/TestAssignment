using TemperatureAlertSystem.Enums;

namespace TemperatureAlertSystem.ThermometerLogic
{
    /* Author: David DLV
    * Date:5/18/2023
    * This class facilitates the definition of what exactly one criterion is when getting or not getting a temperature alert.
    */
    public class Criterion
    {
        public float ArbitraryThreshold { get; }
        public float InsignificantFluctuation { get; }
        public Direction Direction { get; }

        public Criterion(float _arbitraryThreshold, float _insignificantFluctuation, Direction direction)
        {
            ArbitraryThreshold = _arbitraryThreshold;
            InsignificantFluctuation = Math.Abs(_insignificantFluctuation);
            Direction = direction;
        }
    }
}
