using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LearningEF
{
    public class Address
    {
        public int Id { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }

        [JsonIgnore]
        public Person Person { get; set; }
    }
}
