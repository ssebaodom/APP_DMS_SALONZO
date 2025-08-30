namespace SSE.Core.Common.Identity
{
    public class IdentityErrorCode
    {
        /// <summary>
        /// Mật khẩu hiện tại không đúng
        /// </summary>
        public const string PasswordMismatch = nameof(PasswordMismatch);

        /// <summary>
        /// Mật khẩu ngắn hơn yêu cầu
        /// </summary>
        public const string PasswordTooShort = nameof(PasswordTooShort);

        /// <summary>
        /// Mật khẩu phải có ký tự đặc biệt
        /// </summary>
        public const string PasswordRequiresNonAlphanumeric = nameof(PasswordRequiresNonAlphanumeric);

        /// <summary>
        /// Mật khẩu phải có chữ thường
        /// </summary>
        public const string PasswordRequiresLower = nameof(PasswordRequiresLower);

        /// <summary>
        /// Mật khẩu phải có ký tự hoa
        /// </summary>
        public const string PasswordRequiresUpper = nameof(PasswordRequiresUpper);

        /// <summary>
        /// Mật khẩu phải có ký tự số
        /// </summary>
        public const string PasswordRequiresDigit = nameof(PasswordRequiresDigit);

        /// <summary>
        /// Tên đăng nhập đã tồn tại
        /// </summary>
        public const string DuplicateUserName = nameof(DuplicateUserName);

        /// <summary>
        /// Email đã tồn tại
        /// </summary>
        public const string DuplicateEmail = nameof(DuplicateEmail);

        /// <summary>
        /// Tên người dùng không hợp lệ
        /// </summary>
        public const string InvalidUserName = nameof(InvalidUserName);

        /// <summary>
        /// Email không hợp lệ
        /// </summary>
        public const string InvalidEmail = nameof(InvalidEmail);
    }
}