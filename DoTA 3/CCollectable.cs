using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DoTA_3
{
    public abstract class CCollectable
    {
        protected Point position;
        protected Size size;
        protected double lifetime;
        protected Ellipse sprite;
        protected GameWindow game; // Ссылка на окно игры

        public bool IsCollected { get; private set; } = false;

        public CCollectable(Point position, double size, double lifetime, GameWindow game)
        {
            this.position = position;
            this.size = new Size(size, size);
            this.lifetime = lifetime;
            this.game = game;

            // Создаем визуальный объект (круг)
            sprite = new Ellipse();
            sprite.StrokeThickness = 2;
            sprite.Stroke = Brushes.Black;
            sprite.Width = this.size.Width;
            sprite.Height = this.size.Height;
            sprite.RenderTransform = new TranslateTransform(position.X, position.Y);

            // При клике вызываем абстрактный метод
            sprite.MouseLeftButtonDown += (s, e) =>
            {
                if (!IsCollected) onClick();
            };
        }

        // Абстрактный метод (наследники должны его реализовать)
        public abstract void onClick();

        public Ellipse getSprite() => sprite;

        public bool updateLifetime(double delta)
        {
            lifetime -= delta;
            if (lifetime <= 0)
            {
                Remove();
                return true;
            }
            return false;
        }

        public void Remove()
        {
            if (IsCollected) return;
            IsCollected = true;
            game.RemoveSphere(this);
        }
    }
}