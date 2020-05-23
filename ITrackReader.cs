using System.Collections.Generic;

namespace RaceTrack
{
    public interface ITrackReader
    {
        Track Track { get; }
        IEnumerable<Reading> GetRow();
    }
}