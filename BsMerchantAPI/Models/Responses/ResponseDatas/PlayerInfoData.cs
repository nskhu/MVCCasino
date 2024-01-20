﻿namespace BsMerchantAPI.Models.Responses.ResponseDatas;

public class PlayerInfoData
{
    public string UserId { get; set; }
    public string? UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public decimal CurrentBalance { get; set; }
}