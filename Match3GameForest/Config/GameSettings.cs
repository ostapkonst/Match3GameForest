namespace Match3GameForest.Config
{
    public enum GameState
    {
        Init,
        Timed,
        Play,
        Finish,
    }

    public class GameSettings
    {
        public GameState State = GameState.Finish;

        public int PlayingDuration { get; set; } = 60;
        public int MatrixColumns { get; set; } = 8;
        public int MatrixRows { get; set; } = 8;

        public int GameScore { get; set; } = 0;
        public int TimeLeft { get; set; } = 60;

        public bool PlaySound { get; set; } = false;
    }
}
