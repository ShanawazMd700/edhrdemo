Feature: Processes

A short summary of the feature
Background: 
Given I navigate to administration


Scenario: Creating Process Steps
	When I add Process with the name "Process_234"
	And I add Process Steps with the Step Names "Process_234.Step1", "Process_234.Step2", "Process_234.Step3" to the Process "Process_234"
	
Scenario: D_Adding the Process Steps to the Process
	When I add Process Steps with the Step Names "Process_234.Step4", "Process_234.Step5", "Process_234.Step6" to the Process "Process_234"