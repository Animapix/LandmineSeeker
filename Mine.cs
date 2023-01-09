using LandmineSeeker;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmineSeeker
{
    public class Mine : Sprite
    {
        public bool detectable = true;
        
        public Mine()
        {
            AssetsService assets = ServiceLocator.GetService<AssetsService>();
            texture = assets.GetAsset<Texture2D>("MineHole");
            split = (1, 1);
            pivot = new Vector2(7, 4);
        }

        public override void Draw()
        {
            base.Draw();
            DrawUtils.Circle(position, 8, 16, Color.Red);
        }

    }
}
