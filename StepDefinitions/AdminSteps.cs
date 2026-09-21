using PlaywrightDemo.Hooks;
using PlaywrightDemo.Pages;
using Reqnroll;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace PlaywrightDemo.StepDefinitions
{
    [Binding]
    public class AdminSteps
    {
        private readonly AdministrationPage adminPage;
        private readonly Process processPage;
        private string? selectedLineName;
        public AdminSteps()
        {
            var page = Hooks1.Instance?.Page;
            if (page == null)
            {
                throw new InvalidOperationException("Playwright Page is not initialized. Ensure the BeforeScenario hook executed.");
            }

            adminPage = new AdministrationPage(page);
            processPage = new Process(page);
        }

        [Given("I navigate to administration")]
        public async Task GivenINavigateToAdministration()
        {
            await adminPage.GoToAsync();
        }

        [When("I add Process with the name {string}")]
        public async Task WhenIAddProcessWithTheName(string processName)
        {
            await adminPage.ClickAddProcessAsync(processName, processName);
        }

        [When("I add Process Steps with the Step Names {string}, {string}, {string} to the Process {string}")]
        public async Task WhenIAddProcessStepsWithTheStepNamesToTheProcess(string step1, string step2, string step3, string processName)
        {
            await processPage.CreateProcessSteps(step1, step2, step3, processName);
        }
        [Given("I add a UserGroup {string}")]
        public async Task GivenIAddAUserGroup(string userGroupName)
        {
            await adminPage.AddUsergroup(userGroupName);
        }
        [When("I add Users with the User Names {string}, {string}, {string} to the UserGroup {string}")]
        public async Task WhenIAddUsersWithTheUserNamesToTheUserGroup(string user1, string user2, string user3, string userGroupName)
        {
            await processPage.AddingEmailID(user1, user2, user3, userGroupName);
        }
        [When("I navigate to the {string} tab")]
        public async Task WhenINavigateToTheTab(string tabname)
        {
            await adminPage.NavigateToTab(tabname);
        }
        [When("I Create line with the name {string}")]
        public async Task WhenICreateLineWithTheName(string linename)
        {
            await adminPage.AddLinesAsync(linename);
        }
        [When("I add WorkStations {string} and {string} to the Line {string}")]
        public async Task WhenIAddWorkStationsAndToTheLine(string ws1, string ws2, string linename)
        {
            await adminPage.AddWorkStations(linename, ws1, ws2);
        }
        [When("I select Line {string} and click on Workstation {string}")]
        public async Task WhenISelectLineAndClickOnWorkstation(string linename, string workstationName)
        {
            selectedLineName = linename;
            await adminPage.SelectWorkStationAsync(linename, workstationName);
        }

        [When("I add Process Steps {string}, {string}, {string}")]
        public async Task WhenIAddProcessSteps(string step1, string step2, string step3)
        {
            await adminPage.AddProcessStepsAsync(GetSelectedLineName(), step1, step2, step3);
        }

        [When("I add User Group {string}")]
        public async Task WhenIAddUserGroup(string userGroupName)
        {
            await adminPage.AddUserGroupAsync(GetSelectedLineName(), userGroupName);
        }

        [When("I add an Asset with details")]
        public async Task WhenIAddAnAssetWithDetails(DataTable dataTable)
        {
            var row = dataTable.Rows[0];

            await adminPage.AddAssetAsync(
                GetSelectedLineName(),
                row["AssetName"],
                row["AssetType"],
                row["AssetDisplayName"],
                row["SerialNumber"],
                bool.Parse(row["DisplayAtWorkstation"]),
                DateTime.ParseExact(row["ExpirationDate"], "dd-MM-yyyy", CultureInfo.InvariantCulture));
        }

        [When("I add Processes with All Process Steps Set to True")]
        public async Task WhenIAddProcessesWithAllProcessStepsSetToTrue()
        {
            await adminPage.CreateConfigurationAsync("AllProcessStepsSettoTrue");
        }

        [When("I add Processes with All Process Steps Set to False")]
        public async Task WhenIAddProcessesWithAllProcessStepsSetToFalse()
        {
            await adminPage.CreateConfigurationAsync("AllProcessStepsSettoFalse");
        }

        [When("I add Processes when Three Process Steps Set to True One False")]
        public async Task WhenIAddProcessesWhenThreeProcessStepsSetToTrueOneFalse()
        {
            await adminPage.CreateConfigurationAsync("ThreeProcessStepsSettoTrueOneFalse");
        }

        [When("I add Processes When One Process Step Set to True Three False")]
        public async Task WhenIAddProcessesWhenOneProcessStepSetToTrueThreeFalse()
        {
            await adminPage.CreateConfigurationAsync("OneProcessStepSettoTrueThreeFalse");
        }

        private string GetSelectedLineName() =>
            !string.IsNullOrWhiteSpace(selectedLineName)
                ? selectedLineName
                : throw new InvalidOperationException("Select a line and workstation before adding workstation-linked data.");


        [When("I Create Processes based on the Configuration conditions {string}")]
        public async Task WhenICreateProcessesBasedOnTheConfigurationConditionsAsync(string condition)
        {
            await adminPage.CreateConfigurationAsync(condition);
        }


    }
}
