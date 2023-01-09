using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandmineSeeker
{
    public class SceneMenu: Scene
    {
        private string title = "LANDMINE SEEKER";
        private TileMap tileMap;
        private List<Soldat> soldiers;

        private float spawnTimer = 0f;
        private Rectangle minedArea = new Rectangle(16 * 4, 16 * 4, (60 - 8) * 16, (30 - 6) * 16);
        private Random rnd = new Random();

        private InputsService inputs;
        private SpriteFont font;
        public SceneMenu()
        {
            AssetsService asset = ServiceLocator.GetService<AssetsService>();

            font = asset.GetAsset<SpriteFont>("defaultFont");

            inputs = ServiceLocator.GetService<InputsService>();
            tileMap = new TileMap();
            soldiers = new List<Soldat>();
        }


        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (inputs.IsJustPressed(Keys.Space)){
                ServiceLocator.GetService<ScenesService>().Load("Game");
            }

            spawnTimer -= dt;

            if (spawnTimer <= 0)
            {
                Soldat soldat = new Soldat();
                soldat.position = new Vector2(0, rnd.Next(minedArea.Height)) + new Vector2(16, minedArea.Y);
                soldiers.Add(soldat);
                spawnTimer = 1f;
            }


            List<Soldat> removableSoldats = new List<Soldat>();
            foreach (Soldat soldat in soldiers)
            {
                if (soldat.position.X - 50 >= minedArea.Width + minedArea.X)
                {
                    removableSoldats.Add(soldat);
                }
            }
            foreach (var soldat in removableSoldats) soldiers.Remove(soldat);


            foreach (Soldat s in soldiers) { s.Update(gameTime); }
            base.Update(gameTime);
        }


        public override void Draw()
        {
            tileMap.Draw();
            SpriteBatch spriteBatch = ServiceLocator.GetService<SpriteBatch>();
            
            foreach(Soldat s in soldiers) { s.Draw(); }

            DrawUtils.Text(font, title, new Rectangle(0,100,Main.canvas.Width, 40), DrawUtils.Alignment.Center, Color.White);
            DrawUtils.Text(font, "Start Game  Space", new Rectangle(0,140,Main.canvas.Width, 40), DrawUtils.Alignment.Center, Color.White);

            base.Draw();
        }
    }
}
