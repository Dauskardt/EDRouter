﻿using System;
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
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " .NET Framework 5.0" + 
                    " [SH4DOWM4K3R " + DateTime.Now.Year + "]"; } }

        public string Arbeitsverzeichnis { get { return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\EDRouter"; } }

        private string UserArbeitsverzeichnis { get; set; }

        private bool _UserInitComplete = false;
        public bool UserInitComplete 
        {
            get { return _UserInitComplete; }
            set { _UserInitComplete = value; RPCEvent(nameof(UserInitComplete)); }
        
        }

        public string PathToExe { get { return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\EDRouter.exe"; } }

        private static string JournalDirectory { get { return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToString() + "\\Saved Games\\Frontier Developments\\Elite Dangerous"; } }

        private static string DownloadFolder { get { return Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads"; } }

        private Model.Json.JsonWatcher EventWatcher { get; set; }

        private FileSystemWatcher watcher { get; set; }

        private string PathToImportedCSV { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _FileBrowserExpanded = false;
        public bool FileBrowserExpanded
        {
            get { return _FileBrowserExpanded; }
            set { _FileBrowserExpanded = value; RPCEvent(nameof(FileBrowserExpanded)); }
        }

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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
            set { _FuelMain = value; RPCEvent(nameof(FuelMain)); CalcFuelPercent(); 
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _FuelPercent;
        public string FuelPercent
        {
            get { return _FuelPercent; }
            set { _FuelPercent = value; RPCEvent(nameof(FuelPercent)); }
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
        private double _FSDHealth;
        public double FSDHealth
        {
            get { return Math.Floor(_FSDHealth * 100.0); }
            set { _FSDHealth = value; RPCEvent(nameof(FSDHealth)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _FSDBoost;
        public bool FSDBoost
        {
            get { return _FSDBoost; }
            set
            {
                _FSDBoost = value;
                RPCEvent(nameof(FSDBoost)); 
                if (value) { _FSDHealth = _FSDHealth - 0.01; RPCEvent(nameof(FSDHealth)); }
                Debug.Print("### FSD-HEALTH:" + FSDHealth);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _LastAPIMessage;
        public string LastAPIMessage
        {
            get { return _LastAPIMessage; }
            set { _LastAPIMessage = value; RPCEvent(nameof(LastAPIMessage)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _LastAPIIndex;
        public int LastAPIIndex
        {
            get { return _LastAPIIndex; }
            set { _LastAPIIndex = value; RPCEvent(nameof(LastAPIIndex)); }

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
        private Model.Route.ReiseRoute _RouteAktuell;
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
            string RouteDir = Path.Combine(UserArbeitsverzeichnis, "Routen");

            if (Directory.Exists(RouteDir))
            {
                Process.Start("Explorer.exe", RouteDir);
            }
        }

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

                        CopyNextSystemToClipBoard();

                        SaveRouteFile();
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

        public ActionCommand ACEject
        {
            get;
            set;
        }
        public void ACEjectFunc(object parameter)
        {
            if (MessageBox.Show("Aktuelle Route auswerfen?", "Benutzerabfrage", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
            {
                RouteAktuell = null;
                Route = null;
                AktuelleRoutePath = string.Empty;
                ProgramSettings.LastRoute = string.Empty;
                SaveSettings();
            }
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

            ACEject= new ActionCommand
            {
                CanExecuteFunc = obj => true,
                ExecuteFunc = ACEjectFunc
            };

            InitJsonWatcher();
        }

        private bool InitUserDirectory(string FID)
        {
            try
            {
                UserArbeitsverzeichnis = Path.Combine(Arbeitsverzeichnis, FID);

                if (!Directory.Exists(UserArbeitsverzeichnis))
                {
                    Directory.CreateDirectory(UserArbeitsverzeichnis);
                }

                if (!Directory.Exists(Path.Combine(UserArbeitsverzeichnis, "Routen")))
                {
                    Directory.CreateDirectory(Path.Combine(UserArbeitsverzeichnis, "Routen"));
                }

                if (File.Exists(System.IO.Path.Combine(UserArbeitsverzeichnis, "EDRouterSettings.xml")))
                {
                    LoadSettings();
                }
                else
                {
                    ProgramSettings = new Model.Settings();
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Initialisierungsfehler" + Environment.NewLine + ex.Message, "Programmfehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;

        }

        private void LoadSettings()
        {
            if (!Directory.Exists(UserArbeitsverzeichnis))
            {
                Directory.CreateDirectory(UserArbeitsverzeichnis);
            }

            if (File.Exists(System.IO.Path.Combine(UserArbeitsverzeichnis, "EDRouterSettings.xml")))
            {
                try
                {
                    ProgramSettings = DeserializeObjectFromXML<Model.Settings>(System.IO.Path.Combine(UserArbeitsverzeichnis, "EDRouterSettings.xml"));

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
            if (!string.IsNullOrEmpty(UserArbeitsverzeichnis))
            {
                if (Directory.Exists(UserArbeitsverzeichnis))
                {
                    if (ProgramSettings != null)
                    {
                        ProgramSettings.LastRoute = AktuelleRoutePath;
                        SerializeObjectToXML<Model.Settings>(ProgramSettings, System.IO.Path.Combine(UserArbeitsverzeichnis, "EDRouterSettings.xml"));
                    }
                }
            }
        }

        private void InitJsonWatcher()
        {
            EventWatcher = new Model.Json.JsonWatcher(JournalDirectory);
            EventWatcher.EventRaised += EventWatcher_EventRaised;

            string LastJournalFilePath = EventWatcher.GetLastJournalFile(JournalDirectory);

            //EventWatcher.GetAllJsonLines(LastJournalFilePath);
        }

        private void InitGameFiles()
        {
            if (Directory.Exists(UserArbeitsverzeichnis))
            {
                string LoadGamePath = Path.Combine(UserArbeitsverzeichnis, "LoadGame.xml");

                if (File.Exists(LoadGamePath))
                {
                    LastLoadGameEvent = DeserializeObjectFromXML<Model.Events.LoadGameEvent>(LoadGamePath);
                }

                string LoadoutPath = Path.Combine(UserArbeitsverzeichnis, "Loadout.xml");

                if (File.Exists(LoadoutPath))
                {
                    LastLoadoutEvent = DeserializeObjectFromXML<Model.Events.LoadoutEvent>(LoadoutPath);

                    FSDHealth = LastLoadoutEvent.Modules.FirstOrDefault(x => x.Slot == "FrameShiftDrive").Health;

                }
            }
        }

        private void GetRoutenDateien()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                DirectoryInfo DInfo = new DirectoryInfo(Path.Combine(UserArbeitsverzeichnis, "Routen"));

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

        private void EventWatcher_EventRaised(object sender, Model.Events.EventRaisedEventArgs e)
        {

            Debug.Print("Event ViewModel: " + e.EventName);

            if (e.EventName != "Status" && e.EventName != "Music")
            {
                Model.Events.EventBase EventTest = (Model.Events.EventBase)e.EventObject;
                LastAPIMessage = EventTest.timestamp.ToString("HH:mm:ss") + " " + e.EventName;
                //File.AppendAllText(Path.Combine(Arbeitsverzeichnis,"APILOG.txt"),LastAPIMessage + Environment.NewLine);

                LastAPIIndex = e.Line;
            }

            if (e.EventName == "LoadGame")
            {
                Model.Events.LoadGameEvent LG = (Model.Events.LoadGameEvent)e.EventObject;
                Debug.Print("Commander:" + LG.Commander + " Ship:" + LG.Ship_Localised);

                if (LastLoadGameEvent == null)// && LG.timestamp > LastLoadGameEvent.timestamp)
                {
                    LastLoadGameEvent = LG;
                    Schiff = LG.Ship;
                    Tank = LG.FuelCapacity;

                    if (InitUserDirectory(LG.FID))
                    {
                        SerializeObjectToXML<Model.Events.LoadGameEvent>(LG, Path.Combine(UserArbeitsverzeichnis, "LoadGame.xml"));

                        InitGameFiles();

                        GetRoutenDateien();

                        CalcRestsprünge();

                        UserInitComplete = true;

                        LastAPIIndex = e.Line;
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }

            }

            if (UserInitComplete)
            {
                switch (e.EventName)
                {
                    case "Shutdown":
                        //Nichts zu tun, nur anzeigen und speichern..
                        SaveRouteFile();
                        SaveSettings();
                        ClearUI();
                        
                        break;
                    case "MainMenu":
                        LastAPIMessage = DateTime.Now.ToString("HH:mm:ss") + " MainMenu";
                        ClearUI();
                        break;
                    case "FuelScoop":
                        Model.Events.FuelScoopEvent FSE = (Model.Events.FuelScoopEvent)e.EventObject;
                        Debug.Print("Total:" + FSE.Total + " Scooped:" + FSE.Scooped);
                        break;
                    case "ReservoirReplenished":
                        //Model.Events.ReservoirReplenishedEvent RRP = (Model.Events.ReservoirReplenishedEvent)e.EventObject;
                        break;
                    case "FSDTarget":
                        Model.Events.FSDTargetEvent FSDT = (Model.Events.FSDTargetEvent)e.EventObject;
                        Debug.Print("System:" + FSDT.Name + " Star-Class:" + FSDT.StarClass + " Jumps:" + FSDT.RemainingJumpsInRoute);
                        Jumps = FSDT.RemainingJumpsInRoute;

                        SystemNächstes = FSDT.Name;

                        CalcRestsprünge();

                        break;
                    case "FSDJump":
                        Model.Events.FSDJumpEvent FSDJ = (Model.Events.FSDJumpEvent)e.EventObject;
                        Debug.Print("Distanz:" + FSDJ.JumpDist + " System:" + FSDJ.StarSystem);
                        FSDBoost = false;

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

                        CalcRestsprünge();

                        break;
                    case "NavRoute":
                        Model.Events.NavRouteEvent NR = (Model.Events.NavRouteEvent)e.EventObject;
                        Debug.Print(NR.CRC + " [" + NR.timestamp.ToString() + "] " + NR.Event + " von " + NR.Route[0].StarSystem + " nach " + NR.Route[1].StarSystem);

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

                        CalcRestsprünge();

                        break;
                    case "SupercruiseEntry":
                        Model.Events.SupercruiseEntryEvent SCEN = (Model.Events.SupercruiseEntryEvent)e.EventObject;
                        Debug.Print("System:" + SCEN.StarSystem);

                        break;
                    case "SupercruiseExit":
                        Model.Events.SupercruiseExitEvent SCEX = (Model.Events.SupercruiseExitEvent)e.EventObject;
                        Debug.Print("System:" + SCEX.StarSystem);

                        break;
                    case "StartJump":
                        Model.Events.StartJumpEvent SJ = (Model.Events.StartJumpEvent)e.EventObject;

                        Debug.Print("Jump-Typ:" + SJ.JumpType);
                        ;
                        break;
                    case "JetConeBoost":
                        Model.Events.JetConeBoostEvent JCB = (Model.Events.JetConeBoostEvent)e.EventObject;
                        FSDBoost = true;
                        //LastLoadoutEvent.Modules.FirstOrDefault(x => x.Slot == "FrameShiftDrive").Health = FSDHealth;
                        //SerializeObjectToXML<Model.Events.LoadoutEvent>(LastLoadoutEvent, Path.Combine(Arbeitsverzeichnis, "Loadout.xml"));

                        Debug.Print("BoostValue:" + JCB.BoostValue);
                        break;
                    case "Location":
                        Model.Events.LocationEvent Loc = (Model.Events.LocationEvent)e.EventObject;
                        SystemAktuell = Loc.StarSystem;
                        Debug.Print("Star-System:" + Loc.StarSystem);
                        SerializeObjectToXML<Model.Events.LocationEvent>(Loc, Path.Combine(UserArbeitsverzeichnis, "Location.xml"));
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
                        SerializeObjectToXML<Model.Events.LoadoutEvent>(LastLoadoutEvent, Path.Combine(UserArbeitsverzeichnis, "Loadout.xml"));

                        Debug.Print(LastLoadoutEvent.ModulesValue.ToString());

                        FSDHealth = LastLoadoutEvent.Modules.FirstOrDefault(x => x.Slot == "FrameShiftDrive").Health;

                        Debug.Print("### FSD-Health:" + FSDHealth);

                        break;
                    case "LaunchSRV":
                        Model.Events.LaunchSRVEvent LSRV = (Model.Events.LaunchSRVEvent)e.EventObject;
                        Debug.Print("LaunchSRV:" + LSRV.SRVType);
                        break;
                    case "DockSRV":
                        Model.Events.DockSRVEvent DSRV = (Model.Events.DockSRVEvent)e.EventObject;
                        Debug.Print("DockSRV:" + DSRV.SRV);
                        break;
                    case "ModulesInfo":
                        Model.Events.ModulesInfoEvent ModInfo = (Model.Events.ModulesInfoEvent)e.EventObject;
                        Debug.Print("ModuleInfo:" + ModInfo.Modules.Count);
                        break;
                    case "RepairAll":
                        _FSDHealth = 1.0; RPCEvent(nameof(FSDHealth));
                        break;
                    case "AfmuRepairs":
                        Model.Events.AfmuRepairsEvent AfmuRepairs = (Model.Events.AfmuRepairsEvent)e.EventObject;
                        if (AfmuRepairs.Module_Localised == "FSA")
                        {
                            _FSDHealth = AfmuRepairs.Health;
                            RPCEvent(nameof(FSDHealth));
                        }
                        break;
                    case "ApproachBody":
                        Model.Events.ApproachBodyEvent ApproachBody = (Model.Events.ApproachBodyEvent)e.EventObject;
                        Debug.Print("ApproachBody:" + ApproachBody.Body);
                        break;
                    case "LeaveBody":
                        Model.Events.LeaveBodyEvent LeaveBody = (Model.Events.LeaveBodyEvent)e.EventObject;
                        Debug.Print("LeaveBody:" + LeaveBody.Body);
                        break;
                    case "Touchdown":
                        Model.Events.TouchdownEvent Touchdown = (Model.Events.TouchdownEvent)e.EventObject;
                        Debug.Print("Touchdown:" + Touchdown.Body);
                        break;
                    case "Liftoff":
                        Model.Events.LiftoffEvent Liftoff = (Model.Events.LiftoffEvent)e.EventObject;
                        Debug.Print("Liftoff:" + Liftoff.Body);
                        break;
                    case "Embark":
                        Model.Events.EmbarkEvent Embark = (Model.Events.EmbarkEvent)e.EventObject;
                        Debug.Print("Embark:" + Embark.Body);
                        break;
                    case "Disembark":
                        Model.Events.DisembarkEvent Disembark = (Model.Events.DisembarkEvent)e.EventObject;
                        Debug.Print("Disembark:" + Disembark.Body);
                        break;
                    case "SAAScanComplete":
                        Model.Events.SAAScanCompleteEvent SAAScanComplete = (Model.Events.SAAScanCompleteEvent)e.EventObject;
                        Debug.Print("Body:" + SAAScanComplete.BodyName);
                        break;
                    case "SAASignalsFound":
                        Model.Events.SAASignalsFoundEvent SAASignalsFound = (Model.Events.SAASignalsFoundEvent)e.EventObject;
                        Debug.Print("Body:" + SAASignalsFound.BodyName);
                        break;
                    case "FSSDiscoveryScan":
                        Model.Events.FSSDiscoveryScanEvent FSSDiscoveryScan = (Model.Events.FSSDiscoveryScanEvent)e.EventObject;
                        Debug.Print("System:" + FSSDiscoveryScan.SystemName);
                        break;
                    case "Scan":
                        Model.Events.ScanEvent Scan = (Model.Events.ScanEvent)e.EventObject;
                        Debug.Print("Body:" + Scan.BodyName);
                        break;
                    case "Music":
                        Model.Events.MusicEvent Music = (Model.Events.MusicEvent)e.EventObject;
                        Debug.Print("Track:" + Music.MusicTrack);

                        switch (Music.MusicTrack)
                        {
                            case "Supercruise":
                                LastAPIMessage = DateTime.Now.ToString("HH:mm:ss") + " Supercruise";
                                break;
                            case "SystemAndSurfaceScanner":
                                LastAPIMessage = DateTime.Now.ToString("HH:mm:ss") + " SASScanner";
                                break;
                            case "GalaxyMap":
                                LastAPIMessage = DateTime.Now.ToString("HH:mm:ss") + " GalaxyMap";
                                break;
                            case "DestinationFromHyperspace":
                                LastAPIMessage = DateTime.Now.ToString("HH:mm:ss") + " HyperspaceDrop";
                                break;
                            case "SystemMap":
                                LastAPIMessage = DateTime.Now.ToString("HH:mm:ss") + " SystemMap";
                                break;
                            case "NoTrack":
                                break;
                            //case "MainMenu":
                            //    LastAPIMessage = DateTime.Now.ToString("HH:mm:ss") + " MainMenu";
                            //    ClearUI();
                            //    break;
                            case "Exploration":
                                LastAPIMessage = DateTime.Now.ToString("HH:mm:ss") + " Exploration";
                                break;
                            default:
                                break;
                        }

                        LastAPIIndex = e.Line;

                        break;
                    default:
                        break;
                }
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
            watcher.Filters.Add("riches*");
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
                ImportRoute(e.FullPath);
                PathToImportedCSV = e.FullPath;
            }
            else if (e.Name.StartsWith("fleet-carrier") && e.FullPath != PathToImportedCSV)
            {
                ImportFleetCarrierRoute(e.FullPath);
                PathToImportedCSV = e.FullPath;
            }
            else if (e.Name.StartsWith("riches") && e.FullPath != PathToImportedCSV)
            {
                ImportRichesRoute(e.FullPath);
                PathToImportedCSV = e.FullPath;
            }

            GetRoutenDateien();

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

        private void PrintException(Exception ex)
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

        private void CalcRestsprünge()
        {
            if (RouteAktuell != null)
            {
                Restsprünge = RouteAktuell.Where(x => x.besucht == false).Sum(x => x.Sprünge);
            }
        }

        private void CalcFuelPercent()
        {
            if (LastLoadoutEvent != null)
            {
                FuelPercent = " [" + ((100.00 / LastLoadoutEvent.FuelCapacity.Main) * FuelMain).ToString("0.0") + "%]";
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
                
                RouteAktuell = new Model.Route.ReiseRoute();
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
                        E.TimeStamp = DateTime.Now;
                    }

                    RouteAktuell.Add(E);
                }

                string RouteFileName = Path.Combine(UserArbeitsverzeichnis, "Routen", RouteAktuell.GetRouteName + ".xml");

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

                RouteAktuell = new Model.Route.ReiseRoute();
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
                        E.TimeStamp = DateTime.Now;
                    }

                    RouteAktuell.Add(E);
                }

                string RouteFileName = Path.Combine(UserArbeitsverzeichnis, "Routen", RouteAktuell.GetRouteName + ".xml");

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

        private void ImportRichesRoute(string PathToCSV)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                StopWatcher();

                RouteAktuell = new Model.Route.ReiseRoute();
                RouteAktuell.Type = Model.Route.ReiseRoute.RouteType.RR;

                //"System Name","Distance To Arrival","Distance Remaining","Neutron Star","Jumps"

                string[] RawLines = File.ReadAllLines(PathToCSV);

                for (int i = 1; i < RawLines.Length; i++)
                {
                    string[] RawEntrys = RawLines[i].Split(",", StringSplitOptions.RemoveEmptyEntries);

                    for (int x = 0; x < RawEntrys.Length; x++)
                    {
                        RawEntrys[x] = RawEntrys[x].Replace("\"", string.Empty).Replace("\\", string.Empty);
                    }

                    Model.Route.Etappe E = new Model.Route.Etappe(RawEntrys, Model.Route.ReiseRoute.RouteType.RR);

                    if (i == 1)
                    {
                        E.besucht = true;
                        E.TimeStamp = DateTime.Now;
                    }

                    RouteAktuell.Add(E);
                }

                string RouteFileName = Path.Combine(UserArbeitsverzeichnis, "Routen", RouteAktuell.GetRouteName + ".xml");

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

                    CalcRestsprünge();

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
            string RouteFileName = Path.Combine(UserArbeitsverzeichnis, "Routen", RouteAktuell.GetRouteName + ".xml");

            SerializeObjectToXML<Model.Route.ReiseRoute>(RouteAktuell, RouteFileName);
        }

        private void ClearUI()
        {
            UserInitComplete = false;

            //ProgramSettings = new Model.Settings();

            LastLoadGameEvent = null;
            LastLoadoutEvent = null;

            Route = null;
            RouteAktuell = null;

            SelectedRoutenFile = null;
            SelectedEtappe = null;

            //AktuelleRoutePath = string.Empty;
            AktuelleRouteName = string.Empty;
            SystemAktuell = string.Empty;
            SystemZiel = string.Empty;
            SystemNächstes = string.Empty;
            FuelMain = 0.0;
            FuelPercent = string.Empty;
            FuelReservoir = 0.0;
            Jumps = 0;
            Restsprünge = 0;
            Schiff = string.Empty;
            Tank = 0.0;
            FSDHealth = 0.0;
            FSDBoost = false;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void SerializeObjectToXML<T>(T item, string FilePath)
        {
            //var task = Task.Run(() =>
            //{
                XmlSerializer xs = new XmlSerializer(typeof(T));

                using (StreamWriter wr = new StreamWriter(FilePath))
                {
                    xs.Serialize(wr, item);
                    wr.Flush();
                    wr.Close();
                }
            //});
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
