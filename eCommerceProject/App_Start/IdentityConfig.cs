using eCommerceProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eCommerceProject
{
	public class EmailService : IIdentityMessageService
	{
		public Task SendAsync(IdentityMessage message)
		{
			// Plug in your email service here to send an email.
			//return Task.FromResult(0);
			return Task.Factory.StartNew(() =>
			{
				sendMail(message);
			});

		}
		void sendMail(IdentityMessage message)
		{
			#region formatter
			string text = string.Format("Please click on this link to {0}: {1}", message.Subject, message.Body);
			//string html = "News:  <a href=\"" + message.Body + "\">link</a><br/>";
			string html = " <!DOCTYPE html> " +

				"<html> <head> <title ></title>" +
				"<meta http-equiv=\"Content - Type\" content=\"text / html; charset = utf - 8\" /> <meta name = \"viewport\" content = \"width=device-width, initial-scale=1\" ><meta http - equiv = \"X-UA-Compatible\" content = \"IE=edge\" /> " +

						" <style type =\"text / css\" >" +

						 "body, table, td, a { -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; }" +
			"table, td { mso - table - lspace: 0pt; mso - table - rspace: 0pt; }" +
			"img { -ms - interpolation - mode: bicubic; }" +

			"img { border: 0; height: auto; line-height: 100%; outline: none; text-decoration: none; }" +
		 "table { border-collapse: collapse!important; }" +
			"body { height: 100% !important; margin: 0!important; padding: 0!important; width: 100% !important; }" +


			"a[x-apple-data-detectors] { color: inherit!important; text-decoration: none!important;  font-size: inherit!important; font-family: inherit!important; font-weight: inherit!important; line-height: inherit!important; }" +


			"div[style *= \"margin: 16px 0; \"] { margin: 0!important;}" +
 "</style >" +
"<body style =\"margin: 0 !important; padding: 0!important; background-color: #eeeeee; height: 100%;  width: 100% \" bgcolor =\"#eeeeee\">" +







		"<table border =\"0\" cellpadding =\"0\" cellspacing =\"0\" width = \"100%\"> <tr>  <td align = \"center\" style =\"background-color: #eeeeee;\" bgcolor = \"#eeeeee\"> <table align = \"center\" border = \"0\" cellpadding = \"0\" cellspacing = \"0\" width = \"100%\" style = \"max-width:600px;\"> <tr> " +


										"<td align = \"center\" valign =\"top\" style = \"font-size:0; padding: 35px; \" bgcolor =\"#F44336\">" +


									 "<div style = \"display: inline-block; max-width:50 %; min-width:100px; vertical-align:top; width: 100%; \"> " +
												"<table align = \"left\" border = \"0\" cellpadding =\"0\" cellspacing = \"0\" width = \"100%\" style = \"max-width:300px; \">" +

																 "<tr>" +

																"<td align = \"left\" valign = \"top\" style = \"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 36px; font-weight: 800; line-height: 48px; \" class=\"mobile-center\">" +
																"<h2 style = \"font-size: 36px; font-weight: 800; margin: 0; color: #ffffff;\"> CZA Store</h2> " +
														"</td>" +
												"</tr>" +
									 " </table >" +
								"</div>" +



					 "<div style = \"display: inline-block; max-width:50%; min-width:100px; vertical-align:top; width: 100%; \" class=\"mobile-hide\">" +
																		"<table align = \"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100% \" style=\"max-width:300px; \">" +
																"<tr>" +
																"<td align = \"right\" valign = \"top\" style = \"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; line-height: 48px;\"> " +

																						"</td></tr></table></div> </td> </tr>" +

														 "<tr>" +

																"<td align = \"center\" style = \"padding: 35px 35px 20px 35px; background-color: #ffffff;\" bgcolor = \"#ffffff\">" +

																		"<table align = \"center\" border = \"0\" cellpadding = \"0\" cellspacing = \"0\" width = \"100% \" style = \"max-width:600px; \">" +

																											"<tr>" +

																										 "<td align = \"center\" style = \"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding-top: 25px; \">"





																									 //
																									 + message.Body +



																									 "<td align = \"center\" style = \" padding: 35px; background-color: #ff7361;\" bgcolor = \"#1b9ba3\">" +


																										 "<table align = \"center\" border = \"0\" cellpadding = \"0\" cellspacing = \"0\" width = \"100%\" style = \"max-width:600px;\">" +


																																	 "<tr> <td align = \"center\" style = \"font - family: Open Sans, Helvetica, Arial, sans-serif; font-size: 16px; font-weight: 400; line-height: 24px; padding-top: 25px; \">" +


																																									"<h2 style = \"font-size: 24px; font-weight: 800; line-height: 30px; color: #ffffff; margin: 0;\" > Get 30% off your next order. </h2 > </td> </tr>" +

																																"<tr>" +

																																	 "<td align = \"center\" style = \"padding: 25px 0 15px 0;\"> <table border = \"0\" cellspacing = \"0\" cellpadding = \"0\" > <tr>" +


																																										"<td align = \"center\" style = \"border-radius: 5px; \" bgcolor = \"#66b3b7\">" +


																																											 "<a target = \"_blank\" style = \"font-size: 18px; font-family: Open Sans, Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; border-radius: 5px; background-color: #F44336; padding: 15px 30px; border: 1px solid #F44336; display: block;\"> Shop Again </a> </td>  </tr></table></td></tr></table> </td> </tr><tr>" +

																																									 "<td align = \"center\" style = \"padding: 35px; background - color: #ffffff;\" bgcolor = \"#ffffff\">" +



																																										"<table align = \"center\" border = \"0\" cellpadding = \"0\" cellspacing = \"0\" width = \"100% \" style = \"max-width:600px; \" ><tr>" +


																																																			"<td align = \"center\" >" +


																																									 									"<img src = \"logo-footer.png\" width = \"37\" height = \"37\" style =\"display: block; border: 0px; \" />" +



																																																			 "</td></tr><tr>" +



																																																		"<td align = \"center\" style = \"font-family: Open Sans, Helvetica, Arial, sans-serif; font - size: 14px; font-weight: 400; line-height: 24px; padding: 5px 0 10px 0; \" >" +


																																																									"<p style = \"font-size: 14px; font-weight: 800; line-height: 18px; color: #333333;\">" +

																																																				 " 675 Parko Avenue<br>LA, CA 02232 </p></td></tr><tr>" +

																																									 									"<td align = \"left\" style = \"font-family: Open Sans, Helvetica, Arial, sans-serif; font-size: 14px; font - weight: 400; line-height: 24px; \" >" +



																																																									 "<p style = \"font-size: 14px; font-weight: 400; line-height: 20px; color: #777777;\" >If you didn't create an account using this email address, please ignore this email or <a  target=\"_blank\" style=\"color: #777777;\">unsusbscribe</a>." +
																																																					"</p></td></tr></table>" +

																																									 "</td></tr>" +

																																				"</table>" +
																																				"</td>" +
																																		"</tr>" +
																																 "</table>" +

																				 "</body>" +
																																" </html> ";






			//html += HttpUtility.HtmlEncode(@"" + message.Body);
			#endregion

			MailMessage msg = new MailMessage();
			msg.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString());
			msg.To.Add(new MailAddress(message.Destination));
			msg.Subject = message.Subject;
			msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
			msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

			SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
			System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"].ToString(), ConfigurationManager.AppSettings["Password"].ToString());
			smtpClient.Credentials = credentials;
			smtpClient.EnableSsl = true;
			smtpClient.Send(msg);
		}
	}

	public class SmsService : IIdentityMessageService
	{
		public Task SendAsync(IdentityMessage message)
		{
			// Plug in your SMS service here to send a text message.
			return Task.FromResult(0);
		}
	}

	// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
	public class ApplicationUserManager : UserManager<ApplicationUser>
	{
		public ApplicationUserManager(IUserStore<ApplicationUser> store)
				: base(store)
		{
		}

		public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
		{
			var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
			// Configure validation logic for usernames
			manager.UserValidator = new UserValidator<ApplicationUser>(manager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			// Configure validation logic for passwords
			manager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = true,
				RequireDigit = true,
				RequireLowercase = true,
				RequireUppercase = true,
			};

			// Configure user lockout defaults
			manager.UserLockoutEnabledByDefault = true;
			manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
			manager.MaxFailedAccessAttemptsBeforeLockout = 5;

			// Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
			// You can write your own provider and plug it in here.
			manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
			{
				MessageFormat = "Your security code is {0}"
			});
			manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
			{
				Subject = "Security Code",
				BodyFormat = "Your security code is {0}"
			});
			manager.EmailService = new EmailService();
			manager.SmsService = new SmsService();
			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null)
			{
				manager.UserTokenProvider =
						new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
			}
			return manager;
		}
	}

	// Configure the application sign-in manager which is used in this application.
	public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
	{
		public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
				: base(userManager, authenticationManager)
		{
		}

		public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
		{
			return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
		}

		public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
		{
			return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
		}
	}
}
