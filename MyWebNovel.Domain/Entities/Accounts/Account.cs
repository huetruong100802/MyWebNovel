using MyWebNovel.Domain.Entities.Shared;

namespace MyWebNovel.Domain.Entities.Accounts
{
    public sealed class Account : EntityBase
    {
        public Username Username { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public Password Password { get; private set; } = default!;
        public int RoleId { get; private set; }

        private Account()
        {
        }

        private Account(Username username, Email email, Password password, int roleId)
        {
            Username = username;
            Email = email;
            Password = password;
            RoleId = roleId;
        }

        /// <summary>
        /// Creates a new Account entity.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static Account Create(Username username, Email email, Password password, int roleId)
        {
            return new Account(username, email, password, roleId);
        }

        /// <summary>
        /// Changes the password for the Account entity.
        /// </summary>
        /// <param name="newPassword"></param>
        public void ChangePassword(Password newPassword)
        {
            Password = newPassword;
            UpdateLastModified();
        }
    }
}
