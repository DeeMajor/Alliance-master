using Accommodation.DAL.Interface;
using Accommodation.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.DAL.Implementation
{
    public class ManagerRepository : IManagerRepository
    {
        private DatabaseService<Manager> _databaseService;
        public ManagerRepository(DatabaseService<Manager> databaseService)
        {
            _databaseService = databaseService;
        }
        public bool Delete(Manager manager)
        {
            return _databaseService.Delete(manager);
        }

        public IEnumerable<Manager> Find(Func<Manager, bool> predicate)
        {
            return _databaseService.Find(predicate);
        }

        public List<Manager> GetManagers()
        {
            return _databaseService.Get().ToList();
        }

        public Manager GetManagers(int id)
        {
            return _databaseService.Get(id);
        }

        public bool Insert(Manager manager)
        {
            return _databaseService.Insert(manager);
        }

        public bool Update(Manager manager)
        {
            return _databaseService.Update(manager);
        }
    }
}