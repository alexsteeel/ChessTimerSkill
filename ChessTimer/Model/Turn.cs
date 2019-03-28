using System;

namespace ChessTimer
{
    /// <summary>
    /// Ход игрока.
    /// </summary>
    public class Turn
    {
        #region Constructors
        public Turn(int id)
        {
            Id = id;
            Duration = 0;
        }

        #region

        #region Fields.

        /// <summary>
        /// Идентификатор хода.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Время начала хода.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Продолжительность хода.
        /// </summary>
        public double Duration { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Начало хода.
        /// </summary>
        public void Start()
        {
            StartTime = DateTime.Now;
        }

        /// <summary>
        /// Конец/пауза для хода.
        /// </summary>
        public void Stop()
        {
            Duration += (DateTime.Now - StartTime).TotalSeconds;
        }

        #region
    }
}
