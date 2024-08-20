namespace CampingAPI2.Models
{
    public class CampingSite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string OwnerEmail { get; set; }
        public int Capacity { get; set; }
        public int Price { get; set; }
        public bool IsAvailable { get; set; }
        public string ClientEmail { get; set; }
    }
}
