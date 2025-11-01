# Tasks: Sudoku UI & API

**Input**: Design documents from `/specs/001-sudoku-ui-api/`
**Prerequisites**: plan.md, spec.md, research.md

## Phase 1: Setup (Shared Infrastructure)

- [ ] T001 Create project structure per implementation plan
- [ ] T002 Initialize .NET solution and projects (API, UI) in /backend and /frontend
- [ ] T003 [P] Configure devcontainer for containerized development
- [ ] T004 [P] Setup PostgreSQL database container
- [ ] T005 [P] Configure Playwright for UI testing in /frontend

## Phase 2: Foundational (Blocking Prerequisites)

- [ ] T006 Setup CI/CD pipeline for build, test, and deploy
- [ ] T007 [P] Implement shared models/entities in /backend/src/models
- [ ] T008 [P] Configure API project with ASP.NET Core in /backend/src/api
- [ ] T009 [P] Configure Blazor project in /frontend/src
- [ ] T010 [P] Setup xUnit for backend tests in /backend/tests

## Phase 3: User Story 1 - Play Sudoku Puzzle (Priority: P1) 🎯 MVP

**Goal**: User can play a Sudoku puzzle via the UI  
**Independent Test**: Open UI, start puzzle, make moves, complete puzzle

### Tests for User Story 1

- [ ] T011 [P] [US1] Playwright test for starting and completing a puzzle in /frontend/tests
- [ ] T012 [P] [US1] xUnit test for puzzle validation logic in /backend/tests

### Implementation for User Story 1

- [ ] T013 [P] [US1] Create SudokuPuzzle model in /backend/src/models/SudokuPuzzle.cs
- [ ] T014 [P] [US1] Implement puzzle generation service in /backend/src/services/PuzzleService.cs
- [ ] T015 [US1] Implement API endpoint for puzzle generation in /backend/src/api/PuzzleController.cs
- [ ] T016 [US1] Implement Blazor UI for board interaction in /frontend/src/components/SudokuBoard.razor
- [ ] T017 [US1] Add validation logic for moves in /backend/src/services/PuzzleService.cs
- [ ] T018 [US1] Add completion feedback in UI in /frontend/src/components/SudokuBoard.razor

## Phase 4: User Story 2 - API Puzzle Management (Priority: P2)

**Goal**: API client can request, save, and validate puzzles  
**Independent Test**: Use API to request, submit moves, validate completion

### Tests for User Story 2

- [ ] T019 [P] [US2] xUnit test for session persistence in /backend/tests
- [ ] T020 [P] [US2] Playwright test for API puzzle management in /frontend/tests

### Implementation for User Story 2

- [ ] T021 [P] [US2] Create GameSession model in /backend/src/models/GameSession.cs
- [ ] T022 [US2] Implement session persistence logic in /backend/src/services/SessionService.cs
- [ ] T023 [US2] Implement API endpoints for session management in /backend/src/api/SessionController.cs
- [ ] T024 [US2] Integrate session management in Blazor UI in /frontend/src/components/SudokuBoard.razor

## Phase 5: User Story 3 - Error Handling & Feedback (Priority: P3)

**Goal**: User/client receives clear feedback for errors  
**Independent Test**: Trigger invalid move or error, verify feedback

### Tests for User Story 3

- [ ] T025 [P] [US3] Playwright test for error feedback in /frontend/tests
- [ ] T026 [P] [US3] xUnit test for error handling in /backend/tests

### Implementation for User Story 3

- [ ] T027 [P] [US3] Implement error handling logic in /backend/src/services/ErrorService.cs
- [ ] T028 [US3] Implement error feedback in Blazor UI in /frontend/src/components/ErrorMessage.razor
- [ ] T029 [US3] Ensure API returns actionable error messages in /backend/src/api/*

## Final Phase: Polish & Cross-Cutting Concerns

- [ ] T030 [P] Documentation updates in /docs
- [ ] T031 Code cleanup and refactoring
- [ ] T032 Performance optimization across API and UI
- [ ] T033 [P] Additional unit tests in /backend/tests and /frontend/tests
- [ ] T034 Security hardening
- [ ] T035 Run quickstart.md validation

## Dependencies & Execution Order

### Phase Dependencies

- Setup (Phase 1): No dependencies
- Foundational (Phase 2): Depends on Setup completion
- User Stories (Phase 3+): All depend on Foundational phase completion; can proceed in parallel if staffed
- Polish (Final Phase): Depends on all desired user stories being complete

### User Story Dependencies

- User Story 1 (P1): Can start after Foundational; no dependencies on other stories
- User Story 2 (P2): Can start after Foundational; may integrate with US1 but should be independently testable
- User Story 3 (P3): Can start after Foundational; may integrate with US1/US2 but should be independently testable

### Parallel Opportunities

- All tasks marked [P] can run in parallel (different files, no dependencies)
- All tests for a user story marked [P] can run in parallel
- Models within a story marked [P] can run in parallel
- Different user stories can be worked on in parallel by different team members

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. STOP and VALIDATE: Test User Story 1 independently
5. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Test independently → Deploy/Demo (MVP!)
3. Add User Story 2 → Test independently → Deploy/Demo
4. Add User Story 3 → Test independently → Deploy/Demo
5. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1
   - Developer B: User Story 2
   - Developer C: User Story 3
3. Stories complete and integrate independently
