using System.Threading.Tasks;
using Reqnroll;
using PlaywrightDemo.Hooks;
using PlaywrightDemo.Pages;

namespace PlaywrightDemo.StepDefinitions
{
    [Binding]
    public class AdminSteps
    {
        [Given("I navigate to administration")]
        public async Task GivenINavigateToAdministration()
        {
            var page = Hooks1.Instance?.Page;
            if (page == null)
            {
                throw new System.InvalidOperationException("Playwright Page is not initialized. Ensure the BeforeScenario hook executed.");
            }

            var adminPage = new AdministrationPage(page);
            await adminPage.GoToAsync();
        }
    }
}