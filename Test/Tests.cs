using OpenQA.Selenium;
using Shouldly;
using Ui.Actions;
using Ui.Browser;

namespace Test;

[TestFixture]
[Parallelizable(ParallelScope.Children)]
public class Tests
{
    private ThreadLocal<MainActions> Actions { get; } = new();
    private IWebDriver Driver => TestSetup.BrowserFactory.CurrentBrowser;
    
    private static string AppUrl => "https://the-internet.herokuapp.com/add_remove_elements/";
    
    [SetUp]
    public void Setup()
    {
        TestSetup.BrowserFactory.CreateBrowser();
        Actions.Value = new MainActions(Driver);
        Actions.Value.LaunchApp(AppUrl);
    }

    [TearDown]
    public void TearDown()
    {
        TestSetup.BrowserFactory.DisposeCurrentBrowser();
    }

    [Test]
    public void AppOpensWithNoDeleteButtons()
    {
        Actions.Value!.GetNumberOfDeleteButtons()
            .ShouldBe(0, "When the app is first opened, it should have no delete buttons.");
    }

    [Test]
    [TestCase(1, TestName = "{m}_SingleButton")]
    [TestCase(10, TestName = "{m}_MultipleButtons")]
    [TestCase(100, TestName = "{m}_LargeNumberOfButtons")]
    public void AddDeleteButtons(int numberToAdd)
    {
        Actions.Value!.AddDeleteButtons(numberToAdd);
        Actions.Value!.GetNumberOfDeleteButtons()
            .ShouldBe(numberToAdd, "The correct number of delete buttons are not displayed.");
    }
    
    [Test]
    [TestCase(25, 10, TestName = "{m}_SomeButtons")]
    [TestCase(8, 8, TestName = "{m}_AllButtons")]
    public void RemoveMultipleDeleteButtons(int numberToAdd, int numberToRemove)
    {
        Actions.Value!.AddDeleteButtons(numberToAdd);
        
        for (var i = numberToRemove; i > 0; i--)
        {
            Actions.Value!.ClickDeleteButton(i - 1);
        }

        Actions.Value!.GetNumberOfDeleteButtons().ShouldBe(numberToAdd - numberToRemove,
            "The correct number of delete buttons were not removed.");
    }

    [Test]
    [TestCase(1, 0, TestName = "{m}_OnlyButton")]
    [TestCase(15, 0, TestName = "{m}_FirstButton")]
    [TestCase(10, 9, TestName = "{m}_LastButton")]
    [TestCase(5, 3, TestName = "{m}_MiddleButton")]
    public void RemoveSpecificDeleteButtons(int numberToAdd, int indexOfButtonToRemove)
    {
        Actions.Value!.AddDeleteButtons(numberToAdd);
        Actions.Value!.ClickDeleteButton(indexOfButtonToRemove);
        Actions.Value!.GetNumberOfDeleteButtons()
            .ShouldBe(numberToAdd - 1, "Could not confirm that a single delete button was removed.");
    }
}