Feature: In order to test the base DotNetCommand class and execution

Scenario: 😊 Get version of the installed dotnet works well with valid configurations
  Given The valid configurations
    And The arguments to execute was '--version'
   When The DotNetCommand is executed
   Then The succeed result of the execution is 'true'
    And The errorOutput doesn't have any loged error
    And The output Has the dotnet version

Scenario: 😊 Command should execute by the timeout default if doesn't have any valid configuration for that
  Given The configuration with no valid WaitTimeOut
    And The arguments to execute was '--version'
   When The DotNetCommand is executed
   Then The succeed result of the execution is 'true'
    And The errorOutput doesn't have any loged error
    And The output Has the dotnet version
    And The time spend was equal or lest than the default

Scenario: 😊 Short TimeOut should kill the task and result into a not succeed execution
  Given The configuration with the short WaitTimeOut
   And The arguments to execute was 'build -v d'
   When The DotNetCommand is executed
    Then The succeed result of the execution is 'false'
    And The errorOutput doesn't have any loged error