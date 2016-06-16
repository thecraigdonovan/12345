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

namespace _12345.Screens.GameClasses
{
    class TileBoard
    {
        public Tile[,] TileGrid;
        Vector2 startPos = new Vector2(141, 340);
        Vector2 tileWidth = new Vector2(170, 238);
        Vector2 spacing = new Vector2(40, 40);
        public List<Ribbon> Ribbons = new List<Ribbon>();
        public Ribbon CurrentRibbon;
        public static int Multiplier = 1;
        public int countToMultiply = 0;

        int moveScore = 0, selectedCount = 0;

        List<int> AvailableTiles;

        public bool TakeInput = true;
        public TileBoard()
        {
            Reset();
        }
        
        public void Reset()
        {
            SetAvailableTiles();

            TileGrid = new Tile[4, 5];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    TileGrid[i, j] = new Tile(GetAvailableTile(), startPos + (new Vector2(i, j) * tileWidth) + (new Vector2(i, j) * spacing), Tile.Animation.Spawning);
                }
            }

            Multiplier = 1;
            countToMultiply = 0;
            moveScore = 0;
            selectedCount = 0;

            TakeInput = true;
        }
        public void Update(GameTime gameTime)
        {
            UpdateTiles(gameTime);
            if (TakeInput)
                UpdateRibbons(gameTime);
        }

        private void UpdateRibbons(GameTime gameTime)
        {
            if (Main.CurrentTouchCollection.Count > 0)
            {
                if (CurrentRibbon == null)
                {
                    foreach (Tile t in TileGrid)
                    {
                        if (t.BaseValue != 0)
                        {
                            if (t.BigHitbox.Contains(new Point((int)((Main.CurrentTouchCollection[0].Position.X + (GameScreen.Camera.Pos.X - 540)) * Main.xScale), (int)((Main.CurrentTouchCollection[0].Position.Y + (GameScreen.Camera.Pos.Y - 960)) * Main.yScale))))
                            {
                                CurrentRibbon = new Ribbon(t);
                            }
                        }
                    }

                }

                else
                {
                    CurrentRibbon.SetEndPosition((Main.CurrentTouchCollection[0].Position + (GameScreen.Camera.Pos - new Vector2(540, 960)))* new Vector2(Main.xScale, Main.yScale), this);
                }
            }

            else
            {
                if (Main.LastTouchCollection.Count > 0)
                {
                    moveScore = 0;
                    selectedCount = 0;
                    foreach (Tile t in TileGrid)
                    {
                        if (t.Selected)
                        {
                            moveScore += t.CurrentValue;
                            selectedCount++;
                            t.Destroy();
                        }
                    }

                    CurrentRibbon = null;
                    Ribbons.Clear();
                    countToMultiply = 0;
                    GameScreen.CurrentScore += (moveScore * selectedCount);
                    if (selectedCount >= 5 && GameScreen.CurrentState == GameScreen.GameState.GamePlay)
                        GameScreen.AddTime(selectedCount/2);
                }
            }
        }

        private void UpdateTiles(GameTime gameTime)
        {
            foreach (Tile t in TileGrid)
                t.Update(gameTime);


            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (TileGrid[i, j].Destroyed)
                    {
                        AddToAvailableTiles(TileGrid[i, j].BaseValue);
                        TileGrid[i, j].Destroyed = false;
                        TileGrid[i, j].ToRemove = true;
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (TileGrid[i, j].ToRemove)
                    {
                        if (j == 0)
                        {
                            if(GameScreen.CurrentState == GameScreen.GameState.GamePlay)
                                TileGrid[i, j] = new Tile(GetAvailableTile(), startPos + (new Vector2(i, j) * tileWidth) + (new Vector2(i, j) * spacing), Tile.Animation.Spawning);
                            else
                                TileGrid[i, j] = new Tile(0, startPos + (new Vector2(i, j) * tileWidth) + (new Vector2(i, j) * spacing), Tile.Animation.Spawning);
                        }

                        else
                        {
                            TileGrid[i, j] = new Tile(TileGrid[i,j-1].BaseValue, startPos + (new Vector2(i, j) * tileWidth) + (new Vector2(i, j) * spacing), Tile.Animation.Sliding);
                            TileGrid[i, j - 1].ToRemove = true;
                        }
                    }
                }
            }
        }

        public void AddRibbon(Ribbon r)
        {
            Ribbons.Add(r);
            countToMultiply++;
            if (countToMultiply == 4)
            {
                Multiplier += 1;
                countToMultiply = -1;
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Main.Textures["TileBoard"], new Vector2(0, 340 - 285), Color.White);
            spriteBatch.Draw(Main.Textures["TileSlots"], new Vector2(0, 340 - 285), Color.White);

            foreach (Tile t in TileGrid)
                t.Draw(spriteBatch);

            foreach (Ribbon r in Ribbons)
                r.Draw(spriteBatch);
            if (CurrentRibbon != null)
                CurrentRibbon.Draw(spriteBatch);
        }

        private void SetAvailableTiles()
        {
            AvailableTiles = new List<int>();
            for(int i = 1; i <= 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    AvailableTiles.Add(i);
                }
            }
        }

        private int GetAvailableTile()
        {
            int placeToAdd = Main.Rand.Next(0, AvailableTiles.Count);
            int toReturn = AvailableTiles[placeToAdd];
            AvailableTiles.RemoveAt(placeToAdd);
            return toReturn;
        }

        public void AddToAvailableTiles(int i)
        {
            AvailableTiles.Add(i);
        }

        public bool MovesRemaining()
        {
            bool movesRemainig = false;

            for(int i = 0; i < 4; i++)
            {
                if (movesRemainig == true)
                    break;

                for (int j = 0; j < 5; j++)
                {
                    if (TileGrid[i, j].BaseValue != 0)
                    {
                        if (i != 0)
                        {
                            if (TileGrid[i - 1, j].BaseValue == TileGrid[i, j].BaseValue + 1)
                            {
                                movesRemainig = true;
                                break;
                            }
                        }

                        if(i != 3)
                        {
                            if (TileGrid[i + 1, j].BaseValue == TileGrid[i, j].BaseValue + 1)
                            {
                                movesRemainig = true;
                                break;
                            }
                        }

                        if(j != 0)
                        {
                            if (TileGrid[i, j-1].BaseValue == TileGrid[i, j].BaseValue + 1)
                            {
                                movesRemainig = true;
                                break;
                            }
                        }

                        if (j != 4)
                        {
                            if (TileGrid[i, j + 1].BaseValue == TileGrid[i, j].BaseValue + 1)
                            {
                                movesRemainig = true;
                                break;
                            }
                        }

                        if (i != 0 && j != 0)
                        {
                            if (TileGrid[i - 1, j - 1].BaseValue == TileGrid[i, j].BaseValue + 1)
                            {
                                movesRemainig = true;
                                break;
                            }
                        }

                        if (i != 0 && j != 4)
                        {
                            if (TileGrid[i - 1, j + 1].BaseValue == TileGrid[i, j].BaseValue + 1)
                            {
                                movesRemainig = true;
                                break;
                            }
                        }

                        if (i != 3 && j != 0)
                        {
                            if (TileGrid[i + 1, j - 1].BaseValue == TileGrid[i, j].BaseValue + 1)
                            {
                                movesRemainig = true;
                                break;
                            }
                        }

                        if (i != 3 && j != 4)
                        {
                            if (TileGrid[i + 1, j + 1].BaseValue == TileGrid[i, j].BaseValue + 1)
                            {
                                movesRemainig = true;
                                break;
                            }
                        }
                    }
                }
            }
            return movesRemainig;
        }
    }
}