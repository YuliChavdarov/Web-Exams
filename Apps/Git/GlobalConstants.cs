using System;
using System.Collections.Generic;
using System.Text;

namespace Git
{
    public static class GlobalConstants
    {
        public const int UsernameMinLength = 5;
        public const int UsernameMaxLength = 20;
        public const int PasswordMinLength = 6;
        public const int PasswordMaxLength = 20;
        public static readonly string UsernameLengthError = "Username length must be between " + UsernameMinLength + " and " + UsernameMaxLength + " characters.";
        public const string EmailError = "Invalid email.";
        public static readonly string PasswordLengthError = "Password length must be between " + PasswordMinLength + " and " + PasswordMaxLength + " characters.";
        public const string ConfirmPasswordError = "Password and confirmed password did not match.";
        public const string UsernameTakenError = "Username already taken.";
        public const string EmailTakenError = "Email already taken.";
        public const string LoginError = "Invalid username or password.";
        public const int RepositoryNameMinLength = 3;
        public const int RepositoryNameMaxLength = 10;
        public static readonly string RepositoryNameLengthError = "Repository name length must be between " + RepositoryNameMinLength + " and " + RepositoryNameMaxLength + " characters.";
        public const int CommitDescriptionMinLength = 5;
        public static readonly string CommitDescriptionLengthError = "Commit description length must be at least " + CommitDescriptionMinLength + " characters.";
        public const string RepositoryNotFoundError = "Repository not found.";
        public const string CommitNotFoundError = "Commit not found.";
        public const string OnlyOwnerCanDeleteCommitError = "Only the owner of the commit can delete it.";

    }
}
