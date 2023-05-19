using System.ComponentModel;
using TemperatureAlertSystem.Enums;
using TemperatureAlertSystem.Models;
using TemperatureAlertSystem.ThermometerLogic;
using TemperatureAlertSystem.Utilities;

namespace TemperatureAlertSystem.Mutators
{

    /* 
        mutation {
          uploadTemperatures(inputTemperatures: 
        [1.5,
        1.0,
        0.5,
        0.0,
        -0.5,
        0.0,
        -0.5,
        0.0,
        0.5,
        0.0
        ]) 
           {
            isSuccess
            message
           }
         }    
    */
    public class Mutation
    {
        [Description("Uploads the Temperatures as Celsius to The TemperatureAlertLogic's TemperatureAlertSystem.")]
        public async Task<TemperatureOutput> UploadTemperatures(List<float> inputTemperatures)
        {
            var criteriaMap = AlertCriteriaModelReader.ReadAlertCriteriaData();            

            var producer = new ProducerThread(inputTemperatures, criteriaMap);

            var consumers = new List<ConsumerThread>();

            for (int i = 0; i < criteriaMap.Keys.Count; i++)
            {
                var consumerThread = new ConsumerThread(i, producer);
                consumers.Add(consumerThread);
                consumerThread.Start();
            }

            producer.Start();          

            return new TemperatureOutput(true, "Temperatures Loaded successfully Thermometer Alert System is now Running.");
        }       
    }

    public record TemperatureOutput(bool IsSuccess, string Message);
}
