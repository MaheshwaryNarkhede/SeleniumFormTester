# SeleniumFormTester : Selenium Automation Practice Form Test

This project demonstrates a resilient approach to automating web UI testing with C# and Selenium WebDriver. It focuses on testing three fields of an automation practice form in a way that is resistant to UI changes.

## Features

- **Resilient Element Selection**: Multiple selector strategies ensure tests continue working even if IDs, names, or positions change
- **Clean Test Structure**: Uses NUnit for testing framework
- **Screenshot Capture**: Automated screenshot capture for test verification
- **Proper Wait Handling**: WebDriverWait implementation to handle dynamic loading elements

## Fields Tested

1. **Name Field**: Tests input functionality using multiple identification strategies
2. **Email Field**: Tests email input with fallback selectors
3. **Gender Selection**: Tests radio button selection resilient to layout changes

## Setup Instructions

### Prerequisites
- .NET 6.0 SDK or later
- Chrome browser installed
- Visual Studio 2019/2022 or your preferred C# IDE

### Running the Tests

1. Clone this repository
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Build the solution
5. Run the tests via the Test Explorer

## Design Principles

### Resilient Element Selection
The code uses a `FindElementSafely` method that tries multiple selector strategies in order of reliability:
1. ID (most reliable)
2. Name
3. CSS Selectors
4. XPath (with various fallback strategies)

This approach ensures that if one selector fails due to UI changes, others will be tried.

### Wait Handling
The code properly waits for elements to be both visible and interactable before attempting to interact with them, avoiding timing issues.

### Screenshot Capture
Screenshots are automatically taken after each test action to provide visual verification of the test execution.

## Project Structure

- `PracticeFormTests.cs`: Contains the test methods and logic
- `Screenshots/`: Directory for storing test execution screenshots

## Notes for Maintainers

If the website structure changes significantly, you may need to:
1. Add additional selector strategies to the `FindElementSafely` method
2. Update the XPath expressions if the page structure changes dramatically
3. Adjust the wait times if page loading behavior changes

## Future Improvements

- Add more comprehensive test coverage for additional form fields
- Implement a Page Object Model for better maintainability
- Add data-driven testing capabilities
- Implement cross-browser testing
