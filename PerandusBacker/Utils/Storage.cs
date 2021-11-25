using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using PerandusBacker.Json;

namespace PerandusBacker.Utils
{
  internal class UserInfo
  {
    public int SessionSize;
    public int EntropySize;
    public byte[] SafePoeSessionId;
    public byte[] Entropy;
    public string League;
    public string Realm;
  }
  static class Storage
  {
    private static readonly string DocumentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/PerandusBacker/";
    private static byte[] CreateEntropy()
    {
      byte[] entropy = new byte[256];
      using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(entropy);
        return entropy;
      }
    }

    public static void SaveUserInfo(string PoeSessionId, LeagueInfo League)
    {
      byte[] PoeSessionIdBytes = Encoding.UTF8.GetBytes(PoeSessionId);
      byte[] Entropy = CreateEntropy();

      byte[] SafePoeSession = ProtectedData.Protect(PoeSessionIdBytes, Entropy, DataProtectionScope.CurrentUser);

      SaveUserInfo(new UserInfo()
      {
        SafePoeSessionId = SafePoeSession,
        SessionSize = SafePoeSession.Length,
        Entropy = Entropy,
        EntropySize = Entropy.Length,
        League = League.Id,
        Realm = League.Realm
      });
    }
    private static void SaveUserInfo(UserInfo info)
    {
      Directory.CreateDirectory(DocumentsFolder);
      using (BinaryWriter writer = new BinaryWriter(File.Open(DocumentsFolder + "data.dat", FileMode.Create)))
      {
        writer.Write(info.SessionSize);
        writer.Write(info.EntropySize);
        writer.Write(info.SafePoeSessionId);
        writer.Write(info.Entropy);
        writer.Write(info.League);
        writer.Write(info.Realm);
      }
    }

    public static bool LoadUserInfo()
    {
      try
      {
        using (BinaryReader reader = new BinaryReader(File.Open(DocumentsFolder + "data.dat", FileMode.Open)))
        {
          int sessionSize = reader.ReadInt32();
          int entropySize = reader.ReadInt32();
          byte[] safePoeSessionId = reader.ReadBytes(sessionSize);
          byte[] entropy = reader.ReadBytes(entropySize);
          string league = reader.ReadString();
          string realm = reader.ReadString();

          string PoeSessionId = Encoding.UTF8.GetString(ProtectedData.Unprotect(safePoeSessionId, entropy, DataProtectionScope.CurrentUser));
          Network.UpdatePoeSessionId(PoeSessionId);

          Data.League = new LeagueInfo() { Id = league, Realm = realm };
          return true;
        }
      }
      catch
      {
        return false;
      }
    }
  }
}
