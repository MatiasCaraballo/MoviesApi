using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesApp.Migrations
{
    /// <inheritdoc />
    public partial class correctDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieDirectors_Directors_DirectorsDirectorId",
                table: "MovieDirectors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieDirectors_Movies_MoviesMovieId",
                table: "MovieDirectors");

            migrationBuilder.RenameColumn(
                name: "MoviesMovieId",
                table: "MovieDirectors",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "DirectorsDirectorId",
                table: "MovieDirectors",
                newName: "DirectorId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieDirectors_MoviesMovieId",
                table: "MovieDirectors",
                newName: "IX_MovieDirectors_MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDirectors_Directors_DirectorId",
                table: "MovieDirectors",
                column: "DirectorId",
                principalTable: "Directors",
                principalColumn: "DirectorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDirectors_Movies_MovieId",
                table: "MovieDirectors",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieDirectors_Directors_DirectorId",
                table: "MovieDirectors");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieDirectors_Movies_MovieId",
                table: "MovieDirectors");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "MovieDirectors",
                newName: "MoviesMovieId");

            migrationBuilder.RenameColumn(
                name: "DirectorId",
                table: "MovieDirectors",
                newName: "DirectorsDirectorId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieDirectors_MovieId",
                table: "MovieDirectors",
                newName: "IX_MovieDirectors_MoviesMovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDirectors_Directors_DirectorsDirectorId",
                table: "MovieDirectors",
                column: "DirectorsDirectorId",
                principalTable: "Directors",
                principalColumn: "DirectorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieDirectors_Movies_MoviesMovieId",
                table: "MovieDirectors",
                column: "MoviesMovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
