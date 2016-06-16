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

namespace _12345.Screens.Tools
{
    public class Camera_2D
    {
        private const float zoomUpperLimit = 1.5f;
        private const float zoomLowerLimit = .5f;

        private float _zoom;
        private Matrix _transform;
        private Vector2 _pos;
        private float _rotation;
        private int _viewportWidth;
        private int _viewportHeight;
        private int _worldWidth;
        private int _worldHeight;

        public bool IsMoving = false;
        Vector2 movePoint, moveDir;
        float moveSpeed;

        public Camera_2D(Viewport viewport, int worldWidth,
           int worldHeight, float initialZoom)
        {
            _zoom = initialZoom;
            _rotation = 0.0f;
            _pos = Vector2.Zero;
            _viewportWidth = viewport.Width;
            _viewportHeight = viewport.Height;
            _worldWidth = worldWidth;
            _worldHeight = worldHeight;
        }

        #region Properties

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < zoomLowerLimit)
                    _zoom = zoomLowerLimit;
                if (_zoom > zoomUpperLimit)
                    _zoom = zoomUpperLimit;
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public void Move(Vector2 amount)
        {
            _pos += amount;
        }

        public Vector2 Pos
        {
            get { return _pos; }
            set
            {
                float leftBarrier = (float)_viewportWidth *
                       .5f / _zoom;
                float rightBarrier = _worldWidth -
                       (float)_viewportWidth * .5f / _zoom;
                float topBarrier = _worldHeight -
                       (float)_viewportHeight * .5f / _zoom;
                float bottomBarrier = (float)_viewportHeight *
                       .5f / _zoom;
                _pos = value;
                //if (_pos.X < leftBarrier)
                //    _pos.X = leftBarrier;
                //if (_pos.X > rightBarrier)
                //    _pos.X = rightBarrier;
                //if (_pos.Y > topBarrier)
                //    _pos.Y = topBarrier;
                //if (_pos.Y < bottomBarrier)
                //    _pos.Y = bottomBarrier;
            }
        }

        #endregion

        public Matrix GetTransformation()
        {
            _transform =
               Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
               Matrix.CreateTranslation(new Vector3(_viewportWidth * 0.5f,
                   _viewportHeight * 0.5f, 0));

            return _transform;
        }

        public Vector2 CameraMouse(Vector2 currentMouse)
        {
            return new Vector2(currentMouse.X + Pos.X - _viewportWidth / 2, currentMouse.Y + Pos.Y - _viewportHeight / 2);
        }
        public void FollowCharacter(Vector2 pos)
        {
            Pos = pos;
        }

        public void Update(GameTime gametime)
        {
            if (IsMoving)
            {
                Vector2 prePos = Pos;

                Pos += moveDir * moveSpeed;

                if ((prePos - movePoint).Length() < (Pos - movePoint).Length())
                {
                    Pos = movePoint;
                    IsMoving = false;
                }
            }
        }

        public void MoveToPoint(Vector2 vec, float _speed)
        {
            if (Pos != vec)
            {
                movePoint = vec;
                moveDir = (movePoint - Pos);
                moveDir.Normalize();
                moveSpeed = _speed;
                IsMoving = true;
            }
        }
    }
}