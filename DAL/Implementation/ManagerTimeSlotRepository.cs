using Accommodation.DAL.Interface;
using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.DAL.Implementation
{
    public class ManagerTimeSlotRepository : IManagerTimeSlotRepository
    {
        private DatabaseService<ManagerTimeSlot> _databaseService;
        public ManagerTimeSlotRepository(DatabaseService<ManagerTimeSlot> databaseService)
        {
            _databaseService = databaseService;
        }
        public bool Delete(ManagerTimeSlot managerTimeSlot)
        {
            return _databaseService.Delete(managerTimeSlot);
        }

        public IEnumerable<ManagerTimeSlot> Find(Func<ManagerTimeSlot, bool> prdicate)
        {
            return _databaseService.Find(prdicate);
        }

        public ManagerTimeSlot GetManagerTimeSlot(int id)
        {
            return _databaseService.Get(id);
        }

        public List<ManagerTimeSlot> GetManagerTimeSlots()
        {
            return _databaseService.Get().ToList();
        }

        public bool Insert(ManagerTimeSlot managerTimeSlot)
        {
            return _databaseService.Insert(managerTimeSlot);
        }

        public bool Update(ManagerTimeSlot managerTimeSlot)
        {
            return _databaseService.Update(managerTimeSlot);
        }
    }
}