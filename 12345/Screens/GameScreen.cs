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
using _12345.Screens.GameClasses;
using _12345.Screens.UIClasses;
using _12345.Screens.Tools;

namespace _12345.Screens
{
    public class GameScreen : Screen
    {
        static TileBoard tileBoard;

        public static int CurrentScore;

        static TimeSpan currentTime, roundTime;

        static int playTime = 60;

        public static float TimeRatio;
        static Color backgroundColor, timerColour, finalTurnColour;
        public enum GameState
        {
            GamePlay,
            FinalMove,
            GameOver
        }

        public static GameState CurrentState = GameState.GamePlay;


        public static BannerManager BannerManager;

        public static Camera_2D Camera;

        bool timeWasLowerThanTen = false;

        #region GameOverRegion
        float CameraSpeed = 32f;
        Vector2 gOCameraEndPoint = new Vector2(1620, 960);
        enum GameOverState
        {

            MovingCamera,
            Waiting
        }

        GameOverState CurrentGameOverState = GameOverState.MovingCamera;
        static float boardOpacity = 1f;

        #endregion
        public GameScreen() : base()
        {
            Name = "GameScreen";
            timerColour = new Color(4, 10, 26);
            finalTurnColour = new Color(248, 168, 109);
        }

        public override void Load()
        {
            timerColour = new Color(4, 10, 26);
            finalTurnColour = new Color(248, 168, 109);
            StartGame();
            base.Load();
        }

        public override void Update(GameTime gameTime)
        {
            currentTime += gameTime.ElapsedGameTime;
            Camera.Update(gameTime);

            if (CurrentState == GameState.GamePlay)
            {
                GameplayUpdate(gameTime);
            }

            else if (CurrentState == GameState.FinalMove)
            {
                FinalMovesUpdate(gameTime);
            }

            else if (CurrentState == GameState.GameOver)
            {
                GameOverUpdate(gameTime);
            }

            tileBoard.Update(gameTime);
            BannerManager.Update(gameTime);
            base.Update(gameTime);
        }
        

        public void FinalMovesUpdate(GameTime gameTime)
        {
            TimeRatio = (float)(currentTime.TotalMilliseconds / roundTime.TotalMilliseconds);
            backgroundColor = Color.Lerp(timerColour, finalTurnColour, TimeRatio);
            TimeRatio = 1 - TimeRatio;

            if(tileBoard.MovesRemaining() == false)
            {
                GameOver();
            }
        }
        public void GameOverUpdate(GameTime gameTime)
        {
            if(CurrentGameOverState == GameOverState.MovingCamera)
            {

                //Camera.Pos += new Vector2(CameraSpeed, 0);

                if(Camera.Pos.X >= gOCameraEndPoint.X)
                {
                    Camera.Pos = gOCameraEndPoint;
                    CurrentGameOverState = GameOverState.Waiting;
                }
            }

            FadeOutBoards(gameTime);
            if (currentTime > roundTime)
            {
                ScreenManager.MainMenu.MoveIn();
            }
        }
        public void GameplayUpdate(GameTime gameTime)
        {
            TimeRatio = (float)(currentTime.TotalMilliseconds / roundTime.TotalMilliseconds);
            backgroundColor = Color.Lerp(Color.White, timerColour, TimeRatio);

            if (roundTime.TotalSeconds - currentTime.TotalSeconds < 10)
            {
                if (BannerManager.TopBanner.CurrentMovementStatus == Banner.MovementStatus.In)
                    BannerManager.TopBanner.MoveOut();

                int timeRemaining = (int)(roundTime.TotalSeconds - currentTime.TotalSeconds);
                BannerManager.TopBanner.SetText(timeRemaining.ToString() + " Seconds Left");
                timeWasLowerThanTen = true;
            }

            else
            {
                if (timeWasLowerThanTen)
                {
                    BannerManager.TopBanner.MoveIn();
                    timeWasLowerThanTen = false;
                }
            }
            if (currentTime > roundTime)
            {
                CurrentState = GameState.FinalMove;
                currentTime = new TimeSpan(0, 0, 0);
                roundTime = new TimeSpan(0, 0, 1);
                backgroundColor = timerColour;
                BannerManager.BottomBanner.MoveOut("Final Moves");
                BannerManager.TopBanner.SetText("Time Up!");
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Main.Textures["Background"], Vector2.Zero, backgroundColor);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Camera.GetTransformation());
            tileBoard.Draw(spriteBatch);
            Main.ParticleManager.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            BannerManager.Draw(spriteBatch);
            spriteBatch.Draw(Main.Textures["ScoreBoard"], Vector2.Zero, Color.White * boardOpacity);
            spriteBatch.Draw(Main.Textures["BottomBoard"], Vector2.Zero, Color.White * boardOpacity);
            Vector2 scoreOrigin = Main.Fonts["BannerFont"].MeasureString(GameScreen.CurrentScore.ToString()) / 2;
            spriteBatch.DrawString(Main.Fonts["BannerFont"], GameScreen.CurrentScore.ToString(), new Vector2(540, 95f), Color.Black * boardOpacity, 0f, scoreOrigin, 1f, SpriteEffects.None, 1f);
            spriteBatch.End();
            base.Draw(spriteBatch);
        }

        public static void StartGame()
        {
            tileBoard = new TileBoard();
            Reset();
        }
        public static void Reset()
        {
            tileBoard = new TileBoard();
            CurrentState = GameState.GamePlay;
            Camera = new Camera_2D(new Viewport(0, 0, 1080, 1920), 1000, 100, 1f);
            Camera.Pos = new Vector2(540, 960);
            BannerManager = new BannerManager();
            timerColour = new Color(4, 10, 26);
            finalTurnColour = new Color(248, 168, 109);
            roundTime = new TimeSpan(0, 0, playTime);
            currentTime = new TimeSpan(0, 0, 0);
            TimeRatio = (float)(currentTime.TotalMilliseconds / roundTime.TotalMilliseconds);
            CurrentScore = 0;
            boardOpacity = 1f;
            backgroundColor = Color.White;
            tileBoard.Reset();
        }

        public static void GameOver()
        {
            CurrentState = GameState.GameOver;
            currentTime = new TimeSpan(0, 0, 0);
            roundTime = new TimeSpan(0, 0, 5);
            tileBoard.CurrentRibbon = null;
            tileBoard.TakeInput = false;
            BannerManager.TopBanner.MoveIn();
            BannerManager.BottomBanner.MoveIn();
        }

        public static void AddTime(double seconds)
        {
            currentTime -= new TimeSpan(0, 0, 0, 0, (int)(seconds * 1000));
        }

        public static void FadeOutBoards(GameTime gameTime)
        {
            TimeRatio = (float)(currentTime.TotalMilliseconds / roundTime.TotalMilliseconds);
            backgroundColor = Color.Lerp(finalTurnColour, Color.White, TimeRatio);
            boardOpacity = 1 - TimeRatio;
            BannerManager.BottomBanner.Opacity *= boardOpacity;
            BannerManager.TopBanner.Opacity *= boardOpacity;
        }
    }
}