using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.DAL.Interface
{
   public interface ITimeSlotRepository
    {
        List<timeslot> GetTimeSlots();
        timeslot GetTimeSlot(int id);
        bool Insert(timeslot  timeslot);
        bool Update(timeslot timeslot);
        bool Delete(timeslot timeslot);
        IEnumerable<timeslot> Find(Func<timeslot, bool> prdicate);
    }
}
