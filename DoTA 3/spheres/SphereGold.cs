using System.Windows;
using System.Windows.Media;

namespace DoTA_3
{
    public class SphereGold : CCollectable
    {
        public SphereGold(Point pos, double size, double life, GameWindow game)
            : base(pos, size, life, game)
        {
            sprite.Fill = Brushes.Gold;
        }

        public override void onClick()
        {
            game.Player.AddGold(new BigNumber("50"));
            Remove();
        }
    }
}