using System.Collections.Generic;
using Match3GameForest.Core;

namespace Match3GameForest.Entities
{
    public abstract class Bonus : Sprite, IBonus
    {
        public IEnemy Carrier { get; protected set; }

        public bool IsActive { get; private set; }

        public Bonus(ISpriteBatch spriteBatch, ITexture2D spriteStrip)
        : base(spriteBatch, spriteStrip)
        {
        }

        protected IBonus AssignEvents(IEnemy selectedEnemy)
        {
            var enemy = (IEnemy)selectedEnemy.Clone();
            var bonus = (Bonus)Clone();

            enemy.AfterUpdate += bonus.Update;
            enemy.AfterDraw += bonus.Draw;
            enemy.OnPosition += (value) => bonus.Position = value;
            enemy.OnScale += (value) => bonus.Scale = value;
            enemy.OnLightning += (value) => bonus.Lightning = value;

            enemy.OnDestroy += () =>
            {
                bonus.IsActive = true;
                bonus.Hidden = true;
            };

            bonus.Carrier = enemy;
            bonus.IsActive = false;

            return bonus;
        }

        public abstract IList<IBonus> Build(FieldSeries series);

        public void Deactivate() => IsActive = false;
    }
}
