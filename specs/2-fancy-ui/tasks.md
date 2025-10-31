# Tasks for: Fancy, Efficient .NET Web UI (Feature 2)

## Phase 1 - Setup (project initialization)
- [ ] T001 Create Blazor WebAssembly project at src/Sudoku.UI/
- [ ] T002 Add project to solution at src/Sudoku.sln
- [ ] T003 Add README and quickstart at README.md and specs/2-fancy-ui/quickstart.md
- [ ] T004 Add .env.sample for frontend config at src/Sudoku.UI/.env.sample

## Phase 2 - Foundational (blocking prerequisites)
- [ ] T005 [P] Implement entity models in src/Sudoku.UI/Models/ (User, Puzzle, GameSession, Difficulty)
- [ ] T006 [P] Configure HttpClient for REST API integration in src/Sudoku.UI/Services/ApiService.cs
- [ ] T007 [P] Add Material Design UI library (e.g., MudBlazor) to src/Sudoku.UI/
- [ ] T008 [P] Set up routing and layout in src/Sudoku.UI/Pages/_Layout.razor

## Phase 3 - User Story 1 (P1): Registration, Login, and Dashboard
Goal: Users can register, login, and see a visually engaging dashboard.
Independent test: Register, login, and see personalized dashboard.
- [ ] T009 [US1] Implement registration form in src/Sudoku.UI/Pages/Register.razor
- [ ] T010 [US1] Implement login form in src/Sudoku.UI/Pages/Login.razor
- [ ] T011 [US1] Implement dashboard page in src/Sudoku.UI/Pages/Dashboard.razor
- [ ] T012 [US1] Integrate authentication endpoints (/api/auth/register, /api/auth/login) in ApiService.cs
- [ ] T013 [US1] Add authentication state management in src/Sudoku.UI/Services/AuthService.cs
- [ ] T014 [US1] Add unit tests for AuthService in tests/Unit/Sudoku.UI.Tests/AuthServiceTests.cs

## Phase 4 - User Story 2 (P2): Start and Play Puzzle
Goal: Users can select difficulty, start a puzzle, and interact with the board.
Independent test: Select difficulty, start puzzle, interact with board, see live validation.
- [ ] T015 [US2] Implement difficulty selection in src/Sudoku.UI/Pages/Dashboard.razor
- [ ] T016 [US2] Implement puzzle board UI in src/Sudoku.UI/Pages/Puzzle.razor
- [ ] T017 [US2] Integrate puzzle endpoints (/api/puzzles, /api/puzzles/{id}) in ApiService.cs
- [ ] T018 [US2] Implement cell entry, undo/redo, and hints in Puzzle.razor
- [ ] T019 [US2] Add board validation logic in src/Sudoku.UI/Services/BoardValidator.cs
- [ ] T020 [US2] Add unit tests for BoardValidator in tests/Unit/Sudoku.UI.Tests/BoardValidatorTests.cs

## Phase 5 - User Story 3 (P3): Save, Resume, and History
Goal: Users can save progress, resume puzzles, and view history.
Independent test: Save puzzle, logout/login, resume session, view completed puzzles.
- [ ] T021 [US3] Implement save progress feature in Puzzle.razor (calls /api/sessions/{id})
- [ ] T022 [US3] Implement resume session feature in Dashboard.razor (calls /api/sessions/{id})
- [ ] T023 [US3] Implement history page in src/Sudoku.UI/Pages/History.razor (calls /api/users/{userId}/history)
- [ ] T024 [US3] Integrate session endpoints (/api/sessions, /api/sessions/{id}, /api/sessions/{id}/complete) in ApiService.cs
- [ ] T025 [US3] Add unit tests for session management in tests/Unit/Sudoku.UI.Tests/SessionServiceTests.cs

## Final Phase - Polish & Cross-Cutting Concerns
- [ ] T026 Add accessibility features (WCAG 2.1 AA) in src/Sudoku.UI/
- [ ] T027 Add mobile responsiveness and touch support in src/Sudoku.UI/
- [ ] T028 Add error handling and user feedback in src/Sudoku.UI/Services/ErrorService.cs
- [ ] T029 Add integration tests for major user flows in tests/Integration/Sudoku.UI.IntegrationTests/
- [ ] T030 Update documentation and quickstart in README.md and specs/2-fancy-ui/quickstart.md

---

## Dependencies (story completion order)
1. Phase 1 Setup (T001..T004) — must complete before foundational tasks
2. Phase 2 Foundational (T005..T008) — required before user story implementations
3. User Story 1 (T009..T014) — registration/login/dashboard
4. User Story 2 (T015..T020) — puzzle play
5. User Story 3 (T021..T025) — save/resume/history
6. Final Phase (T026..T030) — polish and cross-cutting concerns

## Parallel execution opportunities
- Tasks marked with [P] are safe to run in parallel (different files/dependencies minimal): T005, T006, T007, T008, T014, T020, T025, T029
- UI components and service logic can be developed in parallel after foundational setup

## Independent test criteria (per story)
- US1: Register & login → see dashboard
- US2: Select difficulty → start puzzle → interact with board → live validation
- US3: Save puzzle → logout/login → resume session → view history

## Suggested MVP scope
- Implement Phase 1 + Phase 2 + User Story 1 (T001..T014). This delivers an independently testable MVP: user registration, login, and dashboard.

---

**Total tasks:** 30
**Task count per user story:**
- US1: 6
- US2: 6
- US3: 5
**Parallel opportunities:** 8
**All tasks follow strict checklist format and are immediately executable.**
