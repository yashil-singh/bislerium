
using System.Text.Json;

namespace bislerium.Data
{
    public  class UserService
    {
        public const string SeedUsername = "admin";
        public const string SeedPassword = "admin";

        //To get all the available users
        public static List<Users> GetAll()
        {
            string appUsersFilePath = Utils.GetAppUsersFilePath();
            if (!File.Exists(appUsersFilePath))
            {
                return new List<Users>();
            }

            var json = File.ReadAllText(appUsersFilePath);

            if (json.Trim().Length > 0)
            {
                var deserializedData = JsonSerializer.Deserialize<List<Users>>(json);
                if (deserializedData != null)
                {
                    return deserializedData;
                }
            }

            return new List<Users>();
        }

        //To save all the users
        public static void SaveAll(List<Users> users)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appUsersFilePath = Utils.GetAppUsersFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(users);
            File.WriteAllText(appUsersFilePath, json);
        }

        // To create a new user
        public static List<Users> Create(Guid userId, string username, string password, Roles role)
        {
            List<Users> users = GetAll();
            bool usernameExists = users.Any(x => x.Username == username);

            if (usernameExists)
            {
                throw new Exception("Username already exists.");
            }

            users.Add(
                new Users
                {
                    Username = username,
                    PasswordHash = Utils.HashSecret(password),
                    Role = role,
                    CreatedBy = userId
                }
            );
            SaveAll(users);
            return users;
        }

        //To initialize a user
        public static void SeedUsers()
        {
            var users = GetAll().FirstOrDefault(x => x.Role == Roles.Admin);

            if (users == null)
            {
                Create(Guid.Empty, SeedUsername, SeedPassword, Roles.Admin);
            }
        }

        /*To login the user*/
        public static Users Login(string username, string password)
        {
            List<Users> users = GetAll();
            Users user = users.FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                throw new Exception("User not found. Try again.");
            }

            bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash);


            if (!passwordIsValid)
            {
                throw new Exception("Password incorrect. Try again.");
            }

            return user;
        }

        //To update user password
        public static Users ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            int MIN_PASSWORD_LENGTH = 8;
            List<Users> users = GetAll();
            Users user = users.FirstOrDefault(x => x.ID == userId);

            if (user == null) 
            {
                throw new Exception("User not found.");
            }

            if (oldPassword == newPassword)
            {
                throw new Exception("Please use a different password.");
            }

            bool validPassword = Utils.VerifyHash(oldPassword, user.PasswordHash);

            if (!validPassword)
            {
                throw new Exception("Invalid password. Try again.");
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                throw new Exception("New password cannot be empty or contain only spaces.");
            }

            if (newPassword.Length < MIN_PASSWORD_LENGTH)
            {
                throw new Exception($"New password must be at least {MIN_PASSWORD_LENGTH} characters long.");
            }

            user.PasswordHash = Utils.HashSecret(newPassword);
            user.HasInitialPassword = false;
            SaveAll(users);

            return user;
        }

        //To get a user by their ID
        public static Users GetById(Guid id)
        {
            List<Users> users = GetAll();
            return users.FirstOrDefault(x => x.ID == id);
        }
        
        // To delete a user
        public static List<Users> DeleteByUserId(Guid userId)
        {
            List<Users> users = GetAll();
            Users user = users.FirstOrDefault(x => x.ID == userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            users.Remove(user);
            SaveAll(users);
            return users;
        }

        
    }
}
