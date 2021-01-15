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

		private async Task CallGetAllPlanetsAsync() {
			//Reset Cache
			StarWarsAPICache.ResetPlanets();
			
			string url = _APIBaseURL + "planets/";
			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetAsync(url);
				string apiResponse = await response.Content.ReadAsStringAsync();
				AllPlanetsJsonObject responseObject = JsonConvert.DeserializeObject<AllPlanetsJsonObject>(apiResponse);
				StarWarsAPICache.AddRangeToPlanets(responseObject.details);

				while (responseObject.next != null)
				{
					response = await httpClient.GetAsync(responseObject.next);
					apiResponse = await response.Content.ReadAsStringAsync();
					responseObject = JsonConvert.DeserializeObject<AllPlanetsJsonObject>(apiResponse);
					StarWarsAPICache.AddRangeToPlanets(responseObject.details);
				}

			}
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
				List<ResidentSummary> residents = new List<ResidentSummary>();

				foreach (var url in residentURLS) {
					using (var httpClient = new HttpClient())
					{
						ResidentSummary responseObject = new ResidentSummary();
						var response = await httpClient.GetAsync(url);
						string apiResponse = await response.Content.ReadAsStringAsync();
						responseObject = JsonConvert.DeserializeObject<ResidentSummary>(apiResponse);
						residents.Add(responseObject);
					}
				}

				planetResidents.Residents = residents;

			}

			return planetResidents;
        }

		public async Task<VehicleSummaryViewModel> GetVehicleSummaryAsync(bool refreshCache = false)
		{
			VehicleSummaryViewModel allVehicles = new VehicleSummaryViewModel();

			// If cache is not being refreshed, return AllPlanetsViewModel with the cached planets
			if (StarWarsAPICache.GetVehicles().Count() == 0 || refreshCache)
			{
				await CallGetAllVehiclesAsync();
			}

			var vehicleDetails = StarWarsAPICache.GetVehicles();
			var knownCostVehicles = vehicleDetails.Where(x => x.CostInCredits != "unknown").ToList();

			List<VehicleStatsViewModel> stats = new List<VehicleStatsViewModel>();

			foreach (var v in knownCostVehicles) {
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
				else {
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

		private async Task CallGetAllVehiclesAsync()
		{
			//Reset Cache
			StarWarsAPICache.ResetVehicles();

			string url = _APIBaseURL + "vehicles/";
			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetAsync(url);
				string apiResponse = await response.Content.ReadAsStringAsync();
				AllVehiclesJsonObject responseObject = JsonConvert.DeserializeObject<AllVehiclesJsonObject>(apiResponse);
				StarWarsAPICache.AddRangeToVehicles(responseObject.details);

				while (responseObject.next != null)
				{
					response = await httpClient.GetAsync(responseObject.next);
					apiResponse = await response.Content.ReadAsStringAsync();
					responseObject = JsonConvert.DeserializeObject<AllVehiclesJsonObject>(apiResponse);
					StarWarsAPICache.AddRangeToVehicles(responseObject.details);
				}
			}
		}

	}
}
