using Accommodation.DAL.Interface;
using Accommodation.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.DAL.Implementation
{
    public class RoomRepository : IRoomRepository
    {
        private DatabaseService<Room> _databaseService;
        public RoomRepository(DatabaseService<Room> databaseService)
        {
            _databaseService = databaseService;
        }
        public bool Delete(Room room)
        {
            return _databaseService.Delete(room);
        }

        public IEnumerable<Room> Find(Func<Room, bool> predicate)
        {
            return _databaseService.Find(predicate);
        }

        public List<Room> GetRooms()
        {
            return _databaseService.Get().ToList();
        }

        public Room GetRooms(int id)
        {
            return _databaseService.Get(id);
        }

        public bool Insert(Room room)
        {
            return _databaseService.Insert(room);
        }

        public bool Update(Room room)
        {
            return _databaseService.Update(room);
        }
    }
}