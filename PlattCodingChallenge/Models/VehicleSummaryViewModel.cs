using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattCodingChallenge.Models
{
	public class VehicleSummaryViewModel
	{
		public VehicleSummaryViewModel()
		{
			Details = new List<VehicleStatsViewModel>();
		}

		public int VehicleCount { get; set; }

		public int ManufacturerCount { get; set; }

		public List<VehicleStatsViewModel> Details { get; set; }
    }
}
