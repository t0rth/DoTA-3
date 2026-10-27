using System.Windows;
using System.Windows.Media;

namespace DoTA_3
{
    public class SphereDamage : CCollectable
    {
        public SphereDamage(Point pos, double size, double life, GameWindow game)
            : base(pos, size, life, game)
        {
            sprite.Fill = Brushes.OrangeRed;
        }

        public override void onClick()
        {
            game.Player.IncreaseDamage(1.5);
            Remove();
        }
    }
}