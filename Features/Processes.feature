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


Scenario Outline: Adding processes When based on the Configuration steps
    When I Create Processes based on the Configuration conditions '<condition>'

Examples:
    | condition                          |
    | AllProcessStepsSettoTrue           |
    | AllProcessStepsSettoFalse          |
    | ThreeProcessStepsSettoTrueOneFalse |
    | OneProcessStepSettoTrueThreeFalse  |

	
Scenario: Conducting Workflow for the Website
	When I navigate to the Website "https://app-order-tracker-eus-tst.azurewebsites.net/"
	When I open the QR code "Test_Line_Workstation1_Test_Line1.png" of "WorkStationQR"
	And I open Camera to scan QR Code

Scenario: Conducting Workflow for the Website with Assetscanning
	When I navigate to the Website "https://app-order-tracker-eus-tst.azurewebsites.net/"
	When I open the QR code "Test_Line_Workstation1_Test_Line1.png" of "WorkStationQR"
	And I open Camera to scan QR Code
	When I open the QR code "Test_Process1_OrderQR.png" of "OrderQR"
	And I open Camera to scan QR Code


Scenario: Verify Workstation ID and Name are displayed at the top of the application
	When I navigate to the Website "https://app-order-tracker-eus-tst.azurewebsites.net/"
	When I open the QR code "Test_Line_Workstation1_Test_Line1.png" of "WorkStationQR"
	And I open Camera to scan QR Code
	When I open the QR code "Test_Process1_OrderQR.png" of "OrderQR"
	And I open Camera to scan QR Code
	Then the Workstation ID "Test_Line_Workstation1" and Name "Test_Line_Workstation1" should be displayed at the top of the application