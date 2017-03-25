using HangFire_Infrastructure.CustomAttributeClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HangFireApi.Controllers
{
    [CustomRequestAuthorize]
 
    public class BaseApiController : ApiController
    {
        public BaseApiController()
        { }
    }
}