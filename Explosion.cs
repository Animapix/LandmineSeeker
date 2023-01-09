using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmineSeeker
{
    public class Explosion:Sprite
    {
        public Explosion()
        {
            AssetsService assets = ServiceLocator.GetService<AssetsService>();
            texture = assets.GetAsset<Texture2D>("explosion");
            split = (10, 1);
            AddAnimation("Run", 30, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, false);
            SetAnimation("Run");
            pivot = new Vector2(64,64);
        }
    }
}
