using Godot;
using GravityPlatform.Util;

namespace GravityPlatform.Player
{
    public partial class Player: KinematicBody2D
    {
        private Ticker ptick = new Ticker();
        private ulong? wjumpTick = null;
        private ulong? lfloorTick = null;
        private ulong? jumpTask = null;
        
        private const int MovingBias = 300;
        private float dashDeltaTime = 10.0f;
        private Vector2 dashDirection;

        private bool taskGameOver = false;
        
        private WallDetector lWall;
        private WallDetector rWall;
        private WallDetector celling;
        private WallDetector floor;
        
        public override void _PhysicsProcess(float delta)
        {
            if (taskGameOver)
            {
                taskGameOver = false;
                
                Position = respawnPoint;
                GravityBias = respawnGravity;
                dashDeltaTime = 10;
                LinearVelocity = Vector2.Zero;
                lfloorTick = null;
                wjumpTick = null;
                jumpTask = null;
                
                ptick.Reset();
                return;
            }
            ptick.NextTick();
            var t = false;
            dashDeltaTime += delta;

            if (dashDeltaTime > 0.5f)
            {
                Animation = "default";
                sprite.Rotation = 0;
                sprite.FlipV = false;
                if (CheckOnFloor())
                {
                    lfloorTick = ptick.Tick;
                    if (right.IsPressed && (t = true))
                        LinearVelocity = new Vector2(MovingBias, LinearVelocity.y);
                    if (left.IsPressed && (t = true))
                        LinearVelocity = new Vector2(-MovingBias, LinearVelocity.y);
                    if (!t) LinearVelocity *= Vector2.Down;
                    
                    ResetDash();
                    if (jump.IsJustPressed || ptick.Before(jumpTask, 10))
                    {
                        ApplyJump();
                        lfloorTick = null;
                        jumpTask = null;
                    }
                }
                else if (ptick.Before(lfloorTick, 10) && jump.IsJustPressed)
                    ApplyJump();
                else if (CheckOnWall())
                {
                    if (jump.IsJustPressed || ptick.Before(jumpTask, 15))
                    {
                        LinearVelocity = new Vector2(lWall.IsWallDetected ? MovingBias : -MovingBias, LinearVelocity.y);
                        ApplyJump();
                        ResetDash();
                        wjumpTick = ptick.Tick;
                        jumpTask = null;
                    }
                    else if (grab.IsPressed && ptick.After(wjumpTick, 10))
                    {
                        ResetDash();
                        LinearVelocity = Vector2.Zero;
                        wjumpTick = null;
                    }
                    else ApplyGravity(delta);
                }
                else if (dash.IsJustPressed && Dash())
                { /* Dash! */ }
                else if (jump.IsJustPressed)
                {
                    jumpTask = ptick.Tick;
                    ApplyGravity(delta);
                }
                else ApplyGravity(delta);

                LinearVelocity = MoveAndSlide(LinearVelocity, Vector2.Up);

                if (LinearVelocity.x != 0 && dashDeltaTime != 0)
                    sprite.FlipH = LinearVelocity.x < 0;
            }
            else
            {
                if (dash.IsJustPressed)
                    Dash();
                MoveAndSlide(dashDirection.Normalized() * 8000 * delta * (2.5f + (0.6f - dashDeltaTime)), Vector2.Up);
            }
        }
        
        private bool IsReversedGravity() => GravityBias < 0;
        private bool CheckOnFloor() => !IsReversedGravity() ? floor.IsWallDetected : celling.IsWallDetected;
        private bool CheckOnWall() => lWall.IsWallDetected || rWall.IsWallDetected;
        private void ApplyJump(float bias = 1.0f) => LinearVelocity.y = 500 * bias * (IsReversedGravity() ? 1 : -1);
        private void ApplyGravity(float delta) => LinearVelocity += Vector2.Down * 950 * delta * GravityBias;
        
        private bool Dash()
        {
            if (dashCount <= 0) return false;
            var vt = Vector2.Zero;
            if (up.IsPressed) vt += Vector2.Up;
            if (right.IsPressed) vt += Vector2.Right;
            if (left.IsPressed) vt += Vector2.Left;
            if (down.IsPressed) vt += Vector2.Down;
            if (vt != Vector2.Zero)
            {
                dashCount--;
                dashDeltaTime = 0;
                LinearVelocity.y = 0;

                if (LinearVelocity.x != 0 && vt.x == 0)
                    vt.x = (LinearVelocity.x > 0 ? 1 : -1) * 0.2f;
                
                sprite.Rotation = vt.Angle();
                dashDirection = vt.Normalized();

                Animation = "dash";
                if (dashDirection.x < 0)
                    sprite.FlipV = true;
                sprite.FlipH = false;

                if (dashDirection.x * LinearVelocity.x < 0) LinearVelocity.x *= -1;
                if (LinearVelocity.x == 0 && dashDirection.x != 0) LinearVelocity.x = MovingBias * (dashDirection.x > 0 ? 1 : -1);
            }

            return true;
        }
    }
}