using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Ui.Browser;

public class BrowserFactory
{
    public IWebDriver CurrentBrowser
    {
        get 
        {
            if (WebDriver is {IsValueCreated: false, Value: not null})
            {
                throw new ApplicationException("Unable to retrieve the browser driver for the current thread.");
            }

            return WebDriver.Value!;
        }
    }
    
    public bool UseHeadless { get; init; }
    
    private ThreadLocal<IWebDriver> WebDriver { get; } = new();

    public BrowserFactory(bool useHeadlessMode = false)
    {
        UseHeadless = useHeadlessMode;
    }

    public IWebDriver CreateBrowser()
    {
        WebDriver.Value = CreateChromeDriver();
        return CurrentBrowser;
    }

    public void DisposeCurrentBrowser()
    {
        WebDriver.Value?.Close();
        WebDriver.Value?.Dispose();
    }

    private IWebDriver CreateChromeDriver()
    {
        var options = new ChromeOptions();
        options.AddArguments("window-size=1440,900");
        options.AddArguments("--disable-infobars");
        options.AddArguments("--disk-cache-dir=null");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddExcludedArgument("enable-automation");
        options.AddAdditionalOption("useAutomationExtension", false);
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);

        if (UseHeadless)
        {
            options.AddArgument("--headless=new");
        }

        return new ChromeDriver(options);
    }
}