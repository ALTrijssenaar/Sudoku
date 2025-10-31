namespace Sudoku.Core.Services;

public class BoardValidator
{
    /// <summary>
    /// Validates if a cell value is valid according to Sudoku rules.
    /// </summary>
    /// <param name="board">The current board state (81-element array)</param>
    /// <param name="row">Row index (0-8)</param>
    /// <param name="col">Column index (0-8)</param>
    /// <param name="value">Value to validate (1-9, 0 for empty)</param>
    /// <returns>True if the placement is valid, false otherwise</returns>
    public bool IsValidPlacement(int[] board, int row, int col, int value)
    {
        if (value < 0 || value > 9)
            return false;
        
        if (value == 0) // Empty cell is always valid
            return true;

        // Check row
        for (int c = 0; c < 9; c++)
        {
            if (c != col && board[row * 9 + c] == value)
                return false;
        }

        // Check column
        for (int r = 0; r < 9; r++)
        {
            if (r != row && board[r * 9 + col] == value)
                return false;
        }

        // Check 3x3 box
        int boxRow = (row / 3) * 3;
        int boxCol = (col / 3) * 3;
        for (int r = boxRow; r < boxRow + 3; r++)
        {
            for (int c = boxCol; c < boxCol + 3; c++)
            {
                if ((r != row || c != col) && board[r * 9 + c] == value)
                    return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Checks if the board is completely filled.
    /// </summary>
    public bool IsBoardComplete(int[] board)
    {
        if (board.Length != 81)
            return false;

        for (int i = 0; i < 81; i++)
        {
            if (board[i] == 0)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Validates if the entire board is valid and complete.
    /// </summary>
    public bool IsValidSolution(int[] board)
    {
        if (!IsBoardComplete(board))
            return false;

        // Check all cells
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                int value = board[row * 9 + col];
                // Temporarily remove the value to check if placement would be valid
                board[row * 9 + col] = 0;
                bool isValid = IsValidPlacement(board, row, col, value);
                board[row * 9 + col] = value; // Restore value
                
                if (!isValid)
                    return false;
            }
        }

        return true;
    }
}
