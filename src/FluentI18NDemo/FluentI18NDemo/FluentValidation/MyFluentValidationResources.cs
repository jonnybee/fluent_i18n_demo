using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Web;

namespace FluentI18NDemo.FluentValidation
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
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
}