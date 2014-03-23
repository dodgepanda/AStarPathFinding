using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStarPathFinding
{
    enum GridTerrain { 
        NONE,
        GRASS,
        WATER,
        MOUNTAIN
    };

    class GridSquare
    {
        int X, Y;
        public float GScore{ get; set; }
        public float HScore{ get; set; }
        public GridTerrain Terrain { get; set; }
        public GridSquare Parent { get; set; }
        public GridSquare(int x, int y, GridTerrain terrain = GridTerrain.NONE)
        {
            X = x;
            Y = y;
            Terrain = terrain;
            GScore = 0f;
            HScore = 0f;
            Parent = null;
        }
        public int GetX() { return X; }
        public int GetY() { return Y; }
        public float GetFSCore() { return GScore + HScore; }
        public float GetCostMultiplier()
        {
            switch (Terrain)
            {
                case GridTerrain.NONE: return 1;
                case GridTerrain.GRASS: return 1.1f;
                case GridTerrain.WATER: return 1.5f;
                case GridTerrain.MOUNTAIN: return 3;
                default: return 1;
            }
        }
        public bool IsPassable()
        {
            switch (Terrain)
            {
                case GridTerrain.NONE: return true;
                case GridTerrain.GRASS: return true;
                case GridTerrain.WATER: return true;
                case GridTerrain.MOUNTAIN: return false;
                default: return true;
            }
        }
        public Microsoft.Xna.Framework.Color GetColor()
        {
            switch (Terrain)
            {
                case GridTerrain.NONE: return Microsoft.Xna.Framework.Color.White;
                case GridTerrain.GRASS: return Microsoft.Xna.Framework.Color.LightGreen;
                case GridTerrain.WATER: return Microsoft.Xna.Framework.Color.Blue;
                case GridTerrain.MOUNTAIN: return Microsoft.Xna.Framework.Color.Gray;
                default: return Microsoft.Xna.Framework.Color.Black;
            }
        }
        public float CalculateHScore(GridSquare goal)
        {
            float x = Math.Abs(X - goal.X);
            float y = Math.Abs(Y - goal.Y);

            return (x + y) * 10;
        }
    }
}
