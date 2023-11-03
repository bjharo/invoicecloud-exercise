using OpenQA.Selenium;
using Ui.Browser;

namespace Ui.PageObjects;

internal class MainPage
{
    public IWebElement Title => Driver.GetElement(TitleSelector);
    public IWebElement AddElementBtn => Driver.GetElement(AddElementBtnSelector);
    
    private By TitleSelector => By.CssSelector("h3");
    private By AddElementBtnSelector => By.CssSelector("button[onclick = 'addElement()']");
    private By DeleteBtnSelector => By.CssSelector("#elements [onclick = 'deleteElement()']");
    
    private IWebDriver Driver { get; init; }

    public MainPage(IWebDriver driver)
    {
        Driver = driver;
    }

    public IReadOnlyList<IWebElement> DeleteButtons()
    {
        return Driver.GetElements(DeleteBtnSelector, atLeastOne: false);
    }
}