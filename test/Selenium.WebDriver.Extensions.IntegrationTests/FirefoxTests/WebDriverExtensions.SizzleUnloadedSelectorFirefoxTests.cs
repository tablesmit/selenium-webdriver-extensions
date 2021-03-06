﻿namespace Selenium.WebDriver.Extensions.IntegrationTests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Xunit;

    [Trait("Category", "Integration")]
    [Trait("Browser", "Firefox")]
    [ExcludeFromCodeCoverage]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    public class WebDriverExtensionsSizzleUnloadedSelectorFirefoxTests :
        WebDriverExtensionsSizzleSelectorTests, IClassFixture<FirefoxFixture>
    {
        public WebDriverExtensionsSizzleUnloadedSelectorFirefoxTests(FirefoxFixture fixture)
        {
            this.Browser = fixture.Browser;
            this.Browser.Navigate().GoToUrl(new Uri("http://localhost:50502/SizzleLoaded"));
        }
    }
}
