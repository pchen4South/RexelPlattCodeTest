using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlattCodingChallenge.Models
{
	public class VehicleSummaryViewModel
	{
		public VehicleSummaryViewModel()
		{
			Details = new List<VehicleStatsViewModel>();
		}
		[JsonProperty("vehicle_count")]
		public int VehicleCount { get; set; }
		[JsonProperty("manufacturer_count")]
		public int ManufacturerCount { get; set; }

		public List<VehicleStatsViewModel> Details { get; set; }
    }
}
