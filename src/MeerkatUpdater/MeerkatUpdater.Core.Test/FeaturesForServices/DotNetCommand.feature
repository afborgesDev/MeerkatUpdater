Feature: To test the implementation for DotNetCommand service executor
And grant that the lib is wrapping everyting well

Scenario: 😊 Execution for version results into a Successfull execution with no errors
	Given The command for get the version of the dotnet '--version'
	When the DotNetCommandService is triggered
	Then the results have the sucessd execution and the errors has no items