using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Dynamic;
using OpenQA.Selenium;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using POM.Selenium.Entities;
using POM.Selenium.Contracts;
using POM.Selenium.Shared.Models;
using static System.Text.Json.JsonSerializer;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PomDependencyInjectionExtensions
    {
        public static IServiceCollection AddPomSelenium(
            this IServiceCollection services, 
            bool autoGeneratePages=true,
            string configFile="appsettings.json")
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            configFile = File.ReadAllText(Path.Combine(path, configFile));
            var configObj = Deserialize<ExpandoObject>(configFile);
            
            var rawPagesObjects = PageParser<ExpandoObject>("Pages", configObj);
            var pomPages = PageParser<ExpandoObject>("Pages", rawPagesObjects["Pages"]);
            var schema = pomPages.ToDictionary(k => k.Key, v => PageParser<Selector>("Page", v.Value));

            services.ServiceRegister<PageSchemaModel>(ServiceLifetime.Singleton, schema);

            if(autoGeneratePages)
                GeneratePagesFromJson(schema);
            
            return services;
        }

        private static void GeneratePagesFromJson(Dictionary<string, Dictionary<string, Selector>> schemaObject)
        {
            Directory.CreateDirectory("Pages");

            foreach (var schema in schemaObject)
            {
                var pagePath = Path.Combine("Pages", schema.Key);
                Directory.CreateDirectory(pagePath);

                foreach(var pageValues in schema.Value)
                {
                    var fileName = Path.Combine(pagePath, $"{pageValues.Key}.cs");
                    var project = Path.GetFileName(Directory.GetCurrentDirectory());
                    var templateReplacement = new Dictionary<string, string>
                    { 
                        {"project", project}, 
                        {"pages", "Pages"},
                        {"pageName", schema.Key},
                        {"className", pageValues.Key}
                    };
                    var regex = new Regex("\\{{(.*?)\\}}");
                    var formatedTemplate = regex.Replace(TemplateModel.Class, (match) => templateReplacement[match.Groups[1].Value]);

                    if(!File.Exists(fileName))
                        File.WriteAllText(fileName, formatedTemplate, Encoding.UTF8);
                }
            }
        }

        public static TPage AddPage<TPage>(this IServiceProvider provider, bool requireDriver=true) where TPage : IPageEntity
        {
            var driver = requireDriver 
                ? provider.GetRequiredService<IWebDriver>() 
                : null;

            var selectors = provider.GetPageSelector<TPage>();
            return (TPage)Activator.CreateInstance(typeof(TPage), driver, selectors);
        }

        private static IServiceCollection AddPage<TPage>(this IServiceCollection services, ServiceLifetime serviceLifetime, bool requireDriver =true) 
            where TPage : class, IPageEntity
        {
            var provider = services.BuildServiceProvider();
            var driver = requireDriver
                ? provider.GetRequiredService<IWebDriver>()
                : null;

            var selectors = provider.GetPageSelector<TPage>();
            services.ServiceRegister<TPage>(serviceLifetime, new object[] { driver, selectors });
            return services;
        }

        private static Selector GetPageSelector<TPage>(this IServiceProvider provider) where TPage : IPageEntity
        {
            var schema = provider.GetRequiredService<PageSchemaModel>().Schema;
            var namespaces = typeof(TPage)?.FullName.Split('.');
            var parent = namespaces?[namespaces.Length - 2];

            return schema[parent][typeof(TPage).Name];
        }

        private static Dictionary<string, TParse> PageParser<TParse>(string keyName, ExpandoObject expando) where TParse: class  
        {
            return expando
            .Where(root => root.Key.Contains(keyName))
            .ToDictionary(k => k.Key, v => Deserialize<TParse>(v.Value.ToString()));
        }

        private static IServiceCollection ServiceRegister<TService>(this IServiceCollection services, ServiceLifetime serviceLifetime, params object[] parameters) 
            where TService: class  
        {
            var instance = Activator.CreateInstance(typeof(TService), parameters);

            switch (serviceLifetime)
            {
                case ServiceLifetime.Scoped: services.AddScoped(typeof(TService), (_) => instance); break;
                case ServiceLifetime.Singleton: services.AddSingleton(typeof(TService), (_) => instance); break;
                case ServiceLifetime.Transient: services.AddTransient(typeof(TService), (_) => instance); break;
            }
            return services;
        }
    }
}