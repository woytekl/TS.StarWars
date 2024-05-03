using Newtonsoft.Json;
using TS.StarWars.Infrastructure;

namespace TS.StarWars.DataSource
{
    internal class StarWarsCharacter : IStarWarsCharacter
    {
        public StarWarsCharacter()
        {
            BirthYear = string.Empty;
            EyeColor = string.Empty;
            Gender = string.Empty;
            HairColor = string.Empty;
            Height = string.Empty;
            Homeworld = string.Empty;
            Mass = string.Empty;
            Name = string.Empty;
            SkinColor = string.Empty;
            Starships = [];
            Vehicles = [];
            Films = [];
        }

        [JsonProperty("birth_year", Required = Newtonsoft.Json.Required.Always)]
        public string BirthYear { get; internal set; }

        [JsonProperty("eye_color", Required = Newtonsoft.Json.Required.Always)]
        public string EyeColor { get; internal set; }

        public string Gender { get; internal set; }

        [JsonProperty("hair_color", Required = Newtonsoft.Json.Required.Always)]
        public string HairColor { get; internal set; }

        public string Height { get; internal set; }

        public string Homeworld { get; internal set; }

        public string Mass { get; internal set; }

        public string Name { get; internal set; }

        [JsonProperty("skin_color", Required = Newtonsoft.Json.Required.Always)]
        public string SkinColor { get; internal set; }

        public string[] Starships { get; internal set; }

        public string[] Vehicles { get; internal set; }

        public string[] Films { get; internal set; }
    }
}
