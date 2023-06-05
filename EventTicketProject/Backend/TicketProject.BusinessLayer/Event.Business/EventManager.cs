using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.BusinessLayer.GenericService.Business;
using TicketProject.DataLayer.Concrete;
using TicketProject.DataLayer.EntityFramework.Category;
using TicketProject.DataLayer.EntityFramework.CityFiles;
using TicketProject.DataLayer.EntityFramework.EventFiles;
using TicketProject.DataLayer.EntityFramework.TicketUserFiles;
using TicketProject.DtoLayer.Dtos.EventDto;

namespace TicketProject.BusinessLayer.Event.Business
{
    public class EventManager : IEventService
    {
        private readonly IEventDal _eventDal;
        private readonly ICityDal _cityDal;
        private readonly ICategoryDal _categoryDal;
        private readonly ITicketUserDal _ticketUserDal;

        public EventManager(
            IEventDal eventDal, 
            ICityDal cityDal, 
            ICategoryDal categoryDal, 
            ITicketUserDal ticketUserDal)
        {
            _eventDal = eventDal;
            _cityDal = cityDal;
            _categoryDal = categoryDal;
            _ticketUserDal = ticketUserDal;
        }

        public void TDelete(DataLayer.Concrete.Event entity)
        {
            _eventDal.Delete(entity);
        }
        // Linq sorgusu yazmak için EventModel kullanıldı.
        public List<EventModel> TGetEventAll()
        {
            var cities=  _cityDal.GetAll();
            var categories = _categoryDal.GetAll();
            var users = _ticketUserDal.GetAll();
            var events= _eventDal.GetAll();
            foreach (var item in events)
            {
                if(item.Date <= DateTime.Now)
                {
                    item.Status = "Geçmiş Etkinlik";
                    _eventDal.Update(item);
                }

            }
            var res = (

                from e in events
                join ct in categories on e.CategoryID equals ct.CategoryId into categoryGroup
                from ct in categoryGroup.DefaultIfEmpty()
                join c in cities on e.CityID equals c.CityId into cityGroup
                from c in cityGroup.DefaultIfEmpty()
                join u in users on e.UserID equals Convert.ToInt32(u.Id) into usersGroup
                from u in usersGroup.DefaultIfEmpty()
                select new EventModel
                {
                    EventId = e.EventId,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    Address = e.Address,
                    Capacity = e.Capacity,
                    CityName = c.CityName,
                    CategoryName = ct.CategoryName,
                    FullName = u.Name + " " + u.Surname,
                    AdminConfirm = e.AdminConfirm,
                    Status = e.Status

                }
                ).ToList();

            return res;
        }

        public EventModel TGetByEventId(int id)
        {
            var newEvent = _eventDal.GetById(id);
            EventModel @event = new EventModel();
            @event.EventId = newEvent.EventId;
            @event.Title = newEvent.Title;
            @event.Description = newEvent.Description;
            @event.Date = newEvent.Date;
            @event.Address = newEvent.Address;
            @event.Capacity = newEvent.Capacity;
            var name = _ticketUserDal.GetById(newEvent.UserID).Name;
            var surname = _ticketUserDal.GetById(newEvent.UserID).Surname;
            @event.FullName = name + " " + surname;
            @event.CategoryName = _categoryDal.GetById(newEvent.CategoryID).CategoryName;
            @event.CityName = _cityDal.GetById(newEvent.CityID).CityName;
            @event.Status = newEvent.Status;
            @event.AdminConfirm = newEvent.AdminConfirm;

            return @event;
        }

        public void TInsert(AddEventModel addEventModel)
        {
            TicketProject.DataLayer.Concrete.Event @event = new DataLayer.Concrete.Event();
            var cities = _cityDal.GetAll();
            foreach (var ct in cities)
            {
                if(addEventModel.CityName == ct.CityName)
                {
                    @event.CityID = ct.CityId;
                }
            }
            var categories = _categoryDal.GetAll();
            foreach (var c in categories)
            {
                if(addEventModel.CategoryName == c.CategoryName)
                {
                    @event.CategoryID= c.CategoryId;
                }
            }
            var users = _ticketUserDal.GetAll();
            foreach (var user in users)
            {
                if(addEventModel.FullName == user.Name + " "+ user.Surname) 
                {
                    @event.UserID= Convert.ToInt32(user.Id);
                }
            }
            @event.Title = addEventModel.Title;
            @event.Description = addEventModel.Description;
            @event.Date = addEventModel.Date;
            @event.Address = addEventModel.Address;
            @event.Capacity = addEventModel.Capacity;
            @event.Status = "Onay Bekliyor";
            @event.AdminConfirm = "Onay Bekliyor";

            _eventDal.Insert(@event);
        }
        public void TUpdate(UpdateEventModel updateEventModel)
        {
            TicketProject.DataLayer.Concrete.Event @event = new DataLayer.Concrete.Event();
            var cities = _cityDal.GetAll();
            foreach (var ct in cities)
            {
                if (updateEventModel.CityName == ct.CityName)
                {
                    @event.CityID = ct.CityId;
                }
            }
            var categories = _categoryDal.GetAll();
            foreach (var c in categories)
            {
                if (updateEventModel.CategoryName == c.CategoryName)
                {
                    @event.CategoryID = c.CategoryId;
                }
            }
            var users = _ticketUserDal.GetAll();
            foreach (var user in users)
            {
                if (updateEventModel.FullName == user.Name + " " + user.Surname)
                {
                    @event.UserID = Convert.ToInt32(user.Id);
                }
            }
            @event.EventId=updateEventModel.EventId;
            @event.Title = updateEventModel.Title;
            @event.Description = updateEventModel.Description;
            @event.Date = updateEventModel.Date;
            @event.Address = updateEventModel.Address;
            @event.Capacity = updateEventModel.Capacity;
            if(@event.Status == null)
            {
                @event.Status = "Onay Bekliyor";
                @event.AdminConfirm = "Onay Bekliyor";
            }
            _eventDal.Update(@event);
        }

        public void AdminConfirmAprove(int id)
        {
          var value = _eventDal.GetById(id);
            value.Status = "Onaylandı";
            value.AdminConfirm = "Onaylandı";
            _eventDal.Update(value);
        }

        public void AdminConfirmRefuse(int id)
        {
            var value = _eventDal.GetById(id);
            value.Status = "Reddedildi";
            value.AdminConfirm = "Reddedildi";
            _eventDal.Update(value);
        }
   #region Linq sorgu metotlarından dolayı kullanılmayan metotlar
        // Linq sorgusu ile yeni bir event model döndürdüğümüz için burdan sonraki metotlar kullanılmamaktadır.
        public List<DataLayer.Concrete.Event> TGetAll()
        {
           return _eventDal.GetAll();
        }

        // Delete işlemi için kullanıldı.
        public DataLayer.Concrete.Event TGetById(int id)
        {
          return _eventDal.GetById(id);
        }

        public void TInsert(DataLayer.Concrete.Event entity)
        {
           
        }
        public void TUpdate(DataLayer.Concrete.Event entity)
        {
            throw new NotImplementedException();
        }




        #endregion

        #region filter city,category
        // Filtreleme Katagori adına göre getirme
        public List<EventModel> TGetEventCategoryName(string catergoryName)
        {
            var newList = TGetEventAll();
            var categoryList = new List<EventModel>();
            foreach (var item in newList)
            {
                if (item.CategoryName == catergoryName)
                {
                    categoryList.Add(item);
                    
                }

            }
            return categoryList;
        }
        // Filtreleme Şehir adına göre getirme

        public List<EventModel> TGetEventCityName(string CityName)
        {
            var newList = TGetEventAll();
            var cityList = new List<EventModel>();
            foreach (var item in newList)
            {
                if (item.CityName == CityName)
                {
                    cityList.Add(item);
                    
                }                
            }
            return cityList;

        }
        #endregion
        public void EventCancel(int id)
        {
            var value = _eventDal.GetById(id);
            value.Status = "Etkinlik İptal Edildi";
            value.AdminConfirm = "Etkinlik İptal Edildi";
            _eventDal.Update(value) ;
             
        }
    
        // UserId ye göre Event getirme

        public  List<EventModel> TGetUserId(int userId)
        {
           var newList = TGetAll();
           var userIdList = new List<EventModel>();
            foreach (var item in newList)
            {
               if( item.UserID == userId)
                {
                   var evetId = item.EventId;
                   var newEventList = TGetByEventId(evetId);
                    if (true)
                    {
                        userIdList.Add(newEventList);
					}                   
				   
				}

            }
			return userIdList;
        }
	

	}
}
