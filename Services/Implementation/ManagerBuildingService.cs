using Accommodation.DAL.Interface;
using Accommodation.Models;
using Accommodation.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.Services.Implementation
{
    public class ManagerBuildingService: IManagerBuildingService
    {
        private IManagerBuildingRepository _managerBuildingRepository;
        private readonly IBuildingService _buildingService;

        public ManagerBuildingService(IManagerBuildingRepository  managerBuildingRepository, IBuildingService buildingService)
        {
            _managerBuildingRepository = managerBuildingRepository;
            _buildingService = buildingService;
        }

        public bool ChceckIfExists(ManagerBuilding managerBuilding)
        {
            bool result = false;
            var dbRecords = _managerBuildingRepository.GetAll();
            foreach (var item in dbRecords)
            {
                if(item.ManagerId==managerBuilding.ManagerId && item.BuildingId == managerBuilding.BuildingId)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool Delete(ManagerBuilding managerBuilding)
        {
           return  _managerBuildingRepository.Delete(managerBuilding);
        }

        public IEnumerable<ManagerBuilding> Find(Func<ManagerBuilding, bool> prdicate)
        {
            return _managerBuildingRepository.Find(prdicate);
        }

        public List<ManagerBuilding> GetAll()
        {
            return _managerBuildingRepository.GetAll();
        }

        public string GetBuildingAddress(int managerId)
        {
            var buildingId = (from bm in _managerBuildingRepository.GetAll()
                              where bm.ManagerId == managerId
                              select bm.BuildingId).FirstOrDefault();

            var address = (from ba in _buildingService.GetBuildings()
                           where ba.BuildingId == buildingId
                           select ba.Address).FirstOrDefault();
            return address;
        }

        public ManagerBuilding GetSingleAssociiation(int id)
        {
            return _managerBuildingRepository.GetSingleAssociiation(id);
        }

        public bool Insert(ManagerBuilding managerBuilding)
        {
            return _managerBuildingRepository.Insert(managerBuilding);
        }

        public bool Update(ManagerBuilding managerBuilding)
        {
            return _managerBuildingRepository.Update(managerBuilding);
        }
    }
}