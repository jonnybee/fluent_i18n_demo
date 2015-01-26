using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FluentValidation;
using FluentValidation.Attributes;

namespace FluentI18NDemo.Models
{
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
}