using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSP_Header_Generator;

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

			Assert.IsFalse(String.IsNullOrWhiteSpace(cspHeaderGeneratorDefault.ToString()));
			Assert.IsTrue(String.Equals(cspHeaderGeneratorDefault.ToString(), "default-src 'none';"));
			Assert.ThrowsException<Exception>(cspHeaderGeneratorNoDefault.ToString);
		}

		[TestMethod]
		public void TestCSPHeaderGeneratorCustomValue()
		{
			var cspHeaderGenerator = new CSPHeaderGenerator();
			cspHeaderGenerator.AddDirective(CSPHeaderGenerator.DirectiveType.Default, CSPHeaderGenerator.StaticValues.Self);

			Assert.IsFalse(String.IsNullOrWhiteSpace(cspHeaderGenerator.ToString()));
			Assert.IsTrue(String.Equals(cspHeaderGenerator.ToString(), "default-src 'self';"));
		}

		[TestMethod]
		public void TestCSPHeaderGeneratorGoogleDirectives()
		{
			var cspHeaderGenerator = new CSPHeaderGenerator();
			cspHeaderGenerator.AddGoogleTagManager();

			Assert.IsFalse(String.IsNullOrWhiteSpace(cspHeaderGenerator.ToString()));
			Assert.IsTrue(String.Equals(cspHeaderGenerator.ToString(), "img-src https://www.googletagmanager.com; script-src 'unsafe-inline' https://www.googletagmanager.com;"));

		}

		[TestMethod]
		public void TestCSPHeaderGeneratorCustomDirective()
		{
			var cspHeaderGenerator = new CSPHeaderGenerator();

			cspHeaderGenerator.AddDirective("test", CSPHeaderGenerator.StaticValues.Self);
			cspHeaderGenerator.AddDirective("test-test", CSPHeaderGenerator.StaticValues.Self);

			Assert.IsFalse(String.IsNullOrWhiteSpace(cspHeaderGenerator.ToString()));
			Assert.IsTrue(String.Equals(cspHeaderGenerator.ToString(), "test-src 'self'; test-test 'self';"));
		}
	}
}
