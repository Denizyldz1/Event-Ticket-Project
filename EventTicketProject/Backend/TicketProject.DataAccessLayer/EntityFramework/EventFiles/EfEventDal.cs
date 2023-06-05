using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.DataLayer.Concrete;
using TicketProject.DataLayer.Context;
using TicketProject.DataLayer.Repositories;

namespace TicketProject.DataLayer.EntityFramework.EventFiles
{
    public class EfEventDal : GenericRepository<Event>, IEventDal
    {
        public EfEventDal(TPContext context) : base(context)
        {
        }
    }
}
