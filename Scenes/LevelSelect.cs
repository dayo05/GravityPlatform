using Godot;

namespace GravityPlatform.Scenes
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
            GetTree().ChangeScene("Scenes/Maps/Tutorial/Main.tscn");
        }

        void SMap1Button()
        {
            GetTree().ChangeScene("Scenes/Maps/Map1/M1.tscn");
        }
    }
}
