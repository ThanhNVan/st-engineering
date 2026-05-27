namespace Napoleon.Fos.Core.Settings;

public class JwtSettings
{
    #region [ Properties ]
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string Secret { get; set; }

    public int ValidationLengthInDays { get; set; }
    #endregion
}
