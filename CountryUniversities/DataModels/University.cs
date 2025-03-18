using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CountryUniversities.DataModels
{
    public class University
    {
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("c_name")]
        public string Name { get; set; }

        [Column("c_country")]
        public string Country { get; set; }

        public ICollection<WebPage> WebPages { get; set; }
    }
}