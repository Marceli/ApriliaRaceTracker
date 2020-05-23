using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace RaceTrack
{
    public class RaceChronoReader : ITrackReader
    {
        private readonly string path;

        public RaceChronoReader(string path)
        {
            this.path = path;
            Track = GetTrack();
        }

        public Track Track { get; }

        public IEnumerable<Reading> GetRow()
        {
            using (var reader = new StreamReader(path))
            {
                var cultures = new CultureInfo("en-US");
                var structure = new Dictionary<string, int>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null && line.StartsWith("Time", StringComparison.OrdinalIgnoreCase))
                    {
                        structure = GetStructureDefinition(line);
                        break;
                    }
                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    yield return new Reading
                    {
                        Clock = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc).AddSeconds(Convert.ToDouble(values[structure["Time (s)"]], cultures)),
                        Distance = Convert.ToDouble(values[structure["Distance (m)"]], cultures),
                        Speed = Convert.ToDouble(values[structure["Speed (m/s)"]], cultures),
                        Roll = Convert.ToDouble(values[structure["Lean angle (deg)"]], cultures),
                        Latitude = Convert.ToDecimal(values[structure["Latitude (deg)"]], cultures),
                        Longtitude = Convert.ToDecimal(values[structure["Longitude (deg)"]], cultures),
                        Acceleration = Convert.ToDecimal(values[structure["Combined acceleration (m/s2)"]], cultures)
                    };
                }
            }
        }

        private Dictionary<string, int> GetStructureDefinition(string headerLine)
        {
            var headers = headerLine.Split(',');
            var i = 0;

            return headers.ToDictionary(header => header, header => i++);
        }

        private Track GetTrack()
        {
            using (var reader = new StreamReader(path))
            {
                var track = new Track();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.IndexOf("Created using", StringComparison.OrdinalIgnoreCase) >= 0)
                        track.CreatedBy = line.Substring(line.IndexOf("using") + 6).Split('(')[0];

                    if (line.IndexOf("Session title", StringComparison.OrdinalIgnoreCase) >= 0)
                        track.Session = line.Substring(line.IndexOf("title") + 6).Replace(@"""", "")
                            .Replace(",", "");

                    if (line.IndexOf("Track name,", StringComparison.OrdinalIgnoreCase) >= 0)
                        track.Name = line.Substring(line.IndexOf(@"""") + 1).Replace(@"""", "");

                    if (line.IndexOf("Created,", StringComparison.OrdinalIgnoreCase) >= 0)
                        track.Date =
                            DateTime.ParseExact(line.Substring(line.IndexOf("Created,") + 8).Replace(@",", " "),
                                "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                    if (line.StartsWith("Time", StringComparison.OrdinalIgnoreCase))
                        break;
                }

                return track;
            }
        }
    }
}