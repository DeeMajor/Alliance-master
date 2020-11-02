using Accommodation.DAL.Interface;
using Accommodation.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.DAL.Implementation
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private DatabaseService<RoomType> _databaseService;
        public RoomTypeRepository(DatabaseService<RoomType> databaseService)
        {
            _databaseService = databaseService;
        }
        public bool Delete(RoomType roomType)
        {
            return _databaseService.Delete(roomType);
        }

        public IEnumerable<RoomType> Find(Func<RoomType, bool> predicate)
        {
            return _databaseService.Find(predicate);
        }

        public List<RoomType> GetRoomTypes()
        {
            return _databaseService.Get().ToList();
        }

        public RoomType GetRoomTypes(int id)
        {
            return _databaseService.Get(id);
        }

        public bool Insert(RoomType roomType)
        {
            return _databaseService.Insert(roomType);
        }

        public bool Update(RoomType roomType)
        {
            return _databaseService.Update(roomType);
        }
    }
}