﻿namespace Selenium.WebDriver.Extensions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using Moq;
    using OpenQA.Selenium;
    using Selenium.WebDriver.Extensions;
    using Xunit;
    using By = Selenium.WebDriver.Extensions.By;

    [Trait("Category", "Unit")]
    [ExcludeFromCodeCoverage]
    [SuppressMessage("ReSharper", "ExceptionNotDocumented")]
    [SuppressMessage("ReSharper", "ExceptionNotDocumentedOptional")]
    [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class WebElementExtensionsTests : IDisposable
    {
        private readonly Mock<IWebDriver> driverMock;

        public WebElementExtensionsTests()
        {
            this.driverMock = new Mock<IWebDriver>();
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript(It.IsRegex("window.jQuery"))).Returns(true);
        }

        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void ShouldFindElementWithJQuery()
        {
            var selector = By.JQuerySelector("div");
            
            var rootElement = new Mock<IWebElement>();
            rootElement.SetupGet(x => x.TagName).Returns("div");

            var element = new Mock<IWebElement>();
            element.SetupGet(x => x.TagName).Returns("span");

            var list = new List<IWebElement> { rootElement.Object };
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript("return jQuery('div').get();"))
                .Returns(new ReadOnlyCollection<IWebElement>(list));
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");
            var elementList = new List<IWebElement> { element.Object };
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('span', jQuery('body > div')).get();"))
                .Returns(new ReadOnlyCollection<IWebElement>(elementList));
            
            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.FindElement(By.JQuerySelector("span"));

            Assert.NotNull(result);
            Assert.Equal("span", result.TagName);
        }

        [Fact]
        public void ShouldFindElementsWithJQuery()
        {
            var selector = By.JQuerySelector("div");

            var rootElement = new Mock<IWebElement>();
            rootElement.SetupGet(x => x.TagName).Returns("div");

            var element1 = new Mock<IWebElement>();
            element1.Setup(x => x.TagName).Returns("span");
            element1.Setup(x => x.GetAttribute("class")).Returns("test1");

            var element2 = new Mock<IWebElement>();
            element2.Setup(x => x.TagName).Returns("span");
            element2.Setup(x => x.GetAttribute("class")).Returns("test2");

            var list = new List<IWebElement> { rootElement.Object };
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript("return jQuery('div').get();"))
                .Returns(new ReadOnlyCollection<IWebElement>(list));
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");
            var elementList = new List<IWebElement> { element1.Object, element2.Object };
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('span', jQuery('body > div')).get();"))
                .Returns(new ReadOnlyCollection<IWebElement>(elementList));

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.FindElements(By.JQuerySelector("span"));

            Assert.Equal(2, result.Count);

            Assert.Equal("span", result[0].TagName);
            Assert.Equal("test1", result[0].GetAttribute("class"));

            Assert.Equal("span", result[1].TagName);
            Assert.Equal("test2", result[1].GetAttribute("class"));
        }

        [Fact]
        public void ShouldFindText()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('span', jQuery('body > div')).text();")).Returns("test");
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");
            
            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("span").Text();

            Assert.Equal("test", result);
        }

        [Fact]
        public void ShouldFindHtml()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('span', jQuery('body > div')).html();"))
                .Returns("<p>test</p>");
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");
            
            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("span").Html();

            Assert.Equal("<p>test</p>", result);
        }

        [Fact]
        public void ShouldFindAttribute()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('a', jQuery('body > div')).attr('href');"))
                .Returns("http://github.com");
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");
            
            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("a").Attribute("href");

            Assert.Equal("http://github.com", result);
        }

        [Fact]
        public void ShouldFindPropertyString()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).prop('checked');"))
                .Returns("prop");
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").Property<string>("checked");

            Assert.Equal("prop", result);
        }

        [Fact]
        public void ShouldFindPropertyBoolean()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).prop('checked');"))
                .Returns(true);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").Property("checked");

            Assert.NotNull(result);
            Assert.True(result);
        }

        [Fact]
        public void ShouldFindValue()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).val();")).Returns("test");
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").Value();

            Assert.Equal("test", result);
        }

        [Fact]
        public void ShouldFindCss()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).css('display');"))
                .Returns("hidden");
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").Css("display");

            Assert.Equal("hidden", result);
        }

        [Fact]
        public void ShouldFindWidth()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).width();")).Returns(100L);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").Width();

            Assert.Equal(100L, result);
        }

        [Fact]
        public void ShouldFindHeight()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).height();")).Returns(100L);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").Height();

            Assert.Equal(100L, result);
        }

        [Fact]
        public void ShouldFindInnerWidth()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).innerWidth();"))
                .Returns(100L);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").InnerWidth();

            Assert.Equal(100L, result);
        }

        [Fact]
        public void ShouldFindInnerHeight()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).innerHeight();"))
                .Returns(100L);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").InnerHeight();

            Assert.Equal(100L, result);
        }

        [Fact]
        public void ShouldFindOuterWidth()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).outerWidth();"))
                .Returns(100L);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").OuterWidth();

            Assert.Equal(100L, result);
        }

        [Fact]
        public void ShouldFindOuterHeight()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).outerHeight();"))
                .Returns(100L);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").OuterHeight();

            Assert.Equal(100L, result);
        }

        [Fact]
        public void ShouldFindOuterWidthWithMargin()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).outerWidth(true);"))
                .Returns(100L);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").OuterWidth(true);

            Assert.Equal(100L, result);
        }

        [Fact]
        public void ShouldFindOuterHeightWithMargin()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).outerHeight(true);"))
                .Returns(100L);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").OuterHeight(true);

            Assert.Equal(100L, result);
        }

        [Fact]
        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        public void ShouldFindPosition()
        {
            var dict = new Dictionary<string, object> { { "top", 100 }, { "left", 200 } };

            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).position();")).Returns(dict);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var position = webElement.Object.JQuery("input").Position();

            Assert.NotNull(position);

            Assert.Equal(dict["top"], position.Value.Top);
            Assert.Equal(dict["left"], position.Value.Left);
        }

        [Fact]
        [SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
        public void ShouldFindOffset()
        {
            var dict = new Dictionary<string, object> { { "top", 100 }, { "left", 200 } };

            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).offset();")).Returns(dict);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var offset = webElement.Object.JQuery("input").Offset();

            Assert.NotNull(offset);

            Assert.Equal(dict["top"], offset.Value.Top);
            Assert.Equal(dict["left"], offset.Value.Left);
        }

        [Fact]
        public void ShouldFindScrollLeft()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).scrollLeft();"))
                .Returns(100L);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").ScrollLeft();

            Assert.Equal(100L, result);
        }

        [Fact]
        public void ShouldFindScrollTop()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).scrollTop();"))
                .Returns(100L);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").ScrollTop();

            Assert.Equal(100L, result);
        }

        [Fact]
        public void ShouldFindData()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).data('test');"))
                .Returns("val");
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").Data("test");

            Assert.Equal("val", result);
        }

        [Fact]
        public void ShouldFindIntData()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).data('test');"))
                .Returns(true);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").Data<bool?>("test");

            Assert.NotNull(result);
            Assert.True(result);
        }

        [Fact]
        public void ShouldFindCount()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('input', jQuery('body > div')).length;")).Returns(2L);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("input").Count();

            Assert.Equal(2, result);
        }

        [Fact]
        public void ShouldFindSerialized()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('form', jQuery('body > div')).serialize();"))
                .Returns("search=test");
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("form").Serialized();

            Assert.Equal("search=test", result);
        }

        [Fact]
        public void ShouldFindSerializedArray()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return JSON.stringify(jQuery('form', jQuery('body > div')).serializeArray());"))
                .Returns("[{\"name\":\"s\",\"value\":\"\"}]");
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("form").SerializedArray();

            Assert.Equal("[{\"name\":\"s\",\"value\":\"\"}]", result);
        }

        [Fact]
        public void HasClass()
        {
            var selector = By.QuerySelector("div");
            this.driverMock.As<IJavaScriptExecutor>()
                .Setup(x => x.ExecuteScript("return jQuery('form', jQuery('body > div')).hasClass('test');"))
                .Returns(true);
            this.driverMock.As<IJavaScriptExecutor>().Setup(x => x.ExecuteScript(It.IsRegex("function\\(element\\)")))
                .Returns("body > div");

            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.TagName).Returns("div");
            webElement.SetupGet(x => x.Selector).Returns(selector);
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            var result = webElement.Object.JQuery("form").HasClass("test");

            Assert.Equal(true, result);
        }

        [Fact]
        public void ShouldGetHelper()
        {
            var webElement = new Mock<Core.WebElement>();
            webElement.SetupGet(x => x.WrappedDriver).Returns(this.driverMock.Object);

            webElement.Object.JQuery(By.JQuerySelector("input"));
            Assert.True(true);
        }

        [Fact]
        public void ShouldThrowExceptionWhenGettingHelperWithNullSelectorAndNullWebElement()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => WebElementExtensions.JQuery(null, (JQuery.JQuerySelector)null));
            Assert.Equal("webElement", ex.ParamName);
        }

        [Fact]
        public void ShouldThrowExceptionWhenGettingHelperWithNullStringSelectorAndNullWebElement()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => WebElementExtensions.JQuery(null, (string)null));
            Assert.Equal("webElement", ex.ParamName);
        }

        [Fact]
        public void ShouldThrowExceptionWhenGettingHelperWithNullSelector()
        {
            var webElement = new Mock<Core.WebElement>();
            var ex = Assert.Throws<ArgumentNullException>(() => webElement.Object.JQuery((JQuery.JQuerySelector)null));
            Assert.Equal("selector", ex.ParamName);
        }

        [Fact]
        public void ShouldThrowExceptionWhenGettingHelperWithNullStringSelector()
        {
            var webElement = new Mock<Core.WebElement>();
            var ex = Assert.Throws<ArgumentNullException>(() => webElement.Object.JQuery((string)null));
            Assert.Equal("selector", ex.ParamName);
        }
    }
}
