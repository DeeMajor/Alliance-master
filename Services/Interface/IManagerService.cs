using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Services.Interface
{
   public interface IManagerService
    {
        List<Manager> GetManagers();
        Manager GetManagers(int id);
        bool Insert(Manager manager);
        bool Update(Manager manager);
        bool Delete(Manager manager);
        IEnumerable<Manager> Find(Func<Manager, bool> prdicate);
    }
}
