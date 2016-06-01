using Alchemy4Tridion.Plugins.GUI.Configuration;

namespace PublishrdEnvironment.GUI
{
    public class PluginCommandSet : Alchemy4Tridion.Plugins.GUI.Configuration.CommandSet
    {
        public PluginCommandSet()
        {
            // we only need to add the name of our command
            AddCommand("CheckPublishedEnvironment");
        }
    }
}
