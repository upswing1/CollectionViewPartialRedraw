using System.ComponentModel.DataAnnotations;

namespace CollectionViewPartialRedraw
{
    public class Vehicle
    {
        [Key]
        public string VehicleId { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int Year { get; set; }

        // Reference to the Fleet that the vehicle belongs to
        public int FleetId { get; set; }
        public Fleet Fleet { get; set; }

        public Vehicle(string vehicleId, string make, string model, int year)
        {
            VehicleId = vehicleId;
            Make = make;
            Model = model;
            Year = year;
        }

        public Vehicle() { }

        
    }
}
