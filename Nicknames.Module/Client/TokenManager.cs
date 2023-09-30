using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicknames.Module.Client
{
    public class TokenManager
    {
        private static TokenManager _instance;

        public static TokenManager Get()
        {
            if (_instance == null)
            {
                _instance = new TokenManager();
            }
            return _instance;
        }

        public void StoreToken(string token)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\BannerlordNicknames");
            key.SetValue("AuthToken", token);
            key.Close();
        }

        public bool HasStoredToken()
        {
            RegistryKey readKey = Registry.CurrentUser.OpenSubKey("Software\\BannerlordNicknames");
            string storedToken = readKey.GetValue("AuthToken") as string;
            readKey.Close();

            if (string.IsNullOrEmpty(storedToken))
                return false;
            return true;
        }

        public string GetToken()
        {
            RegistryKey readKey = Registry.CurrentUser.OpenSubKey("Software\\BannerlordNicknames");
            string storedToken = readKey.GetValue("AuthToken") as string;
            readKey.Close();

            return storedToken;
        }
    }
}
