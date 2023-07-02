# Matrix-Muncher

A C# console application that serves as a lightweight matrix calculator for all your linear algebra needs!

![muncherPic](https://github.com/FrogDrops/Matrix-Muncher/assets/130423129/45343e0d-e2c2-44bc-9dc3-4479ea7c5c97)


Matrix Muncher allows you to quickly perform operations and undo changes on your matrix as needed. It is focused on being both lenient with
your inputs while also letting you make quick, back-to-back modifications on your matrix without wasting your time.

It has the following features:
- Can put your matrix in either row reduced or row-echelon form
- Can invert and transpose your matrix
- Can provide the determinant of the matrix
- Can perform matrix multiplication, addition, and subtraction
- Can perform any of the three elementary row operations on the matrix, one at a time
- Can discard your matrix for a new one, change your entries, and save your matrix on a whim
- Can revert your matrix back to a previously saved state in case you change your mind
- Can also be goofy in some occasions!

Additionally, it is very much fraction and decimal friendly!

![menuPic](https://github.com/FrogDrops/Matrix-Muncher/assets/130423129/e0b9c50e-2554-43b1-809f-fc434a10307b)

Since the program relies on floating-point arithmetic, there may be minor precision errors. However, the results should stay correct.

# Elementary Row Operations

![rowOperations](https://github.com/FrogDrops/Matrix-Muncher/assets/130423129/562fce94-20b8-4f15-bd2a-7aab3a6fd0cd)

You can perform any of the three elementary row operations on your matrix: swapping two rows, scaling a row, and adding a multiple 
of one row to another. In such a way, you can practice attaining the row-reduced form without getting bogged down with algebraic mistakes.

To add a multiple of one row to another, one must follow this format:

r[first row number] [+/-] [scalar] * r[second row number]

Which would then affect the first row. However, this format tries to be as flexible as it can in that the scalar can take the form of 
a decimal as well as a fraction. The letter 'r' can also be capitalized if wanted. Examples of valid formats go as such:

r1 + 2 * r2 (Applies across the whole first row)
 
r3 - 1/5 * r1 (Applies across the whole third row)

R1 + 3.5 * R3 (Applies across the whole first row)

Note that, following the rules of this operation, one cannot add a multiple of one row to the same row.

# Installation Information

# [Download Release v1.0](https://github.com/FrogDrops/Matrix-Muncher/releases/tag/v1.0)

Head down to the release, and download the MatrixHelper zip file at the top. Afterwards, unzip it, double-click on the executable file, 
and then you can start messing around with your matrix!

Depending on how wary your computer is, it may mark the file as suspicious, which is normal! If you have any concerns or questions, please reach out to me.
But it's a simple console application that's safe to use! Just tell the computer to execute the 
program anyway, and enjoy <:
