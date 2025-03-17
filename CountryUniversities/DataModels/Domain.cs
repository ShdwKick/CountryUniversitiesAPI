using System.ComponentModel.DataAnnotations.Schema;

namespace CountryUniversities.DataModels
{
    public class Domain
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("c_domain_name")]
        public string DomainName { get; set; }

        [Column("f_university_id")]
        public Guid UniversityId { get; set; }

        public University University { get; set; }
    }
}
