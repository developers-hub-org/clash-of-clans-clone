using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace DevelopersHub.RealtimeNetworking.Server
{
    public class Email
    {

        protected static readonly string host = "smtp.gmail.com";
        protected static readonly int port = 587;
        protected static readonly string address = "demo@gmail.com";
        protected static readonly string password = "abcd";
        protected static readonly string name = "Developers Hub";
        protected static readonly string logo = "https://github.githubassets.com/images/modules/logos_page/GitHub-Mark.png";

        public static bool Send(string to, string subject, string mail)
        {
            MailMessage message = new MailMessage(new MailAddress(address, name), new MailAddress(to));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = mail;
            message.SubjectEncoding = Encoding.UTF8;
            message.BodyEncoding = Encoding.UTF8;
            try
            {
                using (var smtp = new SmtpClient(host, port))
                {
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(address, password);
                    smtp.Send(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async static Task<bool> SendAsync(string to, string subject, string mail)
        {
            Task<bool> task = Task.Run(() =>
            {
                return Send(to, subject, mail);
            });
            return await task;
        }

        public static bool SendEmailVerificationCode(string code, string email)
        {
            string mail = email_verification_code_template;
            mail = mail.Replace("[company_logo_url]", logo);
            mail = mail.Replace("[company_name]", name);
            mail = mail.Replace("[user_name]", email);
            mail = mail.Replace("[email_description]", "You can use this code to recover your account. Enter this code in the game to load your progress.");
            mail = mail.Replace("[verification_code]", code);
            mail = mail.Replace("[remained_time]", Data.recoveryCodeExpiration + " seconds");
            mail = mail.Replace("[copyright_footer]", "© " + DateTime.UtcNow.Year.ToString() + " " + name + " , Torento, Canada");
            return Send(email, "Account Recovery Code", mail);
        }

        public static bool SendEmailConfirmationCode(string code, string email)
        {
            string mail = email_verification_code_template;
            mail = mail.Replace("[company_logo_url]", logo);
            mail = mail.Replace("[company_name]", name);
            mail = mail.Replace("[user_name]", email);
            mail = mail.Replace("[email_description]", "You can use this code to confirm your account. Enter this code in the game and your progress will be synced with this email adress.");
            mail = mail.Replace("[verification_code]", code);
            mail = mail.Replace("[remained_time]", Data.recoveryCodeExpiration + " seconds");
            mail = mail.Replace("[copyright_footer]", "© " + DateTime.UtcNow.Year.ToString() + " " + name + " , Torento, Canada");
            return Send(email, "Email Confirmation Code", mail);
        }

        protected static readonly string email_verification_code_template = @"
        <html>

        <head>

        </head>
        <body>

        <div id="":r8"" class=""ii gt"" jslog=""20277; u014N:xr6bB; 4:W251bGwsbnVsbCxbXV0."">
            <div id = "":pt"" class=""a3s aiL msg5125996902626164894"">
                <div style = ""margin:0;padding:0"" bgcolor=""#FFFFFF"">
                    <table width = ""100%"" height=""100%"" style=""min-width:348px"" border=""0"" cellspacing=""0"" cellpadding=""0"" lang=""en"">
                        <tbody>
                            <tr height = ""32"" style=""height:32px"">
                                <td>
                            
                                </td>
                            </tr>
                            <tr align = ""center"">
                                <td>
                                    <table border=""0"" cellspacing=""0"" cellpadding=""0"" style=""padding-bottom:20px;max-width:516px;min-width:220px"">
                                        <tbody>
                                            <tr>
                                                <td width = ""8"" style=""width:8px"">
                                                </td>
                                                <td>
                                                    <div style = ""border-style:solid;border-width:thin;border-color:#dadce0;border-radius:8px;padding:40px 20px"" align=""center"" class=""m_5125996902626164894mdv2rw"">
                                                        <img src = ""[company_logo_url]"" width=""74"" height=""74"" aria-hidden=""true"" style=""margin-bottom:16px"" alt=""Logo"" class=""CToWUd"">
                                                        <div style = ""font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;border-bottom:thin solid #dadce0;color:rgba(0,0,0,0.87);line-height:32px;padding-bottom:24px;text-align:center;word-break:break-word"">
                                                            <div style=""font-size:24px"">
                                                                [company_name]
                                                            </div>
                                                            <table align = ""center"" style=""margin-top:8px"">
                                                                <tbody>
                                                                    <tr style = ""line-height:normal"">
                                                                        <td>
                                                                            <a style=""font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;color:rgba(0,0,0,0.87);font-size:14px;line-height:20px"">
                                                                                [user_name]
                                                                            </a>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table> 
                                                        </div>
                                                        <div style = ""font-family:Roboto-Regular,Helvetica,Arial,sans-serif;font-size:14px;color:rgba(0,0,0,0.87);line-height:20px;padding-top:20px;text-align:center"">
                                                            <br>
                                                            [email_description]
                                                            <div style=""padding-top:32px;text-align:center"">
                                                                <div style = ""font-family:'Google Sans',Roboto,RobotoDraft,Helvetica,Arial,sans-serif;line-height:16px;color:#ffffff;font-weight:400;text-decoration:none;font-size:16px;display:inline-block;padding:10px 24px;background-color:#4184f3;border-radius:5px;min-width:90px"" target=""_blank"">
                                                                    [verification_code]
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <span class=""im"">
                                                            <div style = ""padding-top:20px;font-size:12px;line-height:16px;color:#5f6368;letter-spacing:0.3px;text-align:center"">
                                                                This code will expire after [remained_time].
                                                            </div>
                                                        </span>
                                                    </div>
                                                    <div>
                                                        <div class=""adm"">
                                                            <div id = ""q_67"" class=""ajR h4"">
                                                                <div class=""ajT"">

                                                                </div>
                                                            </div>
                                                        </div>
                                                    <div class=""h5"">
                                                        <div style = ""text-align:left"">
                                                            <div style=""font-family:Roboto-Regular,Helvetica,Arial,sans-serif;color:rgba(0,0,0,0.54);font-size:11px;line-height:18px;padding-top:12px;text-align:center"">
                                                                <div>This email has been sent at your request, Please do not reply to it.</div>
                                                                    <div style = ""direction:ltr"">
                                                                        [copyright_footer]
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td width= ""8"" style= ""width:8px"">
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr height= ""32"" style= ""height:32px"">
                                <td>

                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        </body>
        </html>";

    }
}
