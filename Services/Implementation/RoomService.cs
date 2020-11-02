using Accommodation.DAL.Interface;
using Accommodation.Models;
using Accommodation.Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Accommodation.Services.Implementation
{
    public class RoomService : IRoomService
    {
        private IRoomRepository _roomRepository;
        private IBuildingRepository _buildingRepository;
        private IRoomTypeService _roomTypeService;

        public RoomService(IRoomRepository roomRepository, IBuildingRepository buildingRepository,IRoomTypeService roomTypeService)
        {
            _roomRepository = roomRepository;
            _buildingRepository = buildingRepository;
            _roomTypeService = roomTypeService;
        }
        public bool Delete(Room room)
        {
            return _roomRepository.Delete(room);
        }

        public IEnumerable<Room> Find(Func<Room, bool> prdicate)
        {
            return _roomRepository.Find(prdicate);
        }

        public List<Room> GetRooms()
        {
            return _roomRepository.GetRooms().ToList();
        }

        public Room GetRooms(int id)
        {
            return _roomRepository.GetRooms(id);
        }

        public bool Insert(Room room)
        {
            return _roomRepository.Insert(room);
        }

        public bool Update(Room room)
        {

            return _roomRepository.Update(room);
        }

        public int GetNoOfRoom(int buildId)
        {
            var nor = from b in _buildingRepository.GetBuildings()
                      where b.BuildingId == buildId
                      select b.TotalNumberOfRooms;
            return Convert.ToInt32(nor);
        }

        public string GetBuildingName(int buildId)
        {
            //var buildingName=from b in _buildingRepository.
            var bn = from b in _buildingRepository.GetBuildings()
                     where b.BuildingId == buildId
                     select b.BuildingName.FirstOrDefault();
            return (string) bn;
        }

        public Random a = new Random(); // replace from new Random(DateTime.Now.Ticks.GetHashCode());
                                        // Since similar code is done in default constructor internally
        public List<int> randomList = new List<int>();
        int MyNumber = 0;
        public string  NewRoomNumber(int buildId)
        {
            MyNumber = _buildingRepository.GetBuildings().Count();
            //if (!randomList.Contains(MyNumber))
            //    randomList.Add(MyNumber);
            string name = GetBuildingName(buildId).Substring(0, 3) + MyNumber;
            return name;
        }

        public int NewNumber(int buildId)
        {
            MyNumber = a.Next(0, GetNoOfRoom(buildId));
            if (!randomList.Contains(MyNumber))
                randomList.Add(MyNumber);

            return MyNumber;
        }

        public bool ChceckPoeple(int roomTypeId, int numberOfPeopl)
        {
            var recuredNumber = (from rt in _roomTypeService.GetRoomTypes()
                                    where rt.roomtypeId == roomTypeId
                                    select rt.NumOfRooms).FirstOrDefault();
            if(recuredNumber >= numberOfPeopl)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetBuildingAddress(int? buildindId)
        {
            var building = (from b in _buildingRepository.GetBuildings()
                            where b.BuildingId == buildindId
                            select b.Address).FirstOrDefault();
            return building;
        }

        public int getBuildingId(int? roomId)
        {
            int buildingId=0;
            var building = _roomRepository.GetRooms();

            foreach (var item in building)
            {
                if (item.BuildingId == roomId)
                {
                    buildingId = item.BuildingId;
                }
            }
            return buildingId;
        }

        public int getNumberOfTenants(int roomTypeId)
        {
            var numberOfTenants = (from rt in _roomTypeService.GetRoomTypes()
                                   where rt.roomtypeId == roomTypeId
                                   select rt.NumOfRooms).FirstOrDefault();
            return numberOfTenants;
        }

        public bool validateNumbers(Room room)
        {
            bool result = false;
            if(room.NoOfPeople>0 && room.RoomPrice > 0)
            {
                result = true;
            }
            return result;
        }

        public void ChangeStatus(int? id)
        {
            throw new NotImplementedException();
        }
    }
}