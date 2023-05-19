# Thermometer Temperature Alert System in Celsius & Fahrenheit
# Technology Stack: 
API: GraphQL, Programming Language: C#, Project: .NET6, ASP.Net Web Application.
# Development Approach Summary: 
There is a GraphQL Mutator at the front end which feeds the Temperature Data from the outside to the application. The application has producer/consumer threads. The producer thread does most of the heavy lifting, and facilitates the progression through the temperature Alert System. There are multiple consumer threads started, 1 for each Alert Criteria, 4 total, easily expandable. There is 1 Producer thread. The producer thread is artificially slowed down with a sleep set at 5 seconds, this is for purposes of being able to read the Temperature output and validate Thresholds and notifications are given when appropriate and not given when not within the consumer's Alert Criteria.
# Requirements & How I Achieved Each Req
----- Test instructions ---

>. Design and implement (in the OO language of your choice) a thermometer class or classes that read the temperature of some external source. 
>. I achieved this Requirement: Implemented in C#. The temperature data originates from GraphQL mutator, which is an external source.

>. The thermometer needs to be able to provide temperature in both Fahrenheit and Celsius.  
>. I achieved this Requirement: The thermometer returns both Fahrenheit and Celsius when an Alert Critieria (i.e. threshold) has been breached.

>. It must be possible for callers of the class(es) to define arbitrary thresholds such as freezing and boiling at which the thermometer class will inform the appropriate callers that a specific threshold has been reached. 
>. I achieved this Requirement: The callers/consumers, can supply any granularity of alert thresholds they desire. The thermometer will inform them only when that specific threshold is breached.

>.Note that callers of the class may not want to be repeatedly informed that a given threshold has been reached if the temperature is fluctuating around the threshold point. For example, consider the following temperature readings from the external source:
>.Example GraphQL mutation payload of temperature data fed into The Thermometer Temperature Alert System:
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
>. I achieved this Requirement. Take a look at my input example above, you will see temperature fluctuations Falling, Rising, above and below freezing point and boiling point. My testing the application has proven that the caller will get notified if the temperatures fluctuate up or down. The code logic and testing is  displayable.

>. Some callers may only want to be informed that the temperature has reached 0 degrees C once because they consider fluctuations of +/- 0.5 degrees insignificant. 
>. I achieved this Requirement: Callers will be informed if the temperature fluctuates above the insignificant absolute value.

>. It may also be important for some callers to be informed that a threshold has been reached only if the threshold was reached from a certain direction. For example, some callers may only care about a freezing point threshold if the previous temperature was above freezing (i.e. they only care about the threshold if it occurred while the temperature was dropping).
>. I have achieved Requirement: There is a Direction attribute in a caller's Alert Criteria it can be None, Rising, Falling. The code logic will alert the caller/consumer if the direction matches the criteria.

>. I threw in a Bonus Feature: A GraphQL Query that will return Celsius temperature, Fahrenheit temperature, along with a message, and of course each field is optional, which is part of the beauty of GraphQL.
