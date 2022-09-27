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
            Offset = new Vector2((int)(tracker.Position.x + 1000) / 2000 * 2000, (int)(tracker.Position.y + 600) / 1200 * 1200);
            base._Process(delta);
        }
    }
}