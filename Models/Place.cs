using System.ComponentModel.DataAnnotations;

namespace TouristPlanner.Models
{
    public class Place
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public string Category { get; set; } = "";
        public string Distance { get; set; } = "";
        public string Description { get; set; } = "";
        public string OpenHours { get; set; } = "";
        public string TravelTime { get; set; } = "";
        public string TravelTips { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public string MapUrl { get; set; } = "";
    }
}