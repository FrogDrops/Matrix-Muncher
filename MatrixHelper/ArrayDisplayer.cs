using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

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
        Console.ResetColor();
        Console.WriteLine("\n------------------------------------------------------------------------------------");
        Console.WriteLine($"\nInsert your {matrix.GetLength(1)} entries row by row, separated by a space. Decimals are okay!\nFor example, for a matrix with 3 columns:\n");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("ROW 1: 1 5 2.5");
        Console.ResetColor();
        Console.WriteLine("\nTo exit early, enter 'exit'. To skip to the next row instead, press enter.\n");

        for (int row = 0; row < matrix.GetLength(0); row++)
        {
            Console.ResetColor();
            Console.Write($"ROW {row + 1}: ");

            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine()!.Trim().ToLower();
            Console.ResetColor();
            
            // Skip the row
            if(String.IsNullOrEmpty(userInput))
            {
                continue;
            }

            // Exit to the menu
            else if(userInput == "exit")
            {
                break;
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

    public void RowReduce()
    {
        
    }

    // Have user themself perform their chosen operations on the matrix
    public void ElementaryRowOperations()
    {
        int commandNumber;
        Console.ResetColor();
        Console.WriteLine("\n------------------------------------------------------------------------------------");

        while(true)
        {
            Console.ResetColor();
            Display();

            Console.Write("\n1. Swap two rows\n2. Scale a row\n3. Add a multiple of one row to another\n4. Go back to the menu\n\nInput a command number: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine()!.Trim();

            if (!int.TryParse(userInput, out commandNumber) || commandNumber > 4 || commandNumber < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Command Number.");
                continue;
            }

            switch(commandNumber)
            {     
                case 1:
                    SwapRows();
                    break;

                case 2:
                    RowScale();
                    break;

                case 3:
                    RowMultipleSum();
                    break;

                case 4:
                    return;
            }

            Console.WriteLine();
        }
    }

    // Have the user two rows, called in ElementaryRowOperation()
    public void SwapRows()
    {
        Console.ResetColor();
        int firstRow;
        int secondRow;
        
        Console.Write("\nChoose the first row to swap: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string userInput = Console.ReadLine()!.Trim();
        
        if (!int.TryParse(userInput, out firstRow) || firstRow <= 0 || firstRow > matrix.GetLength(0))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Row Number.");
            return;
        }
        
        Console.ResetColor();
        Console.Write("Choose the second row to swap: ");
        Console.ForegroundColor = ConsoleColor.Green;
        userInput = Console.ReadLine()!.Trim();
        
        if (!int.TryParse(userInput, out secondRow) || secondRow <= 0 || secondRow > matrix.GetLength(0) || firstRow == secondRow)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Row Number (Make sure your rows are not the same).");
            return;
        }
        
        // Begin swapping the values between the rows (since the rows don't start at 0 for the user, we decrement it by 1 beforehand)
        double tempVal;
        
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            tempVal = matrix[firstRow - 1, j];
            matrix[firstRow - 1, j] = matrix[secondRow - 1, j];
            matrix[secondRow - 1, j] = tempVal;
        }
        
    }

    // Scale a single row by a number, called in ElementaryRowOperations()
    public void RowScale()
    {
        Console.ResetColor();
        int chosenRow;
        double scalar;
        string fractionPattern = @"^(\d+)\/(\d+)$";

        Console.Write("\nChoose a row to scale: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string userInput = Console.ReadLine()!.Trim();
        Console.ResetColor(); 

        if (!int.TryParse(userInput, out chosenRow) || chosenRow <= 0 || chosenRow > matrix.GetLength(0))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Row Number.");
            return;
        }

        Console.Write("Choose a scalar. You may enter your scalar in fraction form (e.g. 1/3).\n\nYour Scalar: ");
        Console.ForegroundColor = ConsoleColor.Green;
        userInput = Console.ReadLine()!.Trim();

        Match match = Regex.Match(userInput, fractionPattern);

        // User entered a fraction instead
        if (match.Success)
        {
            double num1 = double.Parse(match.Groups[1].Value);
            double num2 = double.Parse(match.Groups[2].Value);
            scalar = num1 / num2;
        }

        else if (!double.TryParse(userInput, out scalar))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Scalar.");
            return;
        }

        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            // Subtract row index by one since it starts at one for the user
            if (double.IsInfinity(matrix[chosenRow - 1, j] * scalar))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Scalar (Was your input too big?).");
                return;
            }
            // We add 0.0 to remove the possibility of getting -0.0
            matrix[chosenRow - 1, j] = matrix[chosenRow - 1, j] * scalar + 0.0;  
        }
    }

    // Add a multiple of one row to another, called in ElementaryRowOperations()
    public void RowMultipleSum()
    {
        string inputPattern = @"^r(\d+)\s([+\-])\s(-?\d+(\.\d+)?)\s\*\sr(\d+)$";

        Console.ResetColor();
        Console.WriteLine("\nWrite your operation in this format: r[first row number] [+/-] [scalar] * r[second row number]");
        Console.WriteLine("Make sure to input spaces. If you want to use a fraction as a scalar, enter 'fraction'. \nFor example:\n");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("r1 + 3 * r2\nr2 - 2.5 * r1");
        Console.ResetColor();

        Console.WriteLine("\nEnter 'exit' to exit this option.\n");
        Console.Write("Your Operation: ");

        Console.ForegroundColor = ConsoleColor.Green;
        string userInput = Console.ReadLine()!.Trim().ToLower();
        Console.ResetColor();

        Match match = Regex.Match(userInput, inputPattern);

        if(userInput == "exit")
        {
            return;
        }

        if(!match.Success)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input (Did your input follow the format?).");
            return;
        }

        int firstRow =  int.Parse(match.Groups[1].Value);
        string sign = match.Groups[2].Value;
        double scalar = double.Parse(match.Groups[3].Value);
        int secondRow = int.Parse(match.Groups[5].Value);

        if(firstRow <= 0 || firstRow > matrix.GetLength(0) || secondRow <= 0 || secondRow > matrix.GetLength(0) || firstRow == secondRow)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input (Did your input follow the format? Make sure your rows are not the same).");
            return;
        }

        for(int j = 0; j < matrix.GetLength(1); j++)
        {
            if(sign == "+")
            {
                if(double.IsInfinity(matrix[firstRow - 1, j] + scalar * matrix[secondRow - 1, j]))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Inputted values are too big.");
                    return;
                }

                // We add 0.0 to remove the possibility of getting -0.0
                matrix[firstRow - 1, j] = matrix[firstRow - 1, j] + scalar * matrix[secondRow - 1, j] + 0.0;
            }

            if(sign == "-")
            {
                if (double.IsInfinity(matrix[firstRow - 1, j] - scalar * matrix[secondRow - 1, j]))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Inputted values are too big.");
                    return;
                }

                matrix[firstRow - 1, j] = matrix[firstRow - 1, j] - scalar * matrix[secondRow - 1, j] + 0.0;
            }
        }


    }

    // Scale the entire matrix by a number
    public void Scale()
    {
        Console.WriteLine("\n------------------------------------------------------------------------------------");
        Console.ResetColor();
        double scalar;
        string fractionPattern = @"^(\d+)\/(\d+)$";
       
        Console.Write("\nInsert the scaling factor to apply to your matrix. You may enter your scalar in fraction form (e.g. 1/3).\n\nYour Scalar: ");
        Console.ForegroundColor = ConsoleColor.Green;
        string userInput = Console.ReadLine()!.Trim();
        Console.ResetColor();

        Match match = Regex.Match(userInput, fractionPattern);

        // User entered a fraction instead
        if(match.Success)
        {
            double num1 = double.Parse(match.Groups[1].Value);
            double num2 = double.Parse(match.Groups[2].Value);
            scalar = num1 / num2;
        }

        else if (!double.TryParse(userInput, out scalar))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Scaling Factor (Did you perhaps try dividing by 0?).");
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

    // Transpose the user's matrix (swap the rows with the columns)
    public void Transpose()
    {
        if(matrix.GetLength(0) != matrix.GetLength(1))
        {
            Console.WriteLine("\nSince the matrix is not square (amount of rows and columns are the same), it cannot be transposed.");
            return;

        }

        for(int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = i + 1; j < matrix.GetLength(0); j++)
            {
                double tempVal = matrix[i, j];
                matrix[i, j] = matrix[j, i];
                matrix[j, i] = tempVal;
            }
        }
    }
}
 
