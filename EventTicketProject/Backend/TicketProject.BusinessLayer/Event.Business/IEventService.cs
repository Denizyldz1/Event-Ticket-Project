using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.BusinessLayer.GenericService.Business;
using TicketProject.DataLayer.Concrete;
using TicketProject.DtoLayer.Dtos.EventDto;

namespace TicketProject.BusinessLayer.Event.Business
{
    public interface IEventService : IGenericService<DataLayer.Concrete.Event>
    {
        // Linq sorgusu ile Event tablosunu getirmek için eklendi.
        List<EventModel> TGetEventAll();
        void TInsert(AddEventModel addEventModel);
        void TUpdate(UpdateEventModel updateEventModel);
        EventModel TGetByEventId(int id);
        void AdminConfirmAprove(int id);
        void AdminConfirmRefuse(int id);
        void EventCancel(int id);

        // Filtreleme için kategori adına  göre getir
        List<EventModel> TGetEventCategoryName(string catergoryName);
        //Filtreleme için şehir adına göre getir.
        List<EventModel> TGetEventCityName(string CityName);

        // ID ye göre Eventleri getirme
        List<EventModel> TGetUserId(int userId);



    }
}
