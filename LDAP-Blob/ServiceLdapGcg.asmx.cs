using System.Configuration;
using System.Web.Services;

namespace LDAP_Blob
{
    /// <summary>
    /// Descripción breve de ServiceLdapGcg
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class ServiceLdapGcg : System.Web.Services.WebService
    {
        public string Dominio { set; get; }
        public string path { set; get; }

        public ServiceLdapGcg()
        {
            Dominio = ConfigurationManager.AppSettings["Dominio"];
            path = ConfigurationManager.AppSettings["path"];
        }

        [WebMethod]
        public string ServicioLdap(string usuario, string contraseña)
        {
            return DirectorioActivo.Autenticar(Dominio, usuario, contraseña, path);
        }
    }
}
