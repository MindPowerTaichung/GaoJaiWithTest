﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;

namespace MPERP2015.MP
{
    public class MPAuthHttpModule : IHttpModule
    {
        private const string Realm = "MPERP2015";

        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }
        
        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private static bool AuthenticateUser(string credentials)
        {
            //var encoding = Encoding.GetEncoding("iso-8859-1");
            //credentials = encoding.GetString(Convert.FromBase64String(credentials));

            //var credentialsArray = credentials.Split(':');
            //var username = credentialsArray[0];
            //var password = credentialsArray[1];

            var username = credentials;

            /* REPLACE THIS WITH REAL AUTHENTICATION
            ----------------------------------------------*/
            //if (!(username == "test" && password == "test"))
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            var identity = new GenericIdentity(username);
            SetPrincipal(new GenericPrincipal(identity, null));

            return true;
        }

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];
            if (authHeader != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeaderVal.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authHeaderVal.Parameter != null)
                {
                    AuthenticateUser(authHeaderVal.Parameter);
                }
            }
        }

        // If the request was unauthorized, add the WWW-Authenticate header 
        // to the response.
        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", Realm));
            }
        }

        public void Dispose()
        {
        }
    }
}