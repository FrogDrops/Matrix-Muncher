using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;

Console.ResetColor();
Console.WriteLine("\nWelcome to the Matrix Calculator! To get started, input the number of rows and columns you'd like for your matrix!");
Console.WriteLine("Matrices of up to 10 by 10 size are possible. Make sure your inputs are integers when setting up your matrix size.");

string desiredRows;
string desiredCols;
int numRows;
int numCols;
Matrix myMatrix;

// Setting up the user's matrix
while(true)
{
    Console.ResetColor();

    Console.Write("\nNumber of Rows (Vertical): ");
    Console.ForegroundColor = ConsoleColor.Green;
    desiredRows = Console.ReadLine()!.Trim();
    Console.ResetColor();

    if (string.IsNullOrEmpty(desiredRows) || !int.TryParse(desiredRows, out numRows))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid Input.");
        continue;
    }

    if(numRows > 10 || numRows <= 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Input must be greater than 0 and less than 10, please try again.");
        continue;
    }

    Console.Write("Number of Columns (Horizontal): ");
    Console.ForegroundColor = ConsoleColor.Green;
    desiredCols = Console.ReadLine()!.Trim();
    Console.ResetColor();

    if (string.IsNullOrEmpty(desiredCols) || !int.TryParse(desiredCols, out numCols))
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

string desiredCommand;
int userCommand;

while(true)
{
    myMatrix.Display();
    Console.WriteLine("\nPlease input one of the given numbers to perform a command on your matrix:");
    Console.WriteLine("1. Reinput Your Entries\n2. Row Reduce\n3. Perform Elementary Operations\n4. Invert the Matrix\n5. Find the Determinant\n" +
        "6. Scale Your Matrix by a Factor\n7. Clear Your Matrix\n" +
        "8. Make a New Matrix (Discard current one)\n9. Quit the Program");
    Console.ResetColor();

    Console.Write("\nYour Command Number: ");
    Console.ForegroundColor = ConsoleColor.Green;
    desiredCommand = Console.ReadLine()!.Trim();
    Console.ResetColor();

    if(String.IsNullOrEmpty(desiredCommand) || !int.TryParse(desiredCommand, out userCommand) || userCommand > 11 || userCommand <= 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid Command.");
        continue;
    }

    switch(userCommand)
    {
        case 1:
            myMatrix.EntryInput();
            break;

        case 2:
            break;

        case 3:
            break;

        case 4:
            break;

        case 5:
            break;

        case 6:
            myMatrix.Scale();
            break;

        case 7:
            break;
        case 8:
            System.Environment.Exit(0);
            break;
    }
}

