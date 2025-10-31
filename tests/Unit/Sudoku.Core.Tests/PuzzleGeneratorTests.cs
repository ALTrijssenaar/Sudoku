using Sudoku.Core.Services;
using Sudoku.Infrastructure.Services;

namespace Sudoku.Core.Tests;

public class PuzzleGeneratorTests
{
    private readonly IPuzzleGenerator _generator;
    private readonly BoardValidator _validator;

    public PuzzleGeneratorTests()
    {
        _generator = new PuzzleGenerator();
        _validator = new BoardValidator();
    }

    [Theory]
    [InlineData("Easy")]
    [InlineData("Medium")]
    [InlineData("Hard")]
    public void GeneratePuzzle_ValidDifficulty_ReturnsValidPuzzleAndSolution(string difficulty)
    {
        // Act
        var (initialCells, solution) = _generator.GeneratePuzzle(difficulty);
        
        // Assert
        Assert.NotNull(initialCells);
        Assert.NotNull(solution);
        Assert.Equal(81, initialCells.Length);
        Assert.Equal(81, solution.Length);
    }

    [Theory]
    [InlineData("Easy")]
    [InlineData("Medium")]
    [InlineData("Hard")]
    public void GeneratePuzzle_Solution_IsValidCompleteSudoku(string difficulty)
    {
        // Act
        var (_, solution) = _generator.GeneratePuzzle(difficulty);
        
        // Assert - solution should be a valid complete Sudoku
        Assert.True(_validator.IsValidSolution(solution));
    }

    [Theory]
    [InlineData("Easy")]
    [InlineData("Medium")]
    [InlineData("Hard")]
    public void GeneratePuzzle_InitialCells_HasEmptyCells(string difficulty)
    {
        // Act
        var (initialCells, _) = _generator.GeneratePuzzle(difficulty);
        
        // Assert - puzzle should have empty cells (0s)
        var emptyCount = initialCells.Count(cell => cell == 0);
        Assert.True(emptyCount > 0, "Puzzle should have at least one empty cell");
    }

    [Fact]
    public void GeneratePuzzle_Easy_HasFewerEmptyCellsThanHard()
    {
        // Act
        var (easyPuzzle, _) = _generator.GeneratePuzzle("Easy");
        var (hardPuzzle, _) = _generator.GeneratePuzzle("Hard");
        
        // Assert - Easy should have fewer empty cells than Hard
        var easyEmptyCount = easyPuzzle.Count(cell => cell == 0);
        var hardEmptyCount = hardPuzzle.Count(cell => cell == 0);
        
        Assert.True(easyEmptyCount < hardEmptyCount, 
            $"Easy puzzle should have fewer empty cells ({easyEmptyCount}) than Hard puzzle ({hardEmptyCount})");
    }

    [Theory]
    [InlineData("Easy", 30)]
    [InlineData("Medium", 45)]
    [InlineData("Hard", 55)]
    public void GeneratePuzzle_DifficultyLevel_HasExpectedEmptyCells(string difficulty, int expectedEmpty)
    {
        // Act
        var (initialCells, _) = _generator.GeneratePuzzle(difficulty);
        
        // Assert
        var emptyCount = initialCells.Count(cell => cell == 0);
        Assert.Equal(expectedEmpty, emptyCount);
    }

    [Fact]
    public void GeneratePuzzle_InitialCells_AreSubsetOfSolution()
    {
        // Act
        var (initialCells, solution) = _generator.GeneratePuzzle("Medium");
        
        // Assert - all non-zero cells in initial should match solution
        for (int i = 0; i < 81; i++)
        {
            if (initialCells[i] != 0)
            {
                Assert.Equal(solution[i], initialCells[i]);
            }
        }
    }

    [Fact]
    public void GeneratePuzzle_MultipleGenerations_ProduceDifferentPuzzles()
    {
        // Act - generate two puzzles
        var (puzzle1, _) = _generator.GeneratePuzzle("Medium");
        var (puzzle2, _) = _generator.GeneratePuzzle("Medium");
        
        // Assert - they should be different (with very high probability)
        bool areDifferent = !puzzle1.SequenceEqual(puzzle2);
        Assert.True(areDifferent, "Generated puzzles should be different");
    }
}
