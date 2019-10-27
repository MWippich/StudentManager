using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StudentManager.Models
{
    public class Allergy
    {
        public int StudentID { get; set; }
        public int AllergyID { get; set; }
        [Required(ErrorMessage = "Allergy description field is required")]
        [MaxLength(50)]
        [RegularExpression(@"\D*", ErrorMessage = "Allergy description may only contain alphabetical characters")]
        public string Description { get; set; }

        public Allergy() { }
        public Allergy(string description)
        {
            this.Description = description;
        }
    }
}
