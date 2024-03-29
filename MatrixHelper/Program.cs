﻿using static MatrixMuncher.Functions;

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
Random rand = new Random();

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
            "1. Reenter Entries (And Save It)\n" +
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
        stringCommand = Console.ReadLine()!.Trim().ToLower();
        Console.ForegroundColor = ConsoleColor.White;

        // Congratulate them on their birthday :D
        if(stringCommand == "it's my birthday" || stringCommand == "it's my birthday!")
        {
            Console.WriteLine(separationBar);
            Console.WriteLine("\n                0   0\r\n                |   |\r\n            ____|___|____\r\n         0  |~ ~ ~ ~ ~ ~|   0\r\n         |  |           |   |\r\n      ___|__|___________|___|__\r\n      |/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/|\r\n  0   |       H a p p y       |   0\r\n  |   |/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/|   |\r\n _|___|_______________________|___|__\r\n|/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/|\r\n|                                   |\r\n|         B i r t h d a y! ! !      |\r\n| ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ |\r\n|___________________________________|\r\n");
            Console.WriteLine("Here's to another year! Matrix Muncher wishes you well! :D\n\n(Press any key to continue)\n");
            Console.ReadKey();
            continue;
        }

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
                savedMatrix = (double[,]) userMatrix.Clone();
                Console.WriteLine(separationBar);
                Console.WriteLine("\nYour matrix has been resaved!");
                break;

            case 2:
                Console.WriteLine(separationBar);
                Console.WriteLine("\nBOOM! ROW REDUCED!");
                RowReducedForm(userMatrix);
                break;

            case 3:
                Console.WriteLine(separationBar);
                Console.WriteLine("\nEnemy has been brought to Echelon form. I REPEAT, ENEMY HAS BEEN BROUGHT TO ECHELON FORM.");
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
                Console.WriteLine($"\nEnter the entries for your second {numRows} by {numCols} matrix.\n\nYour original matrix will be multiplied with this new matrix (Original Matrix X New Matrix).");
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
                userMatrix = (double[,]) savedMatrix.Clone();
                Console.WriteLine(separationBar);
                Console.WriteLine("\nThe clock has rewound and time begins again!");
                break;

            case 12:
                looping = false;
                Console.WriteLine(separationBar);
                Console.WriteLine("\nYour last matrix has been erased from existence. Turned to ashes.");
                Console.WriteLine(separationBar);
                break;

            case 13:
                // Say goodbye before terminating
                string[] farewell = {"\nIt's sad, but sometimes moving on with the rest of your life starts with goodbye.\n\n- Sun Tzu", "\nMy marriage is falling apart", "\nTHAT'S ANOTHER TETRIS FOR JONAS!", "\nSometimes, I pause to look at myself. Then, I really regret it.\n\n - Sun Tzu", "\nOh. I guess this is it then.", "\nIt's been great knowing you, except for the times when it wasn't great knowing you.", "\nOh.", "\nCya!" };

                Console.ForegroundColor = ConsoleColor.Yellow;
                int randomQuote = rand.Next(0, farewell.Length);
                Console.WriteLine(farewell[randomQuote]);
                Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.White;
                System.Environment.Exit(0);
                break;
        }
    }
}




