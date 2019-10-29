using System.Collections.Generic;
using Newtonsoft.Json;

namespace Flow.Client.API.Models
{
    [JsonObject]
    public class CreateGroupParams
    {
        [JsonProperty("teamDisplayName")] public string teamDisplayName { get; set; }

        [JsonProperty("teamMailNickname")] public string teamMailNickname { get; set; }

        [JsonProperty("teamDescription")] public string teamDescription { get; set; }

        [JsonProperty("teamOwnerodata.bind")] public string teamOwnersoDatabind { get; set; }

        [JsonProperty("membersodata.bind")] public List<string> membersodatabind { get; set; }

        [JsonProperty("channelsDisplayName")] public string channelsDisplayName { get; set; }

        [JsonProperty("channelsDescription")] public string channelsDescription { get; set; }

        [JsonProperty("tabsDisplayName")] public string tabsDisplayName { get; set; }

        [JsonProperty("tabsWebsiteUrl")] public string tabsWebsiteUrl { get; set; }
    }
}