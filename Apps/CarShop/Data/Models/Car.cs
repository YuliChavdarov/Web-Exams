using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarShop.Data.Models
{
    public class Car
    {
        public Car()
        {
            this.Issues = new HashSet<Issue>();
        }

        public string Id { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string PlateNumber { get; set; }
        [Required]
        public string OwnerId { get; set; }
        public User Owner { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
