using Microsoft.AspNetCore.Mvc;
using PlattCodingChallenge.Models;
using PlattCodingChallenge.Helpers;
using System.Threading.Tasks;
using System.Linq;

namespace PlattCodingChallenge.Controllers
{

	public class HomeController : Controller
	{
		private readonly StarWarsAPI _api = new StarWarsAPI();

		public ActionResult Index()
		{
			return View();
		}

		public async Task<ActionResult> GetAllPlanets()
		{
			var model = new AllPlanetsViewModel();

			model = await _api.GetAllPlanets();
			model.Planets = model.Planets.OrderBy(x => x, new Comparers.PlanetDiameterComparer()).ToList();
			model.CalculateAverageDiameter();

			return View(model);
		}

		public async Task<ActionResult> GetPlanetById(int planetid)
		{
			var model = await _api.GetPlanetById(planetid);
			return View(model);
		}

		public async Task<ActionResult> GetResidentsOfPlanet(string planetname)
		{
			var model = await _api.GetResidentsOfPlanetAsync(planetname);
			model.Residents = model.Residents.OrderBy(x => x.Name).ToList();

			return View(model);
		}

		public async Task<ActionResult> VehicleSummary()
		{
			var model = new VehicleSummaryViewModel();
			model = await _api.GetVehicleSummaryAsync();
			model.Details = model.Details
								.OrderByDescending(x => x.VehicleCount)
								.ThenByDescending(x => x.AverageCost)
								.ToList();

			return View(model);
		}
    }
}
