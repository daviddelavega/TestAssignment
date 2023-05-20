using System.ComponentModel;
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
        public async Task<TemperatureOutput> UploadTemperatures(List<float> inputTemperatures)
        {
            new StartThermometerAlertSystem(inputTemperatures).Start();
            return new TemperatureOutput(true, "Temperatures Loaded successfully. Thermometer Alert System is now Running.");
        }       
    }

    public record TemperatureOutput(bool IsSuccess, string Message);
}
