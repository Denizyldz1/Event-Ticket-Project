using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProject.DataLayer.Concrete
{
    public class TicketUserRole : IdentityRole<int>
    {
        public TicketUserRole() : base()
        {
        }

        public TicketUserRole(string roleName) : base(roleName)
        {
        }

    }
}
