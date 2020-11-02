using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Services.Interface
{
   public interface ITimeSlotManagerService
    {
        List<ManagerTimeSlot> GetManagerTimeSlots();
        ManagerTimeSlot GetManagerTimeSlot(int id);
        bool Insert(ManagerTimeSlot managerTimeSlot);
        bool Update(ManagerTimeSlot managerTimeSlot);
        bool Delete(ManagerTimeSlot managerTimeSlot);
        IEnumerable<ManagerTimeSlot> Find(Func<ManagerTimeSlot, bool> prdicate);
    }
}
