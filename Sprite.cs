using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace LandmineSeeker
{
    public class Sprite
    {
        public Vector2 position = Vector2.Zero;
        public Vector2 pivot = Vector2.Zero;
        public bool flipped = false;
        public float layer = 0;

        public bool hidden = false;

        protected Texture2D texture;
        protected (int horizontal, int vertical) split;
        private int frame = 0;
        private float animationTimer = 0.0f;
        private Dictionary<String, (float frameRate, int[] frames, bool loop)> animations = new Dictionary<string, (float frameRate, int[] frames, bool loop)>();
        private (float frameRate, int[] frames, bool loop) currentAnimation;

        public int width { get => texture.Width / split.horizontal; }
        public int height { get => texture.Height / split.vertical; }

        public bool free = false;

        public Sprite() { }
        public Sprite(Texture2D texture, int hSplit = 1, int vSplit = 1)
        {
            this.texture = texture;
            split = (hSplit, vSplit);
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            if (currentAnimation.frames == null) return;

            if (!currentAnimation.loop && frame == currentAnimation.frames[currentAnimation.frames.Length - 1])
            {
                free = true;
                return;
            }
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            animationTimer += dt * currentAnimation.frameRate;
            if (animationTimer >= currentAnimation.frames.Length) { animationTimer = 0.0f; }
            frame = (int)animationTimer + currentAnimation.frames[0];
        }

        public virtual void Draw()
        {
            if (hidden) return;
            SpriteBatch spriteBatch = ServiceLocator.GetService<SpriteBatch>();
            spriteBatch.Draw(texture, position, GetCurrentFrameRectangle(frame), Color.White, 0, pivot, Vector2.One, flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, layer);

        }

        public void AddAnimation(String name, float frameRate, int[] frames, bool loop = true)
        {
            animations[name] = (frameRate, frames, loop);
        }

        public void SetAnimation(String name)
        {
            currentAnimation = animations[name];
        }

        private Rectangle GetCurrentFrameRectangle(float frame)
        {
            int f = (int)frame;
            float row = f / split.horizontal;
            float column = f - row * split.horizontal;

            double x = column * width;
            double y = row * height;

            return new Rectangle((int)x, (int)y, width, height);
        }
    }
}
