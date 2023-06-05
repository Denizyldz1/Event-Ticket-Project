using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.BusinessLayer.GenericService.Business;
using TicketProject.DataLayer.Concrete;

namespace TicketProject.BusinessLayer.Category.Business
{
    public interface ICategoryService:IGenericService<DataLayer.Concrete.Category>
    {
    }
}
