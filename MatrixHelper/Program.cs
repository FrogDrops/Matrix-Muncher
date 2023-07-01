using static MatrixMuncher.Functions;

// Welcome message!
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("\r\n\r\n  __  __           _            _                     \r\n |  \\/  |         | |          (_)                    \r\n | \\  / |   __ _  | |_   _ __   _  __  __             \r\n | |\\/| |  / _` | | __| | '__| | | \\ \\/ /             \r\n | |  | | | (_| | | |_  | |    | |  >  <              \r\n |_|  |_|  \\__,_|  \\__| |_|    |_| /_/\\_\\             \r\n  __  __                          _                   \r\n |  \\/  |                        | |                  \r\n | \\  / |  _   _   _ __     ___  | |__     ___   _ __ \r\n | |\\/| | | | | | | '_ \\   / __| | '_ \\   / _ \\ | '__|\r\n | |  | | | |_| | | | | | | (__  | | | | |  __/ | |   \r\n |_|  |_|  \\__,_| |_| |_|  \\___| |_| |_|  \\___| |_|   \r\n                                                      \r\n                                                      \r");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine("\nWelcome to Matrix Muncher!\n\nTo get started, input the number of rows and columns you'd like for your matrix!" +
    "\nMatrices of up to 10 by 10 size are possible.");

string stringRows;
string stringCols;
int numRows;
int numCols;
double[,] userMatrix;
double[,] savedMatrix;
string separationBar = "\n---------------------------------------------------------------------------------------------------------------";

// Will loop in case the user wants to discard their matrix and start from scratch again
while (true)
{
    // Ask for the user's desired # of rows and columns
    while (true)
    {
        Console.ForegroundColor = ConsoleColor.White;

        // Rows
        Console.Write("\nNumber of Rows (Vertical): ");
        Console.ForegroundColor = ConsoleColor.Green;
        stringRows = Console.ReadLine()!.Trim();
        Console.ForegroundColor = ConsoleColor.White;

        if (string.IsNullOrEmpty(stringRows) || !int.TryParse(stringRows, out numRows))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input.");
            continue;
        }

        if (numRows > 10 || numRows <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Input must be greater than 0 and less than 10, please try again.");
            continue;
        }

        // Columns
        Console.Write("Number of Columns (Horizontal): ");
        Console.ForegroundColor = ConsoleColor.Green;
        stringCols = Console.ReadLine()!.Trim();
        Console.ForegroundColor = ConsoleColor.White;

        if (string.IsNullOrEmpty(stringCols) || !int.TryParse(stringCols, out numCols))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Input.");
            continue;
        }

        if (numCols > 10 || numCols <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Input must be greater than 0 and less than 10, please try again.");
            continue;
        }

        // The user's size inputs have been validated 
        userMatrix = new double[numRows, numCols];
        InputEntries(userMatrix);

        // Will store contents of the user's matrix in case they want to revert back
        savedMatrix = (double[,]) userMatrix.Clone();
        Console.WriteLine(separationBar);
        Console.WriteLine("\nYour matrix has been saved!");
        break;
    }

    double[,] secondMatrix;
    string stringCommand;
    int userCommand;
    bool looping = true;

    // The main menu for the program
    while (looping)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(separationBar);
        DisplayMatrix(userMatrix);

        Console.WriteLine("\nPlease input one of the given numbers to perform a command on your matrix:\n\n" +
            "1. Reenter Entries\n" +
            "2. Row Reduced Form\n" +
            "3. Row Echelon Form\n" +
            "4. Elementary Row Operations\n" +
            "5. Invert\n" +
            "6. Determinant\n" +
            "7. Scale Matrix\n" +
            "8. Transpose\n" +
            "9. Multiply with Another Matrix\n" +
            "10. Add / Subtract with Another Matrix\n" +
            "11. Revert back to Saved Matrix\n" +
            "12. Make a New Matrix (Discard current one)\n" +
            "13. Quit the Program");
        Console.ForegroundColor = ConsoleColor.White;

        Console.Write("\nYour Command Number: ");
        Console.ForegroundColor = ConsoleColor.Green;
        stringCommand = Console.ReadLine()!.Trim();
        Console.ForegroundColor = ConsoleColor.White;

        if (String.IsNullOrEmpty(stringCommand) || !int.TryParse(stringCommand, out userCommand) || userCommand > 13 || userCommand <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Command.");
            continue;
        }

        switch (userCommand)
        {
            case 1:
                InputEntries(userMatrix);
                savedMatrix = (double[,])userMatrix.Clone();
                Console.WriteLine(separationBar);
                Console.WriteLine("\nYour matrix has been resaved!");
                break;

            case 2:
                RowReducedForm(userMatrix);
                break;

            case 3:
                RowEchelonForm(userMatrix);
                break;

            case 4:
                ElementaryRowOperations(userMatrix);
                break;

            case 5:
                InvertMatrix(userMatrix);
                break;

            case 6:
                if (numRows != numCols)
                {
                    Console.WriteLine(separationBar);
                    Console.WriteLine("\nSince the matrix is not square (amount of rows and columns are the same), the determinant does not exist.");

                }

                else
                {
                    double determinant = Determinant(userMatrix);
                    string singularState = "non-singular";

                    if (determinant == 0)
                    {
                        singularState = "singular";
                    }

                    Console.WriteLine(separationBar);
                    Console.WriteLine($"\nThe determinant of your matrix is {determinant}. It is {singularState}.");
                }

                break;

            case 7:
                ScaleMatrix(userMatrix);
                break;

            case 8:
                Transpose(userMatrix);
                break;

            case 9:
                Console.WriteLine(separationBar);
                Console.WriteLine($"\nEnter the entries for your second {numRows} by {numCols} matrix.\n\nYour original matrix will be multiplied with this new matrix.");
                secondMatrix = new double[numRows, numCols];
                InputEntries(secondMatrix);
                userMatrix = MatrixMultiply(userMatrix, secondMatrix);
                break;

            case 10:
                Console.WriteLine(separationBar);
                Console.WriteLine($"\nEnter the entries for your second {numRows} by {numCols} matrix.\n\nYour original matrix will be summed / subtracted with this new matrix.");
                secondMatrix = new double[numRows, numCols];
                InputEntries(secondMatrix);
                userMatrix = MatrixSum(userMatrix, secondMatrix);
                break;

            case 11:
                userMatrix = savedMatrix;
                break;

            case 12:
                looping = false;
                Console.WriteLine(separationBar);
                break;

            case 13:
                System.Environment.Exit(0);
                break;
        }
    }
}




