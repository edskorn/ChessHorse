using System;

namespace ChessHorse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input chess desk X dimention: ");
            int dimX = Convert.ToInt32(Console.ReadLine());

            Console.Write("Input chess desk Y dimention: ");
            int dimY = Convert.ToInt32(Console.ReadLine());

            ChessDesk chessDesk = new ChessDesk(dimX, dimY);

            for (int i = 0; i < dimX; i++)
            {
                for (int j = 0; j < dimY; j++)
                {
                    chessDesk.InitDesk();
                    Console.WriteLine("i = " + i + " | j = " + j);

                    int stepCount = chessDesk.WarnsdorffCalc(i, j);

                    if (stepCount == dimX * dimY)
                    {
                        //show result from i,j position 
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
