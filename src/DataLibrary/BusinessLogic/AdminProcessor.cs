using DataLibrary.Models;

namespace DataLibrary.BusinessLogic
{
    public static class AdminProcessor
    {
        public static AdminModel AuthoriseAdmin(string username, string password)
        {
            AdminModel data = new AdminModel
            {
                Username = username,
                Password = password
            };

            AdminModel adminModel = SqlDataAccess.LoadSingle<AdminModel>
            (
                @"SELECT Id, Username, Password FROM dbo.Admin
                    WHERE Username = @Username AND Password = @Password",
                data
            );

            return adminModel;
        }
    }
}
