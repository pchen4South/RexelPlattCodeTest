using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlattCodingChallenge.Models
{
	public class SinglePlanetViewModel
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("rotation_period")]
		public string LengthOfDay { get; set; }

		[JsonProperty("orbital_period")]
		public string LengthOfYear { get; set; }

		[JsonProperty("diameter")]
		public string Diameter { get; set; }

		[JsonProperty("climate")]
		public string Climate { get; set; }

		[JsonProperty("gravity")]
		public string Gravity { get; set; }

		[JsonProperty("surface_water")]
		public string SurfaceWaterPercentage { get; set; }

		[JsonProperty("population")]
		public string Population { get; set; } = "0";
	}
}
