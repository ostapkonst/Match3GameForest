namespace Match3GameForest.Config
{
    public enum GameState
    {
        Waite,
        Init,
        Timed,
        Play,
        Finish,
    }

    // Для механики игры
    public class GameSettings
    {
        public GameState State { get; set; } = GameState.Waite;

        public int PlayingDuration { get; set; } = 60;
        public int MatrixColumns { get; set; } = 8;
        public int MatrixRows { get; set; } = 8;

        public int GameScore { get; set; }
        public int TimeLeft { get; set; }
    }
}
