using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketProject.DataLayer.Context;

namespace TicketProject.DataLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly TPContext _context;

        public GenericRepository(TPContext context)
        {
            _context = context;
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public List<T> GetAll()
        {
            var value = _context.Set<T>().ToList();
            return value;


        }

        public T GetById(int id)
        {
            var value = _context.Set<T>().Find(id);
            return value;

        }
             

        public void Insert(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
            
        }

        public void Update(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
    }
}
