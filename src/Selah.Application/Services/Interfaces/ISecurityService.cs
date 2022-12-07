using System.Threading.Tasks;

namespace Selah.Application.Services.Interfaces
{
  public interface ISecurityService
  {
    /// <summary>
    ///     Decrypt the given string using AWS KMS
    /// </summary>
    /// <param name="input">A Base 64 Encoded string to decrypt</param>
    /// <returns>A plain text decrypted string.</returns>
    Task<string> Decrypt(string input);

    /// <summary>
    ///     Encrypt the given string using AWS KMS
    /// </summary>
    /// <param name="input">A Base 64 Encoded string to encrypt</param>
    /// <returns>A Base 64 Encoded encrypted string.</returns>
    Task<string> Encrypt(string input);

  }
}