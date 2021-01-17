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
		
		public async Task<AllPlanetsViewModel> GetAllPlanets(bool refreshCache = false)
		{
			AllPlanetsViewModel allPlanets = new AllPlanetsViewModel();

			// If cache is not being refreshed, return AllPlanetsViewModel with the cached planets
			if (StarWarsAPICache.GetPlanets().Count() == 0 || refreshCache) {
				await CallGetAllPlanetsAsync();
			}

			allPlanets.Planets = StarWarsAPICache.GetPlanets();
			allPlanets.CalculateAverageDiameter();
			return allPlanets;
		}

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

        public async Task<PlanetResidentsViewModel> GetResidentsOfPlanetAsync(string planetname)
        {
            if (StarWarsAPICache.GetPlanets().Count() == 0)
            {
                await CallGetAllPlanetsAsync();
            }
            PlanetResidentsViewModel planetResidents = new PlanetResidentsViewModel();

            var planetsCache = StarWarsAPICache.GetPlanets();
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


		public async Task<VehicleSummaryViewModel> GetVehicleSummaryAsync(bool refreshCache = false)
		{
			// If cache is not being refreshed, return AllPlanetsViewModel with the cached planets
			if (StarWarsAPICache.GetVehicles().Count() == 0 || refreshCache)
			{
				await CallGetAllVehiclesAsync();
			}

			List<VehicleDetailModel> vehicleDetails = StarWarsAPICache.GetVehicles();
			List<VehicleDetailModel> knownCostVehicles = vehicleDetails.Where(x => x.CostInCredits != "unknown").ToList();
			VehicleSummaryViewModel allVehicles = ParseVehicleStats(knownCostVehicles);

			return allVehicles;
		}


		#region Helpers
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

		private async Task CallGetAllPlanetsAsync()
		{
			//Reset Cache
			StarWarsAPICache.ResetPlanets();

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
					StarWarsAPICache.AddRangeToPlanets(responseObject.details);
				}

			}
		}
		private async Task CallGetAllVehiclesAsync()
		{
			//Reset Cache
			StarWarsAPICache.ResetVehicles();

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
					StarWarsAPICache.AddRangeToVehicles(responseObject.details);
				}
			}
		}
		private VehicleSummaryViewModel ParseVehicleStats(List<VehicleDetailModel> knownCostVehicles)
		{
			VehicleSummaryViewModel allVehicles = new VehicleSummaryViewModel();
			List<VehicleStatsViewModel> stats = new List<VehicleStatsViewModel>();

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

		#endregion
	}
}
