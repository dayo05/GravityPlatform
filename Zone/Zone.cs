using Godot;

namespace GravityPlatform.Zone
{
    public abstract class Zone: Area2D
    {
        public override void _Ready()
        {
            Connect("body_entered", this, nameof(OnEnterZone));
            Connect("body_exited", this, nameof(OnExitZone));
            base._Ready();
        }

        protected abstract string Name { get; }

        private void OnEnterZone(Node2D node)
        {
            if (node is Player.Player p)
                p.EmitSignal("IZone", Name);
        }

        private void OnExitZone(Node2D node)
        {
            if (node is Player.Player p)
                p.EmitSignal("OZone", Name);
        }
    }
}