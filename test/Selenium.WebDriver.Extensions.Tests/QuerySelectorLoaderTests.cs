﻿namespace Selenium.WebDriver.Extensions.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using NUnit.Framework;

    /// <summary>
    /// Query selector loader tests.
    /// </summary>
    [TestFixture]
    [Category("Unit Tests")]
    [ExcludeFromCodeCoverage]
    class QuerySelectorLoaderTests
    {
        /// <summary>
        /// Tests script loading.
        /// </summary>
        [Test]
        public void LoadScript()
        {
            var loadScript = new QuerySelectorLoader().LoadScript();
            Assert.IsNull(loadScript);
        }

        /// <summary>
        /// Tests default library URL.
        /// </summary>
        [Test]
        public void DefaultLibraryUrl()
        {
            var url = new QuerySelectorLoader().LibraryUri;
            Assert.IsNull(url);
        }
    }
}