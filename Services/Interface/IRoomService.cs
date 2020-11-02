using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Services.Interface
{
   public interface IRoomService
    {
        List<Room> GetRooms();
        Room GetRooms(int id);
        bool Insert(Room room);
        bool Update(Room room);
        bool Delete(Room room);
        IEnumerable<Room> Find(Func<Room, bool> prdicate);
        int NewNumber(int buildId);

        int GetNoOfRoom(int buildId);
        string GetBuildingName(int buildId);
        string NewRoomNumber(int buildingId);
        string GetBuildingAddress(int? buildindId);
        bool ChceckPoeple(int roomTypeId,int numberOfPeople);
        int getBuildingId(int? roomId);
        int getNumberOfTenants(int roomTypeId);
        bool validateNumbers(Room room);
        void ChangeStatus(int? id);
    }
}
