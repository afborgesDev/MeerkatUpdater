Feature: To grant that the possibilities for the build command can run as expected

Scenario: 😊 Build a valid project has no errors
  Given The solution file to use
  When The comamnd is executed
  Then The function returns a true as a succeed result for the build