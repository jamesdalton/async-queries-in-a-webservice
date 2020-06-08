using System.Collections.Generic;

namespace Api.Models
{
    public class Anime
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<string> Genres { get; set; }
        public int? Episodes { get; set; }
        public double? Rating { get; set; }
        public int? Members { get; set; }
    }
}