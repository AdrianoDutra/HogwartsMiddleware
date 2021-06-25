using Newtonsoft.Json;
using System;

namespace Hogwarts.Middleware.Dtos
{
    public partial class Houses
    {
        [JsonProperty("houses")]
        public House[] HousesHouses { get; set; }
    }

    public partial class House
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("values")]
        public string[] Values { get; set; }

        [JsonProperty("houseGhost")]
        public string HouseGhost { get; set; }

        [JsonProperty("founder")]
        public string Founder { get; set; }

        [JsonProperty("headOfHouse")]
        public string HeadOfHouse { get; set; }

        [JsonProperty("school", NullValueHandling = NullValueHandling.Ignore)]
        public string School { get; set; }

        [JsonProperty("mascot")]
        public string Mascot { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("colors")]
        public string[] Colors { get; set; }
    }
}
