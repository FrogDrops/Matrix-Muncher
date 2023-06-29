using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatrixHelper;

namespace MatrixHelper
{
    class Functions
    {
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

        // Augment the matrix with its identity matrix and row reduce to find its inverse
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
    }
}
