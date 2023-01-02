using System;

namespace ChessHorse
{
    class ChessDesk
    {
        int[,] chess = new int[8, 8];

        int row, col, step; //current Horse position and step number
        int rowDeadlock, colDeadlock; //last deadlock position, where Horse can't move any more
        int directionDeadlock = 0; //deadlock direction index (used for finding alternatives with
                                   //next direction index - it excepts loops)
        int maxStep = 0; //maх step number was found from i,j possition 
        int rollback = 0; //number of reverse steps (when Horse meet a deadlock)
        int rollbackLimit = 2000000; //for loop break

        //direction options for Horse
        readonly int[,] directions = {
            { -2, 1},
            { -1, 2},
            { 1, 2},
            { 2, 1},
            { 2, -1},
            { 1, -2},
            { -1, -2},
            { -2, -1}
        };
        /// <summary>
        /// Public getter for maxStep
        /// </summary>
        /// <returns>Maх step number was found from i,j possition</returns>
        public int GetMaxStep()
        {
            return maxStep;
        }

        /// <summary>
        /// Output the solution was found from i,j possition
        /// </summary>
        public void ShowDesk()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    string formatedStep = string.Format("{0:d2}", chess[i, j]);
                    Console.Write(formatedStep + " ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Reset desk (all sells are set by 0) and step number
        /// </summary>
        public void InitDesk()
        {
            step = 1;

            maxStep = 0;
            rollback = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    chess[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Find solution from i,j possition
        /// </summary>
        /// <param name="i">row start position</param>
        /// <param name="j">column start position</param>
        /// <returns>True - if soution was found</returns>
        public bool CalcDesk(int i, int j)
        {
            row = i;
            col = j;

            chess[row, col] = step;

            while (step < 64 && rollback < rollbackLimit && (TryStep() || FindAlternative()))
            {
                directionDeadlock = 0;

                chess[row, col] = ++step;

                maxStep = step > maxStep ? step : maxStep;
            }

            return step == 64;
        }

        /// <summary>
        /// Find next possible step and do it (mark the found sell by step number) if it possible
        /// </summary>
        /// <returns>True - if next step is done</returns>
        private bool TryStep()
        {
            bool result = false;

            int direction = directionDeadlock;

            while (direction < 8 && !result)
            {
                row += directions[direction, 0];
                col += directions[direction, 1];

                if (row < 8 && row >= 0 && col < 8 && col >= 0 && chess[row, col] == 0)
                {
                    result = true;
                }
                else
                {
                    row -= directions[direction, 0];
                    col -= directions[direction, 1];
                }

                direction++;
            }

            return result;
        }

        /// <summary>
        /// Take a reverse step until a new alternative step is found.
        /// Break loop if step number == 1 or a new alternative step is found.
        /// </summary>
        /// <returns>True - if new alternative step is found</returns>
        private bool FindAlternative()
        {
            bool result = false;

            while (step != 1 && !result)
            {
                FindPrev();
                if (TryStep())
                {
                    result = true;
                }
                chess[rowDeadlock, colDeadlock] = 0;
                step--;
            }

            rollback++;

            return step != 1;
        }

        /// <summary>
        /// Find previous position and take a step to it
        /// </summary>
        private void FindPrev()
        {
            //mark current position as deadlock
            chess[row, col] = -1;
            rowDeadlock = row;
            colDeadlock = col;

            bool result = false;

            int direction = 0;

            while (direction < 8 && !result)
            {
                row += directions[direction, 0];
                col += directions[direction, 1];

                if (row < 8 && row >= 0 && col < 8 && col >= 0 && chess[row, col] == step - 1)
                {
                    directionDeadlock = direction > 3 ? direction - 4 : direction + 4;
                    result = true;
                }
                else
                {
                    row -= directions[direction, 0];
                    col -= directions[direction, 1];
                }

                direction++;
            }

        }

        /// <summary>
        /// Make a new solution by rotating the current solution 90° clockwise
        /// </summary>
        public void RotateDesk()
        {
            int[,] newChess = new int[8, 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    newChess[i, j] = chess[7 - j, i];
                }
            }

            chess = newChess;
        }

        public int WarnsdorffCalc(int x, int y)
        {
            chess[x, y] = step++;

            int direction = FindWarnsdorffDirection(x, y);
            if (direction == -1)
            {
                return 1;
            }
            return 1 + WarnsdorffCalc(x + directions[direction, 0], y + directions[direction, 1]);
        }

        private int FindWarnsdorffDirection(int x, int y)
        {
            int resultDirection = -1;

            int minWays = 8;

            for (int direction = 0; direction < 8; direction++)
            {

                int xWay = x + directions[direction, 0];
                int yWay = y + directions[direction, 1];

                if (xWay < 8 && xWay >= 0 && yWay < 8 && yWay >= 0 && chess[xWay, yWay] == 0)
                {
                    int countWays = CountPossibleWays(xWay, yWay);

                    if (countWays < minWays)
                    {
                        minWays = countWays;
                        resultDirection = direction;
                    }

                    if (minWays == 1) break;
                }
            }

            return resultDirection;
        }

        private int CountPossibleWays(int x, int y)
        {
            int countWays = 0;

            for (int direction = 0; direction < 8; direction++)
            {
                int xWay = x + directions[direction, 0];
                int yWay = y + directions[direction, 1];

                if (xWay < 8 && xWay >= 0 && yWay < 8 && yWay >= 0 && chess[xWay, yWay] == 0)
                {
                    countWays++;
                }
            }

            return countWays;
        }
    }
}
