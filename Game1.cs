using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameOfLife
{
	//  The main class Game1, which start everything, which have constants for dimensions of the grid and the framerate
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		// Updates per second
		public const int UPS = 20;
		//FPS is to define how quickly it'll read mouse input and draw
		public const int FPS = 60;

		// Cell pixel width/height
		public const int CellSize = 10; 
		public const int CellsX = 100;
		public const int CellsY = 50;

		public static bool Paused = true;

		public static SpriteFont Font;
		public static Texture2D Pixel;

		public static Vector2 ScreenSize;

		private Grid grid;

		private KeyboardState keyboardState, lastKeyboardState;

		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		//initialize to create the grid, set the window size for the game, and make sure the mouse is visible so we can click on cells.
		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			IsFixedTimeStep = true;
			TargetElapsedTime = TimeSpan.FromSeconds(1.0 / FPS);

			ScreenSize = new Vector2(CellsX, CellsY) * CellSize;

			graphics.PreferredBackBufferWidth = (int)ScreenSize.X;
			graphics.PreferredBackBufferHeight = (int)ScreenSize.Y;

			IsMouseVisible = true;
		}

		
		protected override void Initialize()
		{
			base.Initialize();

			grid = new Grid();

			keyboardState = lastKeyboardState = Keyboard.GetState();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			Font = Content.Load<SpriteFont>("Font");

			Pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
			Pixel.SetData(new[] { Color.White });
		}

		// pause the game whenever the spacebar is pressed, and clear the screen if backspace is pressed

		protected override void Update(GameTime gameTime)
		{
			keyboardState = Keyboard.GetState();

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			// Toggle pause when spacebar is pressed.
			if (keyboardState.IsKeyDown(Keys.Space) && lastKeyboardState.IsKeyUp(Keys.Space))
				Paused = !Paused;

			// Clear the screen if backspace is pressed.
			if (keyboardState.IsKeyDown(Keys.Back) && lastKeyboardState.IsKeyUp(Keys.Back))
				grid.Clear();

			base.Update(gameTime);

			grid.Update(gameTime);

			lastKeyboardState = keyboardState;
		}

		//protected override void Draw(GameTime gameTime): is to make the background go red, and  to write "Paused" in the background
		protected override void Draw(GameTime gameTime)
		{
			if (Paused)
				GraphicsDevice.Clear(Color.Red);
			else
				GraphicsDevice.Clear(Color.White);

			spriteBatch.Begin();
			if (Paused)
			{
				string paused = "Paused";
				spriteBatch.DrawString(Font, paused, ScreenSize / 2, Color.Gray, 0f, Font.MeasureString(paused) / 2, 1f, SpriteEffects.None, 0f);
			}
			grid.Draw(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
