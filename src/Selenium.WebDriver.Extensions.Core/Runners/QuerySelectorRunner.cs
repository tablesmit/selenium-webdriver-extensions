﻿namespace Selenium.WebDriver.Extensions.Core
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using OpenQA.Selenium;
    
    /// <summary>
    /// The query selector runner.
    /// </summary>
    public class QuerySelectorRunner : JavaScriptRunner
    {
        /// <summary>
        /// Performs a JavaScript query selector search on the <see cref="IWebDriver"/> using given 
        /// <see cref="ISelector"/> selector and script format string.
        /// </summary>
        /// <typeparam name="T">The type of the result to be returned.</typeparam>
        /// <param name="driver">The Selenium web driver.</param>
        /// <param name="selector">The Selenium JavaScript query selector.</param>
        /// <returns>Parsed result of invoking the script.</returns>
        /// <remarks>
        /// Because of the limitations of the Selenium the only valid types are: <see cref="long"/>, 
        /// <see cref="Nullable{Long}"/>, <see cref="bool"/>, <see cref="Nullable"/>, <see cref="string"/>, 
        /// <see cref="IWebElement"/> and <see cref="IEnumerable"/>.
        /// Selenium returns different types depending if element has been found or not. If there's a match a
        /// <see cref="ReadOnlyCollection{IWebElement}"/> is returned, but if there are no matches than it will return
        /// an empty <see cref="ReadOnlyCollection{T}"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Driver is null.
        /// -or- Selector is null.
        /// </exception>
        /// <exception cref="QuerySelectorNotSupportedException">
        /// The query selector is not supported by the browser.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// An element in the sequence cannot be cast to type <typeparamref name="T" />.
        /// </exception>
        /// <exception cref="ArgumentException">Script is empty.</exception>
        public override T Find<T>(IWebDriver driver, ISelector selector)
        {
            if (driver == null)
            {
                throw new ArgumentNullException("driver");
            }

            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            driver.QuerySelector().CheckSupport();
            return JavaScriptRunner.Find<T>(driver, "return " + selector.Selector + ";");
        }
    }
}
