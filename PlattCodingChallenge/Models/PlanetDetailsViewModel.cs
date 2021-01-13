using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlattCodingChallenge.Models
{
	public class PlanetDetailsViewModel
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("population")]
		public string Population { get; set; }

		[JsonProperty("diameter")]
		public string Diameter { get; set; }

		[JsonProperty("terrain")]
		public string Terrain { get; set; }

		[JsonProperty("orbital_period")]
		public string LengthOfYear { get; set; }

		public string FormattedPopulation => Population == "unknown" ? "unknown" : long.Parse(Population).ToString("N0");
    }
}
