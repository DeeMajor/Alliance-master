using Accommodation.DAL.Interface;
using Accommodation.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.DAL.Implementation
{
    public class BuildingRepository : IBuildingRepository
    {
        private DatabaseService<Building> _databaseService;
        public BuildingRepository(DatabaseService<Building> databaseService)
        {
            _databaseService = databaseService;
        }
        public bool Delete(Building building)
        {
           return _databaseService.Delete(building);
        }

        public IEnumerable<Building> Find(Func<Building, bool> predicate)
        {
            return _databaseService.Find(predicate);
        }

        public List<Building> GetBuildings()
        {
            return _databaseService.Get().ToList();
        }

        public Building GetBuildings(int id)
        {
            return _databaseService.Get(id);
        }

        public bool Insert(Building building)
        {
           return _databaseService.Insert(building);
        }

        public bool Update(Building building)
        {
            return _databaseService.Update(building);
        }
    }
}