import { test, expect } from '@playwright/test';

test.describe('Sudoku Game', () => {
  test('should display the game title', async ({ page }) => {
    await page.goto('/');
    await expect(page.locator('h2')).toContainText('Sudoku Game');
  });

  test('should start a new game', async ({ page }) => {
    await page.goto('/');
    
    // Click start new game button
    await page.click('button:has-text("Start New Game")');
    
    // Wait for the board to appear
    await page.waitForSelector('.sudoku-board', { timeout: 10000 });
    
    // Verify the board is displayed
    const board = await page.locator('.sudoku-board');
    await expect(board).toBeVisible();
    
    // Verify there are 81 cells (9x9 grid)
    const cells = await page.locator('.sudoku-cell').count();
    expect(cells).toBe(81);
  });

  test('should allow selecting difficulty', async ({ page }) => {
    await page.goto('/');
    
    // Select medium difficulty
    await page.selectOption('select.form-select', '2');
    
    // Start new game
    await page.click('button:has-text("Start New Game")');
    
    // Wait for the board
    await page.waitForSelector('.sudoku-board', { timeout: 10000 });
    
    // Verify board is visible
    const board = await page.locator('.sudoku-board');
    await expect(board).toBeVisible();
  });

  test('should have initial cells that are disabled', async ({ page }) => {
    await page.goto('/');
    
    // Start new game
    await page.click('button:has-text("Start New Game")');
    
    // Wait for the board
    await page.waitForSelector('.sudoku-board', { timeout: 10000 });
    
    // Check that some cells are disabled (initial cells)
    const disabledCells = await page.locator('.sudoku-cell[disabled]').count();
    expect(disabledCells).toBeGreaterThan(0);
  });

  test('should allow entering numbers in empty cells', async ({ page }) => {
    await page.goto('/');
    
    // Start new game
    await page.click('button:has-text("Start New Game")');
    
    // Wait for the board
    await page.waitForSelector('.sudoku-board', { timeout: 10000 });
    
    // Find an empty, enabled cell
    const emptyCell = page.locator('.sudoku-cell:not([disabled])').first();
    
    // Enter a number
    await emptyCell.fill('5');
    
    // Verify the number was entered (or validate was called)
    // Note: The actual validation happens on change, so we just check the fill worked
    await expect(emptyCell).not.toHaveValue('');
  });

  test('should have check solution button', async ({ page }) => {
    await page.goto('/');
    
    // Start new game
    await page.click('button:has-text("Start New Game")');
    
    // Wait for the board
    await page.waitForSelector('.sudoku-board', { timeout: 10000 });
    
    // Verify check solution button exists
    const checkButton = page.locator('button:has-text("Check Solution")');
    await expect(checkButton).toBeVisible();
  });
});
