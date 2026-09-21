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


Scenario: Adding process steps to the Lines
	When I navigate to the "Lines" tab
	When I select Line "Line_100" and click on Workstation "WorkStation_100"
	When I add Process Steps "Process_234.Step4", "Process_234.Step5", "Process_234.Step6"

Scenario: Adding Users to the WorkStations
	When I navigate to the "Lines" tab
	And I select Line "Line_100" and click on Workstation "WorkStation_100"
	And I add User Group "UserGroup_100"

Scenario: Adding Assets to the Processes
	When I navigate to the "Lines" tab
	When I select Line "Line_100" and click on Workstation "WorkStation_100"
	When I add an Asset with details
		| AssetName    | AssetType    | AssetDisplayName | SerialNumber | DisplayAtWorkstation | ExpirationDate |
		| Asset_100_1  | Sensor       | Asset_100_1       | 1234567890   | true                 | 26-03-2027     |


Scenario: Adding Processes When All Process Steps Set to True
	When I add Processes with All Process Steps Set to True

Scenario: Adding Processes When All Process Steps Set to False
	When I add Processes with All Process Steps Set to False

Scenario: Adding Processes When Three Process Steps Set to True One False
	When I add Processes when Three Process Steps Set to True One False

Scenario: Adding Processes When One Process Step Set to True Three False
	When I add Processes When One Process Step Set to True Three False

Scenario Outline: Adding processes When based on the Configuration steps
    When I Create Processes based on the Configuration conditions '<condition>'

Examples:
    | condition                          |
    | AllProcessStepsSettoFalse          |
    | AllProcessStepsSettoTrue           |
    | ThreeProcessStepsSettoTrueOneFalse |
    | OneProcessStepSettoTrueThreeFalse  |

