using System.ComponentModel.DataAnnotations.Schema;

namespace CountryUniversities.DataModels
{
    public class WebPage
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("c_url")]
        public string Url { get; set; }

        [Column("f_university_id")]
        public Guid UniversityId { get; set; }

        public University University { get; set; }
    }
}
