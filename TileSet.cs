using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LandmineSeeker
{
    public class TileSet
    {
        private Texture2D texture;
        private (int horizontal, int vertical) split;
        public static int tileSize = 16;

        public TileSet(Texture2D texture, int hSplit = 1, int vSplit = 1)
        {
            this.texture = texture;
            split = (hSplit, vSplit);
        }

        public virtual void Draw(int tileNumber, Vector2 position)
        {
            SpriteBatch spriteBatch = ServiceLocator.GetService<SpriteBatch>();
            spriteBatch.Draw(texture, position, GetRectangle(tileNumber), Color.White);

        }

        private Rectangle GetRectangle(int sprite)
        {
            int f = (int)sprite;
            float row = f / split.horizontal;
            float column = f - row * split.horizontal;

            double x = column * tileSize;
            double y = row * tileSize;

            return new Rectangle((int)x, (int)y, tileSize, tileSize);
        }
    }
}
