namespace Ecom.Core.Sharingl;

public class EmailStringBody
{
    public static string send(string email, string token, string component, string message)
    {
        string encodeToken = Uri.EscapeDataString(token);
        return $@"
              <html>
<head>
 <style>
 .button{{
  display: inline-block;
            margin-top: 20px;
            padding: 12px 24px;
            background-color: #4CAF50;
            color: white !important;
            text-decoration: none;
            font-weight: bold;
            border-radius: 4px;
}}
</style>
</head>
                    <body>
                        <h1>{message}</h1>
                                <hr>
                                <br>
                                <a class=""button"" href=""http://localhost:4200/Account/component?email={email}&code={encodeToken}""> 
                                   {message}
                                </a>
                    </body>
              <\html>
";
    }
}