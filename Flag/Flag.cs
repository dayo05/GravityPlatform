using Godot;

namespace GravityPlatform.Flag
{
    public class Flag : Area2D
    {
        void OnEquipFlag(Node2D node)
        {
            if (!(node is Player.Player)) return;
            Hide();
            QueueFree();
            node.EmitSignal("SavePoint", Position);
        }
    }
}