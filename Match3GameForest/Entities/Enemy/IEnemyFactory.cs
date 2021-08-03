namespace Match3GameForest.Entities
{
    public interface IEnemyFactory
    {
        float Scale { get; set; }
        IEnemy Build();
    }
}