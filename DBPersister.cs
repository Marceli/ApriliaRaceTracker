using System.Collections.Generic;
using PetaPoco;
using RaceTrack;

public class DBPersister
{
    private readonly ITrackReader reader;

    public DBPersister(ITrackReader reader)
    {
        this.reader = reader;
    }

    public void Save()
    {
               using (var db = new Database("Track"))
        {
            using (var t = db.GetTransaction())
            {
                db.Insert(reader.Track);

                foreach (var row in reader.GetRow())
                {
                    row.TrackId = reader.Track.Id;
                    db.Insert(row);
                }

                t.Complete();
            }
        }
    }

    public IEnumerable<Reading> Get(int trackId)
    {
        using (var db = new Database("Track"))
        {
            var rows=db.Fetch<Reading>("where track_id=@0 order by id", trackId);
            foreach (var row in rows)
            {
                yield return row;

            }
        }
    }
}