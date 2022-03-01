using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiRequests.FireBaseStorage
{
    public class FireBase
    {
        private const string ONLINE_REPO = @"astronomyproject-36250.appspot.com";

        readonly FirebaseStorage _storage;

        public FireBase()
        {

        }

        public FireBase(string repoConnction)
        {
            _storage = new FirebaseStorage(repoConnction);  
        }

        /// <summary>
        /// Insert file to fire base
        /// </summary>
        /// <param name="path"> path in local machine</param>
        /// <param name="fileName">The name given to the file in Firebase</param>
        /// <returns>download Url link</returns>
        public async Task<string> Insert(string path, string fileName)
        {
            try
            {
                var stream = File.Open(path, FileMode.Open);
                
                // Construct FirebaseStorage with path to where you want to upload the 
                //file and put it there
                var storege = new FirebaseStorage(ONLINE_REPO);
                
                var downloadUrl = await storege
                    .Child(fileName)
                    .PutAsync(stream);
                
                return downloadUrl;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// delete file form fire base storage
        /// </summary>
        /// <param name="nameInFirebase"></param>
        /// <returns></returns>
        public async Task Delete(string nameInFirebase)
        {
            await new FirebaseStorage(ONLINE_REPO).
                Child(nameInFirebase).
                DeleteAsync();

        }

        //return URL for view the image or file
        public async Task<string> Get(string nameToBeKeptInFirebase)
        {
            try
            {
                return await new FirebaseStorage(ONLINE_REPO).
                       Child(nameToBeKeptInFirebase).
                       GetDownloadUrlAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
