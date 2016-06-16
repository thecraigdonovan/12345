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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _12345.Screens.UIClasses
{
    public class Banner
    {
        protected Vector2 Position, StartPosition, EndPosition, TextPosition, TextOrigin;
        protected float speed = 7f, rotation;
        public float Opacity = 0.8f;
        Texture2D texture;
        SpriteFont font;
        string text ="";
        SpriteEffects spriteEffect;
        public enum MovementStatus
        {
            MovingIn,
            MovingOut,
            In,
            Out
        }

        public MovementStatus CurrentMovementStatus = MovementStatus.In;

        protected enum Direction
        {
            OutIsUp,
            OutIsDown
        }

        Direction CurrentDirection;

        Color colour;

        public Banner(Vector2 _startPosition, Vector2 _endPosition)
        {
            texture = Main.Textures["Banner"];
            font = Main.Fonts["BannerFont"];

            StartPosition = _startPosition;
            EndPosition = _endPosition;
            Position = StartPosition;

            TextPosition = new Vector2(540, 75);

            colour = new Color(156, 196, 255);
            rotation = 0;
            if (StartPosition.Y < EndPosition.Y)
            {
                CurrentDirection = Direction.OutIsDown;
                spriteEffect = SpriteEffects.None;
                TextPosition = new Vector2(540, 75);
            }

            else
            {
                CurrentDirection = Direction.OutIsUp;
                spriteEffect = SpriteEffects.FlipVertically;
                TextPosition = new Vector2(540, 183 - 75);
            }

            Opacity = 0.8f;
        }

        public virtual void MoveOut(string _text = "")
        {
            SetText(_text);
            CurrentMovementStatus = MovementStatus.MovingOut;
        }

        public virtual void MoveIn()
        {
            CurrentMovementStatus = MovementStatus.MovingIn;
        }

        public virtual void SetText(string _text)
        {
            text = _text;
            TextOrigin = font.MeasureString(text)/2;
        }

        public virtual void Update(GameTime gameTime)
        {
            if(CurrentMovementStatus == MovementStatus.In || CurrentMovementStatus == MovementStatus.Out)
            {

            }

            else if(CurrentMovementStatus == MovementStatus.MovingOut)
            {
                if (CurrentDirection == Direction.OutIsDown)
                {
                    Position.Y += speed;
                    if (Position.Y > EndPosition.Y)
                    {
                        Position.Y = EndPosition.Y;
                        CurrentMovementStatus = MovementStatus.Out;
                    }
                }

                else if(CurrentDirection == Direction.OutIsUp)
                {
                    Position.Y -= speed;
                    if (Position.Y < EndPosition.Y)
                    {
                        Position.Y = EndPosition.Y;
                        CurrentMovementStatus = MovementStatus.Out;
                    }
                }
            }

            else if (CurrentMovementStatus == MovementStatus.MovingIn)
            {
                if (CurrentDirection == Direction.OutIsUp)
                {
                    Position.Y += speed;
                    if (Position.Y > StartPosition.Y)
                    {
                        Position.Y = StartPosition.Y;
                        CurrentMovementStatus = MovementStatus.In;
                    }
                }

                else
                {
                    Position.Y -= speed;
                    if (Position.Y < StartPosition.Y)
                    {
                        Position.Y = StartPosition.Y;
                        CurrentMovementStatus = MovementStatus.In;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, colour * Opacity, rotation, Vector2.Zero, 1f, spriteEffect, 0.7f);
            spriteBatch.DrawString(font, text, Position + TextPosition, Color.White * Opacity, 0f, TextOrigin, 1f, SpriteEffects.None, 0.8f);
        }
    }

}