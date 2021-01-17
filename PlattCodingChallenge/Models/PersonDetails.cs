using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlattCodingChallenge.Models
{
	public class PersonDetails
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("height")]
		public string Height { get; set; }
		[JsonProperty("weight")]
		public string Weight { get; set; }
		[JsonProperty("gender")]
		public string Gender { get; set; }
		[JsonProperty("hair_color")]
		public string HairColor { get; set; }

		[JsonProperty("eye_color")]
		public string EyeColor { get; set; }
		[JsonProperty("skin_color")]
		public string SkinColor { get; set; }
    }
}
