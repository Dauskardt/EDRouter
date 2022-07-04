using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace EDRouter.ViewModel
{
    class ViewModelMain : ViewModelBase
    {
        #region Declarations..

        public static string Titel { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " Version " +
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version +
                    " [SH4DOWM4K3R " + DateTime.Now.Year + "]"; } }

        public string Arbeitsverzeichnis { get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\EDRouter"; } }

        public string PathToExe { get { return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\EDRouter.exe"; } }

        private static string JournalDirectory { get { return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToString() + "\\Saved Games\\Frontier Developments\\Elite Dangerous"; } }


        private static string DownloadFolder { get { return Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads"; } }

        private Model.Json.JsonWatcher EventWatcher { get; set; }

        private FileSystemWatcher watcher { get; set; }

        private string PathToImportedCSV { get; set; }


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Model.Settings _ProgramSettings = new Model.Settings();
        public Model.Settings ProgramSettings 
        {
            get { return _ProgramSettings; }
            set { _ProgramSettings = value; RPCEvent(nameof(ProgramSettings)); }
        
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ObservableCollection<FileInfo> _RoutenDateien = new ObservableCollection<FileInfo>();
        public ObservableCollection<FileInfo> RoutenDateien
        {
            get { return _RoutenDateien; }
            set { _RoutenDateien = value; RPCEvent(nameof(RoutenDateien)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _AktuelleRoutePath;
        public string AktuelleRoutePath
        {
            get { return _AktuelleRoutePath; }
            set { _AktuelleRoutePath = value; RPCEvent(nameof(AktuelleRoutePath)); if(!string.IsNullOrEmpty(value)){ AktuelleRouteName = Path.GetFileName(value); } else { AktuelleRouteName = string.Empty; }; }
        }

        private string _AktuelleRouteName;
        public string AktuelleRouteName
        {
            get { return _AktuelleRouteName; }
            set { _AktuelleRouteName = value; RPCEvent(nameof(AktuelleRouteName)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _SystemAktuell;
        public string SystemAktuell
        {
            get { return _SystemAktuell; }
            set { _SystemAktuell = value; RPCEvent(nameof(SystemAktuell)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _SystemZiel;
        public string SystemZiel
        {
            get { return _SystemZiel; }
            set { _SystemZiel = value; RPCEvent(nameof(SystemZiel)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _SystemNächstes;
        public string SystemNächstes
        {
            get { return _SystemNächstes; }
            set { _SystemNächstes = value; RPCEvent(nameof(SystemNächstes)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _FuelMain = 0.0;
        public double FuelMain
        {
            get { return _FuelMain; }
            set { _FuelMain = value; RPCEvent(nameof(FuelMain)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _FuelReservoir = 0.0;
        public double FuelReservoir
        {
            get { return _FuelReservoir; }
            set { _FuelReservoir = value; RPCEvent(nameof(FuelReservoir)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _Jumps;
        public int Jumps
        {
            get { return _Jumps; }
            set { _Jumps = value; RPCEvent(nameof(Jumps)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _Restsprünge;
        public int Restsprünge
        {
            get { return _Restsprünge; }
            set { _Restsprünge = value; RPCEvent(nameof(Restsprünge)); }

        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _Schiff;
        public string Schiff
        {
            get { return _Schiff; }
            set { _Schiff = value; RPCEvent(nameof(Schiff)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _Tank = 0.0;
        public double Tank
        {
            get { return _Tank; }
            set { _Tank = value; RPCEvent(nameof(Tank)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _LastAPIMessage;
        public string LastAPIMessage
        {
            get { return _LastAPIMessage; }
            set { _LastAPIMessage = value; RPCEvent(nameof(LastAPIMessage)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Model.Events.Part> _Route;
        public List<Model.Events.Part> Route
        {
            get { return _Route; }
            set { _Route = value; RPCEvent(nameof(Route)); }

        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Model.Events.LoadGameEvent _LastLoadGameEvent;
        public Model.Events.LoadGameEvent LastLoadGameEvent
        {
            get { return _LastLoadGameEvent; }
            set { _LastLoadGameEvent = value; RPCEvent(nameof(LastLoadGameEvent)); }
        
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Model.Events.LoadoutEvent _LastLoadoutEvent;
        public Model.Events.LoadoutEvent LastLoadoutEvent
        {
            get { return _LastLoadoutEvent; }
            set { _LastLoadoutEvent = value; RPCEvent(nameof(LastLoadoutEvent)); }

        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Model.Route.ReiseRoute _RouteAktuell = new Model.Route.ReiseRoute();
        public Model.Route.ReiseRoute RouteAktuell
        {
            get { return _RouteAktuell; }
            set { _RouteAktuell = value; RPCEvent(nameof(RouteAktuell)); }
        
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FileInfo _SelectedRoutenFile;
        public FileInfo SelectedRoutenFile
        {
            get { return _SelectedRoutenFile; }
            set { _SelectedRoutenFile = value; RPCEvent(nameof(SelectedRoutenFile)); }

        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Model.Route.Etappe _SelectedEtappe;
        public Model.Route.Etappe SelectedEtappe
        {
            get { return _SelectedEtappe;  }
            set { _SelectedEtappe = value; RPCEvent(nameof(SelectedEtappe)); }
        }

        #endregion

        #region Action-Commands..

        public ActionCommand ACLoadRouteFile
        {
            get;
            set;
        }

        public void ACLoadRouteFileFunc(object parameter)
        {
            try
            {
                if (parameter != null)
                {
                    LoadRouteFile(parameter.ToString());

                    FileBrowserExpanded = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("IO-Error!" + Environment.NewLine + ex.Message.ToString(), "Fehler beim öffnen der Datei!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ActionCommand ACDeleteRoutenFile
        {
            get;
            set;
        }

        public void ACDeleteRoutenFileFunc(object parameter)
        {
            if (parameter != null)
            {
                if (MessageBox.Show("Routendatei löschen?", "Benutzerabfrage", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
                {
                    System.IO.File.Delete(parameter.ToString());
                    GetRoutenDateien();
                }
            }
        }

        public ActionCommand ACHelp
        {
            get;
            set;
        }

        public void ACHelpFunc(object parameter)
        {
            string HelpText = "Backup:\r\nClick on the 'Backup Files' button to pack the current bindings into a zip archive." +
                "\r\nThe archive is saved in the backup folder.\r\n\r\nRestore:" +
                
                "\r\nTo restore the bindings, click the 'Restore selected File' button." +
                "\r\nAll files in the bindings folder are deleted and the archive content is then copied from the backup to the bindings folder." +
                "\r\n\r\nSettings: " +
                "\r\nSettings are made and saved in the settings dialog (autobackup, times, path etc.)." +
                "\r\n\r\nHint:\r\nA double click on a file opens it in the corresponding standard tool";
            MessageBox.Show(HelpText,"Help", MessageBoxButton.OK,MessageBoxImage.Information);
        }

        public ActionCommand ACRouteFolder
        {
            get;
            set;
        }

        public void ACRouteFolderFunc(object parameter)
        {
            string RouteDir = Path.Combine(Arbeitsverzeichnis, "Routen");

            if (Directory.Exists(RouteDir))
            {
                Process.Start("Explorer.exe", RouteDir);
            }
        }


        //public ActionCommand ACUserBackupFolder
        //{
        //    get;
        //    set;
        //}

        //public void ACUserBackupFolderFunc(object parameter)
        //{
        //    var dialog = new System.Windows.Forms.FolderBrowserDialog();

        //    dialog.Description = "Select Backup-Folder";

        //    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        BindingsManager.BackupFolder = dialog.SelectedPath;
        //        UserSettings.BackupFolder = dialog.SelectedPath;
        //        SaveSettings();
        //    }
        //}

        public ActionCommand ACSetSettings
        {
            get;
            set;
        }

        public void ACSetSettingsFunc(object parameter)
        {
            View.Dialog.DialogSettings DlgSettings = new View.Dialog.DialogSettings();
            DlgSettings.UserSettings = ProgramSettings;
            DlgSettings.Owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            if ((bool)DlgSettings.ShowDialog())
            {
                SaveSettings();
            }
        }

        public ActionCommand ACRoutePlanen
        {
            get;
            set;
        }

        public void ACRoutePlanenFunc(object parameter)
        {

            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.spansh.co.uk/plotter",
                UseShellExecute = true
            });

            InitWatcher();

        }

        public ActionCommand ACRouteReset
        {
            get;
            set;
        }

        public void ACRouteResetFunc(object parameter)
        {
            if (RouteAktuell != null)
            {
                if (MessageBox.Show("Route zurücksetzen?", "Nutzerabfrage", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    RouteAktuell.Reset();
                    SystemAktuell = RouteAktuell[0].System;
                    if (RouteAktuell.SetEtappeBesucht(SystemAktuell))
                    {
                        SystemZiel = RouteAktuell[1].System;
                        Jumps = RouteAktuell[1].Sprünge;
                    }
                }
            }

        }

        public ActionCommand ACCopy
        {
            get;
            set;
        }

        public void ACCopyFunc(object parameter)
        {
            if (RouteAktuell != null)
            {
                CopyNextSystemToClipBoard();
            }
        }

        public ActionCommand ACSetNext
        {
            get;
            set;
        }

        public void ACSetNextFunc(object parameter)
        {
            if (RouteAktuell != null)
            {
                RouteAktuell.SetNext();

                CopyNextSystemToClipBoard();
            }
        }

        public ActionCommand ACSetPrevious
        {
            get;
            set;
        }

        public void ACSetPreviousFunc(object parameter)
        {
            if (RouteAktuell != null)
            {
                RouteAktuell.SetPrevious();

                CopyNextSystemToClipBoard();
            }
        }

        private bool _FileBrowserExpanded = false;

        public bool FileBrowserExpanded
        {
            get { return _FileBrowserExpanded; }
            set { _FileBrowserExpanded = value; RPCEvent(nameof(FileBrowserExpanded)); }
        }


        #endregion

        public ViewModelMain()
        {
            Init();
        }

        ~ViewModelMain()
        {
            SaveSettings();
        }

        private void Init()
        {
            ACLoadRouteFile = new ActionCommand
            {
                CanExecuteFunc = obj => true,
                ExecuteFunc = ACLoadRouteFileFunc
            };

            ACDeleteRoutenFile = new ActionCommand
            {
                CanExecuteFunc = obj => true,
                ExecuteFunc = ACDeleteRoutenFileFunc
            };

            ACHelp = new ActionCommand
            {
                CanExecuteFunc = obj => true,
                ExecuteFunc = ACHelpFunc
            };

            ACSetSettings = new ActionCommand
            {
                CanExecuteFunc = obj => true,
                ExecuteFunc = ACSetSettingsFunc
            };

            ACRoutePlanen = new ActionCommand
            {
                CanExecuteFunc = obj => true,
                ExecuteFunc = ACRoutePlanenFunc
            };

            ACRouteReset = new ActionCommand
            {
                CanExecuteFunc = obj => true,
                ExecuteFunc = ACRouteResetFunc
            };

            ACCopy = new ActionCommand
            {
                CanExecuteFunc = obj => true,
                ExecuteFunc = ACCopyFunc
            };

            ACSetNext = new ActionCommand
            {
                CanExecuteFunc = obj => true,
                ExecuteFunc = ACSetNextFunc
            };

            ACSetPrevious = new ActionCommand
            {
                CanExecuteFunc = obj => true,
                ExecuteFunc = ACSetPreviousFunc
            };

            ACRouteFolder = new ActionCommand
            {
                CanExecuteFunc = obj => true,
                ExecuteFunc = ACRouteFolderFunc
            };



            if (!Directory.Exists(Arbeitsverzeichnis))
            {
                Directory.CreateDirectory(Arbeitsverzeichnis);
            }

            if (!Directory.Exists(Path.Combine(Arbeitsverzeichnis, "Routen")))
            {
                Directory.CreateDirectory(Path.Combine(Arbeitsverzeichnis, "Routen"));
            }

            if (File.Exists(System.IO.Path.Combine(Arbeitsverzeichnis, "EDRouterSettings.xml")))
            {
                LoadSettings();
            }
            else
            {
                ProgramSettings = new Model.Settings();
            }

            InitGameFiles();

            GetRoutenDateien();
        }

        private void LoadSettings()
        {
            if (!Directory.Exists(Arbeitsverzeichnis))
            {
                Directory.CreateDirectory(Arbeitsverzeichnis);
            }

            if (File.Exists(System.IO.Path.Combine(Arbeitsverzeichnis, "EDRouterSettings.xml")))
            {
                try
                {
                    ProgramSettings = DeserializeObjectFromXML<Model.Settings>(System.IO.Path.Combine(Arbeitsverzeichnis, "EDRouterSettings.xml"));

                    if (!string.IsNullOrEmpty(ProgramSettings.LastRoute))
                    {
                        ACLoadRouteFileFunc(ProgramSettings.LastRoute);
                    }
                }
                catch (Exception)
                {
                    ProgramSettings = new Model.Settings();
                }

            }
            else
            {
                ProgramSettings = new Model.Settings();
            }

        }

        public void SaveSettings()
        {
            ProgramSettings.LastRoute = AktuelleRoutePath;

            SerializeObjectToXML<Model.Settings>(ProgramSettings, System.IO.Path.Combine(Arbeitsverzeichnis, "EDRouterSettings.xml"));
        }

        private void InitGameFiles()
        {
            if (Directory.Exists(Arbeitsverzeichnis))
            {
                EventWatcher = new Model.Json.JsonWatcher(JournalDirectory);

                string LastJournalFilePath = EventWatcher.GetLastJournalFile(JournalDirectory);

                EventWatcher.GetAllJsonLines(LastJournalFilePath);

                string LoadGamePath = Path.Combine(Arbeitsverzeichnis, "LoadGame.xml");

                if (File.Exists(LoadGamePath))
                {
                    LastLoadGameEvent = DeserializeObjectFromXML<Model.Events.LoadGameEvent>(LoadGamePath);
                }

                string LoadoutPath = Path.Combine(Arbeitsverzeichnis, "Loadout.xml");

                if (File.Exists(LoadoutPath))
                {
                    LastLoadoutEvent = DeserializeObjectFromXML<Model.Events.LoadoutEvent>(LoadoutPath);
                }

                EventWatcher.EventRaised += EventWatcher_EventRaised;
            }
        }

        private void GetRoutenDateien()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                DirectoryInfo DInfo = new DirectoryInfo(Path.Combine(Arbeitsverzeichnis, "Routen"));

                FileInfo[] RoutenFiles = DInfo.GetFiles("*.xml");

                List<FileInfo> FIList = RoutenFiles.ToList();

                var tempList = FIList.OrderByDescending(p => p.CreationTime);


                RoutenDateien.Clear();

                foreach (var item in tempList)
                {
                    RoutenDateien.Add(item);
                }


            }));
        }

        private void EventWatcher_EventRaised(object sender, Model.Json.EventRaisedEventArgs e)
        {
            //Event ViewModel: StartJump
            //Jump - Typ:Supercruise
            //Event ViewModel: SupercruiseEntry

            Debug.Print("Event ViewModel: " + e.EventName);

            LastAPIMessage = DateTime.Now.ToString("HH:mm:ss") + " " + e.EventName;

            switch (e.EventName)
            {
                case "LoadGame":
                    Model.Events.LoadGameEvent LG = (Model.Events.LoadGameEvent)e.EventObject;
                    Debug.Print("Commander:" + LG.Commander + " Ship:" + LG.Ship_Localised);
                    SerializeObjectToXML<Model.Events.LoadGameEvent>(LG, Path.Combine(Arbeitsverzeichnis, "LoadGame.xml"));
                    LastLoadGameEvent = LG;
                    Schiff = LG.Ship;
                    Tank = LG.FuelCapacity;

                    ResidualJumps();

                    break;
                case "FuelScoop":
                    Model.Events.FuelScoopEvent FSE = (Model.Events.FuelScoopEvent)e.EventObject;
                    Debug.Print("Total:" + FSE.Total + " Scooped:" + FSE.Scooped);
                    //SerializeObjectToXML<Model.Events.FuelScoopEvent>(FSE, Path.Combine(SettingsFolder, "FuelScoop.xml"));
                    break;
                case "ReservoirReplenished":
                    //Model.Events.ReservoirReplenishedEvent RRP = (Model.Events.ReservoirReplenishedEvent)e.EventObject;
                    break;
                case "FSDTarget":
                    Model.Events.FSDTargetEvent FSDT = (Model.Events.FSDTargetEvent)e.EventObject;
                    Debug.Print("System:" + FSDT.Name + " Star-Class:" + FSDT.StarClass + " Jumps:" + FSDT.RemainingJumpsInRoute);
                    Jumps = FSDT.RemainingJumpsInRoute;
                    //SerializeObjectToXML<Model.Events.FSDTargetEvent>(FSDT, Path.Combine(SettingsFolder, "FSDTarget.xml"));
                    SystemNächstes = FSDT.Name;

                    ResidualJumps();

                    break;
                case "FSDJump":
                    Model.Events.FSDJumpEvent FSDJ = (Model.Events.FSDJumpEvent)e.EventObject;
                    Debug.Print("Distanz:" + FSDJ.JumpDist + " System:" + FSDJ.StarSystem);
                    //SerializeObjectToXML<Model.Events.FSDJumpEvent>(FSDJ, Path.Combine(SettingsFolder, "FSDJump.xml"));
                    SystemAktuell = FSDJ.StarSystem;

                    if (Route != null)
                    {
                        Model.Events.Part Sys = Route.Find(x => x.StarSystem == SystemAktuell);
                        
                        int Pos = Route.IndexOf(Sys);
                        
                        if (Pos != -1)
                        {
                            Jumps = (Route.Count - 1) - Pos;
                        }

                    }

                    if (RouteAktuell != null)
                    {
                        if (RouteAktuell.SetEtappeBesucht(SystemAktuell))
                        {
                            SaveRouteFile();

                            RPCEvent(nameof(RouteAktuell));

                            CopyNextSystemToClipBoard();
                        }
                    }

                    ResidualJumps();

                    break;
                case "NavRoute":
                    Model.Events.NavRouteEvent NR = (Model.Events.NavRouteEvent)e.EventObject;
                    Debug.Print(NR.CRC + " [" + NR.timestamp.ToString() + "] " + NR.Event + " von " + NR.Route[0].StarSystem + " nach " + NR.Route[1].StarSystem);
                    //SerializeObjectToXML<Model.Events.NavRouteEvent>(NR, Path.Combine(SettingsFolder, "NavRoute.xml"));

                    if (NR.Route.Count > 0)
                    {
                        SystemAktuell = NR.Route[0].StarSystem;
                        SystemNächstes = NR.Route[1].StarSystem;
                        Route = NR.Route;
                        SystemZiel = NR.Route[NR.Route.Count - 1].StarSystem;
                        Jumps = NR.Route.Count - 1;
                    }
                    else
                    {
                        SystemNächstes = string.Empty;
                    }

                    ResidualJumps();

                    break;
                case "SupercruiseEntry":
                    Model.Events.SupercruiseEntryEvent SCEN = (Model.Events.SupercruiseEntryEvent)e.EventObject;
                    Debug.Print("System:" + SCEN.StarSystem);
                    //SerializeObjectToXML<Model.Events.SupercruiseEntryEvent>(SCEN, Path.Combine(SettingsFolder, "SupercruiseEntr.xml"));
                    break;
                case "SupercruiseExit":
                    Model.Events.SupercruiseExitEvent SCEX = (Model.Events.SupercruiseExitEvent)e.EventObject;
                    Debug.Print("System:" + SCEX.StarSystem);
                    //SerializeObjectToXML<Model.Events.SupercruiseExitEvent>(SCEX, Path.Combine(SettingsFolder, "SupercruiseExit.xml"));
                    break;
                case "StartJump":
                    Model.Events.StartJumpEvent SJ = (Model.Events.StartJumpEvent)e.EventObject;
                    
                    Debug.Print("Jump-Typ:" + SJ.JumpType);
                    //SerializeObjectToXML<Model.Events.StartJumpEvent>(SJ, Path.Combine(SettingsFolder, "StartJump.xml"));
                    break;
                case "Location":
                    Model.Events.LocationEvent Loc = (Model.Events.LocationEvent)e.EventObject;
                    SystemAktuell = Loc.StarSystem;
                    Debug.Print("Star-System:" + Loc.StarSystem);
                    SerializeObjectToXML<Model.Events.LocationEvent>(Loc, Path.Combine(Arbeitsverzeichnis, "Location.xml"));
                    break;
                case "ShipLocker":
                    Model.Events.ShipLockerEvent SL = (Model.Events.ShipLockerEvent)e.EventObject;
                    Debug.Print("ShipLocker:" + SL.timestamp);
                    //SerializeObjectToXML<Model.Events.ShipLockerEvent>(SL, Path.Combine(SettingsFolder, "ShipLocker.xml"));
                    break;
                case "Status":
                    Model.Events.StatusEvent S = (Model.Events.StatusEvent)e.EventObject;

                    if (S.Destination != null)
                    {
                        Debug.Print("[" + S.timestamp + "] Ziel:" + S.Destination.Name + " " + S.Destination.System + S.Destination.Body);
                    }
                    else
                    {
                        Debug.Print("[" + S.timestamp + "]");
                    }

                    if (S.Fuel != null)
                    {
                        FuelMain = S.Fuel.FuelMain;
                        FuelReservoir = S.Fuel.FuelReservoir;
                    }



                    break;
                case "Loadout":
                    LastLoadoutEvent = (Model.Events.LoadoutEvent)e.EventObject;
                    SerializeObjectToXML<Model.Events.LoadoutEvent>(LastLoadoutEvent, Path.Combine(Arbeitsverzeichnis, "Loadout.xml"));
                    break;
                case "LaunchSRV":
                    Model.Events.LaunchSRVEvent LSRV = (Model.Events.LaunchSRVEvent)e.EventObject;
                    Debug.Print("LaunchSRV:" + LSRV.SRVType);
                    break;
                case "DockSRV":
                    Model.Events.DockSRVEvent DSRV = (Model.Events.DockSRVEvent)e.EventObject;
                    Debug.Print("DockSRV:" + DSRV.SRV);
                    break;
                default:
                    break;
            }
        }


        #region FileSystemWatcher..

        private void InitWatcher()
        {

            string downloadsPath = Model.KnownFolders.GetPath(Model.KnownFolder.Downloads);

            watcher = new FileSystemWatcher(downloadsPath);

            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            watcher.Filters.Add("neutron*");
            watcher.Filters.Add("fleet-carrier*");
            //watcher.Filters.Add("*.log");
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;

        }

        private void StopWatcher()
        {
            watcher.EnableRaisingEvents = false;

            watcher.Changed -= OnChanged;
            watcher.Created -= OnCreated;
            watcher.Deleted -= OnDeleted;
            watcher.Renamed -= OnRenamed;
            watcher.Error -= OnError;

            watcher.Dispose();

        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            if (!e.FullPath.ToLower().EndsWith(".csv"))
            {
                return;
            }

            if (e.Name.StartsWith("neutron") && e.FullPath != PathToImportedCSV)
            {
                PathToImportedCSV = e.FullPath;
                ImportRoute(e.FullPath);
                GetRoutenDateien();
            }
            else if (e.Name.StartsWith("fleet-carrier") && e.FullPath != PathToImportedCSV)
            {
                PathToImportedCSV = e.FullPath;
                ImportFleetCarrierRoute(e.FullPath);
                GetRoutenDateien();
            }

            Debug.Print($"Changed: {e.FullPath}");
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Debug.Print(value);
        }

        private void OnDeleted(object sender, FileSystemEventArgs e) =>
            Debug.Print($"Deleted: {e.FullPath}");

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            Debug.Print($"Renamed:");
            Debug.Print($"    Old: {e.OldFullPath}");
            Debug.Print($"    New: {e.FullPath}");
        }

        private void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Debug.Print($"Message: {ex.Message}");
                Debug.Print("Stacktrace:");
                Debug.Print(ex.StackTrace);
                Debug.Print("");
                PrintException(ex.InnerException);
            }
        }

        #endregion

        private void ResidualJumps()
        {
            if (RouteAktuell != null)
            {
                Restsprünge = RouteAktuell.Where(x => x.besucht == false).Sum(x => x.Sprünge);
            }
        }

        private void CopyNextSystemToClipBoard()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (RouteAktuell.NextTarget != null)
                {
                    SelectedEtappe = RouteAktuell.NextTarget;
                    RPCEvent("SelectedEtappe");
                    string NextWaypoint = RouteAktuell.NextTarget.System;

                    Clipboard.SetText(NextWaypoint);

                }
            }));
        }

        private void ImportRoute(string PathToCSV)
        {

            this.Dispatcher.Invoke((Action)(() =>
            {
                StopWatcher();
                
                RouteAktuell.Clear();
                RouteAktuell.Type = Model.Route.ReiseRoute.RouteType.NR;

                //"System Name","Distance To Arrival","Distance Remaining","Neutron Star","Jumps"

                string[] RawLines = File.ReadAllLines(PathToCSV);
            
                for (int i = 1; i < RawLines.Length; i++)
                {
                    string[] RawEntrys =  RawLines[i].Split(",",StringSplitOptions.RemoveEmptyEntries);
                
                    for (int x = 0; x < RawEntrys.Length; x++)
                    {
                        RawEntrys[x] = RawEntrys[x].Replace("\"", string.Empty).Replace("\\", string.Empty);
                    }

                    Model.Route.Etappe E = new Model.Route.Etappe(RawEntrys, Model.Route.ReiseRoute.RouteType.NR);

                    if (i == 1)
                    {
                        E.besucht = true;
                    }

                    RouteAktuell.Add(E);
                }

                string RouteFileName = Path.Combine(Arbeitsverzeichnis, "Routen", RouteAktuell.GetRouteName + ".xml");

                SerializeObjectToXML<Model.Route.ReiseRoute>(RouteAktuell, RouteFileName);
                SelectedRoutenFile = new FileInfo(RouteFileName);

                LoadRouteFile(RouteFileName);

                ProgramSettings.LastRoute = RouteFileName;

                SaveSettings();

                CopyNextSystemToClipBoard();

                try
                {
                    if (File.Exists(PathToCSV))
                    {
                        File.Delete(PathToCSV);
                        PathToImportedCSV = string.Empty;
                    }
                }
                catch (Exception){ Debug.Print("Fehler beim Löschen der CSV-Datei!"); }


            }));


        }

        private void ImportFleetCarrierRoute(string PathToCSV)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                StopWatcher();

                RouteAktuell.Clear();
                RouteAktuell.Type = Model.Route.ReiseRoute.RouteType.FC;

                //"System Name","Distance To Arrival","Distance Remaining","Neutron Star","Jumps"
                //"\"System Name\",\"Distance\",\"Distance Remaining\",\"Tritium in tank\",\"Tritium in market\",\"Fuel Used\",\"Icy Ring\",\"Pristine\",\"Restock Tritium\""
                string[] RawLines = File.ReadAllLines(PathToCSV);

                for (int i = 1; i < RawLines.Length; i++)
                {
                    string[] RawEntrys = RawLines[i].Split(",", StringSplitOptions.RemoveEmptyEntries);

                    for (int x = 0; x < RawEntrys.Length; x++)
                    {
                        RawEntrys[x] = RawEntrys[x].Replace("\"", string.Empty).Replace("\\", string.Empty);
                    }

                    Model.Route.Etappe E = new Model.Route.Etappe(RawEntrys, Model.Route.ReiseRoute.RouteType.FC);

                    if (i == 1)
                    {
                        E.besucht = true;
                    }

                    RouteAktuell.Add(E);
                }

                string RouteFileName = Path.Combine(Arbeitsverzeichnis, "Routen", RouteAktuell.GetRouteName + ".xml");

                SerializeObjectToXML<Model.Route.ReiseRoute>(RouteAktuell, RouteFileName);
                SelectedRoutenFile = new FileInfo(RouteFileName);

                SaveSettings();

                CopyNextSystemToClipBoard();

                try
                {
                    if (File.Exists(PathToCSV))
                    {
                        File.Delete(PathToCSV);
                        PathToImportedCSV = string.Empty;
                    }
                }
                catch (Exception) { Debug.Print("Fehler beim Löschen der CSV-Datei!"); }

            }));


        }

        private void LoadRouteFile(string Path)
        {
            if (System.IO.File.Exists(Path))
            {
                RouteAktuell = DeserializeObjectFromXML<Model.Route.ReiseRoute>(Path);

                AktuelleRoutePath = Path;

                if (!RouteAktuell.Abgeschlossen())
                {

                    if (RouteAktuell.NextTarget != null)
                    {
                        SystemAktuell = RouteAktuell.SystemStandort;

                        SystemZiel = RouteAktuell.NextTarget.System;
                        Jumps = RouteAktuell.NextTarget.Sprünge;
                        CopyNextSystemToClipBoard();
                    }

                    ResidualJumps();

                }
                else
                {
                    SystemZiel = null;
                    Jumps = 0;
                    Restsprünge = 0;
                    SystemAktuell = string.Empty;
                }
            }
            else
            { 
            
            }

        }

        private void SaveRouteFile()
        {
            string RouteFileName = Path.Combine(Arbeitsverzeichnis, "Routen", RouteAktuell.GetRouteName + ".xml");

            SerializeObjectToXML<Model.Route.ReiseRoute>(RouteAktuell, RouteFileName);
        }

        private void SerializeObjectToXML<T>(T item, string FilePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));

            using (StreamWriter wr = new StreamWriter(FilePath))
            {
                xs.Serialize(wr, item);
                wr.Flush();
                wr.Close();
            }
        }

        private T DeserializeObjectFromXML<T>(string FilePath)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (StreamReader sr = new StreamReader(FilePath))
            {
                return (T)xs.Deserialize(sr);
            }
        }

    }
}
