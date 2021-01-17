using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlattCodingChallenge.Models
{
	public class AllPlanetsViewModel
	{
		public AllPlanetsViewModel()
		{
			Planets = new List<PlanetDetailsViewModel>();
		}

		public List<PlanetDetailsViewModel> Planets { get; set; }

		public double AverageDiameter { 
			get {
				try
				{
					int count = 0;
					int diameterCount = 0;
					foreach (PlanetDetailsViewModel p in Planets)
					{
						int diameter = 0;
						bool parsed = int.TryParse(p.Diameter, out diameter);

						if (parsed)
						{
							count++;
							diameterCount += diameter;
						}
					}
					return (double)diameterCount / count;
				}
				catch {
					return 0.0;
				}
			}
		}
    }
}
