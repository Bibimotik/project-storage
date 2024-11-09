using application.MVVM.Model;

namespace application.Abstraction;

public interface IParserINNService
{
	public Task<ParserModel> GetParserDataAsync(string inputINN);
	public Task<(ParserModel?, string)> GetParserDataINN(string inputINN);
}