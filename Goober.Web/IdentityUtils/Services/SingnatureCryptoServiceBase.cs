namespace Goober.Web.IdentityUtils.Services
{
    internal abstract class SingnatureCryptoServiceBase
    {
        internal abstract string EncryptSignature(string plainSignature);
        internal abstract string DecryptSignature(string cipherSignature);
    }
}
