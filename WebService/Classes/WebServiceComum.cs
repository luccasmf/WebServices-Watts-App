using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WebService.Classes
{
    public class WebServiceComum : System.Web.Services.WebService
    {
        protected JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

    }
}