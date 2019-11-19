using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CSP_Header_Generator;

namespace CSP_Header_Generator_Test
{
	[TestClass]
	public class CSPHeaderGeneratorTest
	{
		[TestMethod]
		public void TestCSPHeaderBuilderConstructor()
		{
			var cspHeaderBuilderDefault = new CSPHeaderGenerator(CSPHeaderGenerator.StaticValues.None);
			var cspHeaderBuilderNoDefault = new CSPHeaderGenerator();

			Assert.IsFalse(String.IsNullOrWhiteSpace(cspHeaderBuilderDefault.ToString()));
			Assert.IsTrue(String.Equals(cspHeaderBuilderDefault.ToString(), "default-src 'none';"));
			Assert.ThrowsException<Exception>(cspHeaderBuilderNoDefault.ToString);
		}

		[TestMethod]
		public void TestCSPHeaderBuilderCustomValue()
		{
			var cspHeaderBuilder = new CSPHeaderGenerator();
			cspHeaderBuilder.AddDirective(CSPHeaderGenerator.DirectiveType.Default, CSPHeaderGenerator.StaticValues.Self);

			Assert.IsFalse(String.IsNullOrWhiteSpace(cspHeaderBuilder.ToString()));
			Assert.IsTrue(String.Equals(cspHeaderBuilder.ToString(), "default-src 'self';"));
		}

		[TestMethod]
		public void TestCSPHeaderBuilderGoogleDirectives()
		{
			var cspHeaderBuilder = new CSPHeaderGenerator();
			cspHeaderBuilder.AddGoogleTagManager();

			Assert.IsFalse(String.IsNullOrWhiteSpace(cspHeaderBuilder.ToString()));
			Assert.IsTrue(String.Equals(cspHeaderBuilder.ToString(), "img-src https://www.googletagmanager.com; script-src 'unsafe-inline' https://www.googletagmanager.com;"));

		}

		[TestMethod]
		public void TestCSPHeaderBuilderCustomDirective()
		{
			var cspHeaderBuilder = new CSPHeaderGenerator();

			cspHeaderBuilder.AddDirective("test", CSPHeaderGenerator.StaticValues.Self);
			cspHeaderBuilder.AddDirective("test-test", CSPHeaderGenerator.StaticValues.Self);

			Assert.IsFalse(String.IsNullOrWhiteSpace(cspHeaderBuilder.ToString()));
			Assert.IsTrue(String.Equals(cspHeaderBuilder.ToString(), "test-src 'self'; test-test 'self';"));
		}
	}
}
