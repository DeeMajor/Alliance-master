using Accommodation.DAL.Interface;
using Accommodation.Models;
using Accommodation.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.Services.Implementation
{
    public class ManagerService : IManagerService
    {
        private IManagerRepository _managerRepository;

        public ManagerService(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }
        public bool Delete(Manager manager)
        {
            return _managerRepository.Delete(manager);
        }

        public IEnumerable<Manager> Find(Func<Manager, bool> prdicate)
        {
            return _managerRepository.Find(prdicate);
        }

        public List<Manager> GetManagers()
        {
            return _managerRepository.GetManagers().ToList();
        }

        public Manager GetManagers(int id)
        {
            return _managerRepository.GetManagers(id);
        }

        public bool Insert(Manager manager)
        {
            return _managerRepository.Insert(manager);
        }

        public bool Update(Manager manager)
        {
            return _managerRepository.Update(manager);
        }
    }
}