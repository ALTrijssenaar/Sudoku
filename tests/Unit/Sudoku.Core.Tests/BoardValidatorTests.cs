using Sudoku.Core.Services;

namespace Sudoku.Core.Tests;

public class BoardValidatorTests
{
    private readonly BoardValidator _validator;

    public BoardValidatorTests()
    {
        _validator = new BoardValidator();
    }

    [Fact]
    public void IsValidPlacement_ValidPlacement_ReturnsTrue()
    {
        // Arrange - empty board
        var board = new int[81];
        
        // Act - place a 5 at position (0,0)
        var result = _validator.IsValidPlacement(board, 0, 0, 5);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidPlacement_DuplicateInRow_ReturnsFalse()
    {
        // Arrange - board with a 5 in row 0
        var board = new int[81];
        board[0] = 5; // (0,0) = 5
        
        // Act - try to place another 5 in the same row
        var result = _validator.IsValidPlacement(board, 0, 5, 5);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidPlacement_DuplicateInColumn_ReturnsFalse()
    {
        // Arrange - board with a 7 in column 0
        var board = new int[81];
        board[0] = 7; // (0,0) = 7
        
        // Act - try to place another 7 in the same column
        var result = _validator.IsValidPlacement(board, 5, 0, 7);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidPlacement_DuplicateIn3x3Box_ReturnsFalse()
    {
        // Arrange - board with a 3 in the top-left 3x3 box
        var board = new int[81];
        board[0] = 3; // (0,0) = 3
        
        // Act - try to place another 3 in the same 3x3 box
        var result = _validator.IsValidPlacement(board, 2, 2, 3);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidPlacement_EmptyCell_ReturnsTrue()
    {
        // Arrange
        var board = new int[81];
        
        // Act - place a 0 (empty)
        var result = _validator.IsValidPlacement(board, 0, 0, 0);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidPlacement_InvalidValue_ReturnsFalse()
    {
        // Arrange
        var board = new int[81];
        
        // Act - try invalid values
        var result1 = _validator.IsValidPlacement(board, 0, 0, 10);
        var result2 = _validator.IsValidPlacement(board, 0, 0, -1);
        
        // Assert
        Assert.False(result1);
        Assert.False(result2);
    }

    [Fact]
    public void IsBoardComplete_EmptyBoard_ReturnsFalse()
    {
        // Arrange
        var board = new int[81];
        
        // Act
        var result = _validator.IsBoardComplete(board);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsBoardComplete_PartiallyFilledBoard_ReturnsFalse()
    {
        // Arrange
        var board = new int[81];
        for (int i = 0; i < 40; i++)
        {
            board[i] = (i % 9) + 1;
        }
        
        // Act
        var result = _validator.IsBoardComplete(board);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsBoardComplete_FullyFilledBoard_ReturnsTrue()
    {
        // Arrange - fill the board completely
        var board = new int[81];
        for (int i = 0; i < 81; i++)
        {
            board[i] = (i % 9) + 1;
        }
        
        // Act
        var result = _validator.IsBoardComplete(board);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidSolution_ValidCompleteSolution_ReturnsTrue()
    {
        // Arrange - a valid completed Sudoku board
        var board = new int[]
        {
            5, 3, 4, 6, 7, 8, 9, 1, 2,
            6, 7, 2, 1, 9, 5, 3, 4, 8,
            1, 9, 8, 3, 4, 2, 5, 6, 7,
            8, 5, 9, 7, 6, 1, 4, 2, 3,
            4, 2, 6, 8, 5, 3, 7, 9, 1,
            7, 1, 3, 9, 2, 4, 8, 5, 6,
            9, 6, 1, 5, 3, 7, 2, 8, 4,
            2, 8, 7, 4, 1, 9, 6, 3, 5,
            3, 4, 5, 2, 8, 6, 1, 7, 9
        };
        
        // Act
        var result = _validator.IsValidSolution(board);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidSolution_IncompleteSolution_ReturnsFalse()
    {
        // Arrange - incomplete board
        var board = new int[81];
        for (int i = 0; i < 80; i++)
        {
            board[i] = (i % 9) + 1;
        }
        // Last cell is 0 (empty)
        
        // Act
        var result = _validator.IsValidSolution(board);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidSolution_InvalidSolution_ReturnsFalse()
    {
        // Arrange - board with duplicate in row
        var board = new int[81];
        for (int i = 0; i < 81; i++)
        {
            board[i] = 1; // All cells are 1 (invalid)
        }
        
        // Act
        var result = _validator.IsValidSolution(board);
        
        // Assert
        Assert.False(result);
    }
}
