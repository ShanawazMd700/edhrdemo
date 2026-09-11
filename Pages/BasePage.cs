using System;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightDemo.Pages
{
    public abstract class BasePage
    {
        protected IPage Page { get; }

        protected BasePage(IPage page)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
        }
    }
}
