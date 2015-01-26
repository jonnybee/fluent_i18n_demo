using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Internal;
using FluentValidation.Mvc;
using FluentValidation.Validators;

namespace FluentI18NDemo.FluentValidation
{

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
}