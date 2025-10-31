# Data Model: Fancy, Efficient .NET Web UI (Feature 2)

## Entities

### User
- id: GUID
- email: string
- displayName: string
- passwordHash: string
- createdAt: datetime

### Puzzle
- id: GUID
- difficulty: enum (Easy, Medium, Hard)
- initialCells: array (jsonb)
- solution: array (jsonb)
- generatedAt: datetime

### GameSession
- id: GUID
- userId: GUID
- puzzleId: GUID
- currentState: array (jsonb)
- startedAt: datetime
- lastSavedAt: datetime
- completedAt: datetime (nullable)
- status: enum (InProgress, Completed)

### Difficulty
- name: string
- config: object (cell count, allowed hints, etc.)

## Relationships
- User has many GameSessions
- GameSession references one User and one Puzzle
- Puzzle has one Difficulty

## Validation Rules
- Email must be valid format
- DisplayName required
- Puzzle board must match solution for completion
- No duplicate values in row/column/box
- Difficulty must be one of allowed values

## State Transitions
- GameSession: InProgress → Completed
- User: Registered → Active
