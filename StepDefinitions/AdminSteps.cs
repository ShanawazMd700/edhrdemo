using PlaywrightDemo.Hooks;
using PlaywrightDemo.Pages;
using Reqnroll;
using System;
using System.Threading.Tasks;

namespace PlaywrightDemo.StepDefinitions
{
    [Binding]
    public class AdminSteps
    {
        private readonly AdministrationPage adminPage;
        private readonly Process processPage;

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
            await processPage.SelectProcessAsync(processName);
            await processPage.AddProcessStepsAsync(step1, step2, step3);
            await processPage.SaveProcess(processName);
        }

    }
}