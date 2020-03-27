Feature: In order to make sure that the default configurations can be well generated 

Scenario: Generate a valid yml string for default configurations
	Given The static default method to generate the configurations
	When has a valid string result
	Then The string result should be a valid yml file
	 And the string result could serialize back into the ExecutionConfiguration class

Scenario: Generate a valid yml file for default configurations
	Given The static default method to generate the file using the path: 'meerkatupdated.yml'
	 When has a existent file
	 Then the file is a valid yml
	 And the file can be serialized back into the ExecutionConfiguration class

Scenario Outline: Do not generate a valid yml file using a invalid path
	Given Using the path: '<invalidPath>'
	  When the static method to generate file is executed
	  Then An exception of '<exceptionType>' is raised	  
	Examples: 
	| invalidPath     | exceptionType         |
	| string.empty    | ArgumentNullException |
	| --#$!#@#\       | ArgumentException     |
	| PathWithOutYml\ | ArgumentException     |
	| null            | ArgumentNullException |
	| Path.sln        | ArgumentException     |
