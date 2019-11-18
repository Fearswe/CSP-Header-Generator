# CSP Header Generator

A small and simple library to help generate rules for CSP (Content-Security-Policy) headers.
Quick features:
* Enum for most common directive names
* Constants for some of the common values
* Can add your own directives, should the enum be incomplete
* Methods for quickly adding the ones used by Google Tag Manager and analytics ([Used this as reference](https://developers.google.com/tag-manager/web/csp))

## Example usage
```C#
CSPHeaderBuilder headerBuilder = new CSPHeaderBuilder(CSPHeaderBuilder.StaticValues.None);
headerBuilder.AddDirective(CSPHeaderBuilder.DirectiveType.Font, CSPHeaderBuilder.StaticValues.Self);
headerBuilder.AddDirective(CSPHeaderBuilder.DirectiveType.Img, CSPHeaderBuilder.StaticValues.Self);
headerBuilder.AddDirective(CSPHeaderBuilder.DirectiveType.Img, CSPHeaderBuilder.StaticValues.SchemaData);
headerBuilder.AddDirective(CSPHeaderBuilder.DirectiveType.Style, CSPHeaderBuilder.StaticValues.Self);
headerBuilder.AddGoogleTagManager();

Response.AddHeader("Content-Security-Policy", headerBuilder.ToString());
```
