using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Linq;
using Kids1135PluginBase;

namespace Kids1135
{
    internal class Kids1135ViewModel : INotifyPropertyChanged
    {
        private Page pageFromPlugin;
        private IPlugin selectedPlugin;

        public Page PageFromPlugin
        {
            get => pageFromPlugin;
            set
            {
                pageFromPlugin = value;
                Signal();
            }
        }

        public ObservableCollection<IPlugin> Plugins { get; set; }

        public IPlugin SelectedPlugin
        {
            get => selectedPlugin;
            set
            {
                selectedPlugin = value;
                PageFromPlugin = selectedPlugin?.GetPage();
            }
        }

        public Kids1135ViewModel()
        {
            Plugins = LoadPlugins();
        }

        private ObservableCollection<IPlugin>? LoadPlugins()
        {
            var files = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "Plugins"), "*.dll");
            ObservableCollection<IPlugin> result = new ObservableCollection<IPlugin>();
            foreach (var file in files)
            {
                try
                {
                    var assembly = Assembly.LoadFile(file);
                    var startType = assembly.DefinedTypes.First(s=>s.ImplementedInterfaces.Contains(typeof(IPlugin)));
                    result.Add((IPlugin)Activator.CreateInstance(startType));
                }
                catch { }
            }
            return result;
        }

        private void Signal([CallerMemberName] string prop = null)
         => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}