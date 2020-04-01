Feature: To test the implementation for DotNetCommand service executor
And grant that the lib is wrapping everyting well

Scenario: 😊 Execution for version results into a Successfull execution with no errors
	Given The command '--version'
	When the DotNetCommandService is triggered
	Then the results have the success execution and the errors has no items

Scenario: 😟 Execution for a invalid command fails and returns with errors
  Given The command 'somethingWrong'
  When the DotNetCommandService is triggered
  Then the results have no success and has errors

Scenario: 😟 Execution without correct params should fail
  Given The null reference for Execution parameter
  When The DotNetCommandService uses this reference
  Then The execution trigger an exception

Scenario: 😟 Execution with no workdir should fail
 Given The Execution with a blank workdir parameter
  When The DotNetCommandService uses this reference
  Then The execution trigger an exception

Scenario Outline: 😟 Execution initialization methods should return a new instance
    Given The Execution params
          | Command   | WorkDir   | WaitTimeOut   |
          | <Command> | <WorkDir> | <WaitTimeOut> |
    When The compatible uses the compatible method
    Then The execution trigger an exception

Examples: 
    | Command      | WorkDir      | WaitTimeOut |
    | string.Empty | string.empty | 0           |
    | someWrong    | someWrong    | 0           |
    | someWrong    | string.empty | 1           |
    | string.empty | someWrong    | 1           |