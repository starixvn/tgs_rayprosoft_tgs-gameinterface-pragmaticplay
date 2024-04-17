namespace Tgs.GameInterface.PragmaticPlay.Core;

public interface ICryptographyUtensil
{
	public string GenerateHash(IDictionary<string, string> parameters, string secretKey);
}
