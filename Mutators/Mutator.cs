using System.ComponentModel;
using TemperatureAlertSystem.Enums;
using TemperatureAlertSystem.ThermometerLogic;

namespace TemperatureAlertSystem.Mutators
{
    /* Author: David DLV
     * Date:5/18/2023
     * This class is responsible for the GraphQL Mutator, which facilitates the receiving of external temperature data for the system
     * Below is one good example of the GraphQL mutation payload.
  
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
        0.0,
        0.0,
        50.5,
        98.5,
        99.0,
        100.0,
        101.5,
        100.0,
        99.0,
        101.5
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
        public async Task<Output> UploadTemperatures(List<float> inputTemperatures)
        {
             ThermometerAlertSystem
                .GetProducerThread()
                .SetTemperatures(inputTemperatures);

            return new Output(true, "Temperatures Loaded successfully.");
        }

        [Description("Adds a new Consumer given the Alert Criteria as Celsius into the TemperatureAlertSystem.")]
        public async Task<Output> AddConsumer(List<AlertCriteriaModel> alertCriteria)
        {
            ThermometerAlertSystem.AddConsumer(alertCriteria);           

            return new Output(true, "Consumer(s) with AlertCriteria Added successfully.");
        }
    }

    public record Output(bool IsSuccess, string Message);
    public record AlertCriteriaModel(int id, float arbitraryThreshold, float insignificantFluctuation, Direction direction);








}
