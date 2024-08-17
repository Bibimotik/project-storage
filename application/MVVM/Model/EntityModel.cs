﻿using System.Text.RegularExpressions;

using application.Utilities;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.Model;

public class EntityModel
{
	public Guid Id { get; set; }
	[RequiredForValidation]
	public string Email { get; set; } = string.Empty;
	[RequiredForValidation]
	public string Password { get; set; } = string.Empty;
	[RequiredForValidation]
	public string ConfirmPassword { get; set; } = string.Empty;

	public string? FirstName { get; set; } = null;
	public string? SecondName { get; set; } = null;
	public string? ThirdName { get; set; } = null;
	[RequiredForValidation]
	public string? Phone { get; set; } = null;

	[RequiredForValidation]
	public string INN { get; set; } = string.Empty;
	[RequiredForValidation]
	public string KPP { get; set; } = string.Empty;

	[RequiredForValidation]
	public string FullName { get; set; } = string.Empty;
	[RequiredForValidation]
	public string ShortName { get; set; } = string.Empty;

	[RequiredForValidation]
	public string LegalAddress { get; set; } = string.Empty;
	[RequiredForValidation]
	public string PostalAddress { get; set; } = string.Empty;

	[RequiredForValidation]
	public string OGRN { get; set; } = string.Empty;
	[RequiredForValidation]
	public string Director { get; set; } = string.Empty;

	public string Code { get; set; } = string.Empty;
	public string InputCode { get; set; } = string.Empty;

	public EntityType EntityType { get; set; }
	public static EntityModel Model { get; set; } = new();


	public static bool IsValidEmail(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
			return false;
		try
		{
			var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			return regex.IsMatch(email);
		}
		catch (RegexMatchTimeoutException)
		{
			return false;
		}
	}

	public static bool IsFilledFirstName(string firstname)
	{
		return !string.IsNullOrWhiteSpace(firstname);
	}

	public static bool IsFilledSecondName(string secondname)
	{
		return !string.IsNullOrWhiteSpace(secondname);
	}

	public static bool IsFilledThirdName(string thirdname)
	{
		return !string.IsNullOrWhiteSpace(thirdname);
	}

	public static bool IsFilledPhone(string phone)
	{
		return !string.IsNullOrWhiteSpace(phone);
	}

	public static bool IsFilledPassword(string password)
	{
		if (string.IsNullOrWhiteSpace(password))
			return false;
		try
		{
			var regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$", RegexOptions.Compiled);
			return regex.IsMatch(password);
		}
		catch (RegexMatchTimeoutException)
		{
			return false;
		}
	}

	public static bool IsFilledConfirmPassword(string confirmpassword)
	{
		if (string.IsNullOrWhiteSpace(confirmpassword))
			return false;
		try
		{
			var regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$", RegexOptions.Compiled);
			return regex.IsMatch(confirmpassword);
		}
		catch (RegexMatchTimeoutException)
		{
			return false;
		}
	}

	public static bool ComparePasswords(string password, string confirmPassword) => password == confirmPassword;
}
