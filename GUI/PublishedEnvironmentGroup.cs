using Alchemy4Tridion.Plugins.GUI.Configuration;
using Alchemy4Tridion.Plugins.GUI.Configuration.Elements;

namespace PublishrdEnvironment.GUI
{
    public class PublishedEnvironmentGroup : Alchemy4Tridion.Plugins.GUI.Configuration.ResourceGroup
    {
        public PublishedEnvironmentGroup()
        {
            AddFile("PublishedEnvironment.js");
            AddFile("PublishedEnvironment.css");
            AttachToView("PublishedEnvironment.aspx");
            AddWebApiProxy();
            Dependencies.AddLibraryJQuery();
            Dependencies.Add("Tridion.Web.UI.Editors.CME");
            
        }
    }
}
