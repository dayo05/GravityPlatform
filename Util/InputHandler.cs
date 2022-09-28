using Godot;

namespace GravityPlatform.Util
{
    public class InputHandler
    {
        private string action;
        public bool IsJustPressed => Input.IsActionJustPressed(action);
        public bool IsJustReleased => Input.IsActionJustReleased(action);
        public bool IsPressed => Input.IsActionPressed(action);
        public bool ValidatePress(InputEvent e) => e.IsActionPressed(action);
        public bool ValidateRelease(InputEvent e) => e.IsActionReleased(action);
        
        public InputHandler(string action)
        {
            this.action = action;
        }
    } 
}