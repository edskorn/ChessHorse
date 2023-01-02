using System;

namespace ChessHorse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int dimLimit = 20; //Limit of the chess desk dimension

            int dimX = InputDimension("X", dimLimit);
            int dimY = InputDimension("Y", dimLimit);

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
                        chessDesk.ShowDesk();
                    }
                    else
                    {
                        Console.WriteLine("Max step = " + stepCount);
                    }
                }
            }
        }

        /// <summary>
        /// Read value from Console and parse to int with a given limit
        /// </summary>
        /// <param name="axis">Axis name (X or Y)</param>
        /// <param name="limit">Input value limit (max value)</param>
        /// <returns></returns>
        public static int InputDimension(string axis, int limit) 
        {
            int dimension;
            
            bool loopResult = false;
            do
            {
                Console.Write("Input chess desk {0} dimention (1-{1}): ", axis, limit);
                if (int.TryParse(Console.ReadLine(), out dimension) && dimension >= 1 && dimension <= limit)
                {
                    loopResult = true;
                }
                else
                {
                    Console.WriteLine("INPUT ERROR. Pls, input number in range 1-{0}.", limit);
                }
            } while (!loopResult);

            return dimension;
        }
    }
}
