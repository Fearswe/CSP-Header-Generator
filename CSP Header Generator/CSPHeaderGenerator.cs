using System;
using System.Collections.Generic;
using System.Linq;

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
		/// Default constructor but allows specifying the Default directive value
		/// </summary>
		/// <param name="Default">Value(s) of the Default directive</param>
		public CSPHeaderGenerator(params String[] Default) : this()
		{
			this.AddDirective(DirectiveType.Default, Default);
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
					header += $" {(directive.Key.Contains("-") ? directive.Key : directive.Key + "-src")} {String.Join(" ", directive.Value)};";
				}
			}

			if (String.IsNullOrWhiteSpace(header))
			{
				throw new Exception("No directives declared");
			}
			return header.Trim();
		}

		#region Privates

		private Dictionary<String, List<String>> Directives { get; set; }

		private void AddDirective(String directiveType, List<String> values)
		{
			if (values.Count == 0)
			{
				throw new ArgumentNullException(nameof(values), $"You must provide at least one value to add");
			}

			if (this.Directives.TryGetValue(directiveType, out List<String> directive))
			{
				foreach (var value in values)
				{
					if (!directive.Contains(value))
					{
						directive.Add(value);
					}
				}
			}
			else
			{
				this.Directives.Add(directiveType, values);
			}
		}

		private void AddReportUri(List<String> uris)
		{
			this.AddDirective("report-uri", uris);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="directiveType"></param>
		/// <param name="values"></param>
		private void RemoveDirectiveValue(String directiveType, List<String> values)
		{
			if (values.Count == 0)
			{
				throw new ArgumentNullException(nameof(values), $"You must provide at least one value to remove");
			}

			if (this.Directives.TryGetValue(directiveType, out List<String> directive))
			{
				foreach (var value in values)
				{
					if (directive.Contains(value))
					{
						directive.Remove(value);
					}
				}
			}
		}

		private void ReplaceDirectiveValue(String directiveType, List<String> values)
		{
			if (values.Count == 0)
			{
				throw new ArgumentNullException(nameof(values), $"You must provde at lest one value to replace, if you want to clear a directive use {nameof(this.ClearDirective)} instead");
			}

			if (this.Directives.ContainsKey(directiveType))
			{
				this.Directives[directiveType] = values;
			}
		}

		#endregion Privates

		/// <summary>
		/// Add one or more values to a directive
		/// </summary>
		/// <param name="directiveType">The directive to add to</param>
		/// <param name="values">The value(s) to add</param>
		/// <exception cref="ArgumentNullException">Values may not be empty</exception>
		public void AddDirective(DirectiveType directiveType, params String[] values)
		{
			this.AddDirective(directiveType.ToString().ToLower(), values.ToList<String>());
		}

		/// <summary>
		/// Add one or more values to a directive
		/// </summary>
		/// <param name="directiveType">The directive to add to</param>
		/// <param name="values">The value(s) to add</param>
		/// <exception cref="ArgumentNullException">Values may not be empty</exception>
		public void AddDirective(String directiveType, params String[] values)
		{
			this.AddDirective(directiveType, values.ToList<String>());
		}

		/// <summary>
		/// Add one or more values to report-uri directive
		/// </summary>
		/// <param name="uris">The value(s) to add</param>
		/// <exception cref="ArgumentNullException">Uris may not be empty</exception>
		public void AddReportUri(params String[] uris)
		{
			this.AddReportUri(uris.ToList<String>());
		}

		/// <summary>
		/// Add one or more values to report-uri directive
		/// Note that Uri.ToString() will append a trailing slash
		/// </summary>
		/// <param name="uris">The value(s) to add</param>
		/// <exception cref="ArgumentNullException">Uris may not be empty</exception>
		public void AddReportUri(params Uri[] uris)
		{
			this.AddReportUri(uris.Select(uri => uri.ToString()).ToList<String>());
		}

		/// <summary>
		/// Replaces all values in a directive with new value(s)
		/// </summary>
		/// <param name="directiveType">THe directive to replace</param>
		/// <param name="values">The value(s) to replace</param>
		/// <exception cref="ArgumentNullException">Values may not be empty</exception>
		public void ReplaceDirectiveValues(String directiveType, params String[] values)
		{
			this.ReplaceDirectiveValue(directiveType, values.ToList<String>());
		}

		/// <summary>
		/// Replaces all values in a directive with new value(s)
		/// </summary>
		/// <param name="directiveType">THe directive to replace</param>
		/// <param name="values">The value(s) to replace</param>
		/// <exception cref="ArgumentNullException">Values may not be empty</exception>
		public void ReplaceDirectiveValues(DirectiveType directiveType, params String[] values)
		{
			this.ReplaceDirectiveValue(directiveType.ToString().ToLower(), values.ToList<String>());
		}

		/// <summary>
		/// Remove one or more values from a directive
		/// </summary>
		/// <param name="directiveType">The directive to remove from</param>
		/// <param name="values">The value(s) to remove</param>
		/// <exception cref="ArgumentNullException">Values may not be empty</exception>
		public void RemoveDirectiveValues(DirectiveType directiveType, params String[] values)
		{
			this.RemoveDirectiveValue(directiveType.ToString().ToLower(), values.ToList<String>());
		}

		/// <summary>
		/// Remove one or more values from a directive
		/// </summary>
		/// <param name="directiveType">The directive to remove from</param>
		/// <param name="values">The value(s) to remove</param>
		/// <exception cref="ArgumentNullException">Values may not be empty</exception>
		public void RemoveDirectiveValues(String directiveType, params String[] values)
		{
			this.RemoveDirectiveValue(directiveType, values.ToList<String>());
		}

		/// <summary>
		/// Clear a directive of all values
		/// </summary>
		/// <param name="directiveType">The directive to clear</param>
		public void ClearDirective(String directiveType)
		{
			if (this.Directives.TryGetValue(directiveType, out List<String> directive))
			{
				directive.Clear();
			}
		}

		/// <summary>
		/// Clear a directive of all values
		/// </summary>
		/// <param name="directiveType">The directive to clear</param>
		public void ClearDirective(DirectiveType directiveType)
		{
			this.ClearDirective(directiveType.ToString().ToLower());
		}

		#region Pre-made directives

		/// <summary>
		/// Automatically add the directives needed and used by Google Tag Manager
		/// </summary>
		/// <param name="customJavascriptVariables">If you use custom javascript variables, will allow unsafe-eval for scripts</param>
		public void AddGoogleTagManager(Boolean customJavascriptVariables = false)
		{
			this.AddDirective(DirectiveType.Script, StaticValues.UnsafeInline, "https://www.googletagmanager.com");
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
			this.AddDirective(DirectiveType.Style, "https://tagmanager.google.com", "https://fonts.googleapis.com");
			this.AddDirective(DirectiveType.Img, "https://ssl.gstatic.com", "https://www.gstatic.com");
			this.AddDirective(DirectiveType.Font, "https://fonts.gstatic.com", StaticValues.SchemaData);
		}

		/// <summary>
		/// Automatically add the directives needed and used by Google Analytics
		/// </summary>
		public void AddGoogleAnalytics()
		{
			this.AddDirective(DirectiveType.Script, "https://www.google-analytics.com", "https://ssl.google-analytics.com");
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
			this.AddDirective(DirectiveType.Script, "https://www.googleadservices.com", "https://www.google.com");
			this.AddDirective(DirectiveType.Img, "https://googleads.g.doubleclick.net", "https://www.google.com");
		}

		/// <summary>
		/// Automatically add the directives needed and used by Google Ads Remarketing
		/// </summary>
		public void AddGoogleAdsRemarketing()
		{
			this.AddDirective(DirectiveType.Script, "https://www.googleadservices.com", "https://googleads.g.doubleclick.net", "https://www.google.com");
			this.AddDirective(DirectiveType.Img, "https://www.google.com");
			this.AddDirective(DirectiveType.Frame, "https://bid.g.doubleclick.net");
		}

		#endregion Pre-made directives
	}
}