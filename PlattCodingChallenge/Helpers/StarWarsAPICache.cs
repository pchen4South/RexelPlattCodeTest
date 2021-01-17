using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlattCodingChallenge.Models;

namespace PlattCodingChallenge.Helpers
{
    public static class StarWarsAPICache
    {
        private static List<PlanetDetailsViewModel> Planets = new List<PlanetDetailsViewModel>();
        private static List<VehicleDetailViewModel> Vehicles = new List<VehicleDetailViewModel>();
        private static List<StarshipDetailsViewModel> Starships = new List<StarshipDetailsViewModel>();

        public static dynamic GetData<T>(string type) {
            switch (type)
            {
                case "Planets":
                    return Planets;
                case "Vehicles":
                    return Vehicles;
                case "Starships":
                    return Starships;
                default:
                    return Planets;
            }
        }

        public static void SetData<T>(string type, IList<T> data) {
            switch (type)
            {
                case "Planets":
                    Planets = (List<PlanetDetailsViewModel>)data;
                    break;
                case "Vehicles":
                    Vehicles = (List<VehicleDetailViewModel>)data; ;
                    break;
                case "Starships":
                    Starships = (List<StarshipDetailsViewModel>)data; ;
                    break;
                default:
                    Planets = (List<PlanetDetailsViewModel>)data;
                    break;
            }
        }

        public static void AddRangeToData<T>(string type, IList<T> data) {
            switch (type)
            {
                case "Planets":
                    Planets.AddRange((List<PlanetDetailsViewModel>)data);
                    break;
                case "Vehicles":
                    Vehicles.AddRange((List<VehicleDetailViewModel>)data);
                    break;
                case "Starships":
                    Starships.AddRange((List<StarshipDetailsViewModel>)data);
                    break;
                default:
                    Planets.AddRange((List<PlanetDetailsViewModel>)data);
                    break;
            }
        }

        public static void ResetCache(string type) {
            switch (type)
            {
                case "Planets":
                    Planets = new List<PlanetDetailsViewModel>();
                    break;
                case "Vehicles":
                    Vehicles = new List<VehicleDetailViewModel>();
                    break;
                case "Starships":
                    Starships = new List<StarshipDetailsViewModel>();
                    break;
                default:
                    Planets = new List<PlanetDetailsViewModel>();
                    break;
            }
        }
    }
}
