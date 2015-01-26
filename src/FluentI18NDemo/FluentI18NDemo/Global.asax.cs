using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FluentI18NDemo.FluentValidation;
using FluentValidation;
using FluentValidation.Mvc;
using FluentValidation.Validators;
using i18n;
using i18n.Domain.Concrete;

namespace FluentI18NDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // set custom resources for ProviderType
            ValidatorOptions.ResourceProviderType = typeof(MyFluentValidationResources);
            // custom formatter for DisplayName 
            FluentValidationOptions.Settings =
                new i18nSettings(new WebConfigSettingService(HttpRuntime.AppDomainAppVirtualPath, true));
            ValidatorOptions.DisplayNameResolver = FluentValidationOptions.WeDisplayNameResolver;
            FluentValidationModelValidatorProvider.Configure(p =>
            {
                p.Add(typeof (InclusiveBetweenValidator),
                    (metadata, context, rule, validator) =>
                        new MyRangeFluentValidationPropertyValidator(metadata, context, rule, validator));
                p.Add(typeof(ILengthValidator), (metadata, context, rule, validator) => 
                        new MyStringLengthFluentValidationPropertyValidator(metadata, context, rule, validator));
            });

          

            // Change from the default of 'en'.
            i18n.LocalizedApplication.Current = new LocalizedApplication(new DefaultRootServices())
            {
                DefaultLanguage = "nb-NO",
                // Change from the of temporary redirects during URL localization
                PermanentRedirects = true,
                ApplicationPath = string.IsNullOrEmpty(HttpRuntime.AppDomainAppVirtualPath) ? "/" : HttpRuntime.AppDomainAppVirtualPath
            };

            // Change the URL localization scheme from Scheme1.
            i18n.UrlLocalizer.UrlLocalizationScheme = i18n.UrlLocalizationScheme.Scheme2;

            // Blacklist certain URLs from being 'localized'.
            i18n.UrlLocalizer.IncomingUrlFilters += delegate(Uri url)
            {
                if (url.LocalPath.EndsWith("sitemap.xml", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                return true;
            };


            i18n.UrlLocalizer.QuickUrlExclusionFilter = new System.Text.RegularExpressions.Regex(@"(?:\.(?:less|css)(?:\?|$)|(?i:i18nSkip|glimpse|trace|elmah)|\/Content\/|\/Scripts\/|\/bundles\/|\/fonts\/|\/api\/|\/__browserLink\/)");

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("nb-NO");

            //var setting = ConfigurationManager.AppSettings["i18n.VisualizeMessages"];
            //bool result = !string.IsNullOrEmpty(setting) && setting == "true";


        }
    }
}
