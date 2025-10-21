using CommunityToolkit.Maui.Storage;
using MBS_Gatewaykonfigurator.Models;
using MBS_Gatewaykonfigurator.Models.BACnet;
using MBS_Gatewaykonfigurator.Models.MBS;
using MBS_Gatewaykonfigurator.Services;
using MudBlazor;
using System.Text;



public static class Export
{

    public static async Task Generieren(Gateway gw, GerätevorlageService gerätevorlageService, MudBlazor.ISnackbar sBar, IFolderPicker folderPicker)
    {
        uint adressBacNet = 0;
        uint dispatchCount = 0;
        uint tagIndex = 0;

        StringBuilder sbBacNet = new StringBuilder();

        if (gw == null || gerätevorlageService == null)
        {
            return;
        }

        // Gerätevorlagen laden
        var allGerätevorlagen = await gerätevorlageService.LoadAsync();

        var gerätevorlagen = allGerätevorlagen
            .Where(g => g != null && g.ProjektId == gw.ProjektId && g.GerätevorlageElements.Count > 0)
            .OrderBy(g => g.Name)
            .ToList();


        var filesDict = new Dictionary<string, StringBuilder>();

        void AppendToFile(Dictionary<string, StringBuilder> dict, string fileName, string content)
        {
            if (!dict.ContainsKey(fileName))
            {
                dict[fileName] = new StringBuilder();
            }
            dict[fileName].Append(content);
        }


        // Globale Datenpunkte
        foreach (var dp in gw.DatenpunkteGlobal)
        {


            //Quelle SystemMbs keine Datei Ausgabe notwendig
            if (dp.Quelle is SystemMbs systemMbs)
            {
                systemMbs.Name = dp.Beschreibung!;
            }

            //Dispatch
            if (dp.Quelle != null && dp.Ziel != null)
            {
                //create String
                StringBuilder sb = new();
                sb.AppendLine("# " + dp.Beschreibung);
                sb.AppendLine("[" + ((IDispatchable)dp.Quelle).toStringDispatch() + "]");
                sb.AppendLine("target = " + ((IDispatchable)dp.Ziel).toStringDispatch());
                sb.AppendLine(dp.Dispatch.ToString());

                //append string to file 
                AppendToFile(filesDict, "dispatch.txt", sb.ToString());
                dispatchCount++;
            }

            //Ziel bacnet{}
            if (dp.Ziel is BacNet bacnet)
            {
                //copy to bacnet subclass
                bacnet.Name = dp.Name;
                bacnet.BacDescription = dp.Beschreibung!;
                bacnet.Tag = dp.Tag;

                generiereBACnetDP(bacnet);
            }
        }



        void generiereBACnetDP(BacNet bacnet)
        {
            if (bacnet.TypBacNet != BacNet.Types.NC)
            {
                bacnet.ObjektNummer = adressBacNet;
                adressBacNet++;
            }

            //bacnet
            AppendToFile(filesDict, bacnet.DatenPunktDatei, bacnet.toStringFile());
            AppendToFile(filesDict, bacnet.DatenPunktDatei, "\r\n");

            //plants.cfg
            generierePlants(bacnet);
        }

        void generierePlants(BacNet bacnet)
        {
            //plants.cfg
            if (!string.IsNullOrWhiteSpace(bacnet.Tag))
            {
                StringBuilder sb = new();
                sb.AppendLine("[" + bacnet.Tag + "]");
                sb.AppendLine("id = " + tagIndex);
                sb.AppendLine("name = " + bacnet.BacDescription);
                sb.AppendLine();
                AppendToFile(filesDict, "plants.cfg", sb.ToString());
                tagIndex++;
            }
        }

        // Lokale Datenpunkte
        foreach (var dp in gw.Datenpunkte)
        {
            Gerätevorlage? gerätevorlage = gerätevorlagen.FirstOrDefault((g) => g.Name == dp.Gerätevorlage && gw.ProjektId == g.ProjektId);
            if (gerätevorlage == null || gerätevorlage.GerätevorlageElements.Count == 0)
            {
                //mudblazor meldung anzeigen
                string msg = $"Datenpunkt '{dp.Name}' hat ungültige Gerätevorlage.";
                sBar.Add(msg, Severity.Error);
                continue;
            }

            if (dp.Quelle != null)
            {
                try
                {
                    //setzte Attribute zu Unterklassen
                    gerätevorlage.setNameDescriptionTagAdress(dp.Name, dp.Beschreibung, dp.Tag, dp.Quelle);
                    adressBacNet = gerätevorlage.getLastAdressSetFirstAdressBacNet(adressBacNet);

                    //Quelle
                    if (gerätevorlage.GerätevorlageElements[0].Quelle is Mbs mbs)
                    {
                        AppendToFile(filesDict, mbs.DatenPunktDatei, gerätevorlage.QuellProtokollToStringFile());
                        // AppendToFile(filesDict, mbs.DatenPunktDatei, "\r\n");

                    }

                    //dispatch
                    AppendToFile(filesDict, "dispatch.txt", gerätevorlage.toStringDispatch());
                    // AppendToFile(filesDict, "dispatch.txt", "\r\n");
                    dispatchCount += (uint)gerätevorlage.GerätevorlageElements.Count;

                    //Ziel
                    if (gerätevorlage.GerätevorlageElements[0].Ziel is Mbs mbs2)
                    {
                        AppendToFile(filesDict, mbs2.DatenPunktDatei, gerätevorlage.ZielProtokollToStringFile());
                        //  AppendToFile(filesDict, mbs2.DatenPunktDatei, "\r\n");

                        //plant
                        if (gerätevorlage.GerätevorlageElements[0].Ziel is BacNet bacnet && !string.IsNullOrWhiteSpace(bacnet.Tag))
                        {
                            AppendToFile(filesDict, "plants.cfg", gerätevorlage.toStringPlant(tagIndex, out tagIndex));
                        }
                    }


                }
                catch (Exception e)
                {
                    sBar.Add(e.Message, Severity.Error);
                    return;
                }
            }
        }


        // Ask the user to pick a folder
        //Quelle:https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/essentials/folder-picker?tabs=windows

        var result = await folderPicker.PickAsync(default);

        if (!result.IsSuccessful || result.Folder is null)
        {
            return;
        }

        string folderPath = result.Folder.Path;

        try
        {
            for (int i = 0; i < filesDict.Count; i++)
            {
                // Save files
                await File.WriteAllTextAsync(Path.Combine(folderPath, filesDict.ElementAt(i).Key), filesDict.ElementAt(i).Value.ToString());
            }
        }
        catch (Exception e)
        {
            sBar.Add(e.Message, Severity.Error);
            return;
        }

        gw.AnzahlDispatch = (int)dispatchCount;

    }
}


