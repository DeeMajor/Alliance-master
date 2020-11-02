using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.DAL.Interface
{
   public interface IBuildingRepository
    {
        List<Building> GetBuildings();
        Building GetBuildings(int id);
        bool Insert(Building building);
        bool Update(Building building);
        bool Delete(Building building);
        IEnumerable<Building> Find(Func<Building, bool> prdicate);
    }
}
