using System;
using System.Collections.Generic;

namespace LDAP_Blob
{
    public class ADUser
    {
        private byte[] Sid { set; get; }
        private string Name { set; get; }
        private string DistinguishedName { set; get; }
        private string SAMAccountName { set; get; }
        private List<string> Roles { set; get; }


        public ADUser(byte[] sid, string name, string distinguishedName, string sAMAccountName)
        {
            this.Sid = sid;
            this.Name = name;
            this.DistinguishedName = distinguishedName;
            this.SAMAccountName = sAMAccountName;
        }

        public ADUser(byte[] sid, string name, string distinguishedName, string sAMAccountName, List<string> roles)
        {
            this.Sid = sid;
            this.Name = name;
            this.DistinguishedName = distinguishedName;
            this.SAMAccountName = sAMAccountName;
            this.Roles = roles;
        }
    }
}