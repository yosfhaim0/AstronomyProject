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
        const string ONLINE_REPO = @"astronomyproject-36250.appspot.com";
        public FireBase()
        {

        }
        //insert file to fire base
        public async Task<string> Insert(String Path, String NameToBeKeptInFirebase)
        {
            try
            {
                var stream = File.Open(Path, FileMode.Open);
                // Construct FirebaseStorage with path to where you want to upload the 
                //file and put it there
                var task = new FirebaseStorage(ONLINE_REPO).
                    Child(NameToBeKeptInFirebase)
                            .PutAsync(stream);
                var downloadUrl = await task;
                return downloadUrl;
            }
            catch (Exception)
            {
                throw;
            }

        }
        //delete file form fire base storage
        public async Task Delete(String NameInFirebase)
        {
            await new FirebaseStorage(ONLINE_REPO).
                Child(NameInFirebase).
                DeleteAsync();

        }
        //return URL for view the image or file
        public async Task<String> Get(String NameToBeKeptInFirebase)
        {
            return await new FirebaseStorage(ONLINE_REPO).
                Child(NameToBeKeptInFirebase).
                GetDownloadUrlAsync();
        }


    }
}
