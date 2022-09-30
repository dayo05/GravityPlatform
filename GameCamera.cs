using Godot;

namespace GravityPlatform
{
    public class GameCamera : Camera2D
    {
        private Player.Player tracker;

        public override void _Ready()
        {
            tracker = GetParent().GetNode<Player.Player>("Player");
            Offset = Vector2.Zero;
        }

        private bool isGameOvered = false;

        public override void _Process(float delta)
        {
            if (isGameOvered) return;
            Offset = new Vector2(CalculateX((int)tracker.Position.x, (int)(1000 * Zoom.x)),
                CalculateY((int)tracker.Position.y, (int)(600 * Zoom.y)));
            base._Process(delta);
        }

        private int CalculateY(int value, int size)
        {
            value += size / 2;
            var k = value < 0 ? (value / size - 1) : value / size;
            return (int)(k * size);
        }

        private int CalculateX(int value, int size)
        {
            value -= size / 2;
            var k = value < 0 ? (value / size) : value / size + 1;
            return (int)(k * size);
        }
    }
}