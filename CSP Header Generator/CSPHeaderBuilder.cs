using System;
using System.Collections.Generic;

namespace CSP_Header_Generator
{
	public class CSPHeaderBuilder
	{
		public enum DirectiveType
		{
			Default,
			Font,
			Connect,
			Frame,
			Img,
			Manifest,
			Media,
			Script,
			Style
		}

		public static class StaticValues
		{
			public static String Wildcard = "'*'";
			public static String Self = "'self'";
			public static String None = "'none'";
			public static String UnsafeEval = "'unsafe-eval'";
			public static String UnsafeInline = "'unsafe-inline'";
			public static String SchemaHttp = "http:";
			public static String SchemaHttps = "https:";
			public static String SchemaData = "data:";
			public static String SchemaMediastream = "mediastream:";
			public static String SchemaBlob = "blob:";
			public static String SchemaFilestream = "filesystem:";
		}

		private Dictionary<String, List<String>> Directives { get; set; }

		public CSPHeaderBuilder()
		{
			this.Directives = new Dictionary<String, List<String>>();

			foreach (var directive in Enum.GetValues(typeof(DirectiveType)))
			{
				this.Directives.Add(directive.ToString().ToLower(), new List<String>());
			}
		}

		public CSPHeaderBuilder(String Default) : this()
		{
			this.AddDirective(DirectiveType.Default, Default);
		}

		public void AddDirective(DirectiveType directiveType, String value)
		{
			if (this.Directives.TryGetValue(directiveType.ToString().ToLower(), out List<String> directive))
			{
				directive.Add(value);
			}
			else
			{
				this.Directives.Add(directiveType.ToString().ToLower(), new List<String>());
			}
		}

		public void AddDirective(String directiveType, String value)
		{
			if (this.Directives.TryGetValue(directiveType.ToLower(), out List<String> directive))
			{
				directive.Add(value);
			}
			else
			{
				this.Directives.Add(directiveType.ToLower(), new List<String> { value });
			}
		}


		public void AddGoogleTagManager(Boolean customJavascriptVariables = false)
		{
			this.AddDirective(DirectiveType.Script, StaticValues.UnsafeInline);
			this.AddDirective(DirectiveType.Script, "https://www.googletagmanager.com");

			this.AddDirective(DirectiveType.Img, "https://www.googletagmanager.com");

			if (customJavascriptVariables)
			{
				this.AddDirective(DirectiveType.Script, StaticValues.UnsafeEval);
			}

		}

		public void AddGoogleTagManagerPreview()
		{
			this.AddDirective(DirectiveType.Script, "https://tagmanager.google.com");

			this.AddDirective(DirectiveType.Style, "https://tagmanager.google.com");
			this.AddDirective(DirectiveType.Style, "https://fonts.googleapis.com");

			this.AddDirective(DirectiveType.Img, "https://ssl.gstatic.com");
			this.AddDirective(DirectiveType.Img, "https://www.gstatic.com");

			this.AddDirective(DirectiveType.Font, "https://fonts.gstatic.com");
			this.AddDirective(DirectiveType.Font, StaticValues.SchemaData);
		}

		public void AddGoogleAnalytics()
		{
			this.AddDirective(DirectiveType.Script, "https://www.google-analytics.com");
			this.AddDirective(DirectiveType.Script, "https://ssl.google-analytics.com");

			this.AddDirective(DirectiveType.Img, "https://www.google-analytics.com");

			this.AddDirective(DirectiveType.Connect, "https://www.google-analytics.com");
		}

		public void AddGoogleOptimize()
		{
			this.AddDirective(DirectiveType.Script, "https://www.google-analytics.com");
		}

		public void AddGoogleAdsConversions()
		{
			this.AddDirective(DirectiveType.Script, "https://www.googleadservices.com");
			this.AddDirective(DirectiveType.Script, "https://www.google.com");

			this.AddDirective(DirectiveType.Img, "https://googleads.g.doubleclick.net");
			this.AddDirective(DirectiveType.Img, "https://www.google.com");
		}

		public void AddGoogleAdsRemarketing()
		{
			this.AddDirective(DirectiveType.Script, "https://www.googleadservices.com");
			this.AddDirective(DirectiveType.Script, "https://googleads.g.doubleclick.net");
			this.AddDirective(DirectiveType.Script, "https://www.google.com");

			this.AddDirective(DirectiveType.Img, "https://www.google.com");

			this.AddDirective(DirectiveType.Frame, "https://bid.g.doubleclick.net");
		}


		public override String ToString()
		{
			String header = String.Empty;

			foreach (var directive in this.Directives)
			{
				if (directive.Value.Count > 0)
				{
					header += $" {(directive.Key.ToLower().Contains("-") ? directive.Key.ToLower() : directive.Key.ToLower() + "-src")} {String.Join(" ", directive.Value)};"; 
				}
			}

			if(String.IsNullOrWhiteSpace(header))
			{
				throw new Exception("No directives declared");
			}
			return header.Trim();
		}

	}
}
