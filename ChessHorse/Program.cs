using System;

namespace ChessHorse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChessDesk chessDesk = new ChessDesk();

            //check the possibility of filling the chess desk in a quater range,  
            //because we can get solutions in other ranges by rotating the found result
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    chessDesk.InitDesk();
                    Console.WriteLine("i = " + i + " | j = " + j);

                    int stepCount = chessDesk.WarnsdorffCalc(i, j);

                    if (stepCount == 64)
                    {
                        //show result from i,j position 
                        chessDesk.ShowDesk();

                        //rotate result 3 times to show other 3 solutions

                        Console.WriteLine("i = " + j + " | j = " + (7 - i));
                        chessDesk.RotateDesk();
                        chessDesk.ShowDesk();

                        Console.WriteLine("i = " + (7 - i) + " | j = " + (7 - j));
                        chessDesk.RotateDesk();
                        chessDesk.ShowDesk();

                        Console.WriteLine("i = " + (7 - j) + " | j = " + i);
                        chessDesk.RotateDesk();
                        chessDesk.ShowDesk();

                    }
                    else
                    {
                        Console.WriteLine("Max step = " + stepCount);
                    }
                }
            }
        }
    }
}
