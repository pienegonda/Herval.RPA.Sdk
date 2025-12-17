using Herval.RPA.Sdk.Enums;
using OpenQA.Selenium;

namespace Herval.RPA.Sdk.Helpers
{
    internal static class SeletorHelper
    {
        internal static By ConverterParaBy(ESeletor tipo, string seletor)
        {
            return tipo switch
            {
                ESeletor.Id => By.Id(seletor),
                ESeletor.XPath => By.XPath(seletor),
                ESeletor.Css => By.CssSelector(seletor),
                ESeletor.Name => By.Name(seletor),
                ESeletor.ClassName => By.ClassName(seletor),
                ESeletor.TagName => By.TagName(seletor),
                _ => throw new ArgumentException("Tipo de seletor inválido")
            };
        }
    }
}