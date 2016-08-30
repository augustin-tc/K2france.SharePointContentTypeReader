using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Client;
using System.Configuration;
using System.Text;

namespace K2France.SharePointContentTypeReader.Pages
{
    public partial class DocMetadata : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //get content type name
            string contentTypeName = "Finance document";

            //list all fields for content type 
            using (ClientContext context = new ClientContext(ConfigurationManager.AppSettings["SPweb"]))
            {
                StringBuilder strOut = new StringBuilder();
                Web web = context.Web;
                context.Load(web, Items => Items.ContentTypes);
                context.ExecuteQuery();
                ContentType contentType = web.ContentTypes.FirstOrDefault(x => x.Name == contentTypeName);
                if (contentType != null)
                {
                    context.Load(contentType.Fields);
                    context.ExecuteQuery();
                    foreach(var field in contentType.Fields)
                    {
                        strOut.AppendLine(string.Format("<div><b>{0}</b></div>", field.Title));
                    }
                }
                else
                {
                    Response.Write("Error content type not found");
                }
                Response.Write(strOut);
            }
                //assign all content type fields to the passed id

            }
        void Test()
        {
            using (ClientContext context = new ClientContext(ConfigurationManager.AppSettings["SPweb"]))
            {
                Web web = context.Web;
                context.Load(web);
                string contentTypeName = Request.QueryString["ContentType"];

                //string docNumber = Request.QueryString["DocId"];
                //int documentId = int.Parse(docNumber);
                int documentId = 2;
                var list = web.Lists.GetByTitle("Documents");

                var query = new CamlQuery();
                query.ViewXml = string.Format(
                        @"<View>  
                        <Query> 
                            <Where>
                                <Eq><FieldRef Name='ID' />
                                <Value Type='Counter'>{0}</Value></Eq>
                            </Where> 
                        </Query> 
                    </View>", documentId);

                var item = list.GetItemById(documentId);
                // context.Load(item, Items => Items.DisplayName, Items => Items. ,Items => Items.ContentType, Items=> Items.ContentType.Fields);
                context.Load(item);

                context.ExecuteQuery();

                //Response.Write(item.DisplayName);

                foreach (var field in item.FieldValues)
                {
                    Response.Write("<div>" + field.Key + " : " + field.Value + "<div>");
                }
               

            }
        }
    }
}