namespace Tgs.GameInterface.PragmaticPlay.Core.Exceptions;

public class GameInterfaceException(string error, string description, string json) : Exception($"{GetMessage(error, description)}({json})")
{
    public static string GetMessage(string error, string description)
        => error switch
        {
            "1" => "內部錯誤。請重試。",
            "100" => description,
            "2" => "安全登錄名和安全密碼組合錯誤。",
            "6" => "未找到遊戲或系統不允許遊戲。",
            "7" => "一個或多個輸入參數未設置或設置錯誤。",
            "8" => "轉帳成功(錯誤代碼0或8)，無須操作",
            "17" => "未找到玩家。",
            "21" => "貨幣代碼錯誤或不受支持。",
            _ => "未知錯誤，請聯繫遊戲平台。"
        };
}
