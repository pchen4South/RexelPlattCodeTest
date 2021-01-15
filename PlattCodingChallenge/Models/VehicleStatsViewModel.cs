using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlattCodingChallenge.Models
{
	public class VehicleStatsViewModel
	{
		[JsonProperty("manufacturer_name")]
		public string ManufacturerName { get; set; }
		[JsonProperty("vehicle_count")]
		public int VehicleCount { get; set; }
		[JsonProperty("average_cost")]
		public double AverageCost { get; set; }

		public double TotalCost = 0.0;
	}
}
