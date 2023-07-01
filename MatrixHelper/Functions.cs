using System.Text.RegularExpressions;

namespace MatrixMuncher
{
    class Functions
    {
        static string separationBar = "\n---------------------------------------------------------------------------------------------------------------";
        public static void DisplayMatrix(double[,] matrix)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);
            int[] maxEntryLengths = new int[numCols];

            // Find the maximum length of an entry in each column (to compare later with every other entry in that column)
            for (int col = 0; col < numCols; col++)
            {
                for (int row = 0; row < numRows; row++)
                {
                    int entryLength = matrix[row, col].ToString().Length;
                    if (entryLength > maxEntryLengths[col])
                    {
                        maxEntryLengths[col] = entryLength;
                    }
                }
            }

            // Display the matrix with aligned entries
            for (int row = 0; row < numRows; row++)
            {
                Console.Write("\n|     ");
                for (int col = 0; col < numCols; col++)
                {
                    string entryStr = matrix[row, col].ToString();
                    int numSpaces = maxEntryLengths[col] - entryStr.Length;

                    // Add extra spaces to align the columns
                    Console.Write(entryStr + new string(' ', numSpaces + 5));
                }
                Console.WriteLine("|");
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void InputEntries(double[,] matrix)
        {
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(separationBar);
            Console.WriteLine($"\nInsert your {numCols} entries row by row, separated by a space.\n\nFor example, for a matrix with 3 columns:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\n ROW 1: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1/2 5 2.5");
            Console.WriteLine("\nTo exit the operation, enter 'exit'. To skip to the next row instead (and leave the current row intact), press enter.\n");


            for (int row = 0; row < numRows; row++)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

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
                Console.ForegroundColor = ConsoleColor.White;

                // Skip the row
                if (String.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine();
                    continue;
                }

                // Exit to the menu
                else if (userInput == "exit")
                {
                    break;
                }

                string[] entriesArr = userInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string fractionPattern = @"^(-?\d+)\/(\d+)$";
                int currentCol = 0;
                double num;

                foreach (string entry in entriesArr)
                {
                    if (entry == "")
                    {
                        continue;
                    }

                    // User inputted a fraction instead
                    Match match = Regex.Match(entry, fractionPattern);

                    if(match.Success)
                    {
                        double num1 = double.Parse(match.Groups[1].Value);
                        double num2 = double.Parse(match.Groups[2].Value);

                        if(num2 == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" Invalid Fraction. You cannot divide by 0 :P");
                            row--;
                            break;
                        }

                        num = num1 / num2;
                    }

                    else if (!double.TryParse(entry, out num) || entriesArr.Length > numCols || entriesArr.Length < numCols)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" Invalid entries (The # of entries should correspond to the # of columns)");
                        row--;
                        break;
                    }

                    matrix[row, currentCol] = num;
                    currentCol++;
                }

                Console.WriteLine();
            }
        }

        /*
         * A "mini-menu" where the user can perform one of three elementary operations on their matrix (with their own functions):
         * 1. Swap two rows
         * 2. Scale a row by a factor
         * 3. Add a multiple of one row to another row
         */
        public static void ElementaryRowOperations(double[,] matrix)
        {
            int commandNumber;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(separationBar);
                DisplayMatrix(matrix);

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
            Console.ForegroundColor = ConsoleColor.White;
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

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Choose the second row to swap: ");
            Console.ForegroundColor = ConsoleColor.Green;
            userInput = Console.ReadLine()!.Trim();

            if (!int.TryParse(userInput, out secondRow) || secondRow <= 0 || secondRow > numRows || firstRow == secondRow)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Row Number (Make sure your rows are not the same).");
                return;
            }

            // Begin swapping the values between the rows (user assumes rows start at 1)
            double tempVal;

            for (int j = 0; j < numCols; j++)
            {
                tempVal = matrix[firstRow - 1, j];
                matrix[firstRow - 1, j] = matrix[secondRow - 1, j];
                matrix[secondRow - 1, j] = tempVal;
            }

        }
        
        public static void RowScale(double[,] matrix)
        {
            Console.ForegroundColor = ConsoleColor.White;
            int chosenRow;
            double scalar;
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);
            string fractionPattern = @"^(-?\d+)\/(\d+)$";

            Console.Write("\nChoose a row to scale: ");
            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine()!.Trim();
            Console.ForegroundColor = ConsoleColor.White;

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

                if(num2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Fraction. You cannot divide by 0 :P");
                    return;

                }

                scalar = num1 / num2;
            }

            else if (!double.TryParse(userInput, out scalar))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Scalar.");
                return;
            }

            // User inputted a valid scalar (that's not a fraction)
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

        public static void RowMultipleSum(double[,] matrix)
        {
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);
            string inputPattern = @"^r(\d+)\s([+\-])\s(-?\d+(\.\d+)?|\d+/\d+)\s\*\sr(\d+)$";
            string fractionPattern = @"^(-?\d+(\.\d+)?)/(\d+)$";

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nWrite your operation in this format: r[first row number] [+/-] [scalar] * r[second row number]");
            Console.WriteLine("Spaces are important, and fractions are okay for scalars! This operation will affect the first row number." +
                "\n\nEnter 'exit' to exit this option.\n");
            Console.Write("Your Operation: ");

            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine()!.Trim().ToLower();
            Console.ForegroundColor = ConsoleColor.White;

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
            string stringScalar = match.Groups[3].Value;
            double scalar;
            int secondRow = int.Parse(match.Groups[5].Value);

            Match fractionMatch = Regex.Match(stringScalar, fractionPattern);

            // If user chose a fraction as their scalar
            if(fractionMatch.Success)
            {
                double numerator = double.Parse(fractionMatch.Groups[1].Value);
                double denominator = double.Parse(fractionMatch.Groups[3].Value);

                if(denominator == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Fraction. You cannot divide by 0 :P");
                    return;
                }

                scalar = numerator / denominator;
            }

            else
            {
                scalar = double.Parse(stringScalar);

            }

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

        public static void ScaleMatrix(double[,] matrix)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(separationBar);

            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);
            double scalar;
            string fractionPattern = @"^(-?\d+)\/(\d+)$";

            Console.Write("\nChoose a scalar (Fractions okay!): ");
            Console.ForegroundColor = ConsoleColor.Green;
            string userInput = Console.ReadLine()!.Trim();
            Console.ResetColor();

            Match match = Regex.Match(userInput, fractionPattern);

            // User entered a fraction instead
            if (match.Success)
            {
                double num1 = double.Parse(match.Groups[1].Value);
                double num2 = double.Parse(match.Groups[2].Value);

                if(num2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Fraction. You cannot divide by 0 :P");
                    return;
                }

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

        public static void Transpose(double[,] matrix)
        {
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);

            if (numRows != numCols)
            {
                Console.WriteLine(separationBar);
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

            Console.WriteLine(separationBar);
            Console.WriteLine("\nYour matrix has been transposed!");
        }

        // restrainedCol describes the column number up to which the operations should be performed (useful for inverting the matrix)
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
            // The first half of our work is done here
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
            Console.ForegroundColor = ConsoleColor.White;
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);

            // Make sure the matrix is invertible first
            if(numRows != numCols || Determinant(matrix) == 0)
            {
                Console.WriteLine(separationBar);
                Console.WriteLine("\nSince the determinant of the matrix is 0, its inverse does not exist.");
                return;
            }

            // Transfer info from the user's matrix to our new matrix
            double[,] augmentedMatrix = new double[numRows, numCols * 2];

            for (int row = 0; row < numRows; row++)
            {
                for(int col = 0; col < numCols; col++)
                {
                    augmentedMatrix[row, col] = matrix[row, col];
                }
            }

            // Augment our new matrix with its identity matrix
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

            // Right half of row-reduced augmented matrix contains the inverse of the original matrix
            for(int row = 0; row < numRows; row++)
            {
                for(int col = 0; col < numCols; col++)
                {
                    // Truncate anything past five decimal places
                    matrix[row, col] = Math.Floor(augmentedMatrix[row, col + numCols] * 100000) / 100000;
                }
            }

            Console.WriteLine(separationBar);
            Console.WriteLine($"\nYour matrix has been inverted!");
        }

        // Matrix multiplication is not commutative. It will always be performed in this order: Matrix A x Matrix B
        public static double[,] MatrixMultiply(double[,] matrixA, double[,] matrixB)
        {     
            int numRows = matrixA.GetLength(0);
            int numCols = matrixA.GetLength(1);

            double[,] resultingMatrix = new double[numRows, numCols];

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nYour original matrix:");
            DisplayMatrix(matrixA);
            Console.WriteLine("\n\nTimes the Second Matrix:");
            DisplayMatrix(matrixB);
            Console.WriteLine();

            // Matrix only contains one element
            if (numRows == 1 && numCols == 1)
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

        public static double[,] MatrixSum(double[,] matrixA, double[,] matrixB)
        {
            int numRows = matrixA.GetLength(0);
            int numCols = matrixA.GetLength(1);

            double[,] resultingMatrix = new double[numRows, numCols];

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(separationBar);
            Console.WriteLine("\nYour original matrix:");
            DisplayMatrix(matrixA);
            Console.WriteLine("\n\nSecond Matrix:");
            DisplayMatrix(matrixB);

            Console.Write("\nChoose your operation ( + / - ): ");
            string operation = Console.ReadLine()!.Trim();

            if(operation != "+" && operation != "-")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Operation.");
                return matrixA;
            }

            // Begin summation / subtraction between two matrices
            for(int i = 0; i < numRows; i++)
            {
                for(int j = 0; j < numCols; j++)
                {
                    if ((operation == "+" && double.IsInfinity(matrixA[i, j] + matrixB[i, j])) ||
                    (operation == "-" && double.IsInfinity(matrixA[i, j] - matrixB[i, j])))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Calculations surpass infinity.");
                        return matrixA;
                    }

                    else if (operation == "+")
                    {
                        resultingMatrix[i, j] = matrixA[i, j] + matrixB[i, j];
                    }

                    else
                    {
                        resultingMatrix[i, j] = matrixA[i, j] - matrixB[i, j];
                    }
                }
            }

            return resultingMatrix;
        }
    }
}
