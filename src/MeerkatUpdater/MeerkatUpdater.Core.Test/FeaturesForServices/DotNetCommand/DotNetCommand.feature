Feature: In order to test the base DotNetCommand class and execution

Scenario: 😊 Get version of the installed dotnet works well with valid configurations
  Given The valid configurations
    And The arguments to execute was '--version'
   When The DotNetCommand is executed
   Then The result was a succeed execution
    And The errorOutput doesn't have any loged error
    And The output Has the dotnet version