using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.DataLayer.Concrete;
using TicketProject.DataLayer.Context;
using TicketProject.DataLayer.Repositories;

namespace TicketProject.DataLayer.EntityFramework.TicketFiles
{
    public class EfTicketDal : GenericRepository<Ticket>, ITicketDal
    {
        public EfTicketDal(TPContext context) : base(context)
        {
        }
    }
}
