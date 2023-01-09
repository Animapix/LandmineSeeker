using Microsoft.Xna.Framework;

namespace LandmineSeeker
{
    public abstract class Scene
    {
        public virtual void Load(){}

        public virtual void Update(GameTime gameTime){}

        public virtual void Draw(){}

        public virtual void Unload(){}
    }
}
