using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace StudentManager.Models
{
    [BindProperties(SupportsGet = true)]
    public class Student
    {

        public int StudentID { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Student name may not exceed 100 characters")]
        [RegularExpression(@"\D*", ErrorMessage = "Student name may only contain alphabetical characters")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Member Since")]
        [DataType(DataType.Date)]
        public DateTime? MemberSince { get; set; }
        [Required]
        [Display(Name = "Currently Studying")]
        public bool CurrentlyStudying { get; set; }
        [MaxLength(255)]
        public string Address { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<Allergy> Allergies { get; set; }

        public Student()
        {
            Allergies = new List<Allergy>();
        }
    }
}
