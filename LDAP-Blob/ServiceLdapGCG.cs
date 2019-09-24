using System.Configuration;
using System.Web.Services;

namespace LDAP_Blob
{
    public class ServiceLdapGCG
    {
        public string Dominio { set; get; }
        public string path { set; get; }

        public void ServiceLdapGcg()
        {
            Dominio = ConfigurationManager.AppSettings["Dominio"];
            path = ConfigurationManager.AppSettings["path"];
        }

        [WebMethod]
        public string ConexionLDAP(string usuario, string contraseña)
        {
            return DirectorioActivo.Autenticar(Dominio, usuario, contraseña, path);
        }


    }
}