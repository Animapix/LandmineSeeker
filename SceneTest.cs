using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmineSeeker
{
    public class SceneTest:Scene
    {
        Sprite explosion;
        public override void Load()
        {
            AssetsService assets = ServiceLocator.GetService<AssetsService>();
            explosion = new Sprite(assets.GetAsset<Texture2D>("explosion"), 10);
            explosion.AddAnimation("Run", 30, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, false);
            explosion.SetAnimation("Run");
            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            explosion.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw()
        {
            explosion.Draw();
            base.Draw();
        }
    }
}
