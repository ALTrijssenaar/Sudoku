# Tasks for: Sudoku - Individual Player Platform

## Overview
Feature: Sudoku - Individual Player Platform  
Spec: `specs/1-sudoku-platform/spec.md`  
Plan: `specs/1-sudoku-platform/plan`  
OpenAPI: `specs/1-sudoku-platform/contracts/openapi.yaml`

---

## Phase 1 - Setup (project initialization)

- [ ] T001 Create .NET solution and projects at `src/` (src/Sudoku.sln, src/Sudoku.Api/, src/Sudoku.Core/, src/Sudoku.Infrastructure/) — create project files and add to solution
- [ ] T002 Add `docker-compose.yml` at repository root to run Postgres for local development (service: postgres, volume, env file reference)
- [ ] T003 Create `.env.sample` at repository root containing DB connection variables and JWT secrets (`.env.sample`)
- [ ] T004 Add `src/Sudoku.Api/Program.cs` and minimal launch settings to host API locally (HTTPS and HTTP endpoints)

## Phase 2 - Foundational (blocking prerequisites)

- [ ] T005 [P] Create EF Core `SudokuDbContext` in `src/Sudoku.Infrastructure/Data/SudokuDbContext.cs` and register it in `src/Sudoku.Api/Program.cs`
- [ ] T006 [P] Create `User` entity in `src/Sudoku.Core/Entities/User.cs` (fields: id, email, displayName, passwordHash, createdAt)
- [ ] T007 [P] Create `SudokuPuzzle` entity in `src/Sudoku.Core/Entities/SudokuPuzzle.cs` (fields: id, difficulty, initialCells jsonb, solution jsonb, generatedAt)
- [ ] T008 [P] Create `GameSession` entity in `src/Sudoku.Core/Entities/GameSession.cs` (fields: id, userId, puzzleId, currentState jsonb, startedAt, lastSavedAt, completedAt, status)
- [ ] T009 [P] Add EF Core entity configurations (Fluent API) in `src/Sudoku.Infrastructure/Mapping/*` to enforce JSONB length and value constraints
- [ ] T010 Create initial EF Core migration scaffolding in `src/Sudoku.Infrastructure/Migrations/` (add a migration file or script at `scripts/create-initial-migration.ps1`)
- [ ] T011 Create seed data for puzzles in `src/Sudoku.Infrastructure/Data/SeedPuzzles/` (add one sample JSON puzzle per difficulty)
- [ ] T012 Add a DB repository interface for `GameSession` in `src/Sudoku.Core/Repositories/IGameSessionRepository.cs` and an implementation in `src/Sudoku.Infrastructure/Repositories/GameSessionRepository.cs`
- [ ] T013 Add configuration and secrets binding for JWT and DB in `src/Sudoku.Api/appsettings.Development.json` and read from `.env`

## Phase 3 - User Story 1 (P1): Start and Solve a Puzzle
Goal: Allow a signed-in user to start a puzzle at a selected difficulty and solve it with live validation.
Independent test: Register, login, POST /api/sessions with puzzleId or difficulty, receive session with board, submit cell updates, complete session.

- [ ] T014 [US1] Create puzzle generator service interface `src/Sudoku.Core/Services/IPuzzleGenerator.cs`
- [ ] T015 [US1] Implement puzzle generator `src/Sudoku.Infrastructure/Services/PuzzleGenerator.cs` that generates solvable puzzles and respects difficulty mapping
- [ ] T016 [US1] Create `BoardValidator` service in `src/Sudoku.Core/Services/BoardValidator.cs` to validate cell entries and detect completion
- [ ] T017 [US1] Implement `PuzzlesController` in `src/Sudoku.Api/Controllers/PuzzlesController.cs` with endpoints `GET /api/puzzles?difficulty=` and `GET /api/puzzles/{id}`
- [ ] T018 [US1] Implement `SessionsController` in `src/Sudoku.Api/Controllers/SessionsController.cs` for `POST /api/sessions` (create session) and `POST /api/sessions/{id}/complete` (validate full board)
- [ ] T019 [US1] Implement session persistence methods in `src/Sudoku.Infrastructure/Repositories/GameSessionRepository.cs` used by SessionsController
- [ ] T020 [US1] Write unit tests for `PuzzleGenerator` and `BoardValidator` at `tests/Unit/Sudoku.Core.Tests/PuzzleGeneratorTests.cs` and `tests/Unit/Sudoku.Core.Tests/BoardValidatorTests.cs`
- [ ] T021 [US1] Add integration test for session lifecycle at `tests/Integration/SessionLifecycleTests.cs` (register, login, start session, complete)

## Phase 4 - User Story 2 (P2): Choose Difficulty and Resume
Goal: Users can pick a difficulty and resume an in-progress puzzle later.
Independent test: Start a Medium puzzle, exit, login again, resume the session and see last saved board.

- [ ] T022 [US2] Implement difficulty configuration and mapping in `src/Sudoku.Infrastructure/Config/DifficultyConfig.cs`
- [ ] T023 [US2] Implement PATCH `api/sessions/{id}/cell` in `src/Sudoku.Api/Controllers/SessionsController.cs` to update a single cell (request body: row, col, value)
- [ ] T024 [US2] Implement GET `api/sessions/{id}` resume endpoint in `src/Sudoku.Api/Controllers/SessionsController.cs` to return saved session state
- [ ] T025 [US2] Ensure `GameSessionRepository` supports upsert/save behavior in `src/Sudoku.Infrastructure/Repositories/GameSessionRepository.cs`
- [ ] T026 [US2] Add integration tests for save and resume at `tests/Integration/SessionResumeTests.cs`

## Phase 5 - User Story 3 (P3): Account Management and Progress
Goal: Provide account registration/login and user history of completed puzzles.
Independent test: Register, solve puzzles, query history and see completed sessions.

- [ ] T027 [US3] Implement `AuthController` in `src/Sudoku.Api/Controllers/AuthController.cs` with `POST /api/auth/register` and `POST /api/auth/login` (JWT)
- [ ] T028 [US3] Implement user creation & lookup in `src/Sudoku.Infrastructure/Repositories/UserRepository.cs` and password hashing in `src/Sudoku.Core/Security/PasswordHasher.cs`
- [ ] T029 [US3] Implement `GET /api/users/{userId}/history` in `src/Sudoku.Api/Controllers/UsersController.cs` to return recent completed puzzles
- [ ] T030 [US3] Add integration tests for auth and history at `tests/Integration/AuthAndHistoryTests.cs`
- [ ] T031 [US3] Add seed user and migration helper at `src/Sudoku.Infrastructure/Data/SeedUsers/` for local dev

## Final Phase - Polish & Cross-Cutting Concerns

- [ ] T032 Add Swagger/OpenAPI (Swashbuckle) integration in `src/Sudoku.Api/Program.cs` and ensure OpenAPI at `/swagger`
- [ ] T033 [P] Implement structured error handling middleware at `src/Sudoku.Api/Middleware/ErrorHandlingMiddleware.cs` and centralized validation response format
- [ ] T034 [P] Implement logging configuration and correlation IDs in `src/Sudoku.Api/Logging/` and `appsettings.json`
- [ ] T035 Add CI workflow at `.github/workflows/dotnet.yml` to build, test, and run integration tests using Docker Compose Postgres
- [ ] T036 Add README and developer quickstart at `README.md` and reference `specs/1-sudoku-platform/plan/quickstart.md`
- [ ] T037 Add a simple Postman collection or OpenAPI client at `docs/postman/` for manual testing

---

## Dependencies (story completion order)

1. Phase 1 Setup (T001..T004) — must complete before foundational tasks
2. Phase 2 Foundational (T005..T013) — required before user story implementations
3. User Story 1 (T014..T021) — MVP and highest priority
4. User Story 2 (T022..T026) — depends on T019/T025 for persistence
5. User Story 3 (T027..T031) — depends on foundational auth config (T013)
6. Final Phase (T032..T037) — non-blocking but recommended before release


## Parallel execution opportunities

- Tasks marked with [P] are safe to run in parallel (different files/dependencies minimal): T005, T006, T007, T008, T009, T012, T013, T033, T034
- Puzzle generator (T015) and BoardValidator (T016) can be developed in parallel with EF Core DbContext implementation (T005) as long as interfaces are agreed upon
- Unit tests (T020) can be written in parallel while generator implementation (T015) is in progress

## Task counts

- Total tasks: 37
- Tasks per story/phase:
  - Phase 1 (Setup): 4
  - Phase 2 (Foundational): 9
  - User Story 1 (P1): 8
  - User Story 2 (P2): 5
  - User Story 3 (P3): 5
  - Final Phase (Polish): 6

## Independent test criteria (per story)

- US1: Register & login → start session → submit valid moves → complete session → verified completion stored
- US2: Start Medium puzzle → save work → logout/login → GET session returns last saved board
- US3: Register → solve puzzles → GET /api/users/{userId}/history shows completed sessions

## Suggested MVP scope

- Implement Phase 1 + Phase 2 + User Story 1 (T001..T021). This delivers an independently testable MVP that lets a user register, start a puzzle, solve it, and have the result saved.

## Implementation strategy

- Deliver incrementally: implement the smallest slice for US1 that includes puzzle generation, session creation, and completion. Add auth as minimal JWT (simple registration/login) for MVP. Then add resume and history features.

---

## Notes

- File paths are suggestions and should match final repository structure under `src/`.
- If you want I can scaffold the `src/` projects and add example implementations for the first 10 tasks.

*** End of tasks.md
