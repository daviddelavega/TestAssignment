# Thermometer Temperature Alert System in Celsius & Fahrenheit
# Technology Stack: 
# API: GraphQL 
# Programming Language: C#
# Platform: .NET 6
# Project: ASP.Net Web Application.
# Extras: 30 Sanity Test Cases created. DockerFile created. GraphQL Query created.
# How to Run the Application:
# **1.** Install a GraphQL, I use Altair.
# **2.** Clone this repository.
# **3.** Execute the **TemperatureAlertSystem.exe** file located in **\bin\Debug\net6.0**
# **4.** A Console application should startup listing URLs, you will need this URL for Altair **Now listening on: https://localhost:7142**
      For example this goes into Altair: https://localhost:7142/graphql
# **5.** Using Altair Add a consumer like this example:
 **mutation {
  addConsumer(
    alertCriteria: [
      {
        id: 0
        arbitraryThreshold: -50.5
        insignificantFluctuation: 2.0
        direction: FALLING
      }
    ]
  ) {
    isSuccess
    message
  }
}**

# **6.** Add The Temperature Test Data like this example:
**mutation {
  uploadTemperatures(
    inputTemperatures: [-10.0, -40.0, -50.0, -51.5, -60.0, -40.0]
  ) {
    isSuccess
    message
  }
}**
# Development Approach Summary: 
There is a GraphQL Mutator at the front end which feeds the Temperature Data from the outside to the application. A second mutator feeds in each consumer's Arbitrary Criteria (temperature threshold, insignificant fluctuation, Direction). The application has producer/consumer threads. The producer thread does most of the heavy lifting, and facilitates the progression through the temperature Alert System. There are multiple consumer threads started, 1 for each Alert Criteria, each call to the mutator adds a consumer, or they can be all added once in a list of consumers, giving the API lots of flexibility. 
# Requirements & How I Achieved Them Each
----- Test instructions ---

1. Design and implement (in the OO language of your choice) a thermometer class or classes that read the temperature of some external source. 
I achieved this Requirement: Implemented in C#. The temperature data originates from GraphQL mutator, which is an external source. Each Consumer of the temperature data is also created via a GraphQL API call.

2. The thermometer needs to be able to provide temperature in both Fahrenheit and Celsius.  
I achieved this Requirement: The thermometer returns both Fahrenheit and Celsius when an Alert Critieria (i.e. threshold) has been breached.

3. It must be possible for callers of the class(es) to define arbitrary thresholds such as freezing and boiling at which the thermometer class will inform the appropriate callers that a specific threshold has been reached. 
I achieved this Requirement: The callers/consumers, can supply any granularity of alert thresholds they desire. The thermometer will inform them only when that specific threshold is breached.

4. Note that callers of the class may not want to be repeatedly informed that a given threshold has been reached if the temperature is fluctuating around the threshold point. For example, consider the following temperature readings from the external source (GraphQL mutation):

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
    90.5,
    95.7,
    99.8,
    100.0,
    101.5,
    95.6
    ]) 
       {
        isSuccess
        message
       }
     }
     
Some callers may only want to be informed that the temperature has reached 0 degrees C once because they consider fluctuations of +/- 0.5 degrees insignificant. 
I achieved this Requirement: Callers will be informed if the temperature fluctuates above the insignificant absolute value. I provided an example input data above, you will see temperature fluctuations Falling, Rising, above and below freezing point and boiling point. My testing the application has proven that the caller will get notified if the temperatures fluctuation is greater than the absolute value of the insignificant value. The code logic shows this and my testing proves this.

5. It may also be important for some callers to be informed that a threshold has been reached only if the threshold was reached from a certain direction. For example, some callers may only care about a freezing point threshold if the previous temperature was above freezing (i.e. they only care about the threshold if it occurred while the temperature was dropping).
I have achieved Requirement: There is a Direction attribute in a caller's Alert Criteria it can be None, Rising, Falling. The code logic will alert the caller/consumer if the direction matches the criteria.

I threw in a Bonus Feature: A GraphQL Query that will return Celsius temperature, Fahrenheit temperature, along with a message, and of course each field is optional, which is part of the beauty of GraphQL.
I also created 30 Sanity Test Cases. These 30 test cases will fully cover the Sanity level Regression using Equivalence Partition technique. Test Case file is named Sanity_TestCases_Alert_Criteria.xls in the repo.
