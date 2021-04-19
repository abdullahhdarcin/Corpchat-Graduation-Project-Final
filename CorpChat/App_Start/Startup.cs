using System;
using System.Threading.Tasks;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CorpChat.App_Start.Startup))]

namespace CorpChat.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
