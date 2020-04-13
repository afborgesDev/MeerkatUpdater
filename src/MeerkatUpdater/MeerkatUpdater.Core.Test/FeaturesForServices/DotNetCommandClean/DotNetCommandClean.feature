Feature: In order to test the Clean command execution

@deleteOutPutTest
Scenario: 😊 Clean execution should remove files from output folder
	Given The valid configurations with the solution path
	 When The Clean is executed
	 Then The target solution dll file was cleaned up