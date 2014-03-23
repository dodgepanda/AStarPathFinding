using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStarPathFinding
{
    class Grid
    {
        GridSquare[,] Squares;
        int Width, Height;
        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            Squares = new GridSquare[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Squares[i, j] = new GridSquare(i,j);
                }
            }
        }
        public void Reset()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Squares[i, j].GScore = 0;
                    Squares[i, j].Parent = null;
                }
            }
        }
        public void Clear()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    Squares[i, j].Terrain = GridTerrain.NONE;
                }
            }
        }
        public GridSquare[,] GetSquares() { return Squares; }
        public int GetWidth() { return Width; }
        public int GetHeight() { return Height; }

        public HashSet<GridSquare> GetNeighbors(GridSquare gs)
        {
            HashSet<GridSquare> neighbors = new HashSet<GridSquare>();
            int x, y;
            x = gs.GetX();
            y = gs.GetY();
            if (x > 0)
            {
                neighbors.Add(Squares[x - 1, y]);
                if (y > 0)
                {
                    neighbors.Add(Squares[x - 1, y - 1]);
                }
            }
            if (y > 0)
            {
                neighbors.Add(Squares[x , y-1]);
                if (x < Width-1)
                {
                    neighbors.Add(Squares[x + 1, y - 1]);
                }
            }
            if (x < Width-1)
            {
                neighbors.Add(Squares[x+1, y]);
                if (y < Height-1)
                {
                    neighbors.Add(Squares[x +1 , y +1 ]);
                }
            }
            if (y < Height-1)
            {
                neighbors.Add(Squares[x, y+1] );
                if (x > 0)
                {
                    neighbors.Add(Squares[x - 1, y + 1]);
                }
            }
            return neighbors;
        }
    }
}
