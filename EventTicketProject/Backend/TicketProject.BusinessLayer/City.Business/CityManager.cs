using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.DataLayer.EntityFramework.CityFiles;

namespace TicketProject.BusinessLayer.City.Business
{
    public class CityManager : ICityService
    {
        private readonly ICityDal _cityDal;

        public CityManager(ICityDal cityDal)
        {
            _cityDal = cityDal;
        }

        public void TDelete(DataLayer.Concrete.City entity)
        {
            _cityDal.Delete(entity);
        }


        public List<DataLayer.Concrete.City> TGetAll()
        {
            return _cityDal.GetAll();
        }

        public DataLayer.Concrete.City TGetById(int id)
        {
            return _cityDal.GetById(id);
        }

        public void TInsert(DataLayer.Concrete.City entity)
        {
            _cityDal.Insert(entity);
        }

        public void TUpdate(DataLayer.Concrete.City entity)
        {
            _cityDal.Update(entity);
        }
    }
}
