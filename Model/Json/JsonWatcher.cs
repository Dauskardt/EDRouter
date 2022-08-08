using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Serialization;


namespace EDRouter.Model.Json
{
    //public enum Verfügbarkeitswert { rar, normal, häufig, kaufbar, unbestimmt }

    class JsonWatcher : Model.ModelBase
    {
        public string Arbeitsverzeichnis { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), System.Reflection.Assembly.GetExecutingAssembly().GetName().Name); } }

        private FileSystemWatcher watcher { get; set; }

        private bool ExtendetEvents { get; set; } = false;

        public event EventHandler<Model.Events.EventRaisedEventArgs> EventRaised;

        protected virtual void OnEventRaised(Model.Events.EventRaisedEventArgs e)
        {
            EventHandler<Model.Events.EventRaisedEventArgs> handler = EventRaised;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _StatusMeldung;
        public string StatusMeldung
        {
            get { return _StatusMeldung; }
            set { _StatusMeldung = value; RPCEvent(nameof(StatusMeldung)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _JournalFolder;
        public string JournalFolder
        {
            get { return _JournalFolder; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _Commander;
        public string CommanderProperty
        {
            get
            {
                return _Commander;
            }
            set
            {
                _Commander = value; RPCEvent(nameof(CommanderProperty));
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Model.Events.NavRouteEvent _NaveRoute;
        public Model.Events.NavRouteEvent NaveRoute
        {
            get { return _NaveRoute; }
            set { _NaveRoute = value; RPCEvent(nameof(NaveRoute)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Model.Events.StatusEvent _Status;
        public Model.Events.StatusEvent Status
        {
            get { return _Status; }
            set { _Status = value; RPCEvent(nameof(Status)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Model.Events.ModulesInfoEvent _Module;
        public Model.Events.ModulesInfoEvent Module
        {
            get { return _Module; }
            set { _Module = value; RPCEvent(nameof(Module)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime LastNavRouteTimeStamp = new DateTime(1, 1, 1);
        private DateTime LastStatusTimeStamp = new DateTime(1, 1, 1);

        //private DateTime LastTimeStampMax { get; set; } = new DateTime(1,1,1);

        private DateTime LastEDLogTime { get; set; }

        private bool InGame { get; set; } = false;

        private int LastLineIndex { get; set; } = -1;

        private void GetStatusJson(string path)
        {
            if (File.Exists(path))
            {
                System.Threading.Thread.Sleep(100);

                try
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        if (fs.CanRead)
                        {
                            using (var sr = new StreamReader(fs))
                            {
                                string JsonText = sr.ReadToEnd();

                                Status = JsonSerializer.Deserialize<Model.Events.StatusEvent>(JsonText);

                                OnEventRaised(new Model.Events.EventRaisedEventArgs(Status, Status.Event,0));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print("ERROR: Status - " + ex.Message);
                }
            }
        }

        private void GetNavRouteJson(string path)
        {
            if (File.Exists(path))
            {
                System.Threading.Thread.Sleep(100);

                try
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        if (fs.CanRead)
                        {
                            using (var sr = new StreamReader(fs))
                            {
                                string JsonText = sr.ReadToEnd();

                                NaveRoute = JsonSerializer.Deserialize<Model.Events.NavRouteEvent>(JsonText);
                                OnEventRaised(new Model.Events.EventRaisedEventArgs(NaveRoute, NaveRoute.Event,0));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print("ERROR: NavRoute - " + ex.Message);
                }
            }
        }

        private void GetModulesInfo(string path)
        {
            if (File.Exists(path))
            {
                System.Threading.Thread.Sleep(100);

                try
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        if (fs.CanRead)
                        {
                            using (var sr = new StreamReader(fs))
                            {
                                string JsonText = sr.ReadToEnd();

                                Module = JsonSerializer.Deserialize<Model.Events.ModulesInfoEvent>(JsonText);
                                OnEventRaised(new Model.Events.EventRaisedEventArgs(Module, Module.Event,0));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print("ERROR: NavRoute - " + ex.Message);
                }
            }
        }

        public void GetAllJsonLines(string path)
        {
            if (File.Exists(path))
            {
                if (LastLineIndex == -1)
                {
                    //TODO: Prüfen ob es hier richtig ist!!!
                    LastLineIndex = GetLastLoadGameLine(path);
                }

                if (LastLineIndex > 0 && InGame)
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        if (fs.CanRead)
                        {
                            using (var sr = new StreamReader(fs))
                            {
                                string line;
                                int Index = 0;

                                Events.EventBase EventTest = new Events.EventBase();

                                while ((line = sr.ReadLine()) != null)
                                {
                                    //EventTest = JsonSerializer.Deserialize<Model.Events.EventBase>(line);

                                    if (Index >= LastLineIndex)
                                    {
                                        string EventStrg = line.Substring(47, line.IndexOf('\"', 47) - 47);
                                        Debug.Print("Last Log-Event:" + EventStrg + " [Line " + LastLineIndex.ToString() + "]");

                                        switch (EventStrg)
                                        {
                                            //case "LaunchSRV":
                                            //    Model.Events.LaunchSRVEvent LauchSRV = JsonSerializer.Deserialize<Model.Events.LaunchSRVEvent>(line);
                                            //    OnEventRaised(new EventRaisedEventArgs(LauchSRV, LauchSRV.Event));
                                            //    break;
                                            //case "DockSRV":
                                            //    Model.Events.DockSRVEvent DockSRV = JsonSerializer.Deserialize<Model.Events.DockSRVEvent>(line);
                                            //    OnEventRaised(new EventRaisedEventArgs(DockSRV, DockSRV.Event));
                                            //    break;
                                            //case "FSSSignalDiscovered":
                                            //case "FSSAllBodiesFound":
                                            case "Fileheader":
                                                Model.Events.FileHeaderEvent Fileheader = JsonSerializer.Deserialize<Model.Events.FileHeaderEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(Fileheader, Fileheader.Event, Index));
                                                break;
                                            case "Commander":
                                                Model.Events.CommanderEvent Commander = JsonSerializer.Deserialize<Model.Events.CommanderEvent>(line);
                                                CommanderProperty = Commander.Name;
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(Commander, Commander.Event, Index));
                                                break;
                                            case "LoadGame":
                                                Model.Events.LoadGameEvent LoadGame = JsonSerializer.Deserialize<Model.Events.LoadGameEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(LoadGame, LoadGame.Event, Index));
                                                break;
                                            case "Music":
                                                Model.Events.MusicEvent Music = JsonSerializer.Deserialize<Model.Events.MusicEvent>(line);

                                                if (Music.MusicTrack == "MainMenu")
                                                {
                                                    InGame = false;
                                                    LastLineIndex = -1;
                                                    OnEventRaised(new Model.Events.EventRaisedEventArgs(Music, Music.Event, Index));
                                                    return;
                                                }
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(Music, Music.Event, Index));
                                                break;
                                            case "Loadout":
                                                Model.Events.LoadoutEvent Loadout = JsonSerializer.Deserialize<Model.Events.LoadoutEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(Loadout, Loadout.Event, Index));
                                                break;
                                            case "Shutdown":
                                                Model.Events.ShutdownEvent Shutdown = JsonSerializer.Deserialize<Model.Events.ShutdownEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(Shutdown, Shutdown.Event, Index));
                                                LastLineIndex = -1;
                                                InGame = false;
                                                return;
                                            case "Location":
                                                Model.Events.LocationEvent Location = JsonSerializer.Deserialize<Model.Events.LocationEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(Location, Location.Event, Index));
                                                break;
                                            case "FuelScoop":
                                                Model.Events.FuelScoopEvent FuelScoop = JsonSerializer.Deserialize<Model.Events.FuelScoopEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(FuelScoop, FuelScoop.Event, Index));
                                                break;
                                            case "FSDTarget":
                                                Model.Events.FSDTargetEvent FSDTarget = JsonSerializer.Deserialize<Model.Events.FSDTargetEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(FSDTarget, FSDTarget.Event, Index));
                                                break;
                                            case "FSDJump":
                                                Model.Events.FSDJumpEvent FSDJump = JsonSerializer.Deserialize<Model.Events.FSDJumpEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(FSDJump, FSDJump.Event, Index));
                                                break;
                                            case "SupercruiseEntry":
                                                Model.Events.SupercruiseEntryEvent SupercruiseEntry = JsonSerializer.Deserialize<Model.Events.SupercruiseEntryEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(SupercruiseEntry, SupercruiseEntry.Event, Index));
                                                break;
                                            case "SupercruiseExit":
                                                Model.Events.SupercruiseExitEvent SupercruiseExit = JsonSerializer.Deserialize<Model.Events.SupercruiseExitEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(SupercruiseExit, SupercruiseExit.Event, Index));
                                                break;
                                            case "StartJump":
                                                Model.Events.StartJumpEvent StartJump = JsonSerializer.Deserialize<Model.Events.StartJumpEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(StartJump, StartJump.Event, Index));
                                                break;
                                            case "JetConeBoost":
                                                Model.Events.JetConeBoostEvent JetConeBoost = JsonSerializer.Deserialize<Model.Events.JetConeBoostEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(JetConeBoost, JetConeBoost.Event, Index));
                                                break;
                                            case "ApproachBody":
                                                Model.Events.ApproachBodyEvent ApproachBody = JsonSerializer.Deserialize<Model.Events.ApproachBodyEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(ApproachBody, ApproachBody.Event, Index));
                                                break;
                                            case "LeaveBody":
                                                Model.Events.LeaveBodyEvent LeaveBody = JsonSerializer.Deserialize<Model.Events.LeaveBodyEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(LeaveBody, LeaveBody.Event, Index));
                                                break;
                                            case "Touchdown":
                                                Model.Events.TouchdownEvent Touchdown = JsonSerializer.Deserialize<Model.Events.TouchdownEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(Touchdown, Touchdown.Event, Index));
                                                break;
                                            case "Liftoff":
                                                Model.Events.LiftoffEvent Liftoff = JsonSerializer.Deserialize<Model.Events.LiftoffEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(Liftoff, Liftoff.Event, Index));
                                                break;
                                            case "Embark":
                                                Model.Events.EmbarkEvent Embark = JsonSerializer.Deserialize<Model.Events.EmbarkEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(Embark, Embark.Event, Index));
                                                break;
                                            case "Disembark":
                                                Model.Events.DisembarkEvent Disembark = JsonSerializer.Deserialize<Model.Events.DisembarkEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(Disembark, Disembark.Event, Index));
                                                break;
                                            case "RepairAll":
                                                Model.Events.RepairAllEvent RepairAll = JsonSerializer.Deserialize<Model.Events.RepairAllEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(RepairAll, RepairAll.Event, Index));
                                                break;
                                            case "AfmuRepairs":
                                                Model.Events.AfmuRepairsEvent AfmuRepairs = JsonSerializer.Deserialize<Model.Events.AfmuRepairsEvent>(line);
                                                OnEventRaised(new Model.Events.EventRaisedEventArgs(AfmuRepairs, AfmuRepairs.Event, Index));
                                                break;

                                            case "EngineerProgress":
                                                if (ExtendetEvents)
                                                {
                                                    Model.Events.EngineerProgressEvent EngineerProgress = JsonSerializer.Deserialize<Model.Events.EngineerProgressEvent>(line);
                                                    EngineerProgress.Engineers.Sort();
                                                }
                                                break;
                                            case "ShipLocker":
                                                if (ExtendetEvents)
                                                {
                                                    Model.Events.ShipLockerEvent ShipLocker = JsonSerializer.Deserialize<Model.Events.ShipLockerEvent>(line);
                                                    OnEventRaised(new Model.Events.EventRaisedEventArgs(ShipLocker, ShipLocker.Event, Index));
                                                }
                                                break;
                                            case "Materials":
                                                if (ExtendetEvents)
                                                {
                                                    Model.Events.MaterialsEvent Materials = JsonSerializer.Deserialize<Model.Events.MaterialsEvent>(line);
                                                }
                                                break;
                                            case "Rank":
                                                if (ExtendetEvents)
                                                {
                                                    Model.Events.RankEvent Rank = JsonSerializer.Deserialize<Model.Events.RankEvent>(line);
                                                }
                                                break;
                                            case "Progress":
                                                if (ExtendetEvents)
                                                {
                                                    Model.Events.ProgressEvent Progress = JsonSerializer.Deserialize<Model.Events.ProgressEvent>(line);
                                                }
                                                break;
                                            case "Reputation":
                                                if (ExtendetEvents)
                                                {
                                                    Model.Events.ReputationEvent Reputation = JsonSerializer.Deserialize<Model.Events.ReputationEvent>(line);
                                                }
                                                break;

                                            case "Statistics":
                                                if (ExtendetEvents)
                                                {
                                                    Model.Events.StatisticsEvent Statistics = JsonSerializer.Deserialize<Model.Events.StatisticsEvent>(line);
                                                    OnEventRaised(new Model.Events.EventRaisedEventArgs(Statistics, Statistics.Event, Index));
                                                }
                                                break;

                                            case "SAAScanComplete":
                                                if (ExtendetEvents)
                                                {
                                                    Model.Events.SAAScanCompleteEvent SAAScanComplete = JsonSerializer.Deserialize<Model.Events.SAAScanCompleteEvent>(line);
                                                    OnEventRaised(new Model.Events.EventRaisedEventArgs(SAAScanComplete, SAAScanComplete.Event, Index));
                                                }
                                                break;
                                            case "SAASignalsFound":
                                                if (ExtendetEvents)
                                                {
                                                    Model.Events.SAASignalsFoundEvent SAASignalsFound = JsonSerializer.Deserialize<Model.Events.SAASignalsFoundEvent>(line);
                                                    OnEventRaised(new Model.Events.EventRaisedEventArgs(SAASignalsFound, SAASignalsFound.Event, Index));
                                                }
                                                break;
                                            case "FSSDiscoveryScan":
                                                if (ExtendetEvents)
                                                {
                                                    Model.Events.FSSDiscoveryScanEvent FSSDiscoveryScan = JsonSerializer.Deserialize<Model.Events.FSSDiscoveryScanEvent>(line);
                                                    OnEventRaised(new Model.Events.EventRaisedEventArgs(FSSDiscoveryScan, FSSDiscoveryScan.Event, Index));
                                                }
                                                break;
                                            case "Scan":
                                                if (ExtendetEvents)
                                                {
                                                    Model.Events.ScanEvent Scan = JsonSerializer.Deserialize<Model.Events.ScanEvent>(line);
                                                    OnEventRaised(new Model.Events.EventRaisedEventArgs(Scan, Scan.Event, Index));
                                                }
                                                break;
                                            //case "ReceiveText":
                                            //case "ReservoirReplenished":
                                            //case "RedeemVoucher":
                                            //case "RestockVehicle":
                                            //case "ShipyardSwap":
                                            //case "PayFines":
                                            //case "BuyMicroResources":
                                            //case "SwitchSuitLoadout":
                                            //case "DropItems":
                                            //case "ScanBaryCentre":
                                            //case "ShieldState":
                                            //case "EscapeInterdiction":
                                            //case "BuyAmmo":
                                            //case "BackpackChange":
                                            //case "TradeMicroResources":
                                            //case "SellMicroResources":
                                            //case "Friends":
                                            //case "Undocked":
                                            //case "SquadronStartup":
                                            //case "ModuleInfo":
                                            //case "FighterDestroyed":
                                            //case "FighterRebuilt":
                                            //case "DockFighter":
                                            //case "LaunchFighter":
                                            //case "ShipTargeted":
                                            //case "Bounty":
                                            //case "UnderAttack":
                                            //case "Scanned":
                                            //case "Powerplay":
                                            //case "Missions":
                                            //case "NpcCrewPaidWage":
                                            //case "Cargo":
                                            //case "MissionFailed":
                                            default:
                                                break;
                                        }

                                        //LastTimeStampMax = EventTest.timestamp;
                                    }

                                    Index++;
                                }

                                LastLineIndex = Index; 
                                //LastTimeStampMax += new TimeSpan(0, 0, 1);//EventTest.timestamp;
                            }
                        }
                    }
                }
                else
                {
                    LastLineIndex = -1;
                    InGame = false;
                    OnEventRaised(new Model.Events.EventRaisedEventArgs(new Events.ShutdownEvent(),"Shutdown",-1));
                }
            }
        }

        private int GetLastLoadGameLine(string path)
        {
            int LastLoadGameIndex = -1;

            if (File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (fs.CanRead)
                    {
                        using (var sr = new StreamReader(fs))
                        {
                           
                            int LineIndex = 0;
                            string line;

                            while ((line = sr.ReadLine()) != null)
                            {
                                if (LineIndex > LastLineIndex)
                                {

                                    string EventStrg = line.Substring(47, line.IndexOf('\"', 47) - 47);

                                    Events.EventBase EventTest = JsonSerializer.Deserialize<Model.Events.EventBase>(line);

                                    if (EventStrg == "LoadGame")
                                    {
                                        InGame = true;
                                        LastLoadGameIndex = LineIndex;
                                        //TODO: Position des Letzen Zeichens im Bytearray
                                    }
                                    else if (EventStrg == "Music")
                                    {
                                        Events.MusicEvent musik = JsonSerializer.Deserialize<Model.Events.MusicEvent>(line);

                                        if (musik.MusicTrack == "MainMenu")
                                        {
                                            InGame = false;
                                            LastLineIndex = -1;
                                        }
                                    }
                                    else if (EventStrg == "Shutdown")
                                    {
                                        InGame = false;
                                        LastLineIndex = -1;
                                    }
                                }

                                LineIndex++;
                            }

                            return LastLoadGameIndex;
                        }
                    }
                }
                
                Debug.Print("Last Journal LoadGame Line:" + LastLoadGameIndex.ToString());
            }

            return -1;
        }

        public string GetLastJournalFile(string path)
        {

           List<string> JournalFiles = Directory.GetFiles(path, "Journal.*").ToList<string>();
           JournalFiles.Sort((a, b) => b.CompareTo(a));
           return JournalFiles[0];
        }


        public JsonWatcher(string JournalFolderPath) 
        {
            if (!string.IsNullOrEmpty(JournalFolderPath))
            {
                if (Directory.Exists(JournalFolderPath))
                {
                    _JournalFolder = JournalFolderPath;

                    InitWatcher();
                }
                else
                {
                    throw new DirectoryNotFoundException("Das Verzeichnis " + JournalFolderPath + " wurde nicht gefunden!");
                }
            }
            else
            {
                throw new ArgumentNullException("Es wurde kein Journal-Pfad engegeben!");
            }
        }

        private void InitWatcher()
        {

            watcher = new FileSystemWatcher(JournalFolder);

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

            watcher.Filters.Add("*.json");
            watcher.Filters.Add("*.log");
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            switch (e.Name)
            {
                case "Status.json":
                    GetStatusJson(e.FullPath);
                    break;
                case "ShipLocker.json":
                    break;
                case "NavRoute.json":
                    GetNavRouteJson(e.FullPath);
                    break;
                case "ModulesInfo.json":
                    GetModulesInfo(e.FullPath);
                    break;
                default:
                    if (e.Name.StartsWith("Journal"))
                    {
                        GetAllJsonLines(e.FullPath);
                    }
                    break;
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

    }


}
