namespace BlogApi.JwtConfigure;
public static class Configure
{
     public static string JwtKey { get; set; } = "H#FDI74849JHFILWL388EJE88378IK@I&&KKSE82HHLS009473MSLA";
     public static SmtpConfiguration Smtp = new();

     public class SmtpConfiguration
     {
          public string? Server { get; set; }
          public int Port { get; set; }
          public string? SenderName { get; set; }
          public string? SenderEmail { get; set; }
          public string? Username { get; set; }
          public string? Password { get; set; }
     }
}
