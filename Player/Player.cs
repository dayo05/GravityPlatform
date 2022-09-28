using Godot;
using GravityPlatform.Util;
using GravityPlatform.Zone;

namespace GravityPlatform.Player
{
    public partial class Player : KinematicBody2D
    {
        public float GravityBias = 1.0f;
        private Vector2 LinearVelocity;
        private AnimatedSprite sprite;
        private Vector2 respawnPoint;
        private float respawnGravity = 1.0f;

        private int fixedGravityZoneStack = 0;

        private InputHandler left = new InputHandler("left");
        private InputHandler right = new InputHandler("right");
        private InputHandler up = new InputHandler("up");
        private InputHandler down = new InputHandler("down");
        private InputHandler jump = new InputHandler("jump");
        private InputHandler dash = new InputHandler("dash");
        private InputHandler grab = new InputHandler("grab");
        private InputHandler gravity_1 = new InputHandler("gravity_1");
        
        public override void _Ready()
        {
            sprite = GetNode<AnimatedSprite>("Sprite");
            lWall = GetNode<WallDetector>("LWallArea");
            rWall = GetNode<WallDetector>("RWallArea");
            celling = GetNode<WallDetector>("CellingArea");
            floor = GetNode<WallDetector>("FloorArea");
            
            respawnPoint = Position;
        }

        private int dashCount = 1;

        public override void _Input(InputEvent evt)
        {
            if (gravity_1.ValidatePress(evt) && fixedGravityZoneStack == 0)
                GravityBias *= -1;

            base._Input(evt);
        }
        
        private string Animation { set => sprite.Play(value); }

        private void ResetDash()
        {
            dashCount = 1;
        }

        [Signal]
        delegate void AddDash();

        [Signal]
        delegate void GameOver();

        [Signal]
        delegate void SavePoint(Vector2 point);

        [Signal]
        delegate void IZone(string tag);

        [Signal]
        delegate void OZone(string tag);

        private void SAddDash()
        {
            dashCount++;
        }

        private void SGameOver()
        {
            taskGameOver = true;
        }

        private void SSavePoint(Vector2 point)
        {
            respawnPoint = point;
            respawnGravity = GravityBias;
        }

        private void SIZone(string tag)
        {
            switch (tag)
            {
                case nameof(FixedGravityZone):
                    fixedGravityZoneStack++;
                    break;
            }
        }

        private void SOZone(string tag)
        {
            switch(tag) 
            {
                case nameof(FixedGravityZone):
                    fixedGravityZoneStack--;
                    break;
            }
        }
    }
}