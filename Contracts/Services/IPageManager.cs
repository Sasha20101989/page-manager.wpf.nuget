using System;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace PageManager.WPF
{
    public interface IPageManager
    {
        Type GetPageType(string key);

        System.Windows.Controls.Page GetPage(string key);

        void Configure<VM, V>()
            where VM : ObservableObject
            where V : System.Windows.Controls.Page;
    }
}
