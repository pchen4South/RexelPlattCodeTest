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
        private static List<VehicleDetailModel> Vehicles = new List<VehicleDetailModel>();

        public static List<PlanetDetailsViewModel> GetPlanets() {
            return Planets;
        }

        public static void SetPlanets(List<PlanetDetailsViewModel> planets) {
            Planets = planets;
        }

        public static void AddRangeToPlanets(List<PlanetDetailsViewModel> planets){
            Planets.AddRange(planets);
        }

        public static void ResetPlanets() { 
            Planets = new List<PlanetDetailsViewModel>();
        }
        public static List<VehicleDetailModel> GetVehicles()
        {
            return Vehicles;
        }

        public static void SetVehicles(List<VehicleDetailModel> vehicles)
        {
            Vehicles = vehicles;
        }

        public static void AddRangeToVehicles(List<VehicleDetailModel> vehicles)
        {
            Vehicles.AddRange(vehicles);
        }

        public static void ResetVehicles()
        {
            Vehicles = new List<VehicleDetailModel>();
        }




    }
}
