using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOfLife
{
	/*
	 * Class cells, which are the "life forms" that form the basis of the entire simulation. 
	 * Each cell can be in one of two states: "alive" or "dead"
	 * The cells have three behaviors:
			1) To keep track of their position, bounds, and state, so they can be clicked and drawn correctly.
			2) To toggle between alive and dead when clicked, which allows the user to actually make interesting things happen.
			3) To be drawn as white or black if they're dead or alive, respectively.

	 */
	class Cell
	{
		public Point Position { get; private set; }
		public Rectangle Bounds { get; private set; }

		public bool IsAlive { get; set; }

		//Create a class constructor for the Cell class
		public Cell(Point position)
		{
			Position = position;
			Bounds = new Rectangle(Position.X * Game1.CellSize, Position.Y * Game1.CellSize, Game1.CellSize, Game1.CellSize);

			IsAlive = false;
		}

		public void Update(MouseState mouseState)
		{
			if (Bounds.Contains(new Point(mouseState.X, mouseState.Y)))
			{
				// Make cells come alive with left-click, or kill them with right-click.
				if (mouseState.LeftButton == ButtonState.Pressed)
					IsAlive = true;
				else if (mouseState.RightButton == ButtonState.Pressed)
					IsAlive = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (IsAlive)
				spriteBatch.Draw(Game1.Pixel, Bounds, Color.Black);

		}
	}
}
