Feature: In order to test the Build command execution

@deleteOutPutTest
Scenario: 😊 A succeed build should generate files output
  Given The valid configurations with the solution path for outputPath 'buildSucceedOutPutTest'
   When The Build is executed
   Then The folder 'buildSucceedOutPutTest' should be created with files
    And The execution result should be 'true'

@deleteOutPutTest
Scenario: 😟 A not succeed build should fail the execution
   Given The configurations for a invalid solution path for outputPath 'buildNotSucceedOutPutTest'
   When The Build is executed
   Then The output folder should not be created with files
   And The execution result should be 'false'