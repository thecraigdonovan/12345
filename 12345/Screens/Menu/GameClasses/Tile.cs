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
using _12345.Screens.Tools.Particles;

namespace _12345.Screens.GameClasses
{
    public class Tile
    {
        public enum Animation
        {
            None,
            Spawning,
            Sliding
        }

        Animation CurrentAnimation = Animation.None;
        float scale = 1f, numberScale = 1f;
        public int BaseValue = 1, CurrentValue = 1;
        public Texture2D Texture;

        public Vector2 Position, Center;
        Vector2 position;
        Vector2 Origin = new Vector2(85, 119);
        public Rectangle BigHitbox, SmallHitbox;

        public Color CurrentColour = Color.White, Colour = Color.White;

        public bool Destroyed = false, ToRemove = false;

        public bool Selected = false;

        public Tile(int _value, Vector2 _position, Animation _startAnimation = Animation.None)
        {
            BaseValue = _value;
            CurrentValue = BaseValue;

            if (BaseValue == 1)
            {
                Colour = new Color(248, 228, 117);
            }
            else if (BaseValue == 2)
            {
                Colour = new Color(142, 186, 115);
            }
            else if (BaseValue == 3)
            {
                Colour = new Color(119, 185, 199);
            }
            else if (BaseValue == 4)
            {
                Colour = new Color(177, 143, 191);
            }
            else if (BaseValue == 5)
            {
                Colour = new Color(197, 94, 94);
            }
            else
            {
                Colour = Color.White;
            }
            CurrentColour = Color.Lerp(Colour, new Color(136, 142, 163), GameScreen.TimeRatio - 0.25f);
            Texture = Main.Textures["BlankTile"];

            position = _position;
            Position = _position;

            CurrentAnimation = _startAnimation;
            if (CurrentAnimation == Animation.None)
                SetHitbox();
            else if (CurrentAnimation == Animation.Sliding)
            {
                Position.Y -= 238 + 40;
            }

            else if (CurrentAnimation == Animation.Spawning)
            {
                scale = 0f;
            }
        }

        public void Update(GameTime gameTime)
        {
            CurrentColour = Color.Lerp(Colour, new Color(136, 142, 163), GameScreen.TimeRatio - 0.25f);
            if (!Selected)
            {
                if (TileBoard.Multiplier == 1)
                    CurrentValue = BaseValue;
                else
                    CurrentValue = BaseValue + (5 * (TileBoard.Multiplier - 1));

                if (CurrentValue >= 100)
                    numberScale = 0.75f;
            }
            if (CurrentAnimation == Animation.Spawning)
            {
                scale += 0.09f;

                if (scale >= 1)
                {
                    scale = 1;
                    CurrentAnimation = Animation.None;
                }
            }

            else if (CurrentAnimation == Animation.Sliding)
            {
                Position.Y += 30f;
                if (Position.Y > position.Y)
                {
                    Position.Y = position.Y;
                    CurrentAnimation = Animation.None;
                }
            }

            //Put the none on it's own so it can get a hitbox right away after animation
            if (CurrentAnimation == Animation.None)
            {
                SetHitbox();
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (BaseValue != 0)
            {
                spriteBatch.Draw(Texture, Position + Origin, null, CurrentColour, 0f, Origin, scale, SpriteEffects.None, 0.5f);
                Vector2 fontMeasure = Main.Fonts["TileFont"].MeasureString("" + CurrentValue);
                Vector2 textOrigin = new Vector2(fontMeasure.X / 2, fontMeasure.Y / 2);
                spriteBatch.DrawString(Main.Fonts["TileFont"], CurrentValue.ToString(), Position + new Vector2(85, 135), Color.White, 0f, textOrigin, numberScale * scale, SpriteEffects.None, 1f);
                if (Selected)
                    spriteBatch.Draw(Texture, Position + Origin, null, Color.Black * 0.5f, 0f, Origin, scale, SpriteEffects.None, 0.5f);
            }
        }

        public void SetHitbox()
        {
            Center = Position + Origin;
            BigHitbox = new Rectangle((int)(Position.X), (int)(Position.Y), Texture.Width, Texture.Height);
            SmallHitbox = new Rectangle((int)(Position.X + (Texture.Width / 4)), (int)(Position.Y + (Texture.Height / 4)), Texture.Width / 2, Texture.Height / 2);
        }

        public void Destroy()
        {
            Destroyed = true;
            Main.ParticleManager.Add(new ExplodingTile(this));
        }
    }
}