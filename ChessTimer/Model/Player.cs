using System.Collections.Generic;

namespace ChessTimer
{
    public class Player
    {
        #region Constructors.

        public Player()
        {

        }

        public Player(int id, PlayerName name, int pairId)
        {
            Id = id;
            Name = name;
            PairId = pairId;
            Turns = new List<Turn>();
        }

        #endregion

        #region Fields.

        /// <summary>
        /// Идентификатор игрока.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пары.
        /// </summary>
        public int PairId { get; set; }

        /// <summary>
        /// Имя игрока.
        /// </summary>
        public PlayerName Name { get; set; }

        /// <summary>
        /// Флаг активности.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Список ходов.
        /// </summary>
        public List<Turn> Turns { get; set; }

        /// <summary>
        /// Общая продолжительность ходов игрока в секундах.
        /// </summary>
        public int TotalTurnDuration
        {
            get
            {
                int res = 0;
                foreach (var turn in Turns)
                {
                    if (turn.Duration <= 0)
                        continue;

                    res += (int)turn.Duration;
                }
                return res;
            }
        }

        #endregion

        #region Methods.

        /// <summary>
        /// Начать ход.
        /// </summary>
        public void StartTurn()
        {
            var turn = new Turn(Turns.Count + 1);
            turn.Start();
            Turns.Add(turn);
        }

        /// <summary>
        /// Остановить ход.
        /// </summary>
        public void StopTurn()
        {
            Turns[Turns.Count - 1].Stop();
        }

        #endregion
    }
}
