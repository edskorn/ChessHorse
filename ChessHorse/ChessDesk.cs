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

        public int GetMaxStep()
        {
            return maxStep;
        }

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

        private void FindPrev()
        {
            //mark current possition as deadlock
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
    }
}
