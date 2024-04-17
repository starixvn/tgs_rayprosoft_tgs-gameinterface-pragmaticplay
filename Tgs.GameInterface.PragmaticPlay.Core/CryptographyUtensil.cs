using System.Security.Cryptography;
using System.Text;

namespace Tgs.GameInterface.PragmaticPlay.Core;

internal class CryptographyUtensil : ICryptographyUtensil
{
	public string GenerateHash(IDictionary<string, string> parameters, string secretKey)
	{
		var sortedParameters = new SortedDictionary<string, string>(parameters);
		var values = sortedParameters.Select(p => $"{p.Key}={p.Value}");

		var original = $"{string.Join("&", values)}{secretKey}";

		var toEncryptArray = Encoding.UTF8.GetBytes(original);

		var hash = MD5.HashData(toEncryptArray);

		var sb = new StringBuilder();

		foreach (var t in hash)
		{
			_ = sb.Append(t.ToString("x2"));
		}

		return sb.ToString();
	}
}
