using Ui.Browser;

namespace Test;

[SetUpFixture]
public class TestSetup
{
    public static BrowserFactory BrowserFactory { get; private set; } = null!;

    [OneTimeSetUp]
    public void TestSetupOneTimeSetUp()
    {
        var headlessEnv = Environment.GetEnvironmentVariable("AUTOMATION_HEADLESS");
        var useHeadless = !string.IsNullOrEmpty(headlessEnv) 
                          && (bool.TryParse(headlessEnv, out var parsedHeadless) && parsedHeadless);
        
        BrowserFactory = new BrowserFactory(useHeadless);
    }
}