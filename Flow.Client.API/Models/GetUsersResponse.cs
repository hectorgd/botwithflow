﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Flow.Client.API.Models
{
    public class Value
    {
        public List<object> businessPhones { get; set; }
        public string displayName { get; set; }
        public string givenName { get; set; }
        public object jobTitle { get; set; }
        public string mail { get; set; }
        public object mobilePhone { get; set; }
        public object officeLocation { get; set; }
        public string preferredLanguage { get; set; }
        public string surname { get; set; }
        public string userPrincipalName { get; set; }
        public string id { get; set; }
    }

    public class GetUsersResponse
    {
        [JsonProperty("@odata.contex")] public string odatacontext { get; set; }

        public List<Value> value { get; set; }
    }
}