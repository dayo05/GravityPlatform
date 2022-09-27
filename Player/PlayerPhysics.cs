using Godot;
using GravityPlatform.Util;

namespace GravityPlatform.Player
{
    public partial class Player: KinematicBody2D
    {
        private Ticker ptick = new Ticker();
        private ulong wjumpTick = 0;
        private ulong lfloorTick = 0;
        private ulong jumpTask = 0;
        
        private const int MovingBias = 300;
        private float dashDeltaTime = 10.0f;
        private Vector2 dashDirection;

        private bool taskGameOver = false;
        
        public override void _PhysicsProcess(float delta)
        {
            if (taskGameOver)
            {
                taskGameOver = false;
                
                Position = respawnPoint;
                GravityBias = respawnGravity;
                dashDeltaTime = 10;
                LinearVelocity = Vector2.Zero;
                
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
                    if (Input.IsActionPressed("right") && (t = true))
                        LinearVelocity = new Vector2(MovingBias, LinearVelocity.y);
                    if (Input.IsActionPressed("left") && (t = true))
                        LinearVelocity = new Vector2(-MovingBias, LinearVelocity.y);
                    if (!t) LinearVelocity *= Vector2.Down;
                    
                    ResetDash();
                    if (Input.IsActionJustPressed("jump") || ptick.Before(jumpTask, 5))
                    {
                        ApplyJump();
                        lfloorTick = 0;
                    }
                }
                else if (ptick.Before(lfloorTick, 5) && Input.IsActionJustPressed("jump"))
                    ApplyJump();
                else if (CheckOnWall())
                {
                    if (Input.IsActionJustPressed("jump") || ptick.Before(jumpTask, 5))
                    {
                        LinearVelocity = new Vector2(lWall.IsWallDetected ? MovingBias : -MovingBias, LinearVelocity.y);
                        ApplyJump();
                        wjumpTick = ptick.Tick;
                    }
                    else if (Input.IsActionPressed("grab") && ptick.After(wjumpTick, 5))
                    {
                        ResetDash();
                        LinearVelocity = Vector2.Zero;
                    }
                    else ApplyGravity(delta);
                }
                else if (!(Input.IsActionJustPressed("dash") && Dash()))
                    ApplyGravity(delta);
                else if (Input.IsActionJustPressed("jump"))
                    jumpTask = ptick.Tick;
                
                LinearVelocity = MoveAndSlide(LinearVelocity, Vector2.Up);

                if (LinearVelocity.x != 0 && dashDeltaTime != 0)
                    sprite.FlipH = LinearVelocity.x < 0;
            }
            else
            {
                if (Input.IsActionJustPressed("dash"))
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
            if (Input.IsActionPressed("up")) vt += Vector2.Up;
            if (Input.IsActionPressed("right")) vt += Vector2.Right;
            if (Input.IsActionPressed("left")) vt += Vector2.Left;
            if (Input.IsActionPressed("down")) vt += Vector2.Down;
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