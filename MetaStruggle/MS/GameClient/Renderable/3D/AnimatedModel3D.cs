﻿using System;
using System.Collections.Generic;
using System.Linq;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAnimation;
using XNAnimation.Controllers;

namespace GameClient.Renderable._3D
{
    public class AnimatedModel3D : I3DElement
    {
        #region model fields
        public SkinnedModel Model { get; set; }
        public float XRotation { get; set; }
        public float YRotation { get; set; }
        public float ZRotation { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public string ModelName { get; set; }
        public SceneManager Scene { get; set; }
        private int _animClip;
        public AnimationController AnimationController { get; set; }
        private float _speed;

        //Animation Speed
        public float ASpeed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                AnimationController.Speed = value;
            }
        }

        public float Gravity { get; set; }
        #endregion

        public Matrix World
        {
            get
            {
                return Matrix.Identity *
                       Matrix.CreateRotationX(Pitch) *
                       Matrix.CreateRotationY(Yaw) *
                       Matrix.CreateRotationZ(Roll) *
                       Matrix.CreateScale(Scale) *
                       Matrix.CreateTranslation(Position) *
                       Matrix.CreateRotationX(XRotation) *
                       Matrix.CreateRotationY(YRotation) *
                       Matrix.CreateRotationZ(ZRotation);
            }
        }

        public Animation CurrentAnimation
        {
            get { return (Animation)_animClip; }
        }

        public AnimatedModel3D(string nameCharacter, SceneManager scene, Vector3 position, Vector3 scale, float speed = 1)
        {
            ModelName = nameCharacter;
            Model = Global.RessourceProvider.AnimatedModels[ModelName];
            Scene = scene;
            Position = position;
            Scale = scale;
            _animClip = 0;

            AnimationController = new AnimationController(Model.SkeletonBones)
            {
                Speed = speed,
                TranslationInterpolation = InterpolationMode.Linear,
                OrientationInterpolation = InterpolationMode.Linear,
                ScaleInterpolation = InterpolationMode.Linear
            };

            _speed = speed;

            SetAnimation(Animation.Default);
        }

        public void SetAnimation(Animation animation)
        {
            if (_animClip == (int)animation)
                return;
            _animClip = (int)animation;

            if (_animClip > Model.AnimationClips.Count)
                _animClip = 1;
            AnimationController.StartClip(Model.AnimationClips.Values.Count == 1
                ? Model.AnimationClips.Values[0]
                : Model.AnimationClips.Values[_animClip]);
        }

        public virtual void Update(GameTime gameTime)
        {
            AnimationController.Update(gameTime.ElapsedGameTime, Matrix.Identity);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (ModelMesh mesh in Model.Model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(AnimationController.SkinnedBoneTransforms);
                    effect.EnableDefaultLighting();
                    effect.World = World;
                    effect.View = Scene.Camera.ViewMatrix;
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), spriteBatch.GraphicsDevice.DisplayMode.AspectRatio, 1f, 100f);
                }
                mesh.Draw();
            }

        }
    }

    public enum Animation
    {
        Default = 1,
        Run,
        Jump,
        Attack,
        SpecialAttack
    }
}
