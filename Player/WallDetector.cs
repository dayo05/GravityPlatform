using Godot;

namespace GravityPlatform.Player
{
    public class WallDetector: Area2D
    {
        public bool IsWallDetected { get; private set; } = false;
        private Player player;

        public override void _Ready()
        {
            player = GetParent() as Player;
            player.Connect("GameOver", this, "SGameOver");
            base._Ready();
        }

        void OnBodyEnter(Node2D node)
        {
            if (node is TileMap t)
            {
                if(t.GetCollisionMaskBit(1))
                    player.EmitSignal("GameOver");
                else IsWallDetected = true;
            }
        }

        void OnBodyExit(Node2D node)
        {
            if(node is TileMap)
                IsWallDetected = false;
        }

        private void SGameOver()
        {
            IsWallDetected = false;
        }
    }
}