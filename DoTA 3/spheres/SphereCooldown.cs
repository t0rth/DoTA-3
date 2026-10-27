using System.Windows;
using System.Windows.Media;

namespace DoTA_3
{
    public class SphereCooldown : CCollectable
    {
        public SphereCooldown(Point pos, double size, double life, GameWindow game)
            : base(pos, size, life, game)
        {
            sprite.Fill = Brushes.HotPink;
        }

        public override void onClick()
        {
            game.Player.DecreaseCooldown(0.2);
            Remove();
        }
    }
}