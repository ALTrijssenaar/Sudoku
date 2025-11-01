using Sudoku.Models;
using Sudoku.Services;
using Xunit;

namespace Sudoku.Tests;

public class PuzzleServiceTests
{
    private readonly PuzzleService _puzzleService;

    public PuzzleServiceTests()
    {
        _puzzleService = new PuzzleService();
    }

    [Fact]
    public void GeneratePuzzle_ShouldCreateValidPuzzle()
    {
        // Act
        var puzzle = _puzzleService.GeneratePuzzle(DifficultyLevel.Easy);

        // Assert
        Assert.NotNull(puzzle);
        Assert.NotEqual(Guid.Empty, puzzle.Id);
        Assert.Equal(DifficultyLevel.Easy, puzzle.Difficulty);
        Assert.NotNull(puzzle.InitialBoard);
        Assert.NotNull(puzzle.Solution);
    }

    [Fact]
    public void GeneratePuzzle_ShouldHaveValidSolution()
    {
        // Act
        var puzzle = _puzzleService.GeneratePuzzle(DifficultyLevel.Medium);

        // Assert - Check that solution is a valid Sudoku
        for (int i = 0; i < 9; i++)
        {
            // Check rows
            var rowValues = new HashSet<int>();
            for (int j = 0; j < 9; j++)
            {
                var value = puzzle.Solution[i, j];
                Assert.InRange(value, 1, 9);
                Assert.DoesNotContain(value, rowValues);
                rowValues.Add(value);
            }

            // Check columns
            var colValues = new HashSet<int>();
            for (int j = 0; j < 9; j++)
            {
                var value = puzzle.Solution[j, i];
                Assert.DoesNotContain(value, colValues);
                colValues.Add(value);
            }
        }

        // Check 3x3 boxes
        for (int boxRow = 0; boxRow < 9; boxRow += 3)
        {
            for (int boxCol = 0; boxCol < 9; boxCol += 3)
            {
                var boxValues = new HashSet<int>();
                for (int i = boxRow; i < boxRow + 3; i++)
                {
                    for (int j = boxCol; j < boxCol + 3; j++)
                    {
                        var value = puzzle.Solution[i, j];
                        Assert.DoesNotContain(value, boxValues);
                        boxValues.Add(value);
                    }
                }
            }
        }
    }

    [Fact]
    public void IsValidMove_ShouldReturnTrue_ForValidMove()
    {
        // Arrange
        var board = new int[9, 9];
        board[0, 0] = 1;
        board[0, 1] = 2;

        // Act
        var result = _puzzleService.IsValidMove(board, 0, 2, 3);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidMove_ShouldReturnFalse_ForDuplicateInRow()
    {
        // Arrange
        var board = new int[9, 9];
        board[0, 0] = 1;

        // Act
        var result = _puzzleService.IsValidMove(board, 0, 1, 1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidMove_ShouldReturnFalse_ForDuplicateInColumn()
    {
        // Arrange
        var board = new int[9, 9];
        board[0, 0] = 1;

        // Act
        var result = _puzzleService.IsValidMove(board, 1, 0, 1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidMove_ShouldReturnFalse_ForDuplicateInBox()
    {
        // Arrange
        var board = new int[9, 9];
        board[0, 0] = 1;

        // Act
        var result = _puzzleService.IsValidMove(board, 1, 1, 1);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidMove_ShouldReturnFalse_ForInvalidValue()
    {
        // Arrange
        var board = new int[9, 9];

        // Act
        var result = _puzzleService.IsValidMove(board, 0, 0, 10);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsPuzzleComplete_ShouldReturnTrue_WhenBoardMatchesSolution()
    {
        // Arrange
        var puzzle = _puzzleService.GeneratePuzzle(DifficultyLevel.Easy);

        // Act
        var result = _puzzleService.IsPuzzleComplete(puzzle.Solution, puzzle.Solution);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsPuzzleComplete_ShouldReturnFalse_WhenBoardDoesNotMatchSolution()
    {
        // Arrange
        var puzzle = _puzzleService.GeneratePuzzle(DifficultyLevel.Easy);
        var wrongBoard = (int[,])puzzle.InitialBoard.Clone();

        // Act
        var result = _puzzleService.IsPuzzleComplete(wrongBoard, puzzle.Solution);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(DifficultyLevel.Easy)]
    [InlineData(DifficultyLevel.Medium)]
    [InlineData(DifficultyLevel.Hard)]
    public void GeneratePuzzle_ShouldWorkForAllDifficultyLevels(DifficultyLevel difficulty)
    {
        // Act
        var puzzle = _puzzleService.GeneratePuzzle(difficulty);

        // Assert
        Assert.Equal(difficulty, puzzle.Difficulty);
        Assert.NotNull(puzzle.InitialBoard);
        Assert.NotNull(puzzle.Solution);
    }
}
