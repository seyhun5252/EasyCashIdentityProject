using EasyCashIdentityProject.DtoLayer.Dtos.AppUserDtos;
using EasyCashIdentityProject.EntityLayer.Concrete;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit;

namespace EasyCashIdentityProject.PresentationLayer.Controllers
{
	public class RegisterController : Controller
	{
		private readonly UserManager<AppUser> _userManager;

		public RegisterController(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(AppUserRegisterDto appUserRegisterDto)
		{
			if (ModelState.IsValid)
			{
				Random random = new Random();
				int code = random.Next(100000, 1000000);

				AppUser appUser = new AppUser()
				{
					UserName = appUserRegisterDto.Username,
					Name = appUserRegisterDto.Name,
					Surname = appUserRegisterDto.SurName,
					Email = appUserRegisterDto.Email,
					City = "aaaa",
					District = "bbb",
					ImageUrl = "ccc",
					ConfirmCode = code,
				};

				var result = await _userManager.CreateAsync(appUser, appUserRegisterDto.Password);

				if (result.Succeeded)
				{
					MimeMessage mimeMessage = new MimeMessage();
					MailboxAddress mailboxAddressFrom = new MailboxAddress("Easy Cash Admin", "seyhun5252celebioglu@hotmail.com");
					MailboxAddress mailboxAddressTo = new MailboxAddress("User",appUser.Email);

					mimeMessage.From.Add(mailboxAddressFrom);
					mimeMessage.To.Add(mailboxAddressTo);

					var bodyBuilder = new BodyBuilder();
					bodyBuilder.TextBody = "Kayıt Gerçekleştirmek için onay Kodunuz: " + code;
					mimeMessage.Body = bodyBuilder.ToMessageBody();
					mimeMessage.Subject = "Easy Cash Onay Kodu";

					SmtpClient smtpClient = new SmtpClient();
					smtpClient.Connect("smtp.office365.com", 587, false);
					smtpClient.Authenticate("seyhun5252celebioglu@hotmail.com", "571632Reis.");
					smtpClient.Send(mimeMessage);
					smtpClient.Disconnect(true);

					TempData["Mail"] = appUserRegisterDto.Email;

					return RedirectToAction("Index", "ConfirmMail");
				}
				else
				{
					foreach (var item in result.Errors)
					{
						ModelState.AddModelError("", item.Description);
					}
				}
			}

			return View();
		}
	}
}
