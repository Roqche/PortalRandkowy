using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalRandkowy.Dtos
{
    public class UserForListDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        // Basic information
        public string Gender { get; set; }
        public int Age { get; set; }
        public string ZodiacSign { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
    }
}
