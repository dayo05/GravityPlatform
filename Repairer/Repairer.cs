using Godot;

namespace GravityPlatform.Repairer
{
    public class Repairer : Area2D
    {
        private float elipedtime = 10;
        public override void _Ready()
        {
            GetParent().GetNode<Player.Player>("Player").Connect("GameOver", this, "OnGameOver");
        }

        public override void _Process(float delta)
        {
            elipedtime += delta;
            if (elipedtime > 5.0f)
                Show();
            base._Process(delta);
        }

        void OnEquip(Node2D node)
        {
            if (!(node is Player.Player)) return;
            node.EmitSignal("AddDash");
            Hide();
            elipedtime = 0;
        }

        void OnGameOver()
        {
            elipedtime = 10;
        }
    }
}
