
# Sudoku Platform Constitution

## Core Principles

### Library-First
Every feature is implemented as a standalone, self-contained library. Libraries MUST be independently testable and documented. No organizational-only libraries permitted.

### CLI Interface
All libraries MUST expose functionality via a CLI. Text in/out protocol: stdin/args → stdout, errors → stderr. Support both JSON and human-readable formats.

### Test-First (NON-NEGOTIABLE)
Test-driven development is mandatory. Tests MUST be written and approved before implementation. Red-Green-Refactor cycle strictly enforced.

### Integration Testing
Integration tests are REQUIRED for new library contracts, contract changes, inter-service communication, and shared schemas.

### Observability & Versioning
Structured logging and text I/O are REQUIRED for debuggability. Versioning follows MAJOR.MINOR.PATCH format. Simplicity is prioritized (YAGNI).

## Additional Constraints

Technology stack MUST be open source and leverage the latest stable, well-supported tools and technologies. Prefer dotnet solutions and projects for all core components.

All deliverables MUST be containerized artifacts, provided as devcontainers. The platform MUST be operable with a single click of F5 in supported environments.

If an API is required or any stateful logic is needed, use a separate API project. UI components MUST remain as simple as possible, delegating all business logic to APIs. Direct database access from the UI is strictly prohibited; all data operations MUST go through an API.

Compliance with security and performance standards is REQUIRED. Deployment policies MUST be documented.

## Development Workflow

Code review is REQUIRED for all changes. All tests MUST pass before deployment. Quality gates enforce constitution compliance.

## Governance

The constitution supersedes all other practices. Amendments require documentation, approval, and a migration plan. All PRs/reviews MUST verify compliance. Complexity MUST be justified. Use runtime guidance files for development.

**Version**: 1.0.0 | **Ratified**: TODO(RATIFICATION_DATE): Original adoption date required. | **Last Amended**: 2025-10-31
