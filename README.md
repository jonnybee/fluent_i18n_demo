# FluentValidation + i18n demo
## FluentValidation and i18n internationalization demo project
### How to combine FluentValidation with i18n 

The i18n is designed to replace the use og .NET reesources in favor of an **easier**, globally, recognized standard for localizing ASP.NET web applications. 
FluentValidation offer more flexibility in defining rules and can even be used with IoC to inject configuration objects to determine which rules to run while also supporting client side validation. 

This project is a result from my experience with getting FluentValidation and i18n to play together nicely. There are some pitfalls that wil be highligheted. 

i18n home: http://github.com/turquoiseowl/i18n 
FluentValidation: http://fluentvalidation.codeplex.com

### What do we want to achive
We want to define our view models with DisplayName attributes and have i18n translate these automatically when used as a label in the form. i18n also support translation of parameters but there is at current no published nuget package for v2.0 branch of i18n. (so I have built i18n from the repo as of 24th of january 2015 and added in libs folder for this project)  

We also want FluentValidation to return a format string where the message is used as a key by i18n to get the actual resource and also translate the field name before it is formatted into the returned message. 

```csharp

    [Validator(typeof(CountryValidator))]
    public class Country
    {
        [Display(AutoGenerateField = false)]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        
        [DisplayName("[[[CountryCode]]]")]
        public string CountryCode { get; set; }

        [DisplayName("[[[CountryName]]]")]
        public string CountryName { get; set; }

        [DisplayName("[[[TwoLetterIsoCode]]]")]
        public string TwoLetterIsoCode { get; set; }
    } 

    public class CountryValidator : AbstractValidator<Country>
    {
        public CountryValidator()
        {
            RuleFor(c => c.CountryCode).NotEmpty().Length(1, 11);
            RuleFor(c => c.CountryName).NotEmpty().Length(1, 50);
            RuleFor(c => c.TwoLetterIsoCode).NotNull().Length(1, 2);
        }
    }
```
So when the rule for CountryName - NotEmpty is broken, FluentValidation should return this message to ASP.NET MVC: 
**[[[%0 should not be empty.|||((CountryName))]]]** 
and then i18n will lookup reource for key **"%0 should not be empty."** and **"CountryName"** for the proper language and format the string to be returned to the client! 


### Defining you own i18n resource 
Create a class that holds the resources for FluentValidation that i18n can translate 

```csharp
public class MyFluentValidationResources
{
    public static string CreditCardError { get { return "[[[%0 is not a valid credit card number.|||{PropertyName}]]]"; } }
    public static string email_error { get { return "[[[%0 is not a valid email address.|||{PropertyName}]]]"; } }
    public static string equal_error { get { return "[[[%0 should be equal to '%1'.|||{PropertyName}|||{ComparisonValue}]]]"; } }
    public static string exact_length_error { get { return "[[[%0 must be %1 characters in length.|||{PropertyName}|||{MaxLength}]]]"; } }
    public static string exclusivebetween_error { get { return "[[[%0 must be between %1 and %2 (exclusive).|||{PropertyName}|||{From}|||{To}]]]"; } }
    public static string greaterthan_error { get { return "[[[%0 must be greater than '%1'.|||{PropertyName}|||{ComparisonValue}]]]"; } }
    public static string greaterthanorequal_error { get { return "[[[%0 must be greater than or equal to '%1'.|||{PropertyName}|||{ComparisonValue}]]]"; } }
    public static string inclusivebetween_error { get { return "[[[%0 must be between %1 and %2.|||{PropertyName}|||{From}|||{To}]]]"; } }
    public static string length_error { get { return "[[[%0 must be between %1 and %2 characters.|||{PropertyName}|||{MinLength}|||{MaxLength}]]]"; } }
    public static string lessthan_error { get { return "[[[%0 must be less than '%1'.|||{PropertyName}|||{ComparisonValue}]]]"; } }
    public static string lessthanorequal_error { get { return "[[[%0 must be less than or equal to '%1'.|||{PropertyName}|||{ComparisonValue}]]]"; } }
    public static string notempty_error { get { return "[[[%0 should not be empty.|||{PropertyName}]]]"; } }
    public static string notequal_error { get { return "[[[%0 should not be equal to '%1'.|||{PropertyName}|||{ComparisonValue}]]]"; } }
    public static string notnull_error { get { return "[[[%0 must not be empty.|||{PropertyName}]]]"; } }
    public static string predicate_error { get { return "The specified condition was not met for %0.|||{PropertyName}]]]"; } }
    public static string regex_error { get { return "[[[%0 is not in the correct format.|||{PropertyName}]]]"; } }
    public static string scale_precision_error { get { return "[[[%0 may not be more than %1 digits in total, with allowance for %2 decimals.|||{PropertyName}|||{expectedPrecision}|||{expectedScale}]]]"; } }
}
```
### plugin your i18n resource
And register this class in Application_Start
```csharp
   // set custom resources for ProviderType
   ValidatorOptions.ResourceProviderType = typeof(MyFluentValidationResources);
```

### parameter formatting
We would also like to support tranlation of parameter name
```csharp
   // set custom resources for ProviderType
   ValidatorOptions.ResourceProviderType = typeof(MyFluentValidationResources);
```
     


### FluentValidation.Mvc5 pitfalls and workaround 


