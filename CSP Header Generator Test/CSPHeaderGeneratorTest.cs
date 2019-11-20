using CSP_Header_Generator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CSP_Header_Generator_Test
{
	[TestClass]
	public class CSPHeaderGeneratorTest
	{
		[TestMethod]
		public void TestCSPHeaderGeneratorConstructor()
		{
			var cspHeaderGeneratorDefault = new CSPHeaderGenerator(CSPHeaderGenerator.StaticValues.None);
			var cspHeaderGeneratorNoDefault = new CSPHeaderGenerator();
			Assert.ThrowsException<Exception>(cspHeaderGeneratorNoDefault.ToString);

			var headerValue = cspHeaderGeneratorDefault.ToString();
			Assert.IsFalse(String.IsNullOrWhiteSpace(headerValue), $"{nameof(headerValue)} is null");
			Assert.IsTrue(String.Equals(headerValue, $"default-src {CSPHeaderGenerator.StaticValues.None};"), $"{nameof(headerValue)} is \"{headerValue}\"");
		}

		[TestMethod]
		public void TestCSPHeaderGeneratorAddDirective()
		{
			var cspHeaderGenerator = new CSPHeaderGenerator(CSPHeaderGenerator.StaticValues.None);
			Assert.ThrowsException<ArgumentNullException>(() => { cspHeaderGenerator.AddDirective(CSPHeaderGenerator.DirectiveType.Script); });
			Assert.ThrowsException<ArgumentNullException>(() => { cspHeaderGenerator.AddDirective("script"); });

			cspHeaderGenerator.AddDirective(CSPHeaderGenerator.DirectiveType.Script, CSPHeaderGenerator.StaticValues.Self, "https://google.com");
			cspHeaderGenerator.AddDirective("font", CSPHeaderGenerator.StaticValues.Self);

			cspHeaderGenerator.AddDirective("test", CSPHeaderGenerator.StaticValues.Self);
			cspHeaderGenerator.AddDirective("test-test", CSPHeaderGenerator.StaticValues.Self);

			var headerValue = cspHeaderGenerator.ToString();
			Assert.IsFalse(String.IsNullOrWhiteSpace(headerValue), $"{nameof(headerValue)} is null");
			Assert.IsTrue(String.Equals(headerValue, $"default-src {CSPHeaderGenerator.StaticValues.None}; font-src {CSPHeaderGenerator.StaticValues.Self}; script-src {CSPHeaderGenerator.StaticValues.Self} https://google.com; test-src {CSPHeaderGenerator.StaticValues.Self}; test-test {CSPHeaderGenerator.StaticValues.Self};"), $"{nameof(headerValue)} is \"{headerValue}\"");
		}

		[TestMethod]
		public void TestCSPHeaderGeneratorGoogleDirectives()
		{
			var cspHeaderGenerator = new CSPHeaderGenerator();
			cspHeaderGenerator.AddGoogleTagManager();

			var headerValue = cspHeaderGenerator.ToString();
			Assert.IsFalse(String.IsNullOrWhiteSpace(headerValue), $"{nameof(headerValue)} is null");
			Assert.IsTrue(String.Equals(headerValue, $"img-src https://www.googletagmanager.com; script-src {CSPHeaderGenerator.StaticValues.UnsafeInline} https://www.googletagmanager.com;"), $"{nameof(headerValue)} is \"{headerValue}\"");
		}

		[TestMethod]
		public void TestCSPHeaderGeneratorReportUri()
		{
			var cspHeaderGenerator = new CSPHeaderGenerator();

			cspHeaderGenerator.AddReportUri("http://google.com");
			cspHeaderGenerator.AddReportUri(new Uri("http://test.test"));

			var headerValue = cspHeaderGenerator.ToString();
			Assert.IsFalse(String.IsNullOrWhiteSpace(headerValue), $"{nameof(headerValue)} is null");
			Assert.IsTrue(String.Equals(headerValue, "report-uri http://google.com http://test.test/;"), $"{nameof(headerValue)} is \"{headerValue}\"");
		}

		[TestMethod]
		public void TestCSPHeaderClearDirective()
		{
			var cspHeaderGenerator = new CSPHeaderGenerator(CSPHeaderGenerator.StaticValues.None);

			cspHeaderGenerator.AddDirective(CSPHeaderGenerator.DirectiveType.Script, CSPHeaderGenerator.StaticValues.Self, "http://google.com");
			cspHeaderGenerator.AddDirective("font", CSPHeaderGenerator.StaticValues.Self, "http://google.com");

			var originalValue = cspHeaderGenerator.ToString();

			cspHeaderGenerator.ClearDirective(CSPHeaderGenerator.DirectiveType.Script);
			cspHeaderGenerator.ClearDirective("font");

			var valueChange = cspHeaderGenerator.ToString();
			Assert.IsFalse(String.IsNullOrWhiteSpace(valueChange), $"{nameof(valueChange)} is null");
			Assert.IsFalse(String.Equals(originalValue, valueChange), $"{nameof(originalValue)} and {nameof(valueChange)} are identical when they shouldn't be");
			Assert.IsTrue(String.Equals(valueChange, $"default-src {CSPHeaderGenerator.StaticValues.None};"), $"{nameof(valueChange)} is \"{valueChange}\"");
		}

		[TestMethod]
		public void TestCSPHeaderReplaceDirective()
		{
			var cspHeaderGenerator = new CSPHeaderGenerator(CSPHeaderGenerator.StaticValues.None);
			Assert.ThrowsException<ArgumentNullException>(() => { cspHeaderGenerator.ReplaceDirectiveValues(CSPHeaderGenerator.DirectiveType.Script); });
			Assert.ThrowsException<ArgumentNullException>(() => { cspHeaderGenerator.ReplaceDirectiveValues("script"); });

			cspHeaderGenerator.AddDirective(CSPHeaderGenerator.DirectiveType.Script, CSPHeaderGenerator.StaticValues.Self, "http://google.com");
			cspHeaderGenerator.AddDirective("font", CSPHeaderGenerator.StaticValues.Self, "http://google.com");

			var originalValue = cspHeaderGenerator.ToString();

			cspHeaderGenerator.ReplaceDirectiveValues(CSPHeaderGenerator.DirectiveType.Script, CSPHeaderGenerator.StaticValues.UnsafeInline);
			cspHeaderGenerator.ReplaceDirectiveValues("font", CSPHeaderGenerator.StaticValues.UnsafeInline);

			var valueChange = cspHeaderGenerator.ToString();
			Assert.IsFalse(String.IsNullOrWhiteSpace(valueChange), $"{nameof(valueChange)} is null");
			Assert.IsFalse(String.Equals(originalValue, valueChange), $"{nameof(originalValue)} and {nameof(valueChange)} are identical when they shouldn't be");
			Assert.IsTrue(String.Equals(valueChange, $"default-src {CSPHeaderGenerator.StaticValues.None}; font-src {CSPHeaderGenerator.StaticValues.UnsafeInline}; script-src {CSPHeaderGenerator.StaticValues.UnsafeInline};"), $"{nameof(valueChange)} is \"{valueChange}\"");
		}

		[TestMethod]
		public void TestCSPHeaderRemoveDirective()
		{
			var cspHeaderGenerator = new CSPHeaderGenerator();
			Assert.ThrowsException<ArgumentNullException>(() => { cspHeaderGenerator.RemoveDirectiveValues(CSPHeaderGenerator.DirectiveType.Script); });
			Assert.ThrowsException<ArgumentNullException>(() => { cspHeaderGenerator.RemoveDirectiveValues("script"); });

			cspHeaderGenerator.AddDirective(CSPHeaderGenerator.DirectiveType.Script, CSPHeaderGenerator.StaticValues.Self, "http://google.com", CSPHeaderGenerator.StaticValues.SchemaHttps);

			var originalValue = cspHeaderGenerator.ToString();

			cspHeaderGenerator.RemoveDirectiveValues("script", "http://google.com");

			var valueFirstChange = cspHeaderGenerator.ToString();
			Assert.IsFalse(String.IsNullOrWhiteSpace(valueFirstChange), $"{nameof(valueFirstChange)} is null");
			Assert.IsFalse(String.Equals(originalValue, valueFirstChange), $"{nameof(originalValue)} and {nameof(valueFirstChange)} are identical when they shouldn't be");
			Assert.IsTrue(String.Equals(valueFirstChange, $"script-src {CSPHeaderGenerator.StaticValues.Self} {CSPHeaderGenerator.StaticValues.SchemaHttps};"), $"{nameof(valueFirstChange)} is \"{valueFirstChange}\"");

			cspHeaderGenerator.RemoveDirectiveValues(CSPHeaderGenerator.DirectiveType.Script, CSPHeaderGenerator.StaticValues.SchemaHttps);

			var valueSecondChange = cspHeaderGenerator.ToString();
			Assert.IsFalse(String.IsNullOrWhiteSpace(valueSecondChange), $"{nameof(valueSecondChange)} is null");
			Assert.IsFalse(String.Equals(originalValue, valueSecondChange), $"{nameof(originalValue)} and {nameof(valueSecondChange)} are identical when they shouldn't be");
			Assert.IsFalse(String.Equals(valueFirstChange, valueSecondChange), $"{nameof(valueFirstChange)} and {nameof(valueSecondChange)} are identical when they shouldn't be");
			Assert.IsTrue(String.Equals(valueSecondChange, $"script-src {CSPHeaderGenerator.StaticValues.Self};"), $"{nameof(valueSecondChange)} is \"{valueSecondChange}\"");
		}
	}
}