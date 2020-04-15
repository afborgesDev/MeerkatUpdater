Feature: In order to test the Count projects command execution

@deleteOutPutTest
Scenario: Count Project should return the number of csprojc inside a solution
	Given The valid configurations with the solution path for outputPath 'countProjectOutPutPath'
	 When The CountProject is executed
	 Then The count shoudl be more than '0'