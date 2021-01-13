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



			public ActionResult GetPlanetById(int planetid)
		{
			var model = new SinglePlanetViewModel();

			// TODO: Implement this controller action

			return View(model);
		}

		public ActionResult GetResidentsOfPlanet(string planetname)
		{
			var model = new PlanetResidentsViewModel();

			// TODO: Implement this controller action

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
