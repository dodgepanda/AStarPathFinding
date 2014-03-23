using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStarPathFinding
{
    class PathFinder
    {
        HashSet<GridSquare> OpenSet;
        HashSet<GridSquare> CloseSet;
        HashSet<GridSquare> Path;
        GridSquare CurrentSquare;
        public PathFinder(Grid grid, GridSquare start, GridSquare end)
        {
            grid.Reset();
            OpenSet = new HashSet<GridSquare>();
            CloseSet = new HashSet<GridSquare>();
            Path = new HashSet<GridSquare>();
            //1) Add the starting square (or node) to the open list.
            OpenSet.Add(start);
            //2) Repeat the following:
            while (true)
            {
                //a) Look for the lowest F cost square on the open list. We refer to this as the current square.
                CurrentSquare = GetLowestFScore();
                //b) Switch it to the closed list. 
                OpenSet.Remove(CurrentSquare);
                CloseSet.Add(CurrentSquare);
                //c) For each of the 8 squares adjacent to this current square …
                HashSet<GridSquare> neighbors = grid.GetNeighbors(CurrentSquare);
                HashSet<GridSquare> neighbors2 = grid.GetNeighbors(CurrentSquare);
                //Check to not allow cutting of impassable corners
                foreach (GridSquare neighbor in neighbors)
                {
                    if (!neighbor.IsPassable())
                    {
                        if (CurrentSquare.GetX() == neighbor.GetX())
                        {
                            if(neighbor.GetX() > 0)
                                neighbors2.Remove(grid.GetSquares()[neighbor.GetX() - 1, neighbor.GetY()]);
                            if(neighbor.GetX() < grid.GetWidth()-1)
                                neighbors2.Remove(grid.GetSquares()[neighbor.GetX() + 1, neighbor.GetY()]);
                        }
                        if (CurrentSquare.GetY() == neighbor.GetY())
                        {
                            if (neighbor.GetY() > 0)
                                neighbors2.Remove(grid.GetSquares()[neighbor.GetX(), neighbor.GetY() - 1]);
                            if (neighbor.GetY() < grid.GetHeight() - 1)
                                neighbors2.Remove(grid.GetSquares()[neighbor.GetX(), neighbor.GetY() + 1]);
                        }
                    }
                }
                //end check
                foreach (GridSquare neighbor in neighbors2)
                {
                    //If it is not walkable or if it is on the closed list, ignore it. Otherwise do the following.
                    if(neighbor.IsPassable() && !CloseSet.Contains(neighbor))
                    {
                        float cost = 10;
                        if(Math.Abs(CurrentSquare.GetX()-neighbor.GetX())==1 && Math.Abs(CurrentSquare.GetY()-neighbor.GetY())==1)
                            cost*=1.4f;
                        cost*=neighbor.GetCostMultiplier();

                        //If it isn’t on the open list, add it to the open list. 
                        //Make the current square the parent of this square. Record the F, G, and H costs of the square.
                        if (!OpenSet.Contains(neighbor))
                        {
                            OpenSet.Add(neighbor);
                            neighbor.Parent = CurrentSquare;
                            neighbor.GScore = CurrentSquare.GScore + cost;
                            neighbor.HScore = neighbor.CalculateHScore(end);
                        }
                        else
                        {
                            //If it is on the open list already, check to see if this path to that square is better, 
                            //using G cost as the measure. A lower G cost means that this is a better path. 
                            //If so, change the parent of the square to the current square, 
                            //and recalculate the G and F scores of the square. 
                            //If you are keeping your open list sorted by F score, 
                            //you may need to resort the list to account for the change.
                            if (neighbor.GScore > CurrentSquare.GScore + cost)
                            {
                                neighbor.Parent = CurrentSquare;
                                neighbor.GScore = CurrentSquare.GScore + cost;
                            }
                        }
                    }
                }
                //d) Stop when you:
                    //Add the target square to the closed list, in which case the path has been found (see note below), or
                    //Fail to find the target square, and the open list is empty. In this case, there is no path.   
                if (CloseSet.Contains(end) || OpenSet.Count()==0)
                {
                    //3) Save the path. Working backwards from the target square, 
                    //go from each square to its parent square until you reach the starting square. That is your path. 
                    if (CloseSet.Contains(end))
                    {
                        if (end.Parent != null)
                        {
                            GridSquare pathNode = end.Parent;
                            while (pathNode != start)
                            {
                                Path.Add(pathNode);
                                pathNode = pathNode.Parent;
                            }
                        }
                    }
                    return;
                }
            }
        }

        GridSquare GetLowestFScore()
        {
            GridSquare gs = OpenSet.First();

            foreach (GridSquare os in OpenSet)
            {
                if (os.GetFSCore() < gs.GetFSCore())
                    gs = os;
            }

            return gs;
        }

        public GridSquare GetCurrent() { return CurrentSquare; }
        public HashSet<GridSquare> GetClosedSet() { return CloseSet; }
        public HashSet<GridSquare> GetOpenSet() { return OpenSet; }
        public HashSet<GridSquare> GetPath() { return Path; }
    }
}
