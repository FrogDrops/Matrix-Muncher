using Microsoft.VisualBasic;
using static MatrixHelper.Functions;
using System;
using System.Collections.Generic;
using MatrixHelper;

Console.ResetColor();
Console.WriteLine("\nWelcome to the Matrix Calculator!\n\nTo get started, input the number of rows and columns you'd like for your matrix!");
Console.WriteLine("Matrices of up to 10 by 10 size are possible. Make sure your inputs are integers when setting up your matrix size.");

string stringRows;
string stringCols;
int numRows;
int numCols;
Matrix myMatrix;
Matrix secondMatrix;

// Program will loop in case user wants to resize their matrix 
while(true)
{
    // Ask the user their desired number of rows and columns for their matrix
    while (true)
    {
        Console.ResetColor();

        Console.Write("\nNumber of Rows (Vertical): ");
        Console.ForegroundColor = ConsoleColor.Green;
        stringRows = Console.ReadLine()!.Trim();
        Console.ResetColor();

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

        Console.Write("Number of Columns (Horizontal): ");
        Console.ForegroundColor = ConsoleColor.Green;
        stringCols = Console.ReadLine()!.Trim();
        Console.ResetColor();

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

        myMatrix = new Matrix(numRows, numCols);
        myMatrix.EntryInput();
        break;
    }

    string stringCommand;
    int userCommand;
    bool looping = true;

    // Main menu where user can perform calculations on their matrix
    while(looping)
    {
        Console.ResetColor();
        Console.WriteLine("\n------------------------------------------------------------------------------------");
        myMatrix.Display();

        Console.WriteLine("\nPlease input one of the given numbers to perform a command on your matrix:\n");
        Console.WriteLine("1. Reinput Your Entries\n2. Row Reduced Form\n3. Row Echelon Form\n4. Perform Elementary Row Operations\n5. Invert the Matrix\n6. Find the Determinant\n" +
            "7. Scale Your Matrix by a Factor\n8. Transpose Your Matrix\n" +
            "9. Multiply with Another Matrix\n10. Make a New Matrix (Discard current one)\n11. Quit the Program");
        Console.ResetColor();

        Console.Write("\nYour Command Number: ");
        Console.ForegroundColor = ConsoleColor.Green;
        stringCommand = Console.ReadLine()!.Trim();
        Console.ResetColor();

        if (String.IsNullOrEmpty(stringCommand) || !int.TryParse(stringCommand, out userCommand) || userCommand > 11 || userCommand <= 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid Command.");
            continue;
        }

        switch (userCommand)
        {
            case 1:
                myMatrix.EntryInput();
                break;

            case 2:
                RowReducedForm(myMatrix.givenMatrix);
                break;

            case 3:
                RowEchelonForm(myMatrix.givenMatrix);
                break;

            case 4:
                myMatrix.ElementaryRowOperations();
                break;

            case 5:
                InvertMatrix(myMatrix.givenMatrix);
                break;

            case 6:
                if (myMatrix.numRows != myMatrix.numCols)
                {
                    Console.WriteLine("\nSince the matrix is not square (amount of rows and columns are the same), the determinant does not exist.");

                }

                else
                {
                    double determinant = Functions.Determinant(myMatrix.givenMatrix);
                    string singularState = "non-singular";

                    if (determinant == 0)
                    {
                        singularState = "singular";
                    }

                    Console.WriteLine($"\nThe determinant of your matrix is {determinant}. It is {singularState}.");
                }

                break;

            case 7:
                myMatrix.Scale();
                break;

            case 8:
                myMatrix.Transpose();
                break;

            case 9:
                break;

            case 10:
                looping = false;
                Console.WriteLine("\n------------------------------------------------------------------------------------");
                break;

            case 11:
                System.Environment.Exit(0);
                break;
        }
    }
}


