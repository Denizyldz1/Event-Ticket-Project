using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.DataLayer.Concrete;
using TicketProject.DataLayer.EntityFramework.TicketFiles;
using TicketProject.DataLayer.EntityFramework.TicketUserFiles;

namespace TicketProject.BusinessLayer.TicketUserBusiness
{
    public class TicketUserManager : ITicketUserService
    {
        private readonly ITicketUserDal _ticketUserDal;

        public TicketUserManager(ITicketUserDal ticketUserDal)
        {
            _ticketUserDal = ticketUserDal;
        }


        public void TDelete(TicketUser entity)
        {
            _ticketUserDal.Delete(entity);
        }

        public List<TicketUser> TGetAll()
        {
            return _ticketUserDal.GetAll(); 
        }

        public TicketUser TGetById(int id)
        {
            return _ticketUserDal.GetById(id);
        }

        public void TInsert(TicketUser entity)
        {
            _ticketUserDal.Insert(entity);
        }

        public void TUpdate(TicketUser entity)
        {
            _ticketUserDal.Update(entity);
        }
    }
}
