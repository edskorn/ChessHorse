using System;

namespace ChessHorse
{
    class ChessDesk
    {

        private int deskDimensionX;
        private int deskDimensionY;

        private int[,] chess; //chess desk

        private int step; //current Horse step number

        //direction options for Horse
        private readonly int[,] directions = {
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
        /// ChessDesk Constructor
        /// </summary>
        /// <param name="deskDimensionX">chess desk X dimension</param>
        /// <param name="deskDimensionY">chess desk Y dimension</param>
        public ChessDesk(int deskDimensionX, int deskDimensionY)
        {
            this.deskDimensionX = deskDimensionX;
            this.deskDimensionY = deskDimensionY;
            this.chess = new int[deskDimensionX, deskDimensionY];
        }

        /// <summary>
        /// Output the solution was found from i,j position
        /// </summary>
        public void ShowDesk()
        {
            for (int i = 0; i < deskDimensionX; i++)
            {
                for (int j = 0; j < deskDimensionY; j++)
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

            for (int i = 0; i < deskDimensionX; i++)
            {
                for (int j = 0; j < deskDimensionY; j++)
                {
                    chess[i, j] = 0;
                }
            }
        }

        /// <summary>
        /// Recursive filling of the board according to the Wansdorff algorithm
        /// </summary>
        /// <param name="x">row of the start position</param>
        /// <param name="y">column of the start position</param>
        /// <returns>Number of steps</returns>
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

        /// <summary>
        /// Find direction of the next step according to the Wansdorff algorithm (choose a direction,
        /// that has the fewest possible next steps)
        /// </summary>
        /// <param name="x">row current Horse position</param>
        /// <param name="y">column current Horse position</param>
        /// <returns>Next step direction index or -1 if there are no possible steps</returns>
        private int FindWarnsdorffDirection(int x, int y)
        {
            int resultDirection = -1;

            int minWays = 8;

            for (int direction = 0; direction < 8; direction++)
            {

                int xWay = x + directions[direction, 0];
                int yWay = y + directions[direction, 1];

                if (xWay < deskDimensionX && xWay >= 0 && yWay < deskDimensionY && yWay >= 0 && chess[xWay, yWay] == 0)
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

        /// <summary>
        /// Count amount of possible next steps from x,y position
        /// </summary>
        /// <param name="x">row of the reasearching position</param>
        /// <param name="y">column of the reasearching position</param>
        /// <returns>Amount of possible next steps from x,y position</returns>
        private int CountPossibleWays(int x, int y)
        {
            int countWays = 0;

            for (int direction = 0; direction < 8; direction++)
            {
                int xWay = x + directions[direction, 0];
                int yWay = y + directions[direction, 1];

                if (xWay < deskDimensionX && xWay >= 0 && yWay < deskDimensionY && yWay >= 0 && chess[xWay, yWay] == 0)
                {
                    countWays++;
                }
            }

            return countWays;
        }
    }
}
