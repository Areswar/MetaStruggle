﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Global.EventManager;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameClient.Renderable._3D.Characters
{
    public class Zeus : AnimatedModel3D
    {
        private readonly float _baseYaw;
        private bool _jumping;
        private double _jumppos;
        public bool IsDead;
        public DateTime DeathDate;

        public bool CollideWithMap
        {
            get { return Position.Y < 0.1 && Position.Y > -0.1 && Position.X < 12.58 && Position.X > -23.91; }
        }

        public Zeus(SceneManager scene, Vector3 position, Vector3 scale)
            : base("Zeus", scene, position, scale)
        {
            Pitch = -MathHelper.PiOver2;
            Yaw = MathHelper.PiOver2;
            _baseYaw = Yaw;
            Gravity = 0.005f * Speed;
        }

        public override void Update(GameTime gameTime)
        {
            #region ManageKeyboard
            KeyboardState ks = Global.GameEngine.KeyboardState;

            var pendingAnim = new List<Animation>();

            if (CurrentAnimation != Animation.Jump)
                pendingAnim.Add(Animation.Default);

            if (ks.IsKeyDown(Keys.Z))
            {
                Attack(gameTime);
                pendingAnim.Add(Animation.Attack);
            }
            if (ks.IsKeyDown(Keys.Space) && !_jumping && CollideWithMap)
            {
                Jump(gameTime);
                pendingAnim.Add(Animation.Jump);
                Global.GameEngine.EventManager.ThrowEvent(new EventDatas("Zeus", "CharacterJump", null));
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                MoveRight(gameTime);
                pendingAnim.Add(Animation.Run);
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                MoveLeft(gameTime);
                pendingAnim.Add(Animation.Run);
            }
            #endregion

            #region Death
            if (!IsDead && Position.Y < -10)
            {
                IsDead = true;
                DeathDate = DateTime.Now;
                Global.GameEngine.EventManager.ThrowEvent(new EventDatas("Zeus", "CharacterDie", null));
            }

            if (IsDead && (DateTime.Now - DeathDate).TotalMilliseconds > 5000)
            {
                SetAnimation(Animation.Default);
                IsDead = false;
                Position = new Vector3(-5, 0, -17);
            }
            #endregion

            #region Jump
            if (_jumping)
            {
                UpdateJump(gameTime, pendingAnim);
            }

            if (!CollideWithMap && !_jumping)
            {
                Position -= new Vector3(0, (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Gravity * 2), 0);
                pendingAnim.Add(Animation.Jump);
            }
            #endregion

            SetPriorityAnimation(pendingAnim);

            if (Position.Y >= 0)
                Scene.Camera.SetTarget(this);

            base.Update(gameTime);
        }

        void SetPriorityAnimation(ICollection<Animation> pendingAnim)
        {
            if (pendingAnim.Contains(Animation.Attack))
                SetAnimation(Animation.Attack);
            else if (pendingAnim.Contains(Animation.Jump))
                SetAnimation(Animation.Jump);
            else if (pendingAnim.Contains(Animation.Run) && !_jumping)
                SetAnimation(Animation.Run);
            else if (pendingAnim.Count != 0 && !_jumping)
                SetAnimation(Animation.Default);
        }

        #region Movements
        void MoveRight(GameTime gameTime)
        {
            Yaw = _baseYaw + MathHelper.Pi;
            Position -= new Vector3((float)(gameTime.ElapsedGameTime.TotalMilliseconds * Gravity * Speed), 0, 0);
        }

        void MoveLeft(GameTime gameTime)
        {
            Yaw = _baseYaw;
            Position += new Vector3((float)(gameTime.ElapsedGameTime.TotalMilliseconds * Gravity * Speed), 0, 0);
        }

        void Jump(GameTime gameTime)
        {
            _jumping = true;
            _jumppos = 0;
        }

        private void UpdateJump(GameTime gameTime, ICollection<Animation> pendingAnim)
        {
            _jumppos += gameTime.ElapsedGameTime.TotalMilliseconds * Gravity;
            Position = new Vector3(Position.X, (float)(2 * Math.Sin(_jumppos)), Position.Z);

            if (Position.Y <= 0)
            {
                _jumping = false;
                Position = new Vector3(Position.X, 0, Position.Z);
                _jumppos = 0;
                pendingAnim.Add(Animation.Default);
                return;
            }

            Position -= new Vector3(0, (float)(gameTime.ElapsedGameTime.TotalMilliseconds * Gravity * 2), 0);
        }

        void Attack(GameTime gameTime)
        {

        }
        #endregion

        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {

            base.Draw(gameTime, spriteBatch);
        }
    }
}