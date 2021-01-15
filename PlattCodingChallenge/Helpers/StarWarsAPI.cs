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
			if (PlanetsCache.GetPlanetsFromCache().Count() == 0 || refreshCache) {
				await CallGetAllPlanetsAsync();
			}

			allPlanets.Planets = PlanetsCache.GetPlanetsFromCache();
			allPlanets.CalculateAverageDiameter();
			return allPlanets;
		}

		private async Task CallGetAllPlanetsAsync() {
			//Reset Cache
			PlanetsCache.ResetCache();
			
			string url = _APIBaseURL + "planets/";
			using (var httpClient = new HttpClient())
			{
				var response = await httpClient.GetAsync(url);
				string apiResponse = await response.Content.ReadAsStringAsync();
				AllPlanetsJsonObject responseObject = JsonConvert.DeserializeObject<AllPlanetsJsonObject>(apiResponse);
				PlanetsCache.AddRangeToCache(responseObject.details);

				while (responseObject.next != null)
				{
					response = await httpClient.GetAsync(responseObject.next);
					apiResponse = await response.Content.ReadAsStringAsync();
					responseObject = JsonConvert.DeserializeObject<AllPlanetsJsonObject>(apiResponse);
					PlanetsCache.AddRangeToCache(responseObject.details);
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
            if (PlanetsCache.GetPlanetsFromCache().Count() == 0)
            {
                await CallGetAllPlanetsAsync();
            }
            PlanetResidentsViewModel planetResidents = new PlanetResidentsViewModel();

            var planetsCache = PlanetsCache.GetPlanetsFromCache();
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




    }
}
