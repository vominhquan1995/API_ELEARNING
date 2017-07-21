using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api_ELearning.Models;
using Api_ELearning.Data;
using Api_ELearning.DependencyInjections;

namespace Api_ELearning.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ELearningDbContext _context;
        public NotificationRepository(ELearningDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Notification>All
        {
            get
            {
                return _context.Notifications.ToList();
            }
            set { }
        }

        public bool Add(Notification o)
        {
            try
            {        
                _context.Notifications.Add(o);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Edit(Notification o)
        {
            try
            {
                _context.Update(o);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Notification Get(int id)
        {
            return _context.Notifications.Find(id);
        }
        public bool Remove(int id)
        {
            try
            {
                _context.Notifications.Remove(_context.Notifications.Find(id));
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
