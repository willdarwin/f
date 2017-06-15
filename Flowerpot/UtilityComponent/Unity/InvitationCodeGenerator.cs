using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace UtilityComponent.Unity
{
    public class InvitationCodeGenerator
    {
        private const string Key = "flowerpot";

        public string GetMD5Hash(String input)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5");
        }


        public List<string> GetInvitationCodeByCount(int count)
        {
            var list = new List<string>();
            for (var i = 0; i < count; i++ )
            {
                var rand = new Random(Guid.NewGuid().GetHashCode());
                var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                list.Add(GetMD5Hash(rand.Next() + time + Key));
            }
            return list;
        }
    }
}
