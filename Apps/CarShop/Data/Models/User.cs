using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarShop.Data.Models
{
    public class User : IdentityUser<string>
    {
        public User() : base()
        {
            this.Cars = new HashSet<Car>();
        }

        public bool IsMechanic { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}
