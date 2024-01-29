using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace PageManager.WPF
{
    public class PageManager : IPageManager
    {
        private readonly Dictionary<string, Type> _pages = new();

        private readonly IServiceProvider _serviceProvider;

        public PageManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Type GetPageType(string key)
        {
            Type pageType;
            lock (_pages)
            {
                if (!_pages.TryGetValue(key, out pageType))
                {
                    throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
                }
            }
            return pageType;
        }

        public System.Windows.Controls.Page GetPage(string key)
        {
            Type pageType = GetPageType(key);
            return _serviceProvider.GetService(pageType) as System.Windows.Controls.Page;
        }

        public void Configure<VM, V>()
            where VM : ObservableObject
            where V : System.Windows.Controls.Page
        {
            lock (_pages)
            {
                string key = typeof(VM).FullName;

                if (_pages.ContainsKey(key))
                {
                    throw new ArgumentException($"The key {key} is already configured in PageService");
                }

                Type type = typeof(V);

                if (_pages.Any(p => p.Value == type))
                {
                    throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
                }

                _pages.Add(key, type);
            }
        }
    }
}
