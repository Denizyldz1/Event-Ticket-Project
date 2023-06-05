using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.DataLayer.Concrete;
using TicketProject.DataLayer.Context;
using TicketProject.DataLayer.EntityFramework.TicketFiles;
using TicketProject.DataLayer.Repositories;

namespace TicketProject.DataLayer.EntityFramework.TicketUserFiles
{
    public class EfTicketUserDal : GenericRepository<TicketUser>,ITicketUserDal
    {

        public EfTicketUserDal(TPContext context) : base(context)
        {
        }

    }
}
