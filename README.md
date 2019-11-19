# CSP Header Generator

A small and simple library to help generate rules for CSP (Content-Security-Policy) headers.

#### Quick features:
* Enum for most common directive names
* Constants for some of the common values
* Can add your own directives, should the enum be incomplete
* Methods for quickly adding the ones used by Google Tag Manager and analytics ([Used this as reference](https://developers.google.com/tag-manager/web/csp))

## Example usage
```C#
CSPHeaderGenerator headerGenerator = new CSPHeaderGenerator(CSPHeaderGenerator.StaticValues.None);
headerGenerator.AddDirective(CSPHeaderGenerator.DirectiveType.Font, CSPHeaderGenerator.StaticValues.Self);
headerGenerator.AddDirective(CSPHeaderGenerator.DirectiveType.Img, CSPHeaderGenerator.StaticValues.Self);
headerGenerator.AddDirective(CSPHeaderGenerator.DirectiveType.Img, CSPHeaderGenerator.StaticValues.SchemaData);
headerGenerator.AddDirective(CSPHeaderGenerator.DirectiveType.Style, CSPHeaderGenerator.StaticValues.Self);
headerGenerator.AddGoogleTagManager();

Response.AddHeader("Content-Security-Policy", headerGenerator.ToString());
```
