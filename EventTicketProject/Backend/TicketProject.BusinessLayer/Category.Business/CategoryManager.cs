using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.DataLayer.EntityFramework.Category;

namespace TicketProject.BusinessLayer.Category.Business
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public void TDelete(DataLayer.Concrete.Category entity)
        {
            _categoryDal.Delete(entity);
        }

        public List<DataLayer.Concrete.Category> TGetAll()
        {
            return _categoryDal.GetAll();
        }

        public DataLayer.Concrete.Category TGetById(int id)
        {
            return _categoryDal.GetById(id);
        }

        public void TInsert(DataLayer.Concrete.Category entity)
        {
            _categoryDal.Insert(entity);
        }

        public void TUpdate(DataLayer.Concrete.Category entity)
        {
            _categoryDal.Update(entity);
        }
    }
}
