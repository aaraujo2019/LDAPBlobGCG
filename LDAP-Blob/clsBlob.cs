using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;

namespace LDAP_Blob
{
    public class clsBlob
    {
        private CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount result;
            try
            {
                result = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
            return result;
        }

        public byte[] Descargar(string rutaAlmacenamientoArchivo, string blobName, string CadenaConexion, string Contenedor)
        {
            CloudBlobContainer containerReference = this.CreateStorageAccountFromConnectionString(CadenaConexion).CreateCloudBlobClient().GetContainerReference(Contenedor);
            MemoryStream memoryStream = new MemoryStream();
            byte[] result;
            try
            {
                containerReference.GetBlockBlobReference(blobName).DownloadToStream(memoryStream, null, null, null);
                result = memoryStream.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
            return result;
        }

        public void Guardar(string CadenaConexion, string Contenedor, string blobName, byte[] arrayFile)
        {
            CloudBlockBlob blockBlobReference = this.CreateStorageAccountFromConnectionString(CadenaConexion).CreateCloudBlobClient().GetContainerReference(Contenedor).GetBlockBlobReference(blobName);
            try
            {
                blockBlobReference.UploadFromByteArray(arrayFile, 0, arrayFile.Length, null, null, null);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }
    }
}