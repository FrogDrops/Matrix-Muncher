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
        Console.WriteLine("\nInsert your entries row by row, separated by a space (decimals are OK).");
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

                if(!double.TryParse(entry, out num) || entriesArr.Length > matrix.GetLength(0) || entriesArr.Length < matrix.GetLength(0))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid entries / entries are too big");
                    row--;
                    break;
                }

                matrix[row, currentCol] = num;
                currentCol++;
            }
        }
    }

    public void Scale()
    {
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

                matrix[i, j] = matrix[i, j] * scalar;
            }
        }
    }
}
 
