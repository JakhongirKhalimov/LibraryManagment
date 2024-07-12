using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteBooks_Book_BookId",
                table: "FavouriteBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteBooks_User_UserId",
                table: "FavouriteBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_User_UserId",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Translatables",
                table: "Translatables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavouriteBooks",
                table: "FavouriteBooks");

            migrationBuilder.RenameTable(
                name: "Translatables",
                newName: "Translatable");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "FavouriteBooks",
                newName: "FavouriteBook");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserId",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteBooks_UserId",
                table: "FavouriteBook",
                newName: "IX_FavouriteBook_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteBooks_BookId",
                table: "FavouriteBook",
                newName: "IX_FavouriteBook_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Translatable",
                table: "Translatable",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavouriteBook",
                table: "FavouriteBook",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$wOkuVAcIB4gqjfh20a2QPuzbcCpGCtdLJRfktgdaqDBzMGzVi/NCe");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteBook_Book_BookId",
                table: "FavouriteBook",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteBook_User_UserId",
                table: "FavouriteBook",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User_UserId",
                table: "Notification",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteBook_Book_BookId",
                table: "FavouriteBook");

            migrationBuilder.DropForeignKey(
                name: "FK_FavouriteBook_User_UserId",
                table: "FavouriteBook");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User_UserId",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Translatable",
                table: "Translatable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavouriteBook",
                table: "FavouriteBook");

            migrationBuilder.RenameTable(
                name: "Translatable",
                newName: "Translatables");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "FavouriteBook",
                newName: "FavouriteBooks");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "Notifications",
                newName: "IX_Notifications_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteBook_UserId",
                table: "FavouriteBooks",
                newName: "IX_FavouriteBooks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavouriteBook_BookId",
                table: "FavouriteBooks",
                newName: "IX_FavouriteBooks_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Translatables",
                table: "Translatables",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavouriteBooks",
                table: "FavouriteBooks",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$0hlqHYZxPIHYPE/q1mdDquyl1OR7CvblS2.x.xk.NNyFu36hX4khW");

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteBooks_Book_BookId",
                table: "FavouriteBooks",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavouriteBooks_User_UserId",
                table: "FavouriteBooks",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_User_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
