using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;

namespace NCommander.Plugins
{
    public class AutofacPlugin : IDependencyPlugin
    {
        private Container _container;

        public AutofacPlugin RegisterContainer(Action<AutoFacPluginOptions> configure)
        {
            var options = new AutoFacPluginOptions();
            configure(options);
            _container = options.Container;
            return this;
        }
        public void Inject<T>(T obj) where T : ICommand
        {
            typeof(T).GetProperties(BindingFlags.Public | BindingFlags.DeclaredOnly).ToList().ForEach(x =>
            {
                var value = _container.Resolve(x.PropertyType);
                if (value == null)
                {
                    throw new ArgumentNullException($"Missing Registry for {x.Name}");
                } 
                x.SetValue(obj, value);
            });
        }
    }
}