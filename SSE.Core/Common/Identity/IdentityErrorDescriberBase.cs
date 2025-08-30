using Microsoft.AspNetCore.Identity;

namespace SSE.Core.Common.Identity
{
    public class IdentityErrorDescriberBase : IdentityErrorDescriber
    {
        public override IdentityError PasswordMismatch()
        {
            return new IdentityError { Code = nameof(IdentityErrorCode.PasswordMismatch), Description = "Mật khẩu không chính xác." };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError { Code = nameof(IdentityErrorCode.InvalidEmail), Description = "Tên người dùng không hợp lệ." };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError { Code = nameof(IdentityErrorCode.InvalidEmail), Description = "Email không hợp lệ." };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError { Code = nameof(IdentityErrorCode.DuplicateUserName), Description = "Tên người dùng đã tồn ta." };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError { Code = nameof(IdentityErrorCode.DuplicateEmail), Description = "Email đã tồn tại." };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError { Code = nameof(IdentityErrorCode.PasswordTooShort), Description = "Độ dài mật khẩu không hợp lệ." };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError { Code = nameof(IdentityErrorCode.PasswordRequiresNonAlphanumeric), Description = "Mật khẩu phải có ký tự đặc biệt." };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError { Code = nameof(IdentityErrorCode.PasswordRequiresDigit), Description = "Mật khẩu phải có số." };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError { Code = nameof(IdentityErrorCode.PasswordRequiresLower), Description = "Mật khẩu phải có ký tự thường." };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError { Code = nameof(IdentityErrorCode.PasswordRequiresUpper), Description = "Mật khẩu phải có ký tự viết hoa." };
        }
    }
}