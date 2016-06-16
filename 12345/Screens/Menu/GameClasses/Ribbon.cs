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

namespace _12345.Screens.GameClasses
{
    class Ribbon
    {
        public Vector2 StartPosition, EndPosition;
        Vector2 origin = new Vector2(0, 37.5f);
        public Color Colour = Color.Blue;
        float rotation;
        Rectangle sourceRectangle;
        Texture2D texture;
        float ribbonWidth;
        public float Distance;
        Tile StartTile;
        public Ribbon(Tile _startTile)
        {
            StartTile = _startTile;
            StartPosition = StartTile.Center;
            EndPosition = StartPosition;
            texture = Main.Textures["Ribbon"];
            ribbonWidth = texture.Width;
            Colour = StartTile.Colour;
        }

        public void SetEndPosition(Vector2 _endPosition, TileBoard _tb)
        {
            EndPosition = _endPosition;
            rotation = Tools.Extensions.GetAngle(StartPosition, EndPosition);
            Distance = Vector2.Distance(StartPosition, EndPosition);
            if (Distance > ribbonWidth)
                Distance = ribbonWidth;
            sourceRectangle = new Rectangle(0, 0, (int)Distance, texture.Height);
            EndPosition = StartPosition + Vector2.Normalize(new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation))) * Distance;
            Tile closestTile = null;
            float closestDistance = 10000000;
            foreach (Tile t in _tb.TileGrid)
            {
                if (t != StartTile)
                {
                    if (Vector2.Distance(EndPosition, t.Center) < closestDistance)
                    {
                        closestDistance = Vector2.Distance(EndPosition, t.Center);
                        closestTile = t;
                        if (closestTile.SmallHitbox.Contains(EndPosition) && !closestTile.Selected && closestTile.CurrentValue == StartTile.CurrentValue + 1)
                        {
                            closestTile.Selected = true;
                            StartTile.Selected = true;

                            EndPosition = closestTile.Center;
                            rotation = Tools.Extensions.GetAngle(StartPosition, EndPosition);
                            Distance = Vector2.Distance(StartPosition, EndPosition);
                            if (Distance > ribbonWidth)
                                Distance = ribbonWidth;
                            sourceRectangle = new Rectangle(0, 0, (int)Distance, texture.Height);
                            EndPosition = StartPosition + Vector2.Normalize(new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation))) * Distance;

                            _tb.AddRibbon(this);
                            _tb.CurrentRibbon = new Ribbon(closestTile);
                        }
                    }
                }
            }

            float ratio = 1 - (closestDistance / ribbonWidth);

            Colour = Color.Lerp(StartTile.Colour, closestTile.CurrentColour, ratio);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, StartPosition, sourceRectangle, Colour, rotation, origin, 1f, SpriteEffects.None, 1f);
        }
    }
}