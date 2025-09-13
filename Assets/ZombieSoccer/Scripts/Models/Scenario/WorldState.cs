using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieSoccer.Models
{
    public class WorldState
    {
        public List<City>? Cities { get; set; }

        public string? CurrentUserMatch { get; set; }
    }
}
