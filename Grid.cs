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
     * Class Grid is to hold the all the cells , and implement the logic that tells each one whether it should come alive, stay alive, die, or stay dead, 
     * which based on the following rules:
			1)Any live cell with fewer than two live neighbours dies, as if by underpopulation.
			2)Any live cell with two or three live neighbours lives on to the next generation.
			3)Any live cell with more than three live neighbours dies, as if by overpopulation.
			4)Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
	 */
    class Grid
	{
		public Point Size { get; private set; }

		private Cell[,] cells;
		private bool[,] nextCellStates;

		private TimeSpan updateTimer;

		public Grid()
		{
			Size = new Point(Game1.CellsX, Game1.CellsY);

			cells = new Cell[Size.X, Size.Y];
			nextCellStates = new bool[Size.X, Size.Y];

			for (int i = 0; i < Size.X; i++)
			{
				for (int j = 0; j < Size.Y; j++)
				{
					cells[i, j] = new Cell(new Point(i, j));
					nextCellStates[i, j] = false;
				}
			}

			updateTimer = TimeSpan.Zero;
		}

		public void Clear()
		{
			for (int i = 0; i < Size.X; i++)
				for (int j = 0; j < Size.Y; j++)
					nextCellStates[i, j] = false;

			SetNextState();
		}

		public void Update(GameTime gameTime)
		{
			MouseState mouseState = Mouse.GetState();

			foreach (Cell cell in cells)
				cell.Update(mouseState);

			if (Game1.Paused)
				return;

			updateTimer += gameTime.ElapsedGameTime;

			if (updateTimer.TotalMilliseconds > 1000f / Game1.UPS)
			{
				updateTimer = TimeSpan.Zero;

				// Loop through every cell.
				for (int i = 0; i < Size.X; i++)
				{
					for (int j = 0; j < Size.Y; j++)
					{
						
						bool living = cells[i, j].IsAlive;
						int count = GetLivingNeighbors(i, j);
						bool result = false;
						//Check the cells current state
						//count its living neighbors
						//apply the rules to set its next state
						if (living && count < 2)
							result = false;
						if (living && (count == 2 || count == 3))
							result = true;
						if (living && count > 3)
							result = false;
						if (!living && count == 3)
							result = true;

						nextCellStates[i, j] = result;
					}
				}

				SetNextState();
			}
		}

		// GetLivingNeighbors method, which counts how many of the current cell's neighbors are currently alive
		public int GetLivingNeighbors(int x, int y)
		{
			int count = 0;

			// Check cell on the right.
			if (x != Size.X - 1)
				if (cells[x + 1, y].IsAlive)
					count++;

			// Check cell on the bottom right.
			if (x != Size.X - 1 && y != Size.Y - 1)
				if (cells[x + 1, y + 1].IsAlive)
					count++;

			// Check cell on the bottom.
			if (y != Size.Y - 1)
				if (cells[x, y + 1].IsAlive)
					count++;

			// Check cell on the bottom left.
			if (x != 0 && y != Size.Y - 1)
				if (cells[x - 1, y + 1].IsAlive)
					count++;

			// Check cell on the left.
			if (x != 0)
				if (cells[x - 1, y].IsAlive)
					count++;

			// Check cell on the top left.
			if (x != 0 && y != 0)
				if (cells[x - 1, y - 1].IsAlive)
					count++;

			// Check cell on the top.
			if (y != 0)
				if (cells[x, y - 1].IsAlive)
					count++;

			// Check cell on the top right.
			if (x != Size.X - 1 && y != 0)
				if (cells[x + 1, y - 1].IsAlive)
					count++;

			return count;
		}

		/*
		 * A second grid of cells in memory for the next state of the system. 
		 * Every time we determine the next state of a cell, we store it in our second grid for the next state of the whole system. 
		 * Then, when we've found the next state of every cell, we apply them all at the same time. 
		 * So we add a 2D array of booleans nextCellStates as a private variable, and then add this method to the Grid class:
		 */
		public void SetNextState()
		{
			for (int i = 0; i < Size.X; i++)
				for (int j = 0; j < Size.Y; j++)
					cells[i, j].IsAlive = nextCellStates[i, j];
		}

		/*
		 * public void Draw is to draw it onto the screen. 
		 *The grid will draw each cell by calling their draw methods one at a time, so that all living cells will be black, and the dead ones will be white.
		 */
		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Cell cell in cells)
				cell.Draw(spriteBatch);

			// for loop to Draw vertical gridlines.
			for (int i = 0; i < Size.X; i++)
				// take a single pixel and stretch it to create a long and thin  vertical line.
				spriteBatch.Draw(Game1.Pixel, new Rectangle(i * Game1.CellSize - 1, 0, 1, Size.Y * Game1.CellSize), Color.DarkGray);

			// for loop to Draw horizontal gridlines.
			for (int j = 0; j < Size.Y; j++)
				// take a single pixel and stretch it to create a long and thin  horizontal line.
				spriteBatch.Draw(Game1.Pixel, new Rectangle(0, j * Game1.CellSize - 1, Size.X * Game1.CellSize, 1), Color.DarkGray);
		}
	}
}
