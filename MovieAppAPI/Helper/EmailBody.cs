using System.Web;

namespace MovieAppAPI.Helper
{
    public static class EmailBody
    {
        public static string EmailStringBody(string email, string emailToken)
        {
            return $@"<html>
                <head></head>
                <body>
                    <div style=""width: 300px; padding: 20px;"">
                        <div>
                            <h1>Reset your password</h1>
                            <hr>
                            <p>If you've lost your password or wish to reset it use the link bellow to get started.</p>
                            <div style=""padding: 30px 0; min-height:50px; "">
                                <a href = ""http://localhost:4200/reset/{email}/{HttpUtility.UrlEncode(emailToken)}"" target=""_blank"" style=""color: white; background : dodgerblue; padding: 15px 50px; text-align: center; font-weight: bold; text-decoration: none;"">Reset your password</a>
                            </div>
                            <p>If this was a mistake, just ignore this email and nothing will happen.</p>
                        </div>
                    </div>
                </body>
            </html>";
        }
    }
}
