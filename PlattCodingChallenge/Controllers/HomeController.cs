using Microsoft.AspNetCore.Mvc;
using PlattCodingChallenge.Models;
using PlattCodingChallenge.Helpers;
using System.Threading.Tasks;
using System.Linq;

namespace PlattCodingChallenge.Controllers
{

	public class HomeController : Controller
	{
		private StarWarsAPI _api = new StarWarsAPI();

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
			//var model = new PlanetResidentsViewModel();
			var model = await _api.GetResidentsOfPlanetAsync(planetname);
			model.Residents = model.Residents.OrderBy(x => x.Name).ToList();

			return View(model);
		}

		public ActionResult VehicleSummary()
		{
			var model = new VehicleSummaryViewModel();

			// TODO: Implement this controller action

			return View(model);
		}
    }
}
