using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlattCodingChallenge.Models
{
    public class VehicleDetailModel
    {
		[JsonProperty("manufacturer")]
		public string Manufacturer { get; set; }

		[JsonProperty("cargo_capacity")]
		public string CargoCapacity { get; set; }

		[JsonProperty("cost_in_credits")]
		public string CostInCredits { get; set; }

		[JsonProperty("crew")]
		public int Crew { get; set; }
		
		[JsonProperty("url")]
		public string URL{ get; set; }

		public int VehicleID
		{
			get
			{
				try
				{
					string[] urlSplit = URL.Split(new string[] { "http://swapi.dev/api/vehicles/" }, StringSplitOptions.None);
					string ID = urlSplit[1].TrimEnd('/');
					return Int32.Parse(ID);
				}
				catch
				{
					return -1;
				}
			}

		}
	}
}
