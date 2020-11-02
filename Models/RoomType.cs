using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Accommodation.Models
{
    public class RoomType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int roomtypeId { get; set; }
        [Required]
        [DisplayName("Room Type")]
        [MinLength(5)]
        [RegularExpression(pattern: @"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Numbers and special characters are not allowed.")]
        public string Type { get; set; }
        [DisplayName("Rooms Available")]
        [Range(1, 300)]
        public Nullable<int> RoomAvailable { get; set; }//Status
        [DisplayName("Number Of People")]
        public int NumOfRooms { get; set; } 
        public ICollection <Room> Rooms { get; set; }
 
    }
}