using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sudoku.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sudoku_puzzles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    difficulty = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    initial_cells = table.Column<int[]>(type: "jsonb", nullable: false),
                    solution = table.Column<int[]>(type: "jsonb", nullable: false),
                    generated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sudoku_puzzles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    display_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "game_sessions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    puzzle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_state = table.Column<int[]>(type: "jsonb", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_saved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_sessions", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_sessions_sudoku_puzzles_puzzle_id",
                        column: x => x.puzzle_id,
                        principalTable: "sudoku_puzzles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_game_sessions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_game_sessions_puzzle_id",
                table: "game_sessions",
                column: "puzzle_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_sessions_user_id",
                table: "game_sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_sudoku_puzzles_difficulty",
                table: "sudoku_puzzles",
                column: "difficulty");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_sessions");

            migrationBuilder.DropTable(
                name: "sudoku_puzzles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
