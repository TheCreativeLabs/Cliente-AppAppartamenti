using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Widget;
using AppAppartamenti.Droid.Files;
using AppAppartamenti.Files;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SaveFile))]
namespace AppAppartamenti.Droid.Files
{
    class SaveFile : AppAppartamenti.Files.ISaveFile
    {
        public async Task<string> SaveFiles(string filename, byte[] bytes)
        {
            //inizio Chiara
            //bool permission = false;
            //if (DeviceInfo.Platform.ToString() == Device.Android)
            //{
            //    permission = await DependencyService.Get<ICheckFilePermission>().CheckPermission();
            //}

            //if (!permission)
            //{
            //    return "";
            //}
            //fine Chiara




            //var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments); originale
            var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads); //var path = "/storage/emulated/0/Download/";
            var filePath = Path.Combine(path.ToString(), filename);
            bool error = false;

            //try
            //{
                //var filePath = Path.Combine(documentsPath, filename); originale
                File.WriteAllBytes(filePath, bytes);
                await OpenFile(filePath, filename); //NON FUNZIONA ANCORA QUINDI LO COMMENTO, MA BISOGNA FARLO ANDARE

            //}
            //catch
            //{
            //    error = true;
            //}
            if (error)
            {
                Toast.MakeText(Xamarin.Forms.Forms.Context, "Errore durante il download", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(Xamarin.Forms.Forms.Context, "Download effettuato", ToastLength.Short).Show();
            }
            //        Toast.MakeText(Xamarin.Forms.Forms.Context, "There's no app to open pdf files", ToastLength.Short).Show();

            
            return filePath;

        }

        //public async Task OpenFile(string filePath, string filename)
        //{

        //    var bytes = File.ReadAllBytes(filePath);

        //    //Copy the private file's data to the EXTERNAL PUBLIC location
        //    string externalStorageState = global::Android.OS.Environment.ExternalStorageState;
        //    string application = "";

        //    string extension = System.IO.Path.GetExtension(filePath);

        //    switch (extension.ToLower())
        //    {
        //        case ".doc":
        //        case ".docx":
        //            application = "application/msword";
        //            break;
        //        case ".pdf":
        //            application = "application/pdf";
        //            break;
        //        case ".xls":
        //        case ".xlsx":
        //            application = "application/vnd.ms-excel";
        //            break;
        //        case ".jpg":
        //        case ".jpeg":
        //        case ".png":
        //            application = "image/jpeg";
        //            break;
        //        default:
        //            application = "*/*";
        //            break;
        //    }
        //    //var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" + filename; //+ extension;
        //    var externalPath = filePath;


        //    //inizio Chiara
        //    bool permission = false;
        //    if (DeviceInfo.Platform.ToString() == Device.Android)
        //    {
        //        permission = await DependencyService.Get<ICheckFilePermission>().CheckPermission();
        //    }

        //    if (!permission)
        //    {
        //        return;
        //    }
        //    //fine Chiara


        //    File.WriteAllBytes(externalPath, bytes);

        //    Java.IO.File file = new Java.IO.File(externalPath);
        //    file.SetReadable(true);
        //    //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
        //    Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
        //    Intent intent = new Intent(Intent.ActionView);
        //    intent.SetDataAndType(uri, application);
        //    intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

        //    try
        //    {
        //        Xamarin.Forms.Forms.Context.StartActivity(intent);
        //    }
        //    catch (Exception)
        //    {
        //        Toast.MakeText(Xamarin.Forms.Forms.Context, "There's no app to open pdf files", ToastLength.Short).Show();
        //    }
        //}


        public async Task OpenFile(string filePath, string fileName)
        {
            var bytes = File.ReadAllBytes(filePath);

            //Copy the private file's data to the EXTERNAL PUBLIC location
            string externalStorageState = global::Android.OS.Environment.ExternalStorageState;
            //var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" + global::Android.OS.Environment.DirectoryDownloads + "/" + fileName; //IN QUESTO MODO SALVA DI NUOVO NELLA CARTELLA DOWNLOAD
            var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" + fileName;
            File.WriteAllBytes(externalPath, bytes);

            Java.IO.File file = new Java.IO.File(externalPath);
            file.SetReadable(true);

            string application = "";
            string extension = Path.GetExtension(filePath);

            // get mimeTye
            switch (extension.ToLower())
            {
                case ".txt":
                    application = "text/plain";
                    break;
                case ".doc":
                case ".docx":
                    application = "application/msword";
                    break;
                case ".pdf":
                    application = "application/pdf";
                    break;
                case ".xls":
                case ".xlsx":
                    application = "application/vnd.ms-excel";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    application = "image/jpeg";
                    break;
                default:
                    application = "*/*";
                    break;
            }


            //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
            //Android.Net.Uri uri = Android.Net.Uri.FromFile(file); //QUESTO SI ROMPE
            //Android.Net.Uri uri = FileProvider.GetUriForFile(global::Android.App.Application.Context, global::Android.App.Application.Context.PackageName + ".fileprovider", file);

            //test
            Java.IO.File file2 = new Java.IO.File(filePath);
            file2.SetReadable(true);
            Android.Net.Uri uri = FileProvider.GetUriForFile(global::Android.App.Application.Context, global::Android.App.Application.Context.PackageName + ".fileprovider", file2);
            //fine test

            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, application);
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            Forms.Context.StartActivity(intent);
        }
    }
}

