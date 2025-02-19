﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DPSF;
using GameClient.Global.InputManager;
using GameClient.Renderable.Layout;
using GameClient.SoundEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameClient.Renderable.Scene;
using Microsoft.Xna.Framework.Input;
using Network;

namespace GameClient.Global
{
    public static class GameEngine
    {
        public static Config Config { get; set; }
        public static LayoutStack<IBasicLayout> DisplayStack { get; set; }
        public static SceneManager SceneManager { get; set; }
        public static SoundCenter SoundCenter { get; set; }
        public static LanguageLoader LangCenter { get; set; }
        public static EventManager EventManager { get; set; }
        public static KeyboardState KeyboardState { get; set; }
        public static MouseState MouseState { get; set; }
        public static GamePadState[] GamePadState { get; set; }
        public static InputDevice InputDevice { get; set; }
        public static ParticleEngine ParticleEngine { get; set; }

        public static int PrimaryHeightOfWindows { get; set; }
        public static int PrimaryWidthOfWindows { get; set; }

        public static void InitializeEngine(ContentManager content, GraphicsDeviceManager graphics, Game game)
        {
            EventManager = new EventManager();
            Config = IO.Serialization.LoadConfigFile("config.xml");
            Config.SetGraphics(graphics);
            LangCenter = new LanguageLoader(graphics.GraphicsDevice);
            ParticleEngine = new ParticleEngine(content,game);
            RessourceProvider.Fill(content, game);
            
            SoundCenter = SoundCenter.Instance;
            DisplayStack = new LayoutStack<IBasicLayout>();
            InputDevice = new InputDevice();
            GamePadState = new GamePadState[4];

            PrimaryHeightOfWindows = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            PrimaryWidthOfWindows = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        }
        
        public static void UpdateEngine()
        {
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            GamePadState[0] = GamePad.GetState(PlayerIndex.One);
            GamePadState[1] = GamePad.GetState(PlayerIndex.Two);
            GamePadState[2] = GamePad.GetState(PlayerIndex.Three);
            GamePadState[3] = GamePad.GetState(PlayerIndex.Four);
        }

        public static void SaveDatas()
        {
            IO.Serialization.SaveConfigFile("config.xml", Config);
        }
    }
}
