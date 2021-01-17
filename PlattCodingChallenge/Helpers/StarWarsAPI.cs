using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using PlattCodingChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PlattCodingChallenge.Helpers
{
    public class StarWarsAPI
    {
		private readonly string _APIBaseURL = "https://swapi.dev/api/";
		
		//Returns AllPlanetsViewModel with populated Planets list
		//param: <bool> refreshCache - can force a call to the API, ignoring the cache
		public async Task<AllPlanetsViewModel> GetAllPlanets(bool refreshCache = false)
		{
			AllPlanetsViewModel allPlanets = new AllPlanetsViewModel();
            // If cache is not being refreshed, return AllPlanetsViewModel with the cached planets
            List<PlanetDetailsViewModel> planets = StarWarsAPICache.GetData<List<PlanetDetailsViewModel>>("Planets");
			if (planets.Count() == 0 || refreshCache) {
				await CallGetAllPlanetsAsync();
			}

			allPlanets.Planets = StarWarsAPICache.GetData<List<PlanetDetailsViewModel>>("Planets");
			return allPlanets;
		}

		// returns a SinglePlanetViewModel - data for a single planet queried by planetid
		// param: <int> planetid
        public async Task<SinglePlanetViewModel> GetPlanetById(int planetid) {
			string url = _APIBaseURL + "planets/" + planetid.ToString() + "/";
			
			using (var httpClient = new HttpClient())
			{
				SinglePlanetViewModel responseObject = new SinglePlanetViewModel();
				var response = await httpClient.GetAsync(url);
				string apiResponse = await response.Content.ReadAsStringAsync();
				responseObject = JsonConvert.DeserializeObject<SinglePlanetViewModel>(apiResponse);
				return responseObject;
			}
		}

		// Returns PlanetResidentsViewModel with a List of PersonDetails
		// param: <string> planetname
		public async Task<PlanetResidentsViewModel> GetResidentsOfPlanetAsync(string planetname)
        {
			//If the planetsCache is empty, populate the planets cache
			List<PlanetDetailsViewModel> planetsCache = StarWarsAPICache.GetData<List<PlanetDetailsViewModel>>("Planets");
			if (planetsCache.Count() == 0)
            {
                await CallGetAllPlanetsAsync();
            }
            PlanetResidentsViewModel planetResidents = new PlanetResidentsViewModel();
			// check the cache for most recent, so we can get the planet by Name and list of resident URL's
			planetsCache = StarWarsAPICache.GetData<List<PlanetDetailsViewModel>>("Planets");
			PlanetDetailsViewModel planetDetail = planetsCache.Where(x => x.Name == planetname).FirstOrDefault();

			if (planetDetail != null) {
				var residentURLS = planetDetail.ResidentURLs;
				List<PersonDetails> residents = new List<PersonDetails>();

				foreach (var url in residentURLS) {
					residents.Add(await GetPersonFromURLAsync(url));
				}
				planetResidents.Residents = residents;
			}
			return planetResidents;
        }

		// Returns VehicleSummaryViewModel with a List of VehicleDetailModel 
		//param: <bool> refreshCache - can force a call to the API, ignoring the cache
		public async Task<VehicleSummaryViewModel> GetVehicleSummaryAsync(bool refreshCache = false)
		{
			// If cache is not being refreshed, return AllPlanetsViewModel with the cached planets
			if (StarWarsAPICache.GetData<List<VehicleDetailViewModel>>("Vehicles").Count == 0 || refreshCache)
			{
				await CallGetAllVehiclesAsync();
			}
						
			VehicleSummaryViewModel allVehicles = ParseVehicleStats();

			return allVehicles;
		}

		// Returns ManufacturerSummaryViewModel with a List of StarshipDetailsViewModel 
		//param: <bool> refreshCache - can force a call to the API, ignoring the cache
		public async Task<List<ManufacturerSummaryViewModel>> GetManufacturersOfStarshipsAndVehiclesAsync(bool refreshCache = false)
		{
			// If cache is not being refreshed, return AllPlanetsViewModel with the cached planets
			if (StarWarsAPICache.GetData<List<VehicleDetailViewModel>>("Vehicles").Count == 0 || refreshCache)
			{
				await CallGetAllVehiclesAsync();
			}

			// If cache is not being refreshed, return AllPlanetsViewModel with the cached planets
			if (StarWarsAPICache.GetData<List<StarshipDetailsViewModel>>("Starships").Count == 0 || refreshCache)
			{
				await CallGetAllStarshipsAsync();
			}

			List<VehicleDetailViewModel> allVehicles = StarWarsAPICache.GetData<List<VehicleDetailViewModel>>("Vehicles");
			List<StarshipDetailsViewModel> allStarships = StarWarsAPICache.GetData<List<StarshipDetailsViewModel>>("Starships");

			List<ManufacturerSummaryViewModel> manufacturerSummaries = ParseVehiclesAndStarships(allVehicles, allStarships);
			return manufacturerSummaries;

		}

		#region Helpers
		// Returns a signel PersonDetails object
		// param: <string> url
		private async Task<PersonDetails> GetPersonFromURLAsync(string url)
		{
			using (var httpClient = new HttpClient())
			{
				PersonDetails responseObject = new PersonDetails();
				var response = await httpClient.GetAsync(url);
				string apiResponse = await response.Content.ReadAsStringAsync();
				responseObject = JsonConvert.DeserializeObject<PersonDetails>(apiResponse);
				return responseObject;
			}
		}

		//Helper to make the call to the API to Get all planets
		private async Task CallGetAllPlanetsAsync()
		{
			//Reset Cache
			StarWarsAPICache.ResetCache("Planets");

			string url = _APIBaseURL + "planets/";
			AllPlanetsJsonObject responseObject = new AllPlanetsJsonObject();

			using (var httpClient = new HttpClient())
			{
				while (responseObject.next != null || !responseObject.loaded)
				{
					var response = await httpClient.GetAsync(responseObject.loaded ? responseObject.next : url);
					string apiResponse = await response.Content.ReadAsStringAsync();
					responseObject = JsonConvert.DeserializeObject<AllPlanetsJsonObject>(apiResponse);
					responseObject.loaded = true;
					StarWarsAPICache.AddRangeToData("Planets", responseObject.details);
				}

			}
		}
		//Helper to make the call to the API to Get all vehicles
		private async Task CallGetAllVehiclesAsync()
		{
			//Reset Cache
			StarWarsAPICache.ResetCache("Vehicles");

			string url = _APIBaseURL + "vehicles/";
			AllVehiclesJsonObject responseObject = new AllVehiclesJsonObject();

			using (var httpClient = new HttpClient())
			{

				while (responseObject.next != null || !responseObject.loaded)
				{
					var response = await httpClient.GetAsync(responseObject.loaded ? responseObject.next : url);
					string apiResponse = await response.Content.ReadAsStringAsync();
					responseObject = JsonConvert.DeserializeObject<AllVehiclesJsonObject>(apiResponse);
					responseObject.loaded = true;
					StarWarsAPICache.AddRangeToData("Vehicles", responseObject.details);
				}
			}
		}

		//Helper to make the call to the API to Get all vehicles
		private async Task CallGetAllStarshipsAsync()
		{
			//Reset Cache
			StarWarsAPICache.ResetCache("Starships");

			string url = _APIBaseURL + "starships/";
			AllStarshipsJsonObject responseObject = new AllStarshipsJsonObject();

			using (var httpClient = new HttpClient())
			{

				while (responseObject.next != null || !responseObject.loaded)
				{
					var response = await httpClient.GetAsync(responseObject.loaded ? responseObject.next : url);
					string apiResponse = await response.Content.ReadAsStringAsync();
					responseObject = JsonConvert.DeserializeObject<AllStarshipsJsonObject>(apiResponse);
					responseObject.loaded = true;
					StarWarsAPICache.AddRangeToData("Starships", responseObject.details);
				}
			}
		}


		// Helper to parse the vehicle data
		// Returns VehicleSummaryViewModel
		private VehicleSummaryViewModel ParseVehicleStats()
		{
			VehicleSummaryViewModel allVehicles = new VehicleSummaryViewModel();
			List<VehicleStatsViewModel> stats = new List<VehicleStatsViewModel>();
			List<VehicleDetailViewModel> vehicleDetails = StarWarsAPICache.GetData<List<VehicleDetailViewModel>>("Vehicles");
			List<VehicleDetailViewModel> knownCostVehicles = vehicleDetails.Where(x => x.CostInCredits != "unknown").ToList();

			foreach (var v in knownCostVehicles)
			{
				var statsViewModel = stats.Where(x => x.ManufacturerName == v.Manufacturer).FirstOrDefault();
				if (statsViewModel == null)
				{
					statsViewModel = new VehicleStatsViewModel()
					{
						ManufacturerName = v.Manufacturer,
						VehicleCount = 1
					};
					stats.Add(statsViewModel);
				}
				else
				{
					statsViewModel.VehicleCount += 1;
				}

				if (v.CostInCredits != "unknown")
				{
					statsViewModel.TotalCost += Int32.Parse(v.CostInCredits);
				}

				statsViewModel.AverageCost = (double)statsViewModel.TotalCost / statsViewModel.VehicleCount;


			}
			allVehicles.Details = stats;
			allVehicles.VehicleCount = knownCostVehicles.Count();
			allVehicles.ManufacturerCount = stats.Count();

			return allVehicles;
		}


		private List<ManufacturerSummaryViewModel> ParseVehiclesAndStarships(List<VehicleDetailViewModel> allVehicles, List<StarshipDetailsViewModel> allStarships)
		{
			List<ManufacturerSummaryViewModel> manufacturerSummaries = new List<ManufacturerSummaryViewModel>();
			List<string> manufacturersOfBoth = (from veh in allVehicles
												join ships in allStarships
												on veh.Manufacturer equals ships.Manufacturer
												select veh.Manufacturer).ToList();


			foreach (VehicleDetailViewModel v in allVehicles.Where(x => manufacturersOfBoth.Contains(x.Manufacturer)))
			{
				var existingSummary = manufacturerSummaries.Where(x => x.ManufacturerName == v.Manufacturer).FirstOrDefault();

				if (existingSummary == null)
				{
					existingSummary = new ManufacturerSummaryViewModel();
					existingSummary.ManufacturerName = v.Manufacturer;
					existingSummary.VehicleCount = 1;
					manufacturerSummaries.Add(existingSummary);
				}
				else
				{
					existingSummary.VehicleCount += 1;
				}
				existingSummary.Vehicles.Add(v);
			}

			foreach (StarshipDetailsViewModel s in allStarships.Where(x => manufacturersOfBoth.Contains(x.Manufacturer)))
			{
				var existingSummary = manufacturerSummaries.Where(x => x.ManufacturerName == s.Manufacturer).FirstOrDefault();

				if (existingSummary == null)
				{
					existingSummary = new ManufacturerSummaryViewModel();
					existingSummary.ManufacturerName = s.Manufacturer;
					existingSummary.StarshipCount = 1;
					manufacturerSummaries.Add(existingSummary);
				}
				else
				{
					existingSummary.StarshipCount += 1;
				}
				existingSummary.Starships.Add(s);
			}


			return manufacturerSummaries;
		}

		#endregion
	}
}
