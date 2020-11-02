using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class Room
    {
        [Key]
        public int RoomId { get; set; }
        public int BuildingId { get; set; }
        public int roomtypeId { get; set; }
        public string RoomNumber { get; set; }
        [DisplayName("Number Of People")]

        public int NoOfPeople { get; set; }
        [DisplayName("Room Description")]
        public string roomDescription { get; set; }
        
        public byte[] RoomPicture { get; set; }
        [DisplayName("Room Price"),DataType(DataType.Currency)]
        public decimal RoomPrice { get; set; }
        [DisplayName("Room Status")]
        public string Status { get; set; }
        public virtual Building Buildings { get; set; }
        public virtual RoomType RoomTypes { get; set; }
    }
}