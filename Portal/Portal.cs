using Godot;

namespace GravityPlatform.Portal
{
    public class Portal : Area2D
    {
        public override void _Ready()
        {
        
        }

        void OnPlayerDetected(Node2D node)
        {
            if (!(node is Player.Player)) return;
            GetTree().ChangeScene("Main/SelectLevel.tscn");
            InternalData.isTutorialCleared = true;
        }
    }
}
