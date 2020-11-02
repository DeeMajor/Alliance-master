using Accommodation.DAL.Interface;
using Accommodation.Models;
using Accommodation.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Accommodation.Services.Implementation
{
    public class RoomTypeService : IRoomTypeService
    {
        private IRoomTypeRepository _roomTypeRepository;
        

        public RoomTypeService(IRoomTypeRepository roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }
        public bool Delete(RoomType roomType)
        {
            return _roomTypeRepository.Delete(roomType);
        }

        public IEnumerable<RoomType> Find(Func<RoomType, bool> prdicate)
        {
            return _roomTypeRepository.Find(prdicate);
        }

        public List<RoomType> GetRoomTypes()
        {
            return _roomTypeRepository.GetRoomTypes().ToList();
        }

        public RoomType GetRoomTypes(int id)
        {
            return _roomTypeRepository.GetRoomTypes(id);
        }

        public bool Insert(RoomType roomType)
        {
            return _roomTypeRepository.Insert(roomType);
        }

        public bool Update(RoomType roomType)
        {
            return _roomTypeRepository.Update(roomType);
        }
    }
}