using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.ServiceModel;
using System.ServiceModel.Activation;
using EuforyServices.ServiceContract;
using EuforyServices.ServiceImplementation;
using System.IO;

namespace EuforyServices
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //route apis
            RouteTable.Routes.Add(new ServiceRoute("api", new WebServiceHostFactory(), typeof(EuforyServices.ServiceImplementation.EuforyServices)));


            RouteTable.Routes.MapPageRoute("index", "index", "~/index.aspx");
            RouteTable.Routes.MapPageRoute("contact", "contact", "~/contact.aspx");
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // http://lcdeuropa.com/
            
            try
            {

            
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "https://customer.myclaud.com");
            //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "http://localhost:4200");

                //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin",  Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
                //string k = Request.UrlReferrer.GetLeftPart(UriPartial.Authority);

                Uri requestUrl;
                requestUrl = new Uri(Request.Url.ToString());
                //HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", requestUrl.GetLeftPart(UriPartial.Authority));

                if (Request.UrlReferrer!= null)
                {
                    var h = Request.UrlReferrer.GetLeftPart(UriPartial.Authority);
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", Request.UrlReferrer.GetLeftPart(UriPartial.Authority));
                }


                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");


                 


                if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
                {
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST, PUT, DELETE");
                    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");

                    HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                    HttpContext.Current.Response.End();
                }
                 
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}