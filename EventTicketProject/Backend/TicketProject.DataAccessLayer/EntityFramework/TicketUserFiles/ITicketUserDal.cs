using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.DataLayer.Concrete;
using TicketProject.DataLayer.Repositories;

namespace TicketProject.DataLayer.EntityFramework.TicketUserFiles
{
    public interface ITicketUserDal : IGenericDal<TicketUser>
    {
    }
}
