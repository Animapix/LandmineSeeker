using Microsoft.Xna.Framework.Input;

namespace LandmineSeeker
{
    public interface InputsService
    {
        public bool IsJustPressed(Keys key);
        public bool IsPressed(Keys key);
    }

    public sealed class InputsManager: InputsService
    {
        private KeyboardState _oldState;

        public InputsManager()
        {
            ServiceLocator.RegisterService<InputsManager>(this);
        }

        public void UpdateState()
        {
            _oldState = Keyboard.GetState();
        }

        public bool IsJustPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key) && !_oldState.IsKeyDown(key);
        }

        public bool IsPressed(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
        }
    }
}
