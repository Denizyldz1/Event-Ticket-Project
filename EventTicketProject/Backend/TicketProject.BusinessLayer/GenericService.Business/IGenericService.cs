using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketProject.BusinessLayer.GenericService.Business
{
    public interface IGenericService<T> where T : class
    {
        // İsimlendirme yaparken T ekleme sebebim DataLayer Repository'deki metotlar ile karışmasın.
        void TInsert(T entity);
        void TUpdate(T entity);
        void TDelete(T entity);
        List<T> TGetAll();
        T TGetById(int id);
    }
}
