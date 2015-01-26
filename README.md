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
So when the rule for CountryName - NotEmpty is broken, FluentValidation should return this message to ASP.NET MVC: **[[[%0 should not be empty.|||((CountryName))]]]** and then i18n will lookup reource for key **"%0 should not be empty."** and **"CountryName"** for the proper language and format the string to be returned to the client! 

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
### Plugin your i18n adapted resources
And register this class in Application_Start
```csharp
   // set custom resources for ProviderType
   ValidatorOptions.ResourceProviderType = typeof(MyFluentValidationResources);
```

### Parameter formatting
We would also like to support tranlation of parameter name and so we want to first look for DisplayName attribute on the filed and next for a specified name (WithName()) or the name of the property: 
```csharp
    public static class MyFluentValidationOptions
    {
        public static i18nSettings Settings { get; set; }

        public static string DisplayNameResolver(Type type, MemberInfo member, LambdaExpression expression)
        {
            var displayName = GetDisplayName(type, member, expression);
            if (displayName == null) return null;

            return string.Format("{0}{1}{2}",
                Settings.NuggetParameterBeginToken,
                displayName.Replace(Settings.NuggetBeginToken, string.Empty)
                           .Replace(Settings.NuggetEndToken, string.Empty)
                           .Replace(Settings.NuggetParameterBeginToken, string.Empty)
                           .Replace(Settings.NuggetParameterEndToken, string.Empty),
                           Settings.NuggetParameterEndToken);
        }

        private static string GetDisplayName(Type type, MemberInfo member, LambdaExpression expression)
        {
            if (member != null)
            {
                // check for DisplayName attribute 
                var dnAttr = member.GetCustomAttributes<DisplayNameAttribute>().FirstOrDefault();
                if (dnAttr != null)
                    return (dnAttr).DisplayName;
            }

            // get the name from expression 
            if (expression != null)
            {
                var chain = PropertyChain.FromExpression(expression);
                if (chain.Count > 0)
                    return chain.ToString();
            }

            //return propertyname 
            if (member != null)
                return member.Name;

            return null;
        }
    }
```
     
So the purpose is to get the FieldName and retuned it with the correct NuggetParameterBeginToken and NuggetParameterEndToken as configured for i18n. 

### Plugin your ValidationOptions for parameter name
So to register our FluentValidationOptions we need to add this in Application:Start()
```csharp

    FluentValidationOptions.Settings =
        new i18nSettings(new WebConfigSettingService(HttpRuntime.AppDomainAppVirtualPath, true));
    ValidatorOptions.DisplayNameResolver = MyFluentValidationOptions.DisplayNameResolver;```


### FluentValidation.Mvc5 pitfalls and workaround 
Unfortunately - the creators of FluentValidation.Mvc determined that truncation the error message at the first "." is a good idea for RangeFluentValidation and StringLengthFluentValidation. So we must workaround these formatters to return the entire actual message. 

```csharp

    internal class MyRangeFluentValidationPropertyValidator : FluentValidationPropertyValidator
    {
        InclusiveBetweenValidator RangeValidator
        {
            get { return (InclusiveBetweenValidator)Validator; }
        }

        public MyRangeFluentValidationPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule propertyDescription, IPropertyValidator validator)
            : base(metadata, controllerContext, propertyDescription, validator)
        {
            ShouldValidate = false;
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            if (!ShouldGenerateClientSideRules()) yield break;

            var formatter = new MessageFormatter()
                .AppendPropertyName(Rule.GetDisplayName())
                .AppendArgument("From", RangeValidator.From)
                .AppendArgument("To", RangeValidator.To);

            var message = RangeValidator.ErrorMessageSource.GetString();
            message = formatter.BuildMessage(message);

            yield return new ModelClientValidationRangeRule(message, RangeValidator.From, RangeValidator.To);
        }
    }

    public class MyStringLengthFluentValidationPropertyValidator : FluentValidationPropertyValidator {
		private ILengthValidator LengthValidator {
			get { return (ILengthValidator)Validator; }
		}

        public MyStringLengthFluentValidationPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, PropertyRule rule, IPropertyValidator validator)
			: base(metadata, controllerContext, rule, validator) {
			ShouldValidate = false;
		}

		public override IEnumerable<ModelClientValidationRule> GetClientValidationRules() {
			if(!ShouldGenerateClientSideRules()) yield break;

			var formatter = new MessageFormatter()
				.AppendPropertyName(Rule.GetDisplayName())
				.AppendArgument("MinLength", LengthValidator.Min)
				.AppendArgument("MaxLength", LengthValidator.Max);

			var message = LengthValidator.ErrorMessageSource.GetString();
			message = formatter.BuildMessage(message);

			yield return new ModelClientValidationStringLengthRule(message, LengthValidator.Min, LengthValidator.Max) ;
		}
	}
}
```
### Plug custom validators into FluentValidation.Mvc5  
Our custom validators must then be registered with the FluentValidationModelProvider in Application_Start()

```csharp

    FluentValidationModelValidatorProvider.Configure(p =>
    {
        p.Add(typeof (InclusiveBetweenValidator),
            (metadata, context, rule, validator) =>
                new MyRangeFluentValidationPropertyValidator(metadata, context, rule, validator));
        p.Add(typeof(ILengthValidator), (metadata, context, rule, validator) => 
                new MyStringLengthFluentValidationPropertyValidator(metadata, context, rule, validator));
    });
```

### Summary
This repo contains a VS2013 solution with all the necessary code to make FluentValidation and i18n work together in validation and translation og resources. This is very handy when you application must support several languages and you use i18n to handle your translations. Using this code you will not rely on any of the resources in FluentValidation and your messages/tokens will be detected automatically by the i18n.PostBuild command. 