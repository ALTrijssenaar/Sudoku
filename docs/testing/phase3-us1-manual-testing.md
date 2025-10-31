# Phase 3 - User Story 1: Manual Testing Guide

## Overview
This guide provides manual testing instructions for the Phase 3 - User Story 1 implementation: Start and Solve a Puzzle.

## Prerequisites
1. PostgreSQL database running (via Docker Compose)
2. Database migrations applied
3. API running on `https://localhost:5001` or `http://localhost:5000`

## Starting the Environment

```bash
# Start PostgreSQL
docker-compose up -d

# Apply migrations (from src/Sudoku.Infrastructure directory)
dotnet ef database update

# Run the API (from src/Sudoku.Api directory)
dotnet run
```

## Test Scenarios

### 1. Get/Generate Puzzles

**Test Easy Difficulty:**
```bash
curl -X GET "http://localhost:5000/api/puzzles?difficulty=Easy"
```

Expected: Returns a list with at least one puzzle with 30 empty cells

**Test Medium/Hard:**
```bash
curl -X GET "http://localhost:5000/api/puzzles?difficulty=Medium"
curl -X GET "http://localhost:5000/api/puzzles?difficulty=Hard"
```

### 2. Create and Manage Sessions

```bash
# Get a puzzle first
PUZZLE_ID=$(curl -s "http://localhost:5000/api/puzzles?difficulty=Easy" | jq -r '.[0].id')

# Create session
curl -X POST "http://localhost:5000/api/sessions" \
  -H "Content-Type: application/json" \
  -d "{\"puzzleId\": \"$PUZZLE_ID\"}"

# Get session (use ID from previous response)
SESSION_ID="<paste-session-id>"
curl -X GET "http://localhost:5000/api/sessions/$SESSION_ID"
```

## Unit Tests

All core business logic is covered by 27 passing unit tests:

```bash
cd tests/Unit/Sudoku.Core.Tests
dotnet test
```

## Verification Checklist

- [x] Puzzle generation service implemented
- [x] Board validation service implemented  
- [x] Puzzles controller with difficulty filtering
- [x] Sessions controller for create/get/complete
- [x] 27 unit tests passing
- [ ] Manual API endpoint verification
- [ ] Integration tests (DB config needs fix)
