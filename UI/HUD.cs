using Godot;
using GravityPlatform.Util;

namespace GravityPlatform.UI
{
    public class HUD : Control
    {
        private PopupMenu pauseDialog;
        private InputHandler escape = new InputHandler("escape");
        private Player.Player player;
        public override void _Ready()
        {
            GetNode<Button>("Pause").Connect("pressed", this, nameof(OnPauseClicked));
            pauseDialog = GetNode<PopupMenu>("PauseDialog");
            GetNode<Button>("PauseDialog/Resume").Connect("pressed", this, nameof(OnResumeClicked));
            GetNode<Button>("PauseDialog/Quit").Connect("pressed", this, nameof(OnQuitClicked));
        }

        [Signal]
        delegate void SetPause(bool newState);

        void SSetPause(bool newState)
        {
            GetTree().Paused = newState;
            if (newState) pauseDialog.Show();
            else pauseDialog.Hide();
        }

        private void OnPauseClicked()
            => EmitSignal("SetPause", true);

        private void OnResumeClicked()
            => EmitSignal("SetPause", false);

        private void OnQuitClicked()
        {
            EmitSignal("SetPause", false);
            GetTree().ChangeScene("Main/SelectLevel.tscn");
        }

        public override void _Input(InputEvent evt)
        {
            if (escape.ValidatePress(evt))
            {
                if (GetTree().Paused) OnResumeClicked();
                else OnPauseClicked();
            }
            base._Input(evt);
        }
    }
}
