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

		public class AllPlanetsJsonObject
		{
			public int count { get; set; }
			public string next { get; set; }
			public string previous { get; set; }

			[JsonProperty("results")]
			public List<PlanetDetailsViewModel> details = new List<PlanetDetailsViewModel>();
		}

		public async Task<AllPlanetsViewModel> GetAllPlanets()
		{
			string url = _APIBaseURL + "planets/";
			AllPlanetsViewModel allPlanets = new AllPlanetsViewModel();

			using (var httpClient = new HttpClient())
			{			
                var response = await httpClient.GetAsync(url);
                string apiResponse = await response.Content.ReadAsStringAsync();
				AllPlanetsJsonObject responseObject = JsonConvert.DeserializeObject<AllPlanetsJsonObject>(apiResponse);
				allPlanets.Planets.AddRange(responseObject.details);

				while (responseObject.next != null) {
					response = await httpClient.GetAsync(responseObject.next);
					apiResponse = await response.Content.ReadAsStringAsync();
					responseObject = JsonConvert.DeserializeObject<AllPlanetsJsonObject>(apiResponse);
					allPlanets.Planets.AddRange(responseObject.details);
				}

            }
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




	}
}
