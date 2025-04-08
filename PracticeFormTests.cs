using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using NUnit.Framework;

namespace AutomationPracticeFormTest
{
    [TestFixture]
    public class PracticeFormTests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            // Setup Chrome driver
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            
            // Navigate to the test page
            driver.Navigate().GoToUrl("https://app.cloudqa.io/home/AutomationPracticeForm");
        }

        [Test]
        public void TestNameField()
        {
            // Test name field using ID (more reliable than position)
            string testName = "John Doe";
            
            // Find element by ID (preferred) or by various other attributes
            IWebElement nameField = FindElementSafely(By.Id("name"), 
                                                     By.Name("name"), 
                                                     By.CssSelector("input[placeholder='Name']"));
            
            Assert.IsNotNull(nameField, "Name field could not be found");
            
            nameField.Clear();
            nameField.SendKeys(testName);
            
            // Verify input was accepted
            Assert.AreEqual(testName, nameField.GetAttribute("value"), "Name was not entered correctly");
            
            TakeScreenshot("NameFieldTest");
        }

        [Test]
        public void TestEmailField()
        {
            // Test email field using multiple identifier strategies
            string testEmail = "test@example.com";
            
            IWebElement emailField = FindElementSafely(By.Id("email"), 
                                                      By.Name("email"), 
                                                      By.CssSelector("input[placeholder='Email']"),
                                                      By.XPath("//input[@type='email']"));
            
            Assert.IsNotNull(emailField, "Email field could not be found");
            
            emailField.Clear();
            emailField.SendKeys(testEmail);
            
            // Verify input was accepted
            Assert.AreEqual(testEmail, emailField.GetAttribute("value"), "Email was not entered correctly");
            
            TakeScreenshot("EmailFieldTest");
        }

        [Test]
        public void TestGenderRadioButtons()
        {
            // Test gender radio buttons by using text and input type
            // Using contains to make it more resilient to text changes
            
            IWebElement maleRadio = FindElementSafely(By.XPath("//input[@type='radio' and @value='Male']"),
                                                     By.XPath("//label[contains(text(), 'Male')]/preceding-sibling::input[@type='radio']"),
                                                     By.XPath("//label[contains(text(), 'Male')]/input[@type='radio']"),
                                                     By.XPath("//div[contains(text(), 'Gender')]/following::input[@type='radio'][1]"));
            
            Assert.IsNotNull(maleRadio, "Male radio button could not be found");
            
            if (!maleRadio.Selected)
            {
                // Click the label if the radio is not clickable directly
                try
                {
                    maleRadio.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    var maleLabel = driver.FindElement(By.XPath("//label[contains(text(), 'Male')]"));
                    maleLabel.Click();
                }
            }
            
            // Verify selection
            Assert.IsTrue(maleRadio.Selected, "Male radio button was not selected");
            
            TakeScreenshot("GenderRadioTest");
        }

        // Helper method to find an element using multiple strategies
        private IWebElement FindElementSafely(params By[] locators)
        {
            foreach (var locator in locators)
            {
                try
                {
                    // Wait for element to be visible and interactable
                    return wait.Until(driver => {
                        var element = driver.FindElement(locator);
                        if (element.Displayed && element.Enabled)
                            return element;
                        return null;
                    });
                }
                catch (NoSuchElementException)
                {
                    continue;
                }
                catch (WebDriverTimeoutException)
                {
                    continue;
                }
            }
            
            return null;
        }
        
        private void TakeScreenshot(string testName)
        {
            try
            {
                // Create directory if it doesn't exist
                Directory.CreateDirectory("Screenshots");
                
                // Take screenshot
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var fileName = $"Screenshots/{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                screenshot.SaveAsFile(fileName, ScreenshotImageFormat.Png);
                Console.WriteLine($"Screenshot saved: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to take screenshot: {ex.Message}");
            }
        }

        [TearDown]
        public void Teardown()
        {
            // Close the browser
            driver?.Quit();
        }
    }
}