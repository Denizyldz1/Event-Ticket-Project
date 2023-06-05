
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.DataLayer.Concrete;
using TicketProject.DataLayer.Context;
using TicketProject.DataLayer.EntityFramework.CityFiles;
using TicketProject.DataLayer.Repositories;

namespace TicketProject.DataLayer.EntityFramework.City
{
    public class EfCityDal : GenericRepository<Concrete.City>, ICityDal
    {
        public EfCityDal(TPContext context) : base(context)
        {
        }
    }
}
