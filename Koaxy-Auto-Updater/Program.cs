using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Koaxy_Auto_Updater
{



    

    internal class Program
    {
       static public string MiscFolder = Directory.GetCurrentDirectory() + "//Misc//";
       static public string YourThingToAutoUpdate = Directory.GetCurrentDirectory() + "//YourThingToAutoUpdate";
       static public WebClient wc = new WebClient();
        static public async Task DownloadLatestVers()
        {

            string DownloadDataV2 = await wc.DownloadStringTaskAsync("https:://yoursite.yaya");
            try
            {
                await wc.DownloadFileTaskAsync("https:://yoursite.yaya" + DownloadDataV2 + ".rar", YourThingToAutoUpdate + "\\Yes.zip");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to download KoaxyMenu, Please try again later. {0}", ex.ToString());
            }
        }
        async void Magic()
        {
            WebClient wc = new WebClient();
            string MunchKin = wc.DownloadString("https:://yoursite.yaya");

            if (!File.Exists(YourThingToAutoUpdate + "\\YourThingToAutoUpdate.rar"))
            {
                await DownloadLatestVers();
            }
            else
            {
                // Do nothing
            }
            using (var zipFile = RarArchive.Open(YourThingToAutoUpdate + "\\YourThingToAutoUpdate.rar"))
            {
                foreach (var entry in zipFile.Entries.Where(entry => !entry.IsDirectory))
                {

                    if (!Directory.Exists(YourThingToAutoUpdate + "\\YourThingToAutoUpdate " + MunchKin))
                    {
                        Directory.CreateDirectory(YourThingToAutoUpdate + "\\YourThingToAutoUpdate " + MunchKin);
                    }
                    entry.WriteToDirectory(YourThingToAutoUpdate + "\\YourThingToAutoUpdate " + MunchKin, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
            File.Delete(YourThingToAutoUpdate + "\\KoaxyMenu.rar");
        }

        static void SetupDirectorys()
        {
            if (!Directory.Exists(MiscFolder))
            {
                Directory.CreateDirectory(MiscFolder);

            }
            else
            {
                // Do Nothing
            }

            if (!Directory.Exists(YourThingToAutoUpdate))
            {
                Directory.CreateDirectory(YourThingToAutoUpdate);
            }
            else
            {
                // Do Nothing
            }

            if (!File.Exists(MiscFolder + "//KoaxyMenuVers.txt"))
            {
                File.WriteAllText(MiscFolder + "//KoaxyMenuVers.txt", "");
            }
            else
            {

            }
        }

        static public async Task StartUp()
        {
            bool IsValid = false;
            SetupDirectorys();

            string DownloadData = await wc.DownloadStringTaskAsync("URLTOWEBSERVERVERSION");


            SetupDirectorys();

            string OldData = File.ReadAllText(MiscFolder + "//KoaxyMenuVers.txt");

            if (OldData == DownloadData)
            {
                IsValid = true;
                //MessageBox.Show("Koaxy Menu is upto date");
            }
            else if (OldData != DownloadData)
            {
                IsValid = false;
                Console.WriteLine("Koaxy menu is outdated.. updating");
                await DownloadLatestVers();
               
                Console.WriteLine("The update was Successful!");
            }
            if (!IsValid)
            {
                Console.WriteLine("Downloading New update..");
                File.WriteAllText(MiscFolder + "//KoaxyMenuVers.txt", DownloadData);
            }
        }

        static async Task Main(string[] args)
        {
            await StartUp();
        }
    }
}
