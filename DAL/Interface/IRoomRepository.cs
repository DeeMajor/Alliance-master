using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.DAL.Interface
{
    public interface IRoomRepository
    {
        List<Room> GetRooms();
        Room GetRooms(int id);
        bool Insert(Room room);
        bool Update(Room room);
        bool Delete(Room room);
        IEnumerable<Room> Find(Func<Room, bool> prdicate);
    }
}
