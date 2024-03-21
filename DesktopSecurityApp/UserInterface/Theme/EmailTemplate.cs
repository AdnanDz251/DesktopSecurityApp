namespace DesktopSecurityApp.UserInterface.Theme
{
    public class EmailTemplate
    {
        public string GetEmailBody()
        {
            return @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Email Template</title>
    <style>
        .container {
            text-align: center; /* Centrirajte sadržaj unutar kontejnera */
        }
        .header {
            text-align: left; /* Poravnavanje zaglavlja (header) na desno */
            padding: 10px 0; /* Dodajte razmak oko loga */
        }
        .logo {
            max-width: 100px;
        }
        .warning {
            color: red;
        }
    </style>
</head>
<body>
    <div class='header'>
        <img src='https://i.imgur.com/4UViIvF.png' alt='Desktop Security App Logo' class='logo'>
    </div>
    <div class='container'>
        <h1 class='warning'>!!! WARNING !!!</h1>
        <h3>Someone tried accessing your screen while in Security Mode</h3>
    </div>
</body>
</html>

";
        }
    }
}
