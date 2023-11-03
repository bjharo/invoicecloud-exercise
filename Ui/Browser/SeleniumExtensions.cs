using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Ui.Browser;

public static class SeleniumExtensions
{
    private static TimeSpan DefaultExplicitTimeout => TimeSpan.FromSeconds(60);

    public static IWebElement GetElement(this ISearchContext context, By selector, TimeSpan? timeout = null)
    {
        var actualTimeout = timeout ?? DefaultExplicitTimeout;

        try
        {
            return CreateWait(context, actualTimeout).Until(c => c.FindElement(selector));
        }
        catch (Exception ex) when (ex is WebDriverTimeoutException or WebDriverException)
        {
            if (ex is WebDriverTimeoutException
                || ex.Message.Contains($"timed out after {actualTimeout.TotalSeconds} seconds"))
            {
                throw new NoSuchElementException($"Unable to find element with Selector {selector.ToString()}.", ex);
            }

            throw;
        }
    }

    public static IReadOnlyList<IWebElement> GetElements(this ISearchContext context, By selector,
        TimeSpan? timeout = null, bool atLeastOne = true)
    {
        var actualTimeout = timeout ?? DefaultExplicitTimeout;

        try
        {
            return CreateWait(context, actualTimeout).Until(c => c.FindElements(selector));
        }
        catch (Exception ex) when (ex is WebDriverTimeoutException or WebDriverException)
        {
            if (ex is not WebDriverTimeoutException
                && !ex.Message.Contains($"timed out after {actualTimeout.TotalSeconds} seconds")) throw;
            
            if (!atLeastOne)
            {
                // in this case, we don't care if no elements are found, just return an empty list
                return new List<IWebElement>().AsReadOnly();
            }
                
            throw new NoSuchElementException($"Unable to find element with Selector {selector.ToString()}.", ex);

        }
    }

    private static DefaultWait<ISearchContext> CreateWait(ISearchContext driver, TimeSpan timeout)
    {
        var wait = new DefaultWait<ISearchContext>(driver) {   
            Timeout = timeout,
            PollingInterval = TimeSpan.FromMilliseconds(250)
        };

        wait.IgnoreExceptionTypes(new[] { 
            typeof(StaleElementReferenceException), 
            typeof(NotFoundException), 
            typeof(InvalidOperationException)
        });

        return wait;
    }
}