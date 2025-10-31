# data-model.md

Entities

1. User
   - id: UUID (PK)
   - email: string (unique, validated)
   - display_name: string
   - password_hash: string
   - created_at: timestamp

2. SudokuPuzzle
   - id: UUID (PK)
   - difficulty: enum (Easy, Medium, Hard)
   - initial_cells: jsonb (array[81] of ints, 0 = empty)
   - solution: jsonb (array[81] of ints)
   - generated_at: timestamp

3. GameSession
   - id: UUID (PK)
   - user_id: UUID (FK -> User.id)
   - puzzle_id: UUID (FK -> SudokuPuzzle.id)
   - current_state: jsonb (array[81] of ints)
   - started_at: timestamp
   - last_saved_at: timestamp
   - completed_at: timestamp (nullable)
   - status: enum (in-progress, completed)

Relationships

- User 1..* GameSession
- SudokuPuzzle 1..* GameSession

Validation rules

- `initial_cells`, `solution`, and `current_state` must be arrays of length 81 with values in 0..9 (0 indicates empty cell; 1..9 are digits).
- `solution` must be a valid completed Sudoku and must align with `initial_cells` (i.e., solution cells must preserve givens).
- `difficulty` must be one of the predefined enum values.

State transitions (GameSession)

- New -> in-progress (on session create)
- in-progress -> completed (on successful validation of full board)
- in-progress -> in-progress (on save/update)

Indexes & performance

- Index on `GameSession.user_id` for fetching user history
- Index on `SudokuPuzzle.difficulty` for quick generation queries
- Consider GIN index on jsonb columns if querying inside boards becomes necessary

Seed data

- Seed a small set of puzzles for each difficulty to enable local dev without generator during early development.

*** End of data-model.md
