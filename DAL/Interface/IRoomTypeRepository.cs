using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.DAL.Interface
{
   public interface IRoomTypeRepository
    {
        List<RoomType> GetRoomTypes();
        RoomType GetRoomTypes(int id);
        bool Insert(RoomType roomType);
        bool Update(RoomType roomType);
        bool Delete(RoomType roomType);
        IEnumerable<RoomType> Find(Func<RoomType, bool> prdicate);
    }
}
