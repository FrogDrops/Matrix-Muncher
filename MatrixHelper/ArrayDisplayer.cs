using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;

class Matrix
{
    int m;
    int n;
    double[,] matrix;

    public Matrix(int numRows, int numCols)
    {
        m = numRows;
        n = numCols;
        matrix = new double[m, n];
    }

    // Displays the entire matrix to the user
    public void Display()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        int counter = 1;
        Console.Write("\n|    ");

        foreach(double entry in matrix)
        {
            int numSpaces = 5 - entry.ToString().Length; // Take into account how we space our entries depending on their size to neatly display our matrix

            if (numSpaces <= 0)
            {
                numSpaces = 4;

            }
           
            string repeatedSpace = new string(' ', numSpaces); 
            if(counter > n) // We've reached the end of a row 
            {
                Console.Write("|\n\n|    ");
                counter = 1;
            }

            Console.Write($"{entry}" + repeatedSpace);
            counter++;
        }

        Console.WriteLine("|");
        Console.ResetColor();
    }

    // Lets the user input entries into their matrix row by row 
    public void EntryInput()
    {
        Console.WriteLine("\n------------------------------------------------------------------------------------");
        Console.WriteLine("\nInsert your entries row by row, separated by a space (decimals are OK). For example, for a matrix with 3 columns:");
        Console.WriteLine("ROW 1: 1 5.0 2");
        Console.WriteLine("To exit early, enter 'exit'. To skip to the next row instead, enter 'skip'.\n");

        for (int row = 0; row < matrix.GetLength(0); row++)
        {
            Console.ResetColor();
            Console.Write($"ROW {row + 1}: ");

            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine()!.Trim().ToLower();
            Console.ResetColor();
            
            if(String.IsNullOrEmpty(userInput))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Entries.");
                row--;
                continue;
            }

            else if(userInput == "exit")
            {
                break;
            }

            else if(userInput == "skip")
            {
                continue;
            }

            string[] entriesArr = userInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // Remove the extra spaces in between
            int currentCol = 0;
            double num;

            foreach (string entry in entriesArr)
            {
                if(entry == "")
                {
                    continue;
                }

                if(!double.TryParse(entry, out num) || entriesArr.Length > matrix.GetLength(1) || entriesArr.Length < matrix.GetLength(1))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid entries (The # of entries should correspond to the # of columns)");
                    row--;
                    break;
                }

                matrix[row, currentCol] = num;
                currentCol++;
            }
        }
    }

    public void ElementaryRowOperations()
    {
        int commandNumber;
        Console.WriteLine("\n------------------------------------------------------------------------------------");

        while (true)
        {
            Console.ResetColor();
            Display();

            Console.Write("\n1. Swap two rows\n2. Scale a row\n3. Add a multiple of one row to another\n4. Go back to the menu\n\nInput a command number: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine()!.Trim();

            if (string.IsNullOrEmpty(userInput) || !int.TryParse(userInput, out commandNumber) || commandNumber > 4 || commandNumber < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Command Number.");
                continue;
            }

            // Go back to the menu
            if(commandNumber == 4)
            {
                break;
            }

            // Swap two rows
            else if(commandNumber == 1)
            {
                int firstRow;
                int secondRow;
                Console.ResetColor();

                Console.Write("\nChoose the first row to swap: ");
                Console.ForegroundColor = ConsoleColor.Green;
                userInput = Console.ReadLine()!.Trim();

                if (string.IsNullOrEmpty(userInput) || !int.TryParse(userInput, out firstRow) || firstRow <= 0|| firstRow > matrix.GetLength(0))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Row Number.");
                    continue;
                }

                Console.ResetColor();
                Console.Write("Choose the second row to swap: ");
                Console.ForegroundColor = ConsoleColor.Green;
                userInput = Console.ReadLine()!.Trim();

                if (string.IsNullOrEmpty(userInput) || !int.TryParse(userInput, out secondRow) || secondRow <= 0 || secondRow > matrix.GetLength(0))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Row Number.");
                    continue;
                }

                // Begin swapping the values between the rows (since the rows don't start at 0 for the user, we decrement it by 1 beforehand)
                double tempVal;

                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    tempVal = matrix[firstRow - 1, j];
                    matrix[firstRow - 1, j] = matrix[secondRow - 1, j];
                    matrix[secondRow - 1, j] = tempVal;
                } 
            }

            // Scale a row
            else if(commandNumber == 2) 
            {
                int chosenRow;
                double scalar;

                Console.ResetColor();
                Console.Write("\nChoose a row to scale: ");
                Console.ForegroundColor = ConsoleColor.Green;
                userInput = Console.ReadLine()!.Trim();

                if (string.IsNullOrEmpty(userInput) || !int.TryParse(userInput, out chosenRow) || chosenRow <= 0 || chosenRow > matrix.GetLength(0))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Row Number.");
                    continue;
                }

                Console.ResetColor();
                Console.Write("Choose a scalar: ");
                Console.ForegroundColor = ConsoleColor.Green;
                userInput = Console.ReadLine()!.Trim();

                if (string.IsNullOrEmpty(userInput) || !double.TryParse(userInput, out scalar))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Scalar.");
                    continue;
                }

                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    // Subtract row index by one since it starts at one for the user
                    if(double.IsInfinity(matrix[chosenRow - 1, j] * scalar))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Scalar (Was your input too big?).");
                        continue;

                    }

                    // We add 0.0 to remove the possibility of getting -0.0
                    matrix[chosenRow - 1, j] = matrix[chosenRow - 1, j] * scalar + 0.0;
                }
            }

            else
            {
                break;
            }
        }
    }

    

    public void Scale()
    {
        Console.WriteLine("\n------------------------------------------------------------------------------------");
        Console.ResetColor();
        double scalar;

        Console.Write("\nInsert the scaling factor to multiply your matrix with: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string userInput = Console.ReadLine()!.Trim();
        Console.ResetColor();

        if (string.IsNullOrEmpty(userInput) || !double.TryParse(userInput, out scalar))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Scaling Factor.");
            return;
        }

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = 0; j < matrix.GetLength(1); j++)
            {

                if (double.IsInfinity(matrix[i, j] * scalar))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Chosen scaling factor is too large, please choose a smaller value.");
                    return;
                    
                }

                // We add 0.0 to remove the possibility of getting -0.0
                matrix[i, j] = matrix[i, j] * scalar + 0.0;
            }
        }
    }
}
 
