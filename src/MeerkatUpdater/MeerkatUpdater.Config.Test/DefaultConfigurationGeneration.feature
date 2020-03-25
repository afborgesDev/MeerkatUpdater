Feature: In order to make sure that the default configurations 
can be well generated 

Scenario: Generate a yml string valid configurations
	Given The static default method to generate the configurations
	When The string result should not be empty
	Then The string result should be a valid yml file
	 And the string result could serialize back into the ExecutionConfiguration class