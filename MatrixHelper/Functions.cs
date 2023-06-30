using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using MatrixHelper;

namespace MatrixHelper
{
    class Functions
    {
        public static void Display(double[,] matrix)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            int counter = 1;
            int numCols = matrix.GetLength(1);

            Console.Write("\n|    ");

            foreach (double entry in matrix)
            {
                int numSpaces = 5 - entry.ToString().Length; // Take into account how we space our entries depending on their size to neatly display our matrix

                if (numSpaces <= 0)
                {
                    numSpaces = 4;

                }

                string repeatedSpace = new string(' ', numSpaces);
                if (counter > numCols) // We've reached the end of a row 
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

        public static void EntryInput(double[,] matrix)
        {
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);

            Console.ResetColor();
            Console.WriteLine("\n------------------------------------------------------------------------------------");
            Console.WriteLine($"\nInsert your {numCols} entries row by row, separated by a space. Entries are 0 by default, and decimals are okay!\nFor example, for a matrix with 3 columns:\n");
            Console.WriteLine(" ROW 1: 1 5 2.5");
            Console.WriteLine("\nTo exit the operation, enter 'exit'. To skip to the next row instead (and leave the current row intact), press enter.\n");


            for (int row = 0; row < numRows; row++)
            {
                Console.ResetColor();

                // Accounting for extra space that two-digit numbers take up (The row numbers show up differently to the user by plus 1)
                if (row < 9)
                {
                    Console.Write($" ROW {row + 1}: ");
                }

                else
                {
                    Console.Write($"ROW {row + 1}: ");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                string userInput = Console.ReadLine()!.Trim().ToLower();
                Console.ResetColor();

                // Skip the row
                if (String.IsNullOrEmpty(userInput))
                {
                    continue;
                }

                // Exit to the menu
                else if (userInput == "exit")
                {
                    break;
                }

                string[] entriesArr = userInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // Remove the extra spaces in between
                int currentCol = 0;
                double num;

                foreach (string entry in entriesArr)
                {
                    if (entry == "")
                    {
                        continue;
                    }

                    if (!double.TryParse(entry, out num) || entriesArr.Length > numCols || entriesArr.Length < numCols)
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

        // Have user perform their chosen operations on the matrix
        public static void ElementaryRowOperations(double[,] matrix)
        {
            int commandNumber;

            Console.ResetColor();
            Console.WriteLine("\n------------------------------------------------------------------------------------");

            while (true)
            {
                Console.ResetColor();
                Display(matrix);

                Console.Write("\n1. Swap two rows\n2. Scale a row\n3. Add a multiple of one row to another\n4. Go back to the menu\n\nInput a command number: ");
                Console.ForegroundColor = ConsoleColor.Green;
                string userInput = Console.ReadLine()!.Trim();

                if (!int.TryParse(userInput, out commandNumber) || commandNumber > 4 || commandNumber < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Command Number.");
                    continue;
                }

                switch (commandNumber)
                {
                    case 1:
                        SwapRows(matrix);
                        break;

                    case 2:
                        RowScale(matrix);
                        break;

                    case 3:
                        RowMultipleSum(matrix);
                        break;

                    case 4:
                        return;
                }

                Console.WriteLine();
            }
        }

        public static void SwapRows(double[,] matrix)
        {
            Console.ResetColor();
            int firstRow;
            int secondRow;
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);

            Console.Write("\nChoose the first row to swap: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine()!.Trim();

            if (!int.TryParse(userInput, out firstRow) || firstRow <= 0 || firstRow > numRows)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Row Number.");
                return;
            }

            Console.ResetColor();
            Console.Write("Choose the second row to swap: ");
            Console.ForegroundColor = ConsoleColor.Green;
            userInput = Console.ReadLine()!.Trim();

            if (!int.TryParse(userInput, out secondRow) || secondRow <= 0 || secondRow > numRows || firstRow == secondRow)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Row Number (Make sure your rows are not the same).");
                return;
            }

            // Begin swapping the values between the rows (since the rows don't start at 0 for the user, we decrement it by 1 beforehand)
            double tempVal;

            for (int j = 0; j < numCols; j++)
            {
                tempVal = matrix[firstRow - 1, j];
                matrix[firstRow - 1, j] = matrix[secondRow - 1, j];
                matrix[secondRow - 1, j] = tempVal;
            }

        }
        
        // Scale a single row by a number
        public static void RowScale(double[,] matrix)
        {
            Console.ResetColor();
            int chosenRow;
            double scalar;
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);
            string fractionPattern = @"^(\d+)\/(\d+)$";

            Console.Write("\nChoose a row to scale: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine()!.Trim();
            Console.ResetColor();

            if (!int.TryParse(userInput, out chosenRow) || chosenRow <= 0 || chosenRow > numRows)
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

            for (int j = 0; j < numCols; j++)
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

        // Add a multiple of one row to another
        public static void RowMultipleSum(double[,] matrix)
        {
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);
            string inputPattern = @"^r(\d+)\s([+\-])\s(-?\d+(\.\d+)?)\s\*\sr(\d+)$";

            Console.ResetColor();
            Console.WriteLine("\nWrite your operation in this format: r[first row number] [+/-] [scalar] * r[second row number]");
            Console.WriteLine("Make sure to input spaces. If you want to use a fraction as a scalar, enter 'fraction'. \nFor example:\n");

            Console.WriteLine("r1 + 3 * r2\nr2 - 2.5 * r1");

            Console.WriteLine("\nEnter 'exit' to exit this option.\n");
            Console.Write("Your Operation: ");

            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine()!.Trim().ToLower();
            Console.ResetColor();

            Match match = Regex.Match(userInput, inputPattern);

            if (userInput == "exit")
            {
                return;
            }

            if (!match.Success)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Input (Did your input follow the format?).");
                return;
            }

            int firstRow = int.Parse(match.Groups[1].Value);
            string sign = match.Groups[2].Value;
            double scalar = double.Parse(match.Groups[3].Value);
            int secondRow = int.Parse(match.Groups[5].Value);

            if (firstRow <= 0 || firstRow > numRows || secondRow <= 0 || secondRow > numRows || firstRow == secondRow)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Input (Did your input follow the format? Make sure your rows are not the same).");
                return;
            }

            for (int j = 0; j < numCols; j++)
            {
                if (sign == "+")
                {
                    if (double.IsInfinity(matrix[firstRow - 1, j] + scalar * matrix[secondRow - 1, j]))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Inputted values are too big.");
                        return;
                    }

                    // We add 0.0 to remove the possibility of getting -0.0
                    matrix[firstRow - 1, j] = matrix[firstRow - 1, j] + scalar * matrix[secondRow - 1, j] + 0.0;
                }

                if (sign == "-")
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
        public static void Scale(double[,] matrix)
        {
            Console.WriteLine("\n------------------------------------------------------------------------------------");
            Console.ResetColor();
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);
            double scalar;
            string fractionPattern = @"^(\d+)\/(\d+)$";

            Console.Write("\nInsert the scaling factor to apply to your matrix. You may enter your scalar in fraction form (e.g. 1/3).\n\nYour Scalar: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine()!.Trim();
            Console.ResetColor();

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
                Console.WriteLine("Invalid Scaling Factor (Did you perhaps try dividing by 0?).");
                return;
            }

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
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
        public static void Transpose(double[,] matrix)
        {
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);

            if (numRows != numCols)
            {
                Console.WriteLine("\nSince the matrix is not square (amount of rows and columns are the same), it cannot be transposed.");
                return;

            }

            for (int i = 0; i < numRows; i++)
            {
                for (int j = i + 1; j < numRows; j++)
                {
                    double tempVal = matrix[i, j];
                    matrix[i, j] = matrix[j, i];
                    matrix[j, i] = tempVal;
                }
            }
        }
        // restrainedCol describes the column number up to which the operations should be performed (to leave the rest of the matrix intact)
        public static double[,] RowEchelonForm(double[,] matrix, int restrainedCol = 0)
        {
            
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);

            for(int col = 0; col < numCols - restrainedCol; col++)
            {
                int? pivotRow = null;

                for (int row = col; row < numRows; row++)
                {
                    // Non-zero pivot row found
                    if (matrix[row, col] != 0)
                    {
                        pivotRow = row;
                        break;
                    }    
                }

                // Pivot element not found in the first column, move on
                if (!pivotRow.HasValue)
                {
                    continue;
                }

                // Make all the other elements below the pivot element 0 (and carry the change across the affected row)
                double scaleFactor;

                for (int row = (int)pivotRow + 1; row < numRows; row++)
                {
                    if (matrix[row, col] != 0)
                    {
                        scaleFactor = matrix[row, col] / matrix[(int)pivotRow, col];

                        for(int j = col; j < numCols; j++)
                        {
                            matrix[row, j] = matrix[row, j] - scaleFactor * matrix[(int)pivotRow, j];
                        }
                    }
                }
            }

            return matrix;
        }

        public static double[,] RowReducedForm(double[,] matrix, int restrainedCol = 0)
        {
            matrix = RowEchelonForm(matrix, restrainedCol);

            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);
            
            bool[] visitedPivotRows = new bool[numRows];
            double pivotElement;

            for(int col = 0; col < numCols - restrainedCol; col++)
            {
                int pivotRow = -1;
                for (int row = numRows - 1; row >= 0; row--)
                {
                    // Ensures we don't perform reduction on the same pivot row more than once
                    if (matrix[row, col] != 0 && !visitedPivotRows[row])
                    {
                        pivotRow = row;
                        visitedPivotRows[row] = true;
                        break;
                    }
                }

                if(pivotRow == -1)
                {
                    continue;
                }

                // Divide the entire pivot row so that the pivot element is equal to 1
                pivotElement = matrix[(int)pivotRow, col];
                for(int j = col; j < numCols; j++)
                {
                    matrix[(int)pivotRow, j] /= pivotElement;
                }

                // Make all the other elements above the pivot element 0 (and carry the change across the affected row)
                double scaleFactor;
                for (int row = (int)pivotRow - 1; row >= 0; row--)
                {
                    if (matrix[row, col] != 0)
                    {
                        scaleFactor = matrix[row, col];

                        for (int j = col; j < numCols; j++)
                        {
                            matrix[row, j] = matrix[row, j] - scaleFactor * matrix[(int)pivotRow, j];
                        }
                    }
                }
            }

            return matrix;
        }

        public static double Determinant(double[,] matrix)
        {
            // We don't want to change the user's matrix, only use it as a reference
            double[,] reducedMatrix = RowEchelonForm((double[,])matrix.Clone());
            double determinant = 1;

            for (int i = 0; i < reducedMatrix.GetLength(0); i++)
            {
                determinant *= reducedMatrix[i, i];
            }

            return determinant + 0.0;
        }

        // Done via Gaussian Elimination
        public static void InvertMatrix(double[,] matrix)
        {
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);

            // Make sure the matrix is invertible first
            if(numRows != numCols || Determinant(matrix) == 0)
            {
                Console.WriteLine("\nSince the determinant of the matrix is 0, its inverse does not exist.");
                return;
            }

            double[,] augmentedMatrix = new double[numRows, numCols * 2];

            for (int row = 0; row < numRows; row++)
            {
                for(int col = 0; col < numCols; col++)
                {
                    augmentedMatrix[row, col] = matrix[row, col];
                }
            }

            // Augment with identity matrix
            for (int row = 0; row < numRows; row++)
            {
                for (int col = numCols; col < augmentedMatrix.GetLength(1); col++)
                {
                    if(row + numCols == col)
                    {
                        augmentedMatrix[row, col] = 1;
                    }

                    else
                    {
                        augmentedMatrix[row, col] = 0;
                    }
                    
                }
            }

           augmentedMatrix = RowReducedForm(augmentedMatrix, numCols);

            // Right half of augmented matrix contains the inverse of the original matrix
            for(int row = 0; row < numRows; row++)
            {
                for(int col = 0; col < numCols; col++)
                {
                    matrix[row, col] = augmentedMatrix[row, col + numCols];
                }
            }

        }
        public static double[,] MatrixMultiply(double[,] matrixA, double[,] matrixB)
        {
            int numRows = matrixA.GetLength(0);
            int numCols = matrixA.GetLength(1);

            double[,] resultingMatrix = new double[numRows, numCols];
    
            // Matrix only contains one element
            if(numRows == 1 && numCols == 1)
            {
                if (double.IsInfinity(Math.Pow(matrixA[0, 0], 2)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Calculations surpass infinity. ");
                    return matrixA;
                }

                resultingMatrix[0, 0] = Math.Pow(matrixA[0, 0], 2);
                return resultingMatrix;
            }

            for(int i = 0; i < numRows; i++)
            {
                for(int j = 0; j < numCols; j++)
                {
                    for(int k = 0; k < numCols; k++)
                    {
                        if (double.IsInfinity(resultingMatrix[i ,j] + matrixA[i, k] * matrixB[k, j]))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Calculations surpass infinity. ");
                            return matrixA;
                        }

                        resultingMatrix[i, j] += matrixA[i, k] * matrixB[k, j];
                    }
                }
            }

            return resultingMatrix;
        }
    }
}
