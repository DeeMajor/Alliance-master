using Accommodation.DAL.Interface;
using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.DAL.Implementation
{
    public class ManagerBuildingRepository : IManagerBuildingRepository
    {
        private DatabaseService<ManagerBuilding> _databaseService;
        public ManagerBuildingRepository(DatabaseService<ManagerBuilding> databaseService)
        {
            _databaseService = databaseService;
        }
        public bool Delete(ManagerBuilding managerBuilding)
        {
           return _databaseService.Delete(managerBuilding);
        }

        public IEnumerable<ManagerBuilding> Find(Func<ManagerBuilding, bool> prdicate)
        {
            return _databaseService.Find(prdicate);
        }

        public List<ManagerBuilding> GetAll()
        {
            return _databaseService.Get().ToList();
        }

        public ManagerBuilding GetSingleAssociiation(int id)
        {
            return _databaseService.Get(id);
        }

        public bool Insert(ManagerBuilding managerBuilding)
        {
            return _databaseService.Insert(managerBuilding);
        }

        public bool Update(ManagerBuilding managerBuilding)
        {
            return _databaseService.Update(managerBuilding);
        }
    }
}