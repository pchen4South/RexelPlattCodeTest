using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlattCodingChallenge.Models;

namespace PlattCodingChallenge.Helpers
{
    public static class PlanetsCache
    {
        private static List<PlanetDetailsViewModel> Planets = new List<PlanetDetailsViewModel>();

        public static List<PlanetDetailsViewModel> GetPlanetsFromCache() {
            return Planets;
        }

        public static void SetCache(List<PlanetDetailsViewModel> planets) {
            Planets = planets;
        }

        public static void AddRangeToCache(List<PlanetDetailsViewModel> planets){
            Planets.AddRange(planets);
        }

        public static void ResetCache() { 
            Planets = new List<PlanetDetailsViewModel>();
        }

    }
}
