using TicketProject.DataLayer.Concrete;
using TicketProject.DataLayer.Context;
using TicketProject.DataLayer.Repositories;

namespace TicketProject.DataLayer.EntityFramework.Category
{
    public class EfCategoryDal : GenericRepository<Concrete.Category>, ICategoryDal
    {
        public EfCategoryDal(TPContext context) : base(context)
        {
        }
    }
}
