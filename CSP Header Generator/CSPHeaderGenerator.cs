using System;
using System.Collections.Generic;

namespace CSP_Header_Generator
{
	public class CSPHeaderGenerator
	{
		/// <summary>
		/// Common directive types
		/// </summary>
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


		/// <summary>
		/// Common values for directives
		/// </summary>
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

		/// <summary>
		/// Default constructor
		/// </summary>
		public CSPHeaderGenerator()
		{
			this.Directives = new Dictionary<String, List<String>>();

			foreach (var directive in Enum.GetValues(typeof(DirectiveType)))
			{
				this.Directives.Add(directive.ToString().ToLower(), new List<String>());
			}
		}

		/// <summary>
		/// Default constructor but allows specifying the Default directive
		/// </summary>
		/// <param name="Default">Value of the Default directive</param>
		public CSPHeaderGenerator(String Default) : this()
		{
			this.AddDirective(DirectiveType.Default, Default);
		}

		/// <summary>
		/// Add a value to a directive using the common directive types
		/// </summary>
		/// <param name="directiveType">The directive to add value to</param>
		/// <param name="value">The value to add to the directive</param>
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

		/// <summary>
		/// Add a custom, or common, directive type and assing it a value
		/// </summary>
		/// <param name="directiveType">The directive type to add</param>
		/// <param name="value">The value to set to the directive type</param>
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

		/// <summary>
		/// Add a report-uri directive
		/// </summary>
		/// <param name="uri">The uri to the server to send the report to</param>
		public void AddReportUri(String uri)
		{
			if(this.Directives.TryGetValue("report-uri", out List<String> directive))
			{
				directive.Add(uri);
			}
			else
			{
				this.Directives.Add("report-uri", new List<String> { uri });
			}
		}

		/// <summary>
		/// Add a report-uri directive
		/// </summary>
		/// <param name="uri">The uri to the server to send the report to</param>
		public void AddReportUri(Uri uri)
		{
			this.AddReportUri(uri.ToString());
		}


		/// <summary>
		/// Automatically add the directives needed and used by Google Tag Manager
		/// </summary>
		/// <param name="customJavascriptVariables">If you use custom javascript variables, will allow unsafe-eval for scripts</param>
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

		/// <summary>
		/// Automatically add the directives needed and used by Google Tag Manager Preview
		/// </summary>
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

		/// <summary>
		/// Automatically add the directives needed and used by Google Analytics
		/// </summary>
		public void AddGoogleAnalytics()
		{
			this.AddDirective(DirectiveType.Script, "https://www.google-analytics.com");
			this.AddDirective(DirectiveType.Script, "https://ssl.google-analytics.com");

			this.AddDirective(DirectiveType.Img, "https://www.google-analytics.com");

			this.AddDirective(DirectiveType.Connect, "https://www.google-analytics.com");
		}

		/// <summary>
		/// Automatically add the directives needed and used by Google Optimize
		/// </summary>
		public void AddGoogleOptimize()
		{
			this.AddDirective(DirectiveType.Script, "https://www.google-analytics.com");
		}

		/// <summary>
		/// Automatically add the directives needed and used by Google Ads Conversions
		/// </summary>
		public void AddGoogleAdsConversions()
		{
			this.AddDirective(DirectiveType.Script, "https://www.googleadservices.com");
			this.AddDirective(DirectiveType.Script, "https://www.google.com");

			this.AddDirective(DirectiveType.Img, "https://googleads.g.doubleclick.net");
			this.AddDirective(DirectiveType.Img, "https://www.google.com");
		}

		/// <summary>
		/// Automatically add the directives needed and used by Google Ads Remarketing
		/// </summary>
		public void AddGoogleAdsRemarketing()
		{
			this.AddDirective(DirectiveType.Script, "https://www.googleadservices.com");
			this.AddDirective(DirectiveType.Script, "https://googleads.g.doubleclick.net");
			this.AddDirective(DirectiveType.Script, "https://www.google.com");

			this.AddDirective(DirectiveType.Img, "https://www.google.com");

			this.AddDirective(DirectiveType.Frame, "https://bid.g.doubleclick.net");
		}

		/// <summary>
		/// Generate the complete set of directives ready to use as a header value
		/// </summary>
		/// <returns>The directive set</returns>
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
