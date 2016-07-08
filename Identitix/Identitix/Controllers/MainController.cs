﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Identitix.Controllers
{
    /// <summary>
    /// This is the main server controller of the application.
    /// (I don't think we will need more controllers)
    /// The client (index.html) will talk to this cotroller. 
    /// TODO: Check we'll send the Facebook images from the client, or just the account details.  
    /// </summary>
    public class MainController : ApiController
    {
        // GET: api/Main
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Main/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Main
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Main/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Main/5
        public void Delete(int id)
        {
        }
    }
}
