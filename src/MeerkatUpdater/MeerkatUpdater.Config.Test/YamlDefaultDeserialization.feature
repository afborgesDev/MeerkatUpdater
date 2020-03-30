Feature: To have a safe deserealization for the Yaml file 
		 for the core config and the specific others configuraions

Scenario: Default Deserealization generates a valid ExecutionConfigClass
	Given That the default generated yaml payload
	 When the default deserealization is triggered
	 Then the object result a not null ExecutionConfigClass
	  And the NugetConfigurations is not null
	  And the UpdateConfigurations is not null
	  And the SolutionPath has the default solutionPath
	  And the LogLevel has the default LogLevel

Scenario Outline: [not happy path] Invalid string payload
	Given That the payload '<payload>'
	When the expected execution for deserealization is prepared
	Then the execution results into a Exception 
	Examples: 
	| payload      |
	| string.empty |
	| null         |