namespace Sudoku.UI.Services;

public class BoardValidator
{
    /// <summary>
    /// Validates if the current board state is valid according to Sudoku rules
    /// </summary>
    public bool IsValidState(int[] board)
    {
        if (board == null || board.Length != 81)
            return false;

        // Check all rows
        for (int row = 0; row < 9; row++)
        {
            if (!IsValidGroup(GetRow(board, row)))
                return false;
        }

        // Check all columns
        for (int col = 0; col < 9; col++)
        {
            if (!IsValidGroup(GetColumn(board, col)))
                return false;
        }

        // Check all 3x3 boxes
        for (int box = 0; box < 9; box++)
        {
            if (!IsValidGroup(GetBox(board, box)))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if a value can be placed at a specific position
    /// </summary>
    public bool IsValidPlacement(int[] board, int position, int value)
    {
        if (position < 0 || position >= 81)
            return false;

        if (value < 1 || value > 9)
            return false;

        int row = position / 9;
        int col = position % 9;
        int box = (row / 3) * 3 + (col / 3);

        // Check row
        var rowValues = GetRow(board, row);
        if (rowValues.Contains(value))
            return false;

        // Check column
        var colValues = GetColumn(board, col);
        if (colValues.Contains(value))
            return false;

        // Check box
        var boxValues = GetBox(board, box);
        if (boxValues.Contains(value))
            return false;

        return true;
    }

    /// <summary>
    /// Checks if the puzzle is completely solved
    /// </summary>
    public bool IsSolved(int[] board)
    {
        // All cells must be filled
        if (board.Any(c => c == 0))
            return false;

        // Board must be valid
        return IsValidState(board);
    }

    private int[] GetRow(int[] board, int row)
    {
        return Enumerable.Range(0, 9)
            .Select(col => board[row * 9 + col])
            .Where(v => v != 0)
            .ToArray();
    }

    private int[] GetColumn(int[] board, int col)
    {
        return Enumerable.Range(0, 9)
            .Select(row => board[row * 9 + col])
            .Where(v => v != 0)
            .ToArray();
    }

    private int[] GetBox(int[] board, int box)
    {
        int startRow = (box / 3) * 3;
        int startCol = (box % 3) * 3;

        var values = new List<int>();
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                int value = board[(startRow + r) * 9 + (startCol + c)];
                if (value != 0)
                    values.Add(value);
            }
        }

        return values.ToArray();
    }

    private bool IsValidGroup(int[] group)
    {
        // Check for duplicates
        var distinct = group.Distinct().ToArray();
        return distinct.Length == group.Length;
    }
}
