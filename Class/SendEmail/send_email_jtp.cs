using jasuindo_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using web_api_managemen_user;

namespace EmailClass
{
    public class send_email_jtp
    {
        public static string user_email = Startup.Configuration["Mail:from_mail"]; 
      
        public static void send_email(MailMessage content_email)
        {
            try
            {
                

                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = Startup.Configuration["Mail:host"];
                client.Port = int.Parse(Startup.Configuration["Mail:port"]);

                NetworkCredential credentials = new NetworkCredential(Startup.Configuration["Mail:username"], Startup.Configuration["Mail:password"]);
                client.UseDefaultCredentials = true;
                client.Credentials = credentials;
                client.Send(content_email);
                Console.WriteLine("Send Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void send_email(m_user user,string password)
        {

            try
            {
                
                var filePath = @"Assetts/kemenag_logo.jpeg";

                string html_body = @"
                <html>
                <head>
                <style>
                table, td, th {
                  border: 0px solid black;
                }


                td{
                text-align:left;
                }

                </style>
                <body>

              
                <br>
                <br>
                <div style='margin-top:20'>
	                 Data login sementara sebagai berikut :
                </div>
                <table style='margin-top:20'>
                <tr>
                    <td>User Name</td>
                    <td> : </td>
                    <td>" + user.m_username + @"</td>
                 </tr>
                 <tr>
                    <td>Password</td>
                    <td> : </td>
                    <td>" + password + @"</td>
                 </tr>
                                 
                </table>
                <div style='margin-top:20'>
	                 jika login berhasil dimohon untuk melakukan pergantian password sementara dengan password yang di inginkan pengguna
                </div>
                </body>
                </html>
                ";

                MailMessage mail = new MailMessage();
                ////string path = @"Assetts/kemenag_logo.jpeg";
                ////LinkedResource Img = new LinkedResource(path, MediaTypeNames.Image.Jpeg);
                ////Img.ContentId = "MyImage";
                //AlternateView AV =
                //AlternateView.CreateAlternateViewFromString(html_body, null, MediaTypeNames.Text.Html);
                ////AV.LinkedResources.Add(Img);
                mail.From = new MailAddress(send_email_jtp.user_email);
                mail.To.Add(new MailAddress(user.m_user_email));
                mail.Subject = "password login sementara";
                mail.Body = html_body;
                mail.IsBodyHtml = true;
                //mail.AlternateViews.Add(AV);
                mail.BodyEncoding = Encoding.Default;
                mail.Priority = MailPriority.High;
                //mail.Attachments.Add(att);
                send_email_jtp.send_email(mail);
                Console.WriteLine("Send Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
