using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DotNetCore.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the DotNetCoreUser class
    public class DotNetCoreUser : IdentityUser
    {
        public bool Enabled { get; set; }
        public DateTime FromDate{ get; set; }
        public DateTime ToDate{ get; set; }
    }
}
