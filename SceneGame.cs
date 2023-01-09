using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LandmineSeeker
{
    public class SceneGame: Scene
    {
        private Player player;
        private TileMap tileMap;
        private List<Sprite> sprites = new List<Sprite>();

        private List<Soldat> soldiers = new List<Soldat>();
        private List<Mine> mines = new List<Mine>();
        private List<Explosion> explosions = new List<Explosion>();

        private float spawnTimer = 0f;
        private Rectangle minedArea = new Rectangle(16*4,16*4,(60-8)*16,(30-6)*16);
        private Random rnd = new Random();

        private int minesAmount;
        private int soldatAmount;
        private int savedSoldiers;
        private int harvestedMines;

        private float harvestingTimer;
        private bool spawStart;

        private SoundEffect explosionSFX;
        private InputsService inputs;

        private SpriteFont font;
        
        private enum State
        {
            Harvest,
            Soldiers,
            GameOver
        }
        State currentState;

        public override void Load()
        {
            currentState = State.Harvest;
            soldiers = new List<Soldat>();
            mines = new List<Mine>();
            explosions = new List<Explosion>();
            savedSoldiers = 0;
            harvestedMines = 0;
            minesAmount = 20;
            soldatAmount = 20;
            harvestingTimer = 90;
            spawStart = false;

            inputs = ServiceLocator.GetService<InputsService>();

            AssetsService assets = ServiceLocator.GetService<AssetsService>();
            explosionSFX = assets.GetAsset<SoundEffect>("Explosion_Fast");
            font = assets.GetAsset<SpriteFont>("defaultFont");

            player = new Player();
            player.position = new Vector2(Main.canvas.Width / 2, Main.canvas.Height/2);
            player.HarvestMine += onHarvestMine;

            tileMap = new TileMap();
            
            //Spawm mines
            for (int i = 0; i < minesAmount; i++)
            {
                Mine mine = new Mine();
                mine.position = new Vector2(rnd.Next(minedArea.Width), rnd.Next(minedArea.Height)) + new Vector2(minedArea.X, minedArea.Y); 
                mine.hidden = true;
                mines.Add(mine);
            }
            base.Load();
        }

        public void onHarvestMine(Mine mine)
        {
            harvestedMines ++;
            minesAmount--;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(currentState == State.Harvest)
            {
                harvestingTimer -= dt;
                if (harvestingTimer <= 0) currentState = State.Soldiers;
            }
            else if(currentState == State.Soldiers)
            {
                // Spawn soldiers
                if (soldatAmount > 0)
                {
                    spawnTimer -= dt;

                    if (spawnTimer <= 0)
                    {
                        Soldat soldat = new Soldat();
                        soldat.position = new Vector2(0, rnd.Next(minedArea.Height)) + new Vector2(16, minedArea.Y);
                        soldiers.Add(soldat);
                        spawnTimer = 1f;
                        soldatAmount -= 1;

                        spawStart = true;
                    }
                }
            }
            else if (currentState == State.GameOver)
            {
                if (inputs.IsJustPressed(Keys.Space))
                {
                    ServiceLocator.GetService<ScenesService>().Load("Menu");
                }
                return;
            }


            List<Soldat> removableSoldats = new List<Soldat>();
            List<Mine> removableMines = new List<Mine>();

            foreach (Soldat soldat in soldiers)
            {
                foreach (Mine mine in mines)
                {
                    if (soldat.CollideWithMine(mine)){
                        removableMines.Add(mine);
                        removableSoldats.Add(soldat);
                        var explosion = new Explosion();
                        explosion.position = mine.position;
                        explosions.Add(explosion);
                        explosionSFX.Play();
                        minesAmount--;
                    }
                }

                if (soldat.position.X - 100 >= minedArea.Width + minedArea.X) 
                {
                    removableSoldats.Add(soldat);
                    savedSoldiers++;
                }
            }

            foreach (var mine in removableMines) mines.Remove(mine);
            foreach (var soldat in removableSoldats) soldiers.Remove(soldat);

            
            if(soldiers.Count == 0 && harvestingTimer <= 0 && spawStart)
                currentState = State.GameOver;
   

            player.DetectMines(mines);
            player.Update(gameTime);

            foreach (var soldat in soldiers)
                soldat.Update(gameTime);

            List<Explosion> removableExplosion = new List<Explosion>();
            foreach (var explosion in explosions)
            {
                explosion.Update(gameTime);
                if (explosion.free) removableExplosion.Add(explosion);
            }

            foreach (var explosion in removableExplosion) explosions.Remove(explosion);

            base.Update(gameTime);
        }

        public override void Draw()
        {
            sprites.Clear();
            sprites.Add(player);
            sprites.AddRange(soldiers);

            tileMap.Draw();

            foreach(var mine in mines)
                mine.Draw();

            sprites.Sort((a, b) => a.position.Y.CompareTo(b.position.Y));
            foreach (var sprite in sprites)
                sprite.Draw();

            foreach (var explosion in explosions)
                explosion.Draw();

            DrawUtils.Rectangle(new Vector2(minedArea.X, minedArea.Y), minedArea.Width,minedArea.Height,Color.Red);
            
            SpriteBatch spriteBatch = ServiceLocator.GetService<SpriteBatch>();
            spriteBatch.DrawString(font, minesAmount + " MINES" , new Vector2(20, -8), Color.White);
            spriteBatch.DrawString(font, (int)harvestingTimer + " S BEFORE SPAWN", new Vector2(200, -8), Color.White);
            spriteBatch.DrawString(font, "PRESS SPACE TO HARVEST MINE", new Vector2(500, -8), Color.White);

            if (currentState == State.GameOver)
            {
                DrawUtils.Text(font, "GAMEOVER ", new Rectangle(0, 100, Main.canvas.Width, 80), DrawUtils.Alignment.Center, Color.White);
                DrawUtils.Text(font, "HARVESTED MINES " + harvestedMines, new Rectangle(0, 180, Main.canvas.Width, 80), DrawUtils.Alignment.Center, Color.White);
                DrawUtils.Text(font, "SAVED SOLDIERS " + savedSoldiers, new Rectangle(0, 210, Main.canvas.Width, 80), DrawUtils.Alignment.Center, Color.White);
                DrawUtils.Text(font, "SCORE " + savedSoldiers * savedSoldiers * 100, new Rectangle(0, 240, Main.canvas.Width, 80), DrawUtils.Alignment.Center, Color.White);

                DrawUtils.Text(font, "MENU SPACE", new Rectangle(0, 320, Main.canvas.Width, 80), DrawUtils.Alignment.Center, Color.White);
            }

            base.Draw();
        }
    }
}
