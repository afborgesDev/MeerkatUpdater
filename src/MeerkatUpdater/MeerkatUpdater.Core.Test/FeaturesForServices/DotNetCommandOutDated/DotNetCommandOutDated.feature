Feature: In order to test the OutDated command execution

@deleteOutPutTest
Scenario: Outdated command should return informations about the projects inside the solution
	Given The valid configurations with the solution path for outputPath 'outDatedOutPutPath'
	 When The OutDated is executed
	 Then The list of project info should not be null
	  And The project info should have informatinos about the packages