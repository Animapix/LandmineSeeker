using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace LandmineSeeker
{
    public class TileMap
    {
        public Vector2 position;
        public int columns = Main.canvas.Width/16;
        public int rows = Main.canvas.Height / 16;
        private Random rnd;
        private TileSet tileSet;

        private int[,] tiles;

        public TileMap()
        {
            rnd = new Random();
            AssetsService assets = ServiceLocator.GetService<AssetsService>();
            tileSet = new TileSet(assets.GetAsset<Texture2D>("TileSet"), 9,2);

            tiles = new int[columns, rows];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < columns; c++)
                    tiles[c,r] = rnd.Next(8);

            for (int c = 0; c < columns; c++)
            {
                tiles[c, 2] = 9;
                tiles[c, rows-1] = 10;
            }

            for (int r = 3; r < rows-1; r++)
            {
                tiles[0, r] = 8;
                tiles[1, r] = 8;
                tiles[2, r] = 8;
                tiles[columns-1, r] = 8;
                tiles[columns-2, r] = 8;
                tiles[columns-3, r] = 8;
            }
        }

        public void Draw()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Vector2 pos = new Vector2(c * TileSet.tileSize, r * TileSet.tileSize) - position;
                    tileSet.Draw(tiles[c, r], pos);
                }
            }
        }
    }
}
