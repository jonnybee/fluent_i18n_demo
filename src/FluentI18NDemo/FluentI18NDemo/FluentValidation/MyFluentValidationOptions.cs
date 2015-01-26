using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using FluentValidation.Internal;
using i18n.Domain.Concrete;

namespace FluentI18NDemo.FluentValidation
{
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
}