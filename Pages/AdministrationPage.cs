using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightDemo.Pages
{
    public class AdministrationPage : BasePage
    {
        private const string AdministrationUrl = "https://app-order-tracker-eus-tst.azurewebsites.net/administration";

        public AdministrationPage(IPage page) : base(page)
        {
        }

        public async Task GoToAsync()
        {
            await Page.GotoAsync(AdministrationUrl);
            Thread.Sleep(15000); 
        }
    }
}
