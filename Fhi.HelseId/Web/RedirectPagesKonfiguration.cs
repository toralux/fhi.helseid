﻿using System.Linq;

namespace Fhi.HelseId.Web
{
    public class RedirectPagesKonfigurasjon
    {
        public string Forbidden { get; set; } = "/Forbidden.html";
        public string LoggedOut{ get; set; } =  "/loggedout.html";
        public string Error { get; set; } = "/Error.html";
        public string Statuscode { get; set; } = "/Statuscode.html";

        public bool KonfigurasjonErGyldig()
        {
            return
                new string[]
                {
                    Forbidden,
                    LoggedOut,
                    Error,
                    Statuscode
                }.All(url => url.StartsWith("/"));
        }
    }
}
