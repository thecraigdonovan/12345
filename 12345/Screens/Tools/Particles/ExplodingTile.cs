using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using _12345.Screens.GameClasses;

namespace _12345.Screens.Tools.Particles
{
    public class ExplodingTile : Particle
    {
        Texture2D texture;
        Vector2 startPosition, topRightPos, topLeftPos, bottomRightPos, bottomLeftPos, pieceOrigin, pieceVelocity;
        float speed = 8.5f, gravity = 0f, opacity = 1, opacityDegreation = 0.0775f, drag = 0.125f, rotation = 0f, rotationSpeed = 0.3f;
        Color colour;

        public ExplodingTile(Tile t) : base()
        {
            texture = t.Texture;
            colour = t.CurrentColour;
            startPosition = t.Position + new Vector2(texture.Width, texture.Height) / 2;
            float width = texture.Width / 4;
            float height = texture.Height / 4;
            pieceOrigin = new Vector2(width, height);

            topLeftPos = startPosition + new Vector2(-width, -height);
            topRightPos = startPosition + new Vector2(width, -height);
            bottomRightPos = startPosition + new Vector2(width, height);
            bottomLeftPos = startPosition + new Vector2(-width, height);
            pieceVelocity = new Vector2(speed, speed);
            rotation = (float)(Main.Rand.NextDouble() * Math.PI);
        }

        public override void Update(GameTime gameTime)
        {
            topLeftPos += pieceVelocity * new Vector2(-1, -1);
            topRightPos += pieceVelocity * new Vector2(1, -1);
            bottomRightPos += pieceVelocity * new Vector2(1, 1);
            bottomLeftPos += pieceVelocity * new Vector2(-1, 1);
            pieceVelocity.Y += gravity;
            pieceVelocity.X -= drag;
            if (pieceVelocity.X < 0)
                pieceVelocity.X = 0;

            opacity -= opacityDegreation;
            if (opacity <= 0)
            {
                opacity = 0;
                Finished = true;
            }

            rotation += rotationSpeed;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, topLeftPos, new Rectangle(0, 0, texture.Width / 2, texture.Height / 2), colour * opacity, -rotation, pieceOrigin, 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(texture, topRightPos, new Rectangle(texture.Width / 2, 0, texture.Width / 2, texture.Height / 2), colour * opacity, rotation, pieceOrigin, 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(texture, bottomLeftPos, new Rectangle(0, texture.Height / 2, texture.Width / 2, texture.Height / 2), colour * opacity, -rotation, pieceOrigin, 1f, SpriteEffects.None, 1f);
            spriteBatch.Draw(texture, bottomRightPos, new Rectangle(texture.Width / 2, texture.Height / 2, texture.Width / 2, texture.Height / 2), colour * opacity, rotation, pieceOrigin, 1f, SpriteEffects.None, 1f);


            base.Draw(spriteBatch);
        }
    }
}