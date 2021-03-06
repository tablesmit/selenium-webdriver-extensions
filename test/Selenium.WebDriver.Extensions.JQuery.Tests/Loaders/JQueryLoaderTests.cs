﻿namespace Selenium.WebDriver.Extensions.JQuery.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Selenium.WebDriver.Extensions.Core;
    using Xunit;

    [Trait("Category", "Unit")]
    [ExcludeFromCodeCoverage]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    public class JQueryLoaderTests
    {
        [Fact]
        public void ShouldThrowExceptionWhenLoadingScriptWithNullArguments()
        {
            var loader = new JQueryLoader();

            var ex = Assert.Throws<ArgumentNullException>(() => loader.LoadScript(null));
            Assert.Equal("args", ex.ParamName);
        }

        [Fact]
        public void ShouldThrowExceptionWhenLoadingLoadScriptWithEmptyArguments()
        {
            var loader = new JQueryLoader();

            Assert.Throws<LoaderException>(() => loader.LoadScript(Enumerable.Empty<string>().ToArray()));
        }
    }
}
