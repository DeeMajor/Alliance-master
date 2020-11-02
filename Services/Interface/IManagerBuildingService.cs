﻿using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Services.Interface
{
   public interface IManagerBuildingService
    {
        List<ManagerBuilding> GetAll();
        ManagerBuilding GetSingleAssociiation(int id);
        bool Insert(ManagerBuilding managerBuilding);
        bool Update(ManagerBuilding managerBuilding);
        bool Delete(ManagerBuilding managerBuilding);
        IEnumerable<ManagerBuilding> Find(Func<ManagerBuilding, bool> prdicate);

        bool ChceckIfExists(ManagerBuilding managerBuilding);

        string GetBuildingAddress(int managerId);
    }
}