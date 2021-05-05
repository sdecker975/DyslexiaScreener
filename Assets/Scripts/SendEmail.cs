using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SendEmail : MonoBehaviour {
    public void SendEmailButton()
    {
        // Path to directory of files to compress and decompress.
        string filePath = "data";
        FileInfo fileInfo = new FileInfo(filePath);
        DirectoryInfo di = new DirectoryInfo(fileInfo.FullName);

        print(di.FullName);

        lzip.compressDir(di.FullName, 9, filePath + " " + Settings.dateTime + ".zip", false, null);

        print("Hit email");
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("acnl.caredata@gmail.com");
        mail.To.Add("acnl.caredata@gmail.com");
        mail.Subject = "Care Data from Admin: " + Settings.id;
        mail.Body = "Version: " + Menu.versionNumber;

        Attachment data = new Attachment(filePath + " " + Settings.dateTime + ".zip", MediaTypeNames.Application.Octet);
        mail.Attachments.Add(data);

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("acnl.caredata@gmail.com", "Neurocog1!") as ICredentialsByHost;
        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)

        { return true; };

        smtpServer.Send(mail);
    }

}
