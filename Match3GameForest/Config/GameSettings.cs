namespace Match3GameForest.Config
{
    public enum GameState
    {
        Init,
        Timed,
        Play,
        Pause,
        Finish,
    }

    public class GameSettings
    {
        public GameState State { get; set; }

        public int PlayingDuration => 60;
        public int MatrixColumns => 8;
        public int MatrixRows => 8;

        public int GameScore { get; set; }
        public int TimeLeft { get; set; }
    }
}
