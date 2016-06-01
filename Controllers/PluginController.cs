using Alchemy4Tridion.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Xml;
using Tridion.ContentManager.CoreService.Client;
using Tridion.Logging;

namespace PublishrdEnvironment.Controllers
{ 
    /// <summary>
    /// An ApiController to create web services that your plugin can interact with.
    /// </summary>
    /// <remarks>
    /// The AlchemyRoutePrefix accepts a Service Name as its first parameter.  This will be used by both
    /// the generated Url's as well as the generated JS proxy.
    /// <c>/Alchemy/Plugins/{YourPluginName}/api/{ServiceName}/{action}</c>
    /// <c>Alchemy.Plugins.YourPluginName.Api.Service.action()</c>
    /// 
    /// The attribute is optional and if you exclude it, url's and methods will be attached to "api" instead.
    /// <c>/Alchemy/Plugins/{YourPluginName}/api/{action}</c>
    /// <c>Alchemy.Plugins.YourPluginName.Api.action()</c>
    /// </remarks>
    [AlchemyRoutePrefix("Service")]
    public class PluginController : AlchemyApiController
    {
        /// // GET /Alchemy/Plugins/{YourPluginName}/api/{YourServiceName}/YourRoute
        /// <summary>
        /// Just a simple example..
        /// </summary>
        /// <returns>A string "Your Response" as the response message.</returns>
        /// </summary>
        /// <remarks>
        /// All of your action methods must have both a verb attribute as well as the RouteAttribute in
        /// order for the js proxy to work correctly.
        /// </remarks>
        //[HttpGet]
        //[Route("CheckPublishedEnvironment")]
        //public string SayHello()
        //{
        //    return String.Format("Hello Alchemist {0}!", User.GetName());
        //}

        [HttpPost]
        [Route("PublishedEnvironment")]
        public ModelClass[] PublishedEnvironment(GetParameter selectedItem)
        {
            string _selectedItemTcmId = selectedItem.OrganizationalItemId;
            string _stagingTcmId = selectedItem.stagingTcmId;
            string _liveTcmId = selectedItem.liveTcmId;

            var result = Client.GetListXml(_selectedItemTcmId, new OrganizationalItemItemsFilterData
            {
                ItemTypes = new[] { ItemType.Page,ItemType.Component },
                Recursive = true,
                BaseColumns = ListBaseColumns.Default
            });
            var doc = new XmlDocument();
            doc.Load(result.CreateReader());
            XmlElement items = doc.DocumentElement;
            List<ModelClass> listOfItems = new List<ModelClass>();
            foreach (XmlElement item in items)
            {
                ModelClass _model = new ModelClass();
                string tcmId = item.GetAttribute("ID");
                string title = item.GetAttribute("Title");
                var objectList = Client.GetListPublishInfo(tcmId);
                List<string> publishedToTargets = GetResultForCurrentPublication(objectList, tcmId);

                _model.itemTcmId = tcmId;
                _model.itemTitle = title;

                foreach (var target in publishedToTargets)
                {
                    if (target == _stagingTcmId)
                    {
                        _model.isPublishedToStaging = true;
                    }
                    else if (target == _liveTcmId)
                    {
                        _model.isPublishedToLive = true;
                    }
                }
                listOfItems.Add(_model);
            }
            
            return listOfItems.ToArray();
        }
        static DataTable ConvertToDatatable(List<ModelClass> list)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("TcmId");
            dt.Columns.Add("Title");
            dt.Columns.Add("Published To Staging");
            dt.Columns.Add("Published To Live");
            foreach (var item in list)
            {
                dt.Rows.Add(item.itemTcmId, item.itemTitle, Convert.ToString(item.isPublishedToStaging), Convert.ToString(item.isPublishedToLive));
            }

            return dt;
        }
        private static List<string> GetResultForCurrentPublication(PublishInfoData[] objectList, string tcmId)
        {
            List<string> targets = new List<string>();
            string pubIdOfItem = GetPublicationIdofItem(tcmId);
            foreach (var item in objectList)
            {
                string pubid = GetPublicationIdofIRepository(item.Repository.IdRef);
                if (pubid == pubIdOfItem)
                {
                    targets.Add(item.PublicationTarget.IdRef);
                }
            }
            return targets;
        }
        private static string GetPublicationIdofIRepository(string repoId)
        {
            string pubId = null;
            Regex regex = new Regex(@"tcm:\d+\-(\d+).*");
            Match match = regex.Match(repoId);
            if (match.Success)
            {
                pubId = match.Groups[1].Value;
            }
            return pubId;
        }
        private static string GetPublicationIdofItem(string id)
        {
            string pubId = null;
            Regex regex = new Regex(@"tcm:(\d+).*");
            Match match = regex.Match(id);
            if (match.Success)
            {
                pubId = match.Groups[1].Value;
            }
            return pubId;
        }
    }
}
