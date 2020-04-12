Feature: In order to test the Build command execution

@deleteOutPutTest
Scenario: 😊 A succeed build should generate files output
  Given The valid configurations with the solution path
   When The Build is executed
   Then The folder 'outputTest' should be created with files
    And The execution result should be true
