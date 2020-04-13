Feature: In order to test the Build command execution

@deleteOutPutTest
Scenario: 😊 A succeed build should generate files output
  Given The valid configurations with the solution path
   When The Build is executed
   Then The folder 'outputTest' should be created with files
    And The execution result should be 'true'

@deleteOutPutTest
Scenario: 😟 A not succeed build should fail the execution
   Given The configurations for a invalid solution
   When The Build is executed
   Then The folder 'outputTest' should be created with files
   And The execution result should be 'false'