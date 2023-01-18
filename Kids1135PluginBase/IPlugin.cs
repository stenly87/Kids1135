using System;
using System.Windows.Controls;

namespace Kids1135PluginBase
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        string Author { get; }
        Page GetPage();
    }
}
