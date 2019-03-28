using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessTimer
{
    public class GameResource
    {
        #region Constructors

        public GameResource(string language)
        {
            Language = language;

            Players = new List<Player>();

            OpenMessage = "Hi! Say the players amount please.";
            HelpMessage = "You can say tell me start a new game of chess, or, you can say exit... What can I help you with?";
            HelpReprompt = "What can I help you with?";
            StopMessage = "Good game. Goodbye!";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Язык.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Наименование навыка.
        /// </summary>
        public string SkillName { get; set; }

        /// <summary>
        /// Игроки.
        /// </summary>
        public List<Player> Players { get; set; }

        /// <summary>
        /// Имена игроков.
        /// </summary>
        private List<string> PlayerNames { get; set; }

        /// <summary>
        /// Первое сообщение с вопросом о количестве игроков.
        /// </summary>
        public string OpenMessage { get; set; }

        /// <summary>
        /// Сообщение о начале игры.
        /// </summary>
        private string StartMessage
        {
            get { return GetRandomMessage(StartMessages); }
        }

        /// <summary>
        /// Варианты стартовых сообщений.
        /// </summary>
        private List<string> StartMessages => new List<string>()
            {
                "It's time to play chess!",
                "It's time to fight in chess for the first place!",
                "We need to know who is the best chess player today."
            };

        /// <summary>
        /// Старт хода.
        /// </summary>
        public string StartTurnMessage { get; set; }

        /// <summary>
        /// Пауза.
        /// </summary>
        public string PauseTurnMessage { get; set; }

        /// <summary>
        /// Остановка игры.
        /// </summary>
        public string StopGameMessage { get; set; }

        /// <summary>
        /// Продолжение игры.
        /// </summary>
        public string ContinueGameMessage { get; set; }

        /// <summary>
        /// Помощь.
        /// </summary>
        public string HelpMessage { get; set; }

        /// <summary>
        /// Сообщение для непонятных ответов пользователя.
        /// </summary>
        public string HelpReprompt { get; set; }

        /// <summary>
        /// Завершение приложения.
        /// </summary>
        public string StopMessage { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Получение случайного сообщения из списка.
        /// </summary>
        /// <param name="messages">Список сообщений.</param>
        /// <returns>Случайное сообщение.</returns>
        public string GetRandomMessage(List<string> messages)
        {
            var rnd = new Random();
            int index = rnd.Next(0, messages.Count - 1);
            return messages[index];
        }

        /// <summary>
        /// Получение информации об игроках в зависимости от их количества.
        /// </summary>
        /// <param name="count">Количество игроков.</param>
        /// <returns>Сообщение с именами игроков.</returns>
        public string GetPlayesInfo(int count)
        {
            FillPlayersList(count);

            if (Players.Count == 2)
            {
                return $"Ok, two players. Your names are {Players[0].Name} and {Players[1].Name}. {Players[0].Name} will start the game. Are you ready?";
            }
            else if (Players.Count == 4)
            {
                return $"Ok, four players. First pair is {Players[0].Name} and {Players[1].Name}. Second pair is {Players[2].Name} and {Players[3].Name}." +
                    $" Are you ready to start the game?";
            }
            else if (Players.Count == 6)
            {
                return $"Ok, six players. First pair is {Players[0].Name} and {Players[1].Name}. Second pair is {Players[2].Name} and {Players[3].Name}." +
                $" Third pair is {Players[4].Name} and {Players[5].Name}. Are you ready to start the game?";
            }
            else if (Players.Count == 8)
            {
                return $"Ok, eight players. First pair is {Players[0].Name} and {Players[1].Name}. Second pair is {Players[2].Name} and {Players[3].Name}." +
                $" Third pair is {Players[4].Name} and {Players[5].Name}. Fourth pair is {Players[6].Name} and {Players[7].Name}. Are you ready to start the game?";
            }
            else
            {
                return "Choose between two, four, six or eight, please.";
            }
        }

        /// <summary>
        /// Заполнение списка игроков.
        /// </summary>
        /// <param name="count"></param>
        private void FillPlayersList(int count)
        {
            Players.Clear();

            int pairId = 1;
            for (int i = 1; i <= count; i++)
            {
                Players.Add(new Player(i, (PlayerName)i, pairId));

                if (i % 2 == 0)
                    pairId++;
            }
        }

        /// <summary>
        /// Старт игры.
        /// </summary>
        /// <returns></returns>
        public string StartGames()
        {
            if (Players.Count == 0)
                return "The game has not started yet.";

            var res = "";

            for (int i = 1; i <= Players.Count / 2; i++)
            {
                res += $" {StartNewGame(i)}, ";
            }

            var activePlayers = Players.Where(x => x.IsActive).Select(x => x.Name).ToList();

            var res2 = "The first move makes ";
            foreach (var name in activePlayers)
            {
                res2 += $"{name}, ";
            }

            return $"The game begins. {res.TrimEnd(' ').TrimEnd(',')}. {res2.TrimEnd(' ').TrimEnd(',')}.";
        }


        /// <summary>
        /// Старт игры для одной пары.
        /// </summary>
        /// <returns></returns>
        public string StartNewGame(int pairId)
        {
            var activePlayers = Players.Where(x => x.PairId == pairId).ToList();
            activePlayers[0].IsActive = true;
            activePlayers[0].StartTurn();
            var pairPlayers = Players.
                Where(x => x.PairId == pairId).
                Select(x => x.Name).
                ToList();
            return $"{(OrdinalNumber)pairId} pair: {pairPlayers[0]} and {pairPlayers[1]}";
        }

        /// <summary>
        /// Начало хода следующего игрока.
        /// </summary>
        /// <returns>Сообщение о ходе следующего игрока.</returns>
        public string StartNextPlayerTurn(string name)
        {
            if (Players.Count == 0)
                return "The game has not started yet.";

            int pairId = GetPairId(name);

            var currentPlayer = Players.Where(x => x.PairId == pairId && x.IsActive).First();
            currentPlayer.StopTurn();

            var nextPlayer = Players.Where(x => x.PairId == pairId && !x.IsActive).First();
            currentPlayer.IsActive = false;
            nextPlayer.IsActive = true;
            nextPlayer.StartTurn();

            return $"{nextPlayer.Name}'s move began.";
        }

        /// <summary>
        /// Пауза игры для одной пары.
        /// </summary>
        /// <returns></returns>
        public string PauseGame(string name)
        {
            if (Players.Count == 0)
                return "The game has not started yet.";

            int pairId = GetPairId(name);

            var activePairPlayer = Players.Where(x => x.PairId == pairId && x.IsActive).First();
            activePairPlayer.StopTurn();
            var pairPlayers = Players.
                Where(x => x.PairId == pairId).
                Select(x => x.Name).
                ToList();

            return $"For players {pairPlayers[0]} and {pairPlayers[1]} the game is paused.";
        }

        /// <summary>
        /// Продолжение игры для одной пары.
        /// </summary>
        /// <returns></returns>
        public string ContinueGame(string name)
        {
            if (Players.Count == 0)
                return "The game has not started yet.";

            int pairId = GetPairId(name);

            var activePairPlayer = Players.Where(x => x.PairId == pairId && x.IsActive).First();
            activePairPlayer.StartTurn();
            var pairPlayers = Players.
                Where(x => x.PairId == pairId).
                Select(x => x.Name).
                ToList();

            return $"For players {pairPlayers[0]} and {pairPlayers[1]} the game is continue.";
        }

        /// <summary>
        /// Получение информации о текущей длительности ходов игроков.
        /// </summary>
        /// <returns></returns>
        public string GetGameInfo()
        {
            if (Players.Count == 0)
                return "The game has not started yet.";

            var resultInfo = GetTurnsInfo();
            var hasNoTurnsInfo = GetHasNoTurns();

            if (hasNoTurnsInfo != null && resultInfo != null)
            {
                return $"{resultInfo} {hasNoTurnsInfo}";
            }
            else if (resultInfo != null)
            {
                return resultInfo;
            }
            else if (hasNoTurnsInfo != null)
            {
                return hasNoTurnsInfo;
            }
            else
            {
                return "No moves have been made yet.";
            }
        }

        /// <summary>
        /// Получение информации о текущей длительности ходов игроков.
        /// </summary>
        /// <returns></returns>
        public string GetTurnsInfo()
        {
            var resultInfo = "";

            foreach (var player in Players)
            {
                if (player.TotalTurnDuration <= 0)
                    continue;

                resultInfo += $"for {player.Name} is {GetTurnDurationUnit(player.TotalTurnDuration)}, ";
            }

            if (resultInfo != "")
            {
                resultInfo = $"Total turn duration {resultInfo.TrimEnd(' ').TrimEnd(',')}. ";
                return resultInfo;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Получение информации о продолжительности хода.
        /// </summary>
        /// <param name="sec">Секунды.</param>
        /// <returns>Сообщение с продолжительностью хода.</returns>
        private string GetTurnDurationUnit(int sec)
        {
            if (sec < 60)
            {
                return $"{sec} seconds";
            }
            else
            {
                return $"{sec/60} minutes and {sec%60} seconds";
            }
        }

        /// <summary>
        /// Получить информацию по игрокам, не совершавшим ходов.
        /// </summary>
        /// <returns></returns>
        public string GetHasNoTurns()
        {
            var hasNoMoves = "";
            var count = 0;

            foreach (var player in Players)
            {
                if (player.TotalTurnDuration > 0)
                    continue;

                hasNoMoves += $"{player.Name}, ";
                count++;
            }

            if (count == 0)
                return null;

            if (count == 1)
            {
                hasNoMoves = hasNoMoves.TrimEnd(' ').TrimEnd(',') + " has no completed moves.";
            }

            if (count > 1)
            {
                hasNoMoves = hasNoMoves.TrimEnd(' ').TrimEnd(',') + " have no completed moves.";
            }

            return hasNoMoves;
        }

        /// <summary>
        /// Получение id пары по имени одного из игроков.
        /// </summary>
        /// <param name="name">Имя игрока.</param>
        /// <returns>Идентификатор пары.</returns>
        public int GetPairId(string name)
        {
            return Players.Where(x => x.Name.ToString().ToLower() == name).Select(x => x.PairId).First();
        }

        #endregion
    }
}
