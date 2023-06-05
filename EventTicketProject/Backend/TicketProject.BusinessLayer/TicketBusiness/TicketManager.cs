using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.DataLayer.Concrete;
using TicketProject.DataLayer.EntityFramework.EventFiles;
using TicketProject.DataLayer.EntityFramework.TicketFiles;
using TicketProject.DataLayer.EntityFramework.TicketUserFiles;
using TicketProject.DtoLayer.Dtos.EventDto;
using TicketProject.DtoLayer.Dtos.Ticket;

namespace TicketProject.BusinessLayer.TicketBusiness
{
    public class TicketManager : ITicketService
    {
        private readonly ITicketDal _ticketDal;
        private readonly ITicketUserDal _ticketUserDal;
        private readonly IEventDal _eventDal;

        public TicketManager(ITicketDal ticketDal, ITicketUserDal ticketUserDal, IEventDal eventDal)
        {
            _ticketDal = ticketDal;
            _ticketUserDal = ticketUserDal;
            _eventDal = eventDal;
        }
        public List<TicketModel> TGetTicketAll()
        {
            var tickets = _ticketDal.GetAll();
            var users = _ticketUserDal.GetAll();
            var events = _eventDal.GetAll();

            var res = (
                from t in tickets
                join u in users on t.UserID equals Convert.ToInt32(u.Id) into userGroup
                from u in userGroup.DefaultIfEmpty()
                join e in events on t.EventID equals e.EventId into eventGroup
                from e in eventGroup.DefaultIfEmpty()
                select new TicketModel
                {
                    TicketId = t.TicketId,
                    Title = e.Title,
                    TicketNumber = t.TicketNumber,
                    Date = e.Date,
                    NameSurname = u.Name + " " + u.Surname,
                    Status = t.Status,
                }
                ).ToList();
            return res;
        }
        public TicketModel TGetByTicketId(int id)
        {
            var newTicket = _ticketDal.GetById(id);
            TicketModel @ticket = new TicketModel();
            var date = _eventDal.GetById(newTicket.EventID).Date;
            @ticket.Date = _eventDal.GetById(newTicket.EventID).Date;
            @ticket.TicketId = newTicket.TicketId;
            if (ticket.Date < DateTime.Now)
            {
                TicketCancel(ticket.TicketId);
            }
         
            @ticket.Title = _eventDal.GetById(newTicket.EventID).Title;
            
            var name = _ticketUserDal.GetById(newTicket.UserID).Name;
            var surname = _ticketUserDal.GetById(newTicket.UserID).Surname;
            @ticket.NameSurname = name + " " + surname;
            ticket.TicketNumber = newTicket.TicketNumber;
            @ticket.Status = newTicket.Status;

            
            return @ticket;
        }

        public Ticket TGetById(int id)
        {
            return _ticketDal.GetById(id);
        }


        #region Kullanılmayan metotlar
        // Bilet güncelleme işlemi yapılmayacak.
        public void TUpdate(Ticket entity)
        {
            throw new NotImplementedException();
        }
        // Bilet silme işlemi şuanlık yapılmayacak.
        public void TDelete(Ticket entity)
        {
            throw new NotImplementedException();
        }
        // GetAll yerine Linq sorgusu çalıştıran GetTicketAll kullanılacak
        public List<Ticket> TGetAll()
        {
            return _ticketDal.GetAll();
        }
        // AddTicketModel kullanılacak
        public void TInsert(Ticket entity)
        {
            _ticketDal.Insert(entity);
        }


        #endregion


        public TicketModel TGetByTicketNumber(string ticketNumber)
        {
            var newList = TGetTicketAll();
            var ticketList = new TicketModel();
            foreach (var item in newList)
            {
                if (item.TicketNumber == ticketNumber)
                {
                    ticketList.TicketId =item.TicketId;
                    ticketList.Title = item.Title;
                    ticketList.NameSurname = item.NameSurname;
                    ticketList.Date = item.Date;
                    ticketList.Status = item.Status;
                    ticketList.TicketNumber = item.TicketNumber;

                }

            }
            return ticketList;

        }

        public List<TicketModel> TGetByUserId(int userId)
        {
            var ticketAllList = TGetAll();
            var ticketNewList = new List<TicketModel>();
            foreach (var item in ticketAllList)
            {
                if(item.UserID == userId && item.Status==true)
                {
                    var newTicketList = TGetByTicketId(item.TicketId);                   
                    ticketNewList.Add(newTicketList);
                }
            }
            return ticketNewList;
        }

        public void TicketCancel(int id)
        {
            var ticketList = _ticketDal.GetById(id);
            if (ticketList != null)
            {
                ticketList.Status = false;
                _ticketDal.Update(ticketList);
            }

        }
        public string TTicketInsert(AddTicketModel addTicketModel)
        {
            var events = _eventDal.GetById(addTicketModel.EventID);
            var newCapacity = events.Capacity;
            var ticketAllList = TGetAll();

            Ticket newTicket= new Ticket();
            newTicket.EventID = addTicketModel.EventID;
            newTicket.UserID = addTicketModel.UserID;
            newTicket.Status = true;


            if(ticketAllList.Count > 0)
            {
                foreach (var item in ticketAllList)
                {
                    if (item.EventID == addTicketModel.EventID && item.UserID == addTicketModel.UserID)
                    {
                        return "Duplicate";
                    }

                    if (item.EventID != addTicketModel.EventID) // daha önce bu etkinliğe bilet alınmamış.
                    {
                        var beginNumber = "0001";
                        newTicket.TicketNumber = $"{newTicket.EventID}{newTicket.UserID}{beginNumber}";
                    }
                    else
                    {
                        var maxTicketNumber = ticketAllList.Where(t => t.EventID == addTicketModel.EventID)
                                                           .Max(t => int.Parse(t.TicketNumber.Substring(t.TicketNumber.Length - 4)));
                        if (maxTicketNumber < newCapacity)
                        {
                            var incrementedTicketNumber = (maxTicketNumber + 1).ToString();
                            newTicket.TicketNumber = $"{newTicket.EventID}{newTicket.UserID}{incrementedTicketNumber}";

                        }
                        else
                        {
                            return "CapacityFull";
                        }                      
                    }
                }
            }
            else
            {
                var beginNumber = "0001";
                newTicket.TicketNumber = $"{newTicket.EventID}{newTicket.UserID}{beginNumber}";
            }
            _ticketDal.Insert(newTicket);
            return "Success";
        }

    }
}
