namespace Tgs.GameInterface.PragmaticPlay.Core.UnitTest;

public class CryptographyUtensilUnitTest
{
	[Fact]
	public void Test1()
	{
		var utensil = new CryptographyUtensil();

		var parameters = new Dictionary<string, string>
		{
			["secureLogin"] = "tgs_tgstfw",
			["externalPlayerId"] = "4ucmqplxb029"
		};

		var actual = utensil.GenerateHash(parameters, "testKey");

		Assert.NotNull(actual);
		Assert.Equal("9e4cc4026644bb30e91ccf2b577b2691", actual);
	}
}
