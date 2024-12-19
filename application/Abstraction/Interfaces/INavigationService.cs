namespace application.Abstraction;

public interface INavigationService
{
	void ShowAuth();
	void ShowMain();
	void ShowAdmin();
	void ShowManagerRole();
	void ShowWorkerRole();
	void ShowAnalystRole();
}
