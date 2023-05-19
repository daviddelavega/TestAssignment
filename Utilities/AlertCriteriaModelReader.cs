using System.Globalization;
using TemperatureAlertSystem.Enums;
using TemperatureAlertSystem.Models;
using TemperatureAlertSystem.ThermometerLogic;

namespace TemperatureAlertSystem.Utilities
{
   /* Author: David DLV
    * Date:5/18/2023
    * This class has a single responsibility to read the Alert Criteria that a Consumer of The Thermometer Temperature data sets. 
    * simple class file, which stores Alert Criterias for various consumers.
    * Given additional time to develop the application, I would move the data to a csv or yaml to parse there.
    */
    public class AlertCriteriaModelReader
    {
        public static Dictionary<int, Criterion> ReadAlertCriteriaData()
        {
            Criterion criterion_0 = new(0f, 0.5f, Direction.None);
            Criterion criterion_1 = new(0f, 0.5f, Direction.Rising);
            Criterion criterion_2 = new(-0.5f, 0f, Direction.Falling);
            Criterion criterion_3 = new(100f, 0.5f, Direction.Falling);
            Criterion criterion_4 = new(100f, 0f, Direction.Rising);

            var criteriaMap = new Dictionary<int, Criterion>();
            criteriaMap.Add(0, criterion_0);
            criteriaMap.Add(1, criterion_1);
            criteriaMap.Add(2, criterion_2);
            criteriaMap.Add(3, criterion_3);
            criteriaMap.Add(4, criterion_4);

            return criteriaMap;
        }     
    }
}
