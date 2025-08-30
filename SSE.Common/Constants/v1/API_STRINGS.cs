namespace SSE.Common.Constants.v1
{
    public static class API_STRINGS
    {
        public const string APP = "ERP APP";
        public const string SERVER_CONNECTED = "SERVER API IS RUNNING!";
        public const string EMAIL_AUTHOR = "truongkhoanthcn@gmail.com";
        public const string INVALID_REFRESH_TOKEN = "Refresh token không còn hiệu lực!. Vui lòng đăng nhập lại.";
        public const string TOKEN_NOT_EXIST = "Refresh token không tồn tại!";
        public const string FACEBOOK_TOKEN_NOT_VALID = "Không tìm được tài khoản. Vui lòng kiểm tra lại!";
        public const string PASSWORD_INVALID = "Mật khẩu không chính xác!";
        public const string SINGIN_FAILD = "Đăng nhập thất bại!";
        public const string CHANGE_PASS_FAILD = "Chưa thay đổi được mật khẩu!";
        public const string RESET_PASS_FAILD = "Chưa reset được mật khẩu!";
        public const string RESET_PASS_SUCCESSED = "Mật khẩu mới của bạn là: ";
        public const string MAIL_RECEIVED = " Vui lòng kiểm tra email của bạn để lấy mã OTP.";
        public const string PHONE_RECEIVED = " Vui lòng kiểm tra tin nhắn của bạn để lấy mã OTP.";
        public const string OTP_VALID_MESS = ". Mã có hiệu lực trong vòng {0} phút.";
        public const string ACCOUNT_EXIST = "Tài khoản đã tồn tại!";
        public const string ACCOUNT_NOT_EXIST = "Tài khoản không tồn tại!";
        public const string RESET_MAIL_SUBJECT = "Yêu cầu đổi mật khẩu";
        public const string RESET_MAIL_BODY = "Mã OTP để thay đổi mật khẩu là: ";
        public const string SEND_OTP_TO_REGISTER = "Mã OTP để đăng ký tài khoản là: ";
        public const string SMS_PASS_INFO = "Mật khẩu để nhập tài khoản của số điện thoại {0} là: {1}. Xin cảm ơn! ";

        public const string SEND_SMS_REGISTER_ERROR = "Tài khoản đăng ký thành công. " +
                            "Tuy nhiên mật khẩu chưa gửi được tới số điện thoại của bạn. " +
                            "Vui lòng liên hệ hotline để được xử lý.";

        public const string WAITING_FOR_OTP = "Vui lòng chờ trong giây lát hoặc thử lại!";
        public const string FORGOT_PASSWORD_OTP_SUCCESS = "Đã gửi thành công mã OTP về số điện thoại!";
        public const string SEND_OTP_SUCCESS = "Đã gửi thành công mã OTP về số điện thoại!";
        public const string FORGOT_PASSWORD_MAIL_SUCCESS = "Đã gửi thành công mã OTP về email!";
        public const string OTP_DESTROYED = "OTP không còn hiệu lực!";
        public const string OTP_INVALID = "OTP không hợp lệ!";
        public const string UPDATE_USER_FAILD = "Cập nhật lỗi. Vui lòng kiểm tra lại!";
        public const string EXCEPTION_MESS_DEFAULT = "Có lỗi xảy ra! Vui lòng kiểm tra log để biết thêm chi tiết.";
        public const string EXCEPTION_MESS_NOT_FOUND = "Không có thông tin!";
        public const string EXCEPTION_MESS_FORBIDDEN = "Server từ chối xử lý yêu cầu!";
        public const string NOTIF_BOT_FOUND = "Không tìm thấy thông báo!";

        // Login Strings
        public const string USER_INFO_INVALID = "Host Id hoặc tên đăng nhập không đúng!";

        public const string USER_NAME_INVALID = "Tên đăng nhập không đúng!";
        public const string USER_NAME_LOCK = "Tài khoản của bạn đã bị khóa!";
        public const string pASS_INVALID = "Mật khẩu không đúng!";

        // Config
        public const string SET_APP_DBNAME_FAILED = "Chưa chọn được giá trị mặc định công ty!";

        public const string GET_COMPANIES_FAILED = "Không lấy được danh sách công ty!";
        public const string GET_UNITS_FAILED = "Không lấy được danh sách đơn vị!";
    }
}