using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Principal;
using System.Text;

namespace LDAP_Blob
{
    public class DirectorioActivo
    {
        public static string Autenticar(string dominio, string usuario, string pwd, string path)
        {
            string username = dominio + "\\" + usuario;
            DirectoryEntry searchRoot = new DirectoryEntry(path, username, pwd);
            string result;
            try
            {
                DirectorySearcher directorySearcher = new DirectorySearcher(searchRoot);
                directorySearcher.Filter = "(sAMAccountName=" + usuario + ")";
                if (directorySearcher.FindOne() == null)
                {
                    result = string.Empty;
                }
                else
                {
                    result = JsonConvert.SerializeObject(DirectorioActivo.DetallesPorUsuario(directorySearcher));
                }
            }
            catch (Exception ex)
            {
                string mensajeError = ex.Message;
                result = string.Empty;
            }
            return result;
        }

        public static string Autenticar2(string dominio, string usuario, string pwd, string path)
        {
            string username = dominio + "\\" + usuario;
            DirectoryEntry searchRoot = new DirectoryEntry(path, username, pwd);
            string result;
            try
            {
                DirectorySearcher filtroBusqueda = new DirectorySearcher(searchRoot);
                filtroBusqueda.Filter = "(sAMAccountName=" + usuario + ")";
                filtroBusqueda.FindOne();
                result = JsonConvert.SerializeObject(filtroBusqueda);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public static List<ADUser> DetallesPorUsuario(DirectorySearcher search)
        {
            List<ADUser> list = new List<ADUser>();
            List<string> list2 = new List<string>();
            foreach (SearchResult searchResult in search.FindAll())
            {
                foreach (object current in searchResult.Properties["memberof"])
                {
                    list2.Add(current.ToString());
                }
                ADUser item = new ADUser((byte[])searchResult.Properties["objectSid"][0], searchResult.Properties["name"][0].ToString(), searchResult.Properties["distinguishedName"][0].ToString(), searchResult.Properties["sAMAccountName"][0].ToString(), list2);
                list.Add(item);
            }
            return list;
        }

        public static SearchResult ExisteUsuarioAD(string dominio, string usuario)
        {
            return new DirectorySearcher(new DirectoryEntry(string.Format("LDAP://{0}", dominio)), string.Format("(SAMAccountName={0})", usuario)).FindOne();
        }

        public static List<string> GetGroups(string dominio, string user, string pwd, string path)
        {
            string username = dominio + "\\" + user;
            DirectoryEntry searchRoot = new DirectoryEntry(path, username, pwd);
            DirectorySearcher directorySearcher = new DirectorySearcher(searchRoot);
            List<string> listaGrupos = new List<string>();
            try
            {
                directorySearcher.Filter = "samAccountName=" + user.Trim();
                DirectoryEntry directoryEntry = directorySearcher.FindOne().GetDirectoryEntry();
                directoryEntry.RefreshCache(new string[] { "tokenGroups" });
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("(|");
                foreach (byte[] array in directoryEntry.Properties["tokenGroups"])
                {
                    stringBuilder.Append("(objectSid=");
                    for (int i = 0; i < array.Length; i++)
                    {
                        stringBuilder.AppendFormat("\\{0}", array[i].ToString("X2"));
                    }
                    stringBuilder.AppendFormat(")", Array.Empty<object>());
                }
                stringBuilder.Append(")");
                IEnumerator enumerator = new DirectorySearcher(searchRoot, stringBuilder.ToString()).FindAll().GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        string item = (string)((SearchResult)enumerator.Current).Properties["samAccountName"][0];
                        listaGrupos.Add(item);
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                listaGrupos.Add(ex.Message);
            }
            return listaGrupos;
        }

        private static string sIDtoString(byte[] sid)
        {
            return new SecurityIdentifier(sid, 0).ToString();
        }
    }
}