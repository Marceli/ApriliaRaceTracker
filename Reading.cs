using System;
using PetaPoco;

namespace RaceTrack
{
    public class Reading
    {
        private DateTime clock;
        public int Id { get; set; }
        public decimal Longtitude { get; set; }
        public decimal Latitude { get; set; }
        public double Speed { get; set; }

        public DateTime Clock { get; set; }

        public double Distance { get; set; }
        public double Roll { get; set; }
        public float WheelSlip { get; set; }
        public float Power { get; set; }
        public float Torque { get; set; }
        public int Rpm { get; set; }
        public float FrontSpeed { get; set; }
        public float RearSpeed { get; set; }
        public decimal Acceleration { get; set; }
        [Column("Track_Id")]
        public int TrackId { get; set; }
    }
}