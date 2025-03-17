using System.ComponentModel.DataAnnotations.Schema;

namespace CountryUniversities.DataModels
{
    public class University
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("c_name")]
        public string Name { get; set; }

        [Column("c_country")]
        public string Country { get; set; }

        [Column("c_state_province")]
        public string? StateProvince { get; set; }

        [Column("c_alpha_two_code")]
        public string? AlphaTwoCode { get; set; }

        public ICollection<WebPage> WebPages { get; set; }
        public ICollection<Domain> Domains { get; set; }
    }
}
