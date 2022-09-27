using Godot;

namespace GravityPlatform.Main
{
    public class LevelSelect : Node
    {
        public override void _Ready()
        {
            GD.Print(InternalData.isTutorialCleared);
            base._Ready();
        }

        void STutorialButton()
        {
            GetTree().ChangeScene("Tutorial/Main.tscn");
        }
    }
}
