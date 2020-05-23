using System;
using System.Security.AccessControl;
using PetaPoco;

namespace RaceTrack
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Session { get; set; }
        public DateTime Date { get; set; }
        public string CreatedBy { get; set; }
        [Column("User_Id")]
        public int UserId { get; set; }
        
    }
}