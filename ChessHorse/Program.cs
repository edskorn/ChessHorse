using System;
using System.Diagnostics;

namespace ChessHorse
{
    internal class Program
    {


        static void Main(string[] args)
        {
            ChessDesk chessDesk = new ChessDesk();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    chessDesk.initDesk();
                    Console.WriteLine("i = " + i + " | j = " + j);
                    if (chessDesk.calcDesk(i, j))
                    {
                        chessDesk.showDesk();

                        int ii, jj;

                        ii = j;
                        jj = 7 - i; 
                        
                        Console.WriteLine("i = "  + ii + " | j = " + jj);
                        chessDesk.transponDesk();
                        chessDesk.showDesk();

                        ii = 7 - i;
                        jj = 7 - j;

                        Console.WriteLine("i = " + ii + " | j = " + jj);
                        chessDesk.transponDesk();
                        chessDesk.showDesk();

                        ii = 7 - j;
                        jj = i;

                        Console.WriteLine("i = " + ii + " | j = " + jj);
                        chessDesk.transponDesk();
                        chessDesk.showDesk();

                    }
                    else
                    {
                        Console.WriteLine("Max step = " + chessDesk.getMaxStep());
                    }
                }
                
            }
        }
    }

    class ChessDesk
    {
        int[,] chess = new int[8, 8];

        int row, col, step;
        int rowDeadlock, colDeadlock;
        int optionDeadlock = 0;
        int maxStep = 0;
        int minRollback = 64;
        int rollbacks = 0;

        //step options for Horse
        int[,] steps = { 
            { -2, 1}, 
            { -1, 2}, 
            { 1, 2 },
            { 2, 1},
            { 2, -1},
            { 1, -2},
            { -1, -2},
            { -2, -1}
        };

        public void showDesk()
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

        public void initDesk()
        {
            step = 1;

            maxStep = 0;
            minRollback = 64;
            rollbacks = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    chess[i, j] = 0;
                }
            }
        }

        public bool calcDesk(int i, int j)
        {
            row = i;
            col = j;

            chess[row, col] = step;

            while (step < 64 && rollbacks < 5000000 && (tryStep() || findAlternative())) {
                optionDeadlock = 0;
                step++;
                chess[row, col] = step;

                maxStep = step > maxStep ? step : maxStep;

            }

            return step == 64 ? true : false;
        }

        bool tryStep()
        {
            bool result = false;

            int stepOption = optionDeadlock;

            while (stepOption < 8 && !result)
            {
                row += steps[stepOption, 0];
                col += steps[stepOption, 1];

                if (row < 8 && row >= 0 && col < 8 && col >= 0 && chess[row, col] == 0)
                {
                    result = true;
                }
                else 
                {
                    row -= steps[stepOption, 0];
                    col -= steps[stepOption, 1];
                } 

                stepOption++;
            }
            
            return result;
        }

        bool findAlternative()
        {
            bool result = false;

            while (step != 1 && !result)
            {
                findPrev();
                if (tryStep())
                {
                    result = true;
                }
                chess[rowDeadlock, colDeadlock] = 0;
                step--;
            }

            rollbacks++;
            minRollback = step < minRollback ? step : minRollback;

            return step == 1 ? false : true;
        }

        void findPrev()
        {
            chess[row, col] = -1;
            rowDeadlock = row;
            colDeadlock = col;

            bool result = false;

            int stepOption = 0;

            while (stepOption < 8 && !result)
            {
                row += steps[stepOption, 0];
                col += steps[stepOption, 1];

                if (row < 8 && row >= 0 && col < 8 && col >= 0 && chess[row, col] == step - 1)
                {
                    optionDeadlock = stepOption > 3 ? stepOption - 4 : stepOption + 4;
                    result = true;
                }
                else
                {
                    row -= steps[stepOption, 0];
                    col -= steps[stepOption, 1];
                }

                stepOption++;
            }

        }

        public int getMaxStep()
        { 
            return maxStep;
        }

        public void transponDesk()
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
