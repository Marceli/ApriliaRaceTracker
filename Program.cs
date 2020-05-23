using System.IO;

namespace RaceTrack
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var persiter = new DBPersister(new RaceChronoReader(@"..\messages\RaceChrono.csv"));
            persiter.Save();
            using (var file = File.Create(@"..\messages\RaceChronoModified.csv"))
            using (var writer = new StreamWriter(file))
            {
                    writer.WriteLine(
                        $"Longtitude,Latitude,Speed,Clock");
                foreach (var reading in persiter.Get(8))
                    writer.WriteLine(
                        $"{reading.Longtitude},{reading.Latitude},{reading.Speed},{reading.Clock:yy-MM-dd hh:mm:ss.fff}");
            }
        }
    }
}