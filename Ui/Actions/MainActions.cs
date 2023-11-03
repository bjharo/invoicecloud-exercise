using Ardalis.GuardClauses;
using OpenQA.Selenium;
using Shouldly;
using Ui.PageObjects;

namespace Ui.Actions;

public class MainActions
{
    private IWebDriver Driver { get; init; }
    private MainPage MainPage { get; init; }

    public MainActions(IWebDriver driver)
    {
        Driver = driver;
        MainPage = new MainPage(Driver);
    }

    public void LaunchApp(string url)
    {
        Driver.Navigate().GoToUrl(url);
        MainPage.Title.Text.ShouldBe("Add/Remove Elements", "Could not confirm app was loaded.");
    }

    public void AddDeleteButtons(int numberToAdd)
    {
        Guard.Against.NegativeOrZero(numberToAdd, nameof(numberToAdd), "At least one button must be added.");
        
        for (var i = 0; i < numberToAdd; i++)
        {
            MainPage.AddElementBtn.Click();
        }
    }

    public int GetNumberOfDeleteButtons()
    {
        return MainPage.DeleteButtons().Count;
    }

    public void ClickDeleteButton(int buttonIndex)
    {
        Guard.Against.Negative(buttonIndex, nameof(buttonIndex), "A valid button index must be >= 0.");

        var deleteButtons = MainPage.DeleteButtons();

        deleteButtons.Count.ShouldBeGreaterThanOrEqualTo(buttonIndex + 1,
            $"Could not find a button at index {buttonIndex}");

        deleteButtons[buttonIndex].Click();
    }
}