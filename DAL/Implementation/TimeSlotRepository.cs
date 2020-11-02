using Accommodation.DAL.Interface;
using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.DAL.Implementation
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private DatabaseService<timeslot> _databaseService;
        public TimeSlotRepository(DatabaseService<timeslot> databaseService)
        {
            _databaseService = databaseService;
        }
        public bool Delete(timeslot timeslot)
        {
            return _databaseService.Delete(timeslot);
        }

        public IEnumerable<timeslot> Find(Func<timeslot, bool> prdicate)
        {
            return _databaseService.Find(prdicate);
        }

        public timeslot GetTimeSlot(int id)
        {
            return _databaseService.Get(id);
        }

        public List<timeslot> GetTimeSlots()
        {
            return _databaseService.Get().ToList();
        }

        public bool Insert(timeslot timeslot)
        {
            return _databaseService.Insert(timeslot);
        }

        public bool Update(timeslot timeslot)
        {
            return _databaseService.Update(timeslot);
        }
    }
}