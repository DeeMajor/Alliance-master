using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Services.Interface
{
    public interface IBuildingService
    {
        List<Building> GetBuildings();
        Building GetBuildings(int id);
        bool Insert(Building building);
        bool Update(Building building);
        bool Delete(Building building);
        IEnumerable<Building> Find(Func<Building, bool> prdicate);
        bool CheckManager(int managerid);

        int getBuildingId(string ownerEmail);

        //double CslTot();
    }
}