using System;

namespace ChessTimer
{
    public class Turn
    {
        public Turn(int id)
        {
            Id = id;
            Duration = 0;
        }

        public int Id { get; set; }

        public DateTime StartTime { get; set; }

        public double Duration { get; set; }

        public void Start()
        {
            StartTime = DateTime.Now;
        }

        public void Stop()
        {
            Duration += (DateTime.Now - StartTime).TotalSeconds;
        }
    }
}
