using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlattCodingChallenge.Models
{
	public class AllPlanetsJsonObject
	{
		public int count { get; set; }
		public string next { get; set; }
		public string previous { get; set; }

		[JsonProperty("results")]
		public List<PlanetDetailsViewModel> details = new List<PlanetDetailsViewModel>();
	}
}
