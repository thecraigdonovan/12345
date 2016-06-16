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

namespace _12345.Screens.Menu
{
    public class MenuItem
    {
        protected Texture2D texture;
        protected Vector2 startPosition, currentPosition, leftPosition, rightPosition;

        Vector2 textPosition, measureString;
        public Rectangle Hitbox, textHitbox;

        float speed = 10;
        float textSpeed = 10f;
        public string text = "";
        float scale = 1f;

        public enum MovementState
        {
            MovingIn,
            In,
            MovingOut,
            Out
        }

        public MovementState CurrentMovementState = MovementState.In;

        Color textColor = Color.White;

        public MenuItem(Vector2 _startPosition)
        {
            startPosition = _startPosition;
            currentPosition = startPosition;
            textPosition = currentPosition + new Vector2(60, 60);

            leftPosition = startPosition + new Vector2(-1080, 0);
            rightPosition = startPosition + new Vector2(1080, 0);

            scale = 1 - ((float)Main.Rand.Next(0, 25)/100f);
            speed += (float)Main.Rand.Next(0, 11);
            SetHitbox();
            SetTextHitbox();
        }

        public void SetTextHitbox()
        {
            measureString = Main.Fonts["MenuFont"].MeasureString(text);
            textHitbox = new Rectangle((int)(textPosition.X), (int)(textPosition.Y + (60 * scale)), (int)(measureString.X * scale), (int)((measureString.Y-130) * scale));
        }
        public void Update(GameTime gameTime)
        {
            textColor = Color.White;
            SetHitbox();
            textPosition += new Vector2(-Main.Accel.CurrentValue.Acceleration.X, Main.Accel.CurrentValue.Acceleration.Y) * (textSpeed + (textSpeed * (1-scale)));
            SetTextHitbox();
            TextCollision();

            if(CurrentMovementState == MovementState.MovingOut)
            {
                currentPosition.X -= speed;
                textPosition.X -= speed;
                if(currentPosition.X < leftPosition.X)
                {
                    currentPosition.X = leftPosition.X;
                    textPosition.X = leftPosition.X;
                    CurrentMovementState = MovementState.Out;
                }
            }

            else if(CurrentMovementState == MovementState.MovingIn)
            {
                currentPosition.X += speed;
                textPosition.X += speed;
                if(currentPosition.X > startPosition.X)
                {
                    currentPosition.X = startPosition.X;
                    CurrentMovementState = MovementState.In;
                }
            }
        }


        private void TextCollision()
        {
            if(textHitbox.Right > Hitbox.Right)
            {
                textPosition.X = Hitbox.Right - (measureString.X * scale);
                SetTextHitbox();
            }

            if(textHitbox.Left < Hitbox.Left)
            {
                textPosition.X = Hitbox.X;
                SetTextHitbox();
            }

            if (textHitbox.Bottom > Hitbox.Bottom)
            {
                textPosition.Y -= textHitbox.Bottom - Hitbox.Bottom;
                SetTextHitbox();
            }

            if(textHitbox.Top < Hitbox.Top)
            {
                textPosition.Y += Hitbox.Top - textHitbox.Top;
                SetTextHitbox();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, currentPosition, Color.White);
            spriteBatch.DrawString(Main.Fonts["MenuFont"], text, textPosition, textColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
        }
        public virtual void OnClick()
        {

        }

        public virtual void OnHover()
        {
            textColor = Color.Black;
        }

        public virtual void OnHold()
        {

        }

        private void SetHitbox()
        {
            Hitbox = new Rectangle((int)currentPosition.X, (int)currentPosition.Y, 1080, 384);
        }
    }
}