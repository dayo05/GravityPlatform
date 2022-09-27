using Godot;

namespace GravityPlatform.Util
{
    public class InputHandler
    {
        private string action;

        public bool IsJustPressed { get; private set; } = false;
        public bool IsJustReleased { get; private set; } = false;
        public bool IsPressed { get; private set; } = false;
        
        public InputHandler(string action)
        {
            this.action = action;
        }

        public void Handle(InputEvent evt)
        {
            if (evt.IsActionPressed(action))
            {
                IsJustPressed = true;
                IsPressed = true;
            }

            if (evt.IsActionReleased(action))
            {
                IsJustReleased = true;
            }
        }

        public void Bump()
        {
            if (IsJustReleased) IsPressed = false;
            IsJustPressed = false;
            IsJustReleased = false;
        }
    } 
}