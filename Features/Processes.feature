Feature: Processes

A short summary of the feature
Background: 
Given I navigate to administration


Scenario: Creating Process Steps
	When I add Process with the name "Process_237"
	And I add Process Steps with the Step Names "Process_237.Step1", "Process_237.Step2", "Process_237.Step3" to the Process "Process_237"

Scenario: D_Adding the Process Steps to the Process
	When I add Process Steps with the Step Names "Process_234.Step4", "Process_234.Step5", "Process_234.Step6" to the Process "Process_234"

Scenario: Creating the Users in User Group
  Given I add a UserGroup "UserGroup_100"


Scenario: Adding Users to the User Group
	When I navigate to the "User Groups" tab
	When I add Users with the User Names "amaganti@gnhearing.com", "xxsurko@gnhearing.com", "ppuvvala@gnhearing.com" to the UserGroup "UserGroup_100"

Scenario: Creating Lines
	When I navigate to the "Lines" tab
	When I Create line with the name "Line_100"


Scenario: Adding WorkStations to the lines 
	When I navigate to the "Lines" tab
	And I add WorkStations "WorkStation_100" and "WorkStation_101" to the Line "Line_100"