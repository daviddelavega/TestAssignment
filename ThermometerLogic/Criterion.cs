using TemperatureAlertSystem.Enums;

namespace TemperatureAlertSystem.ThermometerLogic
{
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
