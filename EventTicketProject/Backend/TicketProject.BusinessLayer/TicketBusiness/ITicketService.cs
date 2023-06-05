using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.BusinessLayer.GenericService.Business;
using TicketProject.DataLayer.Concrete;
using TicketProject.DtoLayer.Dtos.EventDto;
using TicketProject.DtoLayer.Dtos.Ticket;

namespace TicketProject.BusinessLayer.TicketBusiness
{
    public interface ITicketService: IGenericService<Ticket>
    {
        // Linq sorgusu ile bütün biletleri getir
        List<TicketModel> TGetTicketAll();
        // Linq sorgusu ticketId ile  tek bilet getir.
        TicketModel TGetByTicketId(int id);
        // Linq sorgusu ticketNumber ile tek bilet getir.
        TicketModel TGetByTicketNumber(string ticketNumber);
        // Linq sorgusu userId ile User tüm biletlerini getir.
        List<TicketModel> TGetByUserId(int userId);
        // Bilet iptal etme Status: False yap
        void TicketCancel(int id);
        // Ticket number üreterek kayıt yap
        string TTicketInsert(AddTicketModel addTicketModel);
    }
}
