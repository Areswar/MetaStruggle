﻿using GameClient.Global;
using GameClient.Renderable.GUI;
using GameClient.Renderable.GUI.Items;
using GameClient.Renderable.GUI.Items.ListItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Menus
{
    class CharacterSelector
    {
        private Menu Menu;
        private SpriteBatch _spriteBatch;
        private GraphicsDeviceManager _graphics;

        public CharacterSelector(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            _spriteBatch = spriteBatch;
            _graphics = graphics;
        }

        public Menu Create()
        {
            System.Threading.Thread.Sleep(200);
            Menu = new Menu(RessourceProvider.MenuBackgrounds["SimpleMenu"]);
            Menu.Add("CharacterSelector.Text", new SimpleText("Text.SelectPlayer", new Vector2(15, 15),
                Item.PosOnScreen.TopLeft, RessourceProvider.Fonts["MenuLittle"], Color.White));
            Menu.Add("CharacterSelector.Item", new ListImageButtons(new Rectangle(15, 22, 70, 45), RessourceProvider.CharacterFaces, "MSTheme",
                RessourceProvider.Fonts["HUDlittle"]));
            Menu.Add("PlayerName.Text", new SimpleText("Text.TextboxPlayer", new Vector2(15, 75),
                Item.PosOnScreen.TopLeft, RessourceProvider.Fonts["MenuLittle"], Color.White));
            Menu.Add("PlayerName.Item", new Textbox("",new Rectangle(15,82,300,0), "MSTheme",
                RessourceProvider.Fonts["Menu"], Color.White));

            Menu.Add("NextButton.Item", new MenuButton("Menu.Next", new Vector2(70, 90), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, NextButton));
            Menu.Add("ReturnButton.Item", new MenuButton("Menu.Back", new Vector2(15, 90), RessourceProvider.Fonts["Menu"], Color.White,
                Color.DarkOrange, MainMenu.Back));

            return Menu;
        }

        void NextButton()
        {
            Textbox playerNameTextbox = Menu.Items["PlayerName.Item"] as Textbox;
            ListImageButtons characterSelector = Menu.Items["CharacterSelector.Item"] as ListImageButtons;

            if (playerNameTextbox.Text == "" || characterSelector.NameSelected == "")
                return;

            System.Threading.Thread.Sleep(200);

            GameEngine.DisplayStack.Push( new ServerSelector(_spriteBatch, _graphics, characterSelector.NameSelected,
                    playerNameTextbox.Text).Create());
        }
    }
}
