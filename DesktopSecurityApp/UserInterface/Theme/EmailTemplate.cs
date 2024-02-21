using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /* Stil za gumb */
        .btn {
            padding: 12px 24px;
            border-radius: 4px;
            color: #FFF;
            background: #2B52F5;
            display: inline-block;
            margin: 30px auto; 
            text-decoration: none;
        }
        .container {
            text-align: center; /* Centrirajte sadržaj unutar kontejnera */
        }
        .footer {
          margin-top: 2rem;
          text-align: center;
        }
    </style>
</head>
<body>
    <div class='container'>
        <h1>Desktop Security App</h1>
        <h3>Someone pressed the wrong Key-Bind on your computer.</h3>
        <button class='btn'>I pressed</button>
        <p>If you didn't press the wrong key-bind, you can ignore this email.</p>
        <p>Thanks,<br>DSA team</p>
    </div>
<div class='footer'>
        <img src='https://banner2.cleanpng.com/20180506/uiw/kisspng-network-security-computer-network-computer-securit-cyber-security-5aef20fd419b15.5111930615256209892687.jpg' alt='Desktop Security App Logo' style='max-width: 200px;'>
    </div>
</body>
</html>
";
        }
    }
}
