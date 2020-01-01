using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIT.Models
{
	public class ExternalLoginConfirmationViewModel
	{
		[Required]
		[Display(Name = "Адрес электронной почты")]
		public string Email { get; set; }
	}

	public class ExternalLoginListViewModel
	{
		public string ReturnUrl { get; set; }
	}

	public class SendCodeViewModel
	{
		public string SelectedProvider { get; set; }
		public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
		public string ReturnUrl { get; set; }
		public bool RememberMe { get; set; }
	}

	public class VerifyCodeViewModel
	{
		[Required]
		public string Provider { get; set; }

		[Required]
		[Display(Name = "Код")]
		public string Code { get; set; }
		public string ReturnUrl { get; set; }

		[Display(Name = "Запомнить браузер?")]
		public bool RememberBrowser { get; set; }

		public bool RememberMe { get; set; }
	}

	public class ForgotViewModel
	{
		[Required]
		[Display(Name = "Адрес электронной почты")]
		public string Email { get; set; }
	}

	public class LoginViewModel
	{
		[Required]
		[Display(Name = "Адрес электронной почты")]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Display(Name = "Запомнить меня")]
		public bool RememberMe { get; set; }
	}

	public class RegisterViewModel
	{
		[Required]
		[StringLength(30, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 3)]
		[Display(Name = "Имя")]
		public string Name { get; set; }

		[Required]
		[StringLength(30, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 3)]
		[Display(Name = "Фамилия")]
		public string Surname { get; set; }

		[Required]
		[StringLength(30, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 3)]
		[Display(Name = "Отчество")]
		public string Patronic { get; set; }

		[Required]
		[RegularExpression(@"^(ЧМ-)[0-9]+", ErrorMessage = "Значение таб. номера должно начинаться с ЧМ-")]
		[StringLength(10, ErrorMessage = "Значение {0} должно содержать {2} символов.", MinimumLength = 10)]
		[Display(Name = "Таб. №")]
		public string TabNo { get; set; }

		[Required]
		[RegularExpression(@"^([8])[0-9]+", ErrorMessage = "Неправильно заполнено поле Сот")]
		[StringLength(11, ErrorMessage = "Значение {0} должно содержать {2} символов.", MinimumLength = 11)]
		[Display(Name = "Сот.")]
		public string PhoneNumber { get; set; }

		[Required]
		[EmailAddress]
		//[RegularExpression(@"[A-Za-z0-9._%+-]+@(mechel)\.(com)", ErrorMessage = "Неправильно задан Email")]
		[Display(Name = "Адрес электронной почты")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Подтверждение пароля")]
		[Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
		public string ConfirmPassword { get; set; }
	}

	public class ResetPasswordViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Адрес электронной почты")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Подтверждение пароля")]
		[Compare("Password", ErrorMessage = "Пароль и его подтверждение не совпадают.")]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }
	}

	public class ForgotPasswordViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Почта")]
		public string Email { get; set; }
	}
}
