using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

public class Emailing
{
    public static bool SendEmail(string fromEmail, string toEmail, string subject, string body)
    {
        if (fromEmail == string.Empty)
            fromEmail = ConfigurationManager.AppSettings["MailFrom"].ToString();

        if (toEmail == string.Empty)
            toEmail = ConfigurationManager.AppSettings["MailTo"].ToString();

        subject = subject == string.Empty ? "Placeberry info" : subject;

        /*----------------------------------------------------------------------
         * The specified string is not in the form required for a subject 
         * The result may be that the System.Net.Mail namespace throws an exception, and usually the culprit 
         * is the subject line as mentioned in the title of this post. Here is an easy fix: 
         * subject= Regex.Replace(subject, @"[^ -~]", string.Empty); 
         * */
        subject = Regex.Replace(subject, @"[^ -~]", string.Empty);
        /*----------------------------------------------------------------------*/

        MailMessage mailMessage = new MailMessage(fromEmail, toEmail, subject, body);
        mailMessage.IsBodyHtml = true;

        SmtpClient smtpClient = new SmtpClient();
        smtpClient.EnableSsl = true;
        

        try
        {
            smtpClient.Send(mailMessage);
        }
        catch (Exception e)
        {
            SendExceptionToEmail(e);
            return false;
        }

        return true;
    }

    public static bool SendEmailThroughGmail(string pGmailEmail, string pGmailPassword, string pTo, string pSubject, string pBody)
    {
        string pGmailMailServer = "smtp.gmail.com";

        try
        {
            System.Web.Mail.MailMessage myMail = new System.Web.Mail.MailMessage();
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", pGmailMailServer);
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", "465");
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
            //sendusing: cdoSendUsingPort, value 2, for sending the message using 
            //the network.

            //smtpauthenticate: Specifies the mechanism used when authenticating 
            //to an SMTP 
            //service over the network. Possible values are:
            //- cdoAnonymous, value 0. Do not authenticate.
            //- cdoBasic, value 1. Use basic clear-text authentication. 
            //When using this option you have to provide the user name and password 
            //through the sendusername and sendpassword fields.
            //- cdoNTLM, value 2. The current process security context is used to 
            // authenticate with the service.
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            //Use 0 for anonymous
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", pGmailEmail);
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", pGmailPassword);
            myMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
            myMail.From = pGmailEmail;
            myMail.To = pTo;
            myMail.Subject = pSubject;
            myMail.BodyFormat = System.Web.Mail.MailFormat.Html;
            myMail.Body = pBody;

            System.Web.Mail.SmtpMail.SmtpServer = pGmailMailServer + ":465";
            System.Web.Mail.SmtpMail.Send(myMail);
            return true;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public static void SendExceptionToEmail(Exception ex)
    {
        if (ex.GetType() != typeof(System.Threading.ThreadAbortException) &&
            ex.GetType() != typeof(System.Web.HttpException))
        {
            string body = "<div style='font: 11px Verdana; color: #333;'>[" + DateTime.Now.ToString() + "]";
            HttpContext context = HttpContext.Current;

            body += "<br /><br /><b>URL: </b><br />" + context.Request.Url.ToString() + "</b>";

            if (context.Request.UrlReferrer != null)
                body += "<br /><br /><b>URL referrer: </b><br />" + context.Request.UrlReferrer.AbsoluteUri + "</b>";

            body += "<br /><br /><b>TYPE: </b><br />" + ex.GetType().ToString() + "</b>";
            body += "<br /><b>MESSAGE: </b><br />" + ex.Message + "</b>";
            body += "<br />-------------------------------------------------------------------";
            body += "<br />STACK TRACE:<br />" + ex.StackTrace;
            body += "</div>";

            SendEmail(ConfigurationManager.AppSettings["ExceptionEmail"].ToString(),
                ConfigurationManager.AppSettings["ExceptionEmail"].ToString(), "Placeberry BUG Report - " +
                context.Request.UserHostName + " - " + ex.Message, body);
        }
    }
}
