using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LandmineSeeker
{
    public class Soldat: Sprite
    {
        private enum State
        {
            running,
            idle,
            shooting
        }
        private Random random = new Random();
        private Vector2 centerOffset = new Vector2(0, 4);

        private float timer = 5f;
        private State currentState = State.idle;

        public Soldat()
        {
            AssetsManager assetsManager = ServiceLocator.GetService<AssetsManager>();
            texture = assetsManager.GetAsset<Texture2D>("Infantry");
            split = (8, 7);
            AddAnimation("Idle", 10, new int[] { 0, 1, 2, 3 });
            AddAnimation("Run", 12, new int[] { 8, 9, 10, 11, 12, 13, 14, 15 });
            AddAnimation("Fire", 10, new int[] { 16, 18, 19, 20 });
            SetAnimation("Idle");
            pivot = new Vector2(30, 32);
        }


        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(timer <= 0)
            {
                Array values = Enum.GetValues(typeof(State));
                currentState = (State)values.GetValue(random.Next(values.Length));
                timer = random.Next(3);
            }
            else
            {
                timer -= dt;
            }

            if(currentState == State.running) {
                SetAnimation("Run");
                position += new Vector2(1, 0) * dt * 120;
                base.Update(gameTime);
            }
            else if (currentState == State.shooting) { SetAnimation("Fire"); }
            else if (currentState == State.idle) { SetAnimation("Idle"); }

            base.Update(gameTime);

        }

        public bool CollideWithMine(Mine mine)
        {
            var dx = mine.position.X - position.X + centerOffset.X;
            var dy = mine.position.Y - position.Y + centerOffset.Y;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance < 8 + 8 && mine.detectable) return true;
            return false;
        }

        public override void Draw()
        {
            base.Draw();
            DrawUtils.Circle(position - centerOffset, 8, 64, Color.Green);
        }
    }
}
