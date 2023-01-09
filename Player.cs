using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LandmineSeeker
{
    public class Player : Sprite
    {
        private Sprite detector;
        private Mine detectedMine = null;
        private float detectionRadius = 100;
        private SoundEffect detectorSoundFX;
        private float harvestingRadius = 12;
        private float detectorSoundTimer = 0f;
        private bool canHarvest = false;
        private Vector2 centerOffset = new Vector2(0, 4);
        private Sprite warning;

        public System.Action<Mine> HarvestMine;

        public Player()
        {
            AssetsService assets = ServiceLocator.GetService<AssetsService>();
            texture = assets.GetAsset<Texture2D>("Player");
            split = (16, 3);
            AddAnimation("Idle", 2, new int[] { 32, 33 });
            AddAnimation("Run", 24, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
            AddAnimation("RunE", 24, new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 });       
            SetAnimation("Idle");
            pivot = new Vector2(16, 31);

            detector = new Sprite(assets.GetAsset<Texture2D>("Detector"));
            detectorSoundFX = assets.GetAsset<SoundEffect>("beep-sound");

            warning = new Sprite(assets.GetAsset<Texture2D>("Warning"));
            warning.hidden = true;
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            InputsManager inputs = ServiceLocator.GetService<InputsManager>();

            Vector2 direction = Vector2.Zero;
            if (inputs.IsPressed(Keys.Right)) direction += new Vector2(1, 0);
            else if (inputs.IsPressed(Keys.Left)) direction -= new Vector2(1, 0);
            if (inputs.IsPressed(Keys.Down)) direction += new Vector2(0, 1);
            else if (inputs.IsPressed(Keys.Up)) direction -= new Vector2(0, 1);
            if (direction != Vector2.Zero) direction.Normalize();
            position += direction * 120 * dt;

            if (direction == Vector2.Zero) SetAnimation("Idle");
            else SetAnimation("RunE");

            if (direction.X < 0 && !flipped) flipped = true;
            else if (direction.X > 0 && flipped) flipped = false;

            detector.position = position - new Vector2(16,25);
            detector.flipped = flipped;

            // Detection
            if (detectedMine != null)
            {
                var distance = Vector2.Distance(position - centerOffset, detectedMine.position);
                if (detectorSoundTimer <= 0)
                {
                    detectorSoundFX.Play();
                    detectorSoundTimer = distance * 0.005f;
                }
                if (distance < harvestingRadius) { 
                    canHarvest = true;
                    warning.position = position - new Vector2(5,45);
                    warning.hidden = false;
                }
                else {
                    warning.hidden = true;
                    canHarvest = false; 
                }

                if (inputs.IsJustPressed(Keys.Space) && canHarvest)
                {
                    
                    detectedMine.hidden = false;
                    detectedMine.detectable = false;
                    if (HarvestMine != null)
                    {
                        HarvestMine(detectedMine);
                        warning.hidden = true;
                    }
                }

            }
            if (detectorSoundTimer > 0) detectorSoundTimer -= dt;

            base.Update(gameTime);
        }


        public void DetectMines(List<Mine> mines)
        {
            float minDist = float.MaxValue;
            foreach (Mine item in mines)
            {
                var distance = Vector2.Distance(position - centerOffset, item.position);
                if (distance < minDist && item.detectable)
                {
                    detectedMine = item;
                    minDist = distance;
                }
            }

            if (minDist > detectionRadius) detectedMine = null;
        }

        public override void Draw()
        {
            base.Draw();
            detector.Draw();
            warning.Draw();
            DrawUtils.Circle(position - centerOffset, detectionRadius, 64, Color.Green);
            DrawUtils.Circle(position - centerOffset, harvestingRadius, 16, Color.Green);
        }
    }
}
