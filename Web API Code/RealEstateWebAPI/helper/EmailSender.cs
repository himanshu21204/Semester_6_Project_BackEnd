using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public class EmailSender
{
	private readonly IConfiguration _configuration;

	public EmailSender(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public bool SendOTPEmail(string email, string otp)
	{
		bool status = false;
		try
		{
			// Load email settings from configuration
			string fromEmail = _configuration.GetValue<string>("AppSettings:EmailSettings:From");
			string smtpServer = _configuration.GetValue<string>("AppSettings:EmailSettings:SmtpServer");
			int port = _configuration.GetValue<int>("AppSettings:EmailSettings:Port");
			string secretKey = _configuration.GetValue<string>("AppSettings:SecretKey");
			bool enableSSL = _configuration.GetValue<bool>("AppSettings:EmailSettings:EnablSSL") || true;

			// Email content
			string subject = "Your OTP for Password Reset";
			string message = $"Dear User,\n\nYour OTP for password reset is: {otp}\n\nThis OTP is valid for 5 minutes. Do not share it with anyone.\n\nBest Regards,\nYour Real Estate Team";

			// Setup MailMessage
			MailMessage mailMessage = new MailMessage()
			{
				From = new MailAddress(fromEmail),
				Subject = subject,
				Body = message,
				IsBodyHtml = false
			};
			mailMessage.To.Add(email);

			// Configure SMTP Client
			SmtpClient smtpClient = new SmtpClient(smtpServer)
			{
				Port = port,
				Credentials = new NetworkCredential(fromEmail, secretKey),
				UseDefaultCredentials = false,
				EnableSsl = enableSSL
			};

			// Send Email
			smtpClient.Send(mailMessage);
			status = true;
		}
		catch (Exception ex)
		{
			Console.WriteLine("Error sending OTP email: " + ex.Message);
		}
		return status;
	}
}