using System.ComponentModel.DataAnnotations;

namespace MOProject.Models
{
    public class Setting1
    {
        [Key] // Specify this as the primary key
        public int Id1 { get; set; } // Or another property as the key
        public string? SiteName1 { get; set; }
        public string? Title1 { get; set; }
        public string? ShortDescription1 { get; set; }
        public string? ThumbnailUrl1 { get; set; }
        public string? FacebookUrl1 { get; set; }
        public string? InstagramUrl1 { get; set; }

      
    }
}
