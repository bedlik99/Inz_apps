using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using ServerAPI.Entities;
using ServerAPI.Repositories;
using ServerAPI.Settings;
using System;
using System.Linq;
using ServerAPI.Exceptions;
using System.Collections.Generic;
using ServerAPI.DTOs;
using MailKit.Security;


namespace ServerAPI.Services
{
	public class EmailService : IEmailRepo
	{
		private readonly EmailSettings _emailSettings;
		private readonly ServerDBContext _context;

		public EmailService(EmailSettings emailSettings, ServerDBContext context)
		{
			_emailSettings = emailSettings;
			_context = context;
		}
		public string SendEmail(EmailData emailData)
		{
			try
			{
				MimeMessage emailMessage = new MimeMessage();

				MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
				emailMessage.From.Add(emailFrom);

				MailboxAddress emailTo = new MailboxAddress(emailData.EmailToName, emailData.EmailToId);
				emailMessage.To.Add(emailTo);

				emailMessage.Subject = emailData.EmailSubject;

				BodyBuilder emailBodyBuilder = new BodyBuilder();
				emailBodyBuilder.TextBody = emailData.EmailBody;
				emailMessage.Body = emailBodyBuilder.ToMessageBody();

				SmtpClient emailClient = new SmtpClient();
				//Funkcja obsługi odwołań nie może sprawdzić odwołania certyfikatu.
				//Ta linia kodu pozwala na wykonanie zapytania. Docs w zakładkach
				emailClient.CheckCertificateRevocation = false;
				emailClient.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
				emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
				emailClient.Send(emailMessage);
				emailClient.Disconnect(true);
				emailClient.Dispose();

				return $"An email has been sent to {emailData.EmailToId}";
			}
			catch (Exception ex)
			{
				throw new BadRequestException(ex.Message);
			}
		}

		public string SendEmail(IEnumerable<RegisteredLabUserDTO> setOfEmailData)
		{
			var mailList = new List<MimeMessage>();
			foreach (var user in setOfEmailData)
			{
				MimeMessage emailMessage = new MimeMessage();
				MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
				emailMessage.From.Add(emailFrom);
				//Dodac imie nazwisko
				MailboxAddress emailTo = new MailboxAddress(user.Email, user.Email);
				emailMessage.To.Add(emailTo);
				emailMessage.Subject = "REJESTRACJA NA LABORATORIUM";
				BodyBuilder emailBodyBuilder = new BodyBuilder();
				emailBodyBuilder.TextBody = $"Dane użytkownika do logowania się na laboratorium: \n Username: {user.Email} \n UniqueCode: {user.UniqueCode} ";
				emailMessage.Body = emailBodyBuilder.ToMessageBody();
				mailList.Add(emailMessage);
			}
			//is async needed here?
			try
			{
				SmtpClient emailClient = new SmtpClient();
				emailClient.CheckCertificateRevocation = false;
				emailClient.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
				emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
				foreach(var emailMessage in mailList)
				{
					emailClient.Send(emailMessage);
				}				
				emailClient.Disconnect(true);
				emailClient.Dispose();
				return $"Operacja zakończyła się sukcesem. Poświadczenia zostały wysłane prawidłowo.";
			}

			//CZY JAK WYWALI BŁĄD TO NIE POWINIENEM ODWOŁAĆ DODAWANIE DO BD?
			catch (Exception ex)
			{
				foreach (var item in setOfEmailData)
				{
					var user = _context
					.RegisteredUserItems
					.SingleOrDefault(u => u.Email == item.Email && u.UniqueCode == item.UniqueCode);					
					_context.RegisteredUserItems.Remove(user);
					_context.SaveChanges();
				}

				throw new BadRequestException(ex.Message + " EMAIL ISSUE!");
			}
		}
	}
}
