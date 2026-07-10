using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLib;
namespace PluginService;

public class PluginService
{
    private readonly string _path;

    private enum Visit_Metric { None, Visiting, Visited }

    public PluginService(string path)
    {
        _path = path;
    }

    public List <ICommand> LoadSortPlugins()
    {
        List<PluginMetadata> plugins = DiscoverPlugins();

        List <Type> sortedTypes = DFS(plugins);

        return PluginsActivator(sortedTypes);
    }

    private List<PluginMetadata> DiscoverPlugins()
    {
        var foundPlugins = new List <PluginMetadata>();

        if (!Directory.Exists(_path))
        return foundPlugins;

        string[] DLL_path = Directory.GetFiles(_path, "*.dll");

        foreach (string dll_path in DLL_path)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dll_path);

                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(ICommand).IsAssignableFrom(type) && type.IsClass&& !type.IsAbstract)
                    {
                        var attribute = type.GetCustomAttribute<PluginLoadAttribute>();

                        if (attribute != null)
                        {
                            foundPlugins.Add(new PluginMetadata
                            {
                            PluginType = type,
                            PluginName = attribute.PluginName,
                            Dependencies = attribute.Dependencies.ToList()
                            });
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Не удалось прочитать библиотеку {dll_path} из-за ошибки: {exception.Message}");
            }
        }
        return foundPlugins;
    }

    private List<Type> DFS(List<PluginMetadata> plugins)
    {
        var sortedTypes = new List<Type>();

        var pluginMap = plugins.ToDictionary(p => p.PluginName, p => p);

        var states = plugins.ToDictionary(p => p.PluginName, p => Visit_Metric.None);

        foreach (var plugin in plugins)
        {
            if (states[plugin.PluginName] == Visit_Metric.None)
            VisitNode(plugin.PluginName, pluginMap, states, sortedTypes);
        }
        return sortedTypes;
    }

    private void VisitNode(
        string currentName, 
        Dictionary<string, PluginMetadata> pluginMap, 
        Dictionary<string, Visit_Metric> states, 
        List<Type> sortedTypes)
    {
        states[currentName] = Visit_Metric.Visiting;
        if (pluginMap.TryGetValue(currentName, out var currentPlugin))
        {
            foreach (var dependency in currentPlugin.Dependencies)
            {
                if (!pluginMap.ContainsKey(dependency))
                {
                    throw new InvalidOperationException(
                        $"Плагин '{currentName}' требует зависимость '{dependency}', но её нет ни в одной DLL.");
                }

                if (states[dependency] == Visit_Metric.Visiting)
                {
                    throw new InvalidOperationException(
                        $"Во время выполнения была замечена циклическая зависимость плагинов: '{currentName}' <-> '{dependency}'");
                }

                if (states[dependency] == Visit_Metric.None)
                {
                    VisitNode(dependency, pluginMap, states, sortedTypes);
                }
            }
        }

        states[currentName] = Visit_Metric.Visited;

        if (currentPlugin != null)
        sortedTypes.Add(currentPlugin.PluginType);
    }

    private List <ICommand> PluginsActivator(List<Type> sortedTypes)
    {
        var activations = new List <ICommand>();

        foreach (var type in sortedTypes)
        {
            var activated = Activator.CreateInstance(type) as ICommand;

            if (activated != null)
            activations.Add(activated);
        }
        return activations;
    }

    public class PluginMetadata
    {
        public Type PluginType { get; set;} = null!;
        public string PluginName { get; set;} = string.Empty;
        public List <string> Dependencies { get; set; } = new();
    }
}
