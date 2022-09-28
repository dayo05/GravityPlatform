using Godot;
using GravityPlatform.Util;

namespace GravityPlatform.ui
{
    public class HUD : Control
    {
        private PopupMenu pauseDialog;
        private InputHandler escape = new InputHandler("escape");
        public override void _Ready()
        {
            GetNode<Button>("Pause").Connect("pressed", this, nameof(OnPause));
            pauseDialog = GetNode<PopupMenu>("PauseDialog");
            GetNode<Button>("PauseDialog/Resume").Connect("pressed", this, nameof(OnResume));
            GetNode<Button>("PauseDialog/Quit").Connect("pressed", this, nameof(OnQuit));
        }

        private void OnPause()
        {
            GetTree().Paused = true;
            pauseDialog.Show();
        }

        private void OnResume()
        {
            GetTree().Paused = false;
            pauseDialog.Hide();
        }

        private void OnQuit()
        {
            GetTree().Paused = false;
            pauseDialog.Hide();
            GetTree().ChangeScene("Main/SelectLevel.tscn");
        }

        public override void _Input(InputEvent evt)
        {
            if (escape.ValidatePress(evt))
            {
                if (GetTree().Paused) OnResume();
                else OnPause();
            }
            base._Input(evt);
        }
    }
}
