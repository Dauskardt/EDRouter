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
    public enum Verfügbarkeitswert { rar, normal, häufig, kaufbar, unbestimmt }

    class JsonWatcher : Model.ModelBase
    {
        public string Arbeitsverzeichnis { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), System.Reflection.Assembly.GetExecutingAssembly().GetName().Name); } }

        private FileSystemWatcher watcher { get; set; }


        public event EventHandler<EventRaisedEventArgs> EventRaised;

        protected virtual void OnEventRaised(EventRaisedEventArgs e)
        {
            EventHandler<EventRaisedEventArgs> handler = EventRaised;

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
        private DateTime LastEDLogTime { get; set; }

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

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private string _ActualLogFile;
        //public string ActualLogFile
        //{
        //    get { return _ActualLogFile; }
        //}

        private Model.Events.NavRouteEvent _NaveRoute;
        public Model.Events.NavRouteEvent NaveRoute
        {
            get { return _NaveRoute; }
            set { _NaveRoute = value; RPCEvent(nameof(NaveRoute)); }
        }

        private Model.Events.StatusEvent _Status;
        public Model.Events.StatusEvent Status
        {
            get { return _Status; }
            set { _Status = value; RPCEvent(nameof(Status)); }
        }

        private Model.Events.ModulesInfoEvent _Module;
        public Model.Events.ModulesInfoEvent Module
        {
            get { return _Module; }
            set { _Module = value; RPCEvent(nameof(Module)); }
        }


        private DateTime TimeStampMax = DateTime.Now;

        private DateTime LastNavRouteTimeStamp = new DateTime(1, 1, 1);
        private DateTime LastStatusTimeStamp = new DateTime(1, 1, 1);

        private void GetLaunchSRV()
        { 
        
        
        }

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

                                OnEventRaised(new EventRaisedEventArgs(Status, Status.Event));
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
                                OnEventRaised(new EventRaisedEventArgs(NaveRoute, NaveRoute.Event));
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
                                OnEventRaised(new EventRaisedEventArgs(Module, Module.Event));
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
                                EventTest = JsonSerializer.Deserialize<Model.Events.EventBase>(line);

                                //string EventTestStrg = line.Substring(47, line.IndexOf('\"', 47) - 47);

                                //if (EventTestStrg == "Loadout")
                                //{
                                //    Model.Events.LoadoutEvent Loadout = JsonSerializer.Deserialize<Model.Events.LoadoutEvent>(line);
                                //    OnEventRaised(new EventRaisedEventArgs(Loadout, Loadout.Event));
                                //}


                                if (EventTest.timestamp > TimeStampMax)
                                {
                                    string EventStrg = line.Substring(47, line.IndexOf('\"', 47) - 47);

                                    switch (EventStrg)
                                    {
                                        //case "ReceiveText":
                                        //case "Powerplay":
                                        //case "Music":
                                        //case "Missions":
                                        //case "NpcCrewPaidWage":
                                        //case "Cargo":
                                        //case "MissionFailed":

                                        //case "FSSSignalDiscovered":
                                        //case "FSSAllBodiesFound":
                                        case "FuelScoop":
                                            Model.Events.FuelScoopEvent FuelScoop = JsonSerializer.Deserialize<Model.Events.FuelScoopEvent>(line);
                                            //Journal.Add(FuelScoop);
                                            OnEventRaised(new EventRaisedEventArgs(FuelScoop, FuelScoop.Event));
                                            break;
                                        case "ReservoirReplenished":
                                            break;
                                        case "FSDTarget":
                                            Model.Events.FSDTargetEvent FSDTarget = JsonSerializer.Deserialize<Model.Events.FSDTargetEvent>(line);
                                            //Journal.Add(FSDTarget);
                                            OnEventRaised(new EventRaisedEventArgs(FSDTarget, FSDTarget.Event));
                                            break;
                                        case "FSDJump":
                                            Model.Events.FSDJumpEvent FSDJump = JsonSerializer.Deserialize<Model.Events.FSDJumpEvent>(line);
                                            //Journal.Add(FSDJump);
                                            OnEventRaised(new EventRaisedEventArgs(FSDJump, FSDJump.Event));
                                            break;
                                        case "NavRoute":
                                            break;
                                        case "SupercruiseEntry":
                                            Model.Events.SupercruiseEntryEvent SupercruiseEntry = JsonSerializer.Deserialize<Model.Events.SupercruiseEntryEvent>(line);
                                            //Journal.Add(SupercruiseEntry);
                                            OnEventRaised(new EventRaisedEventArgs(SupercruiseEntry, SupercruiseEntry.Event));
                                            break;
                                        case "SupercruiseExit":
                                            Model.Events.SupercruiseExitEvent SupercruiseExit = JsonSerializer.Deserialize<Model.Events.SupercruiseExitEvent>(line);
                                            //Journal.Add(SupercruiseExit);
                                            OnEventRaised(new EventRaisedEventArgs(SupercruiseExit, SupercruiseExit.Event));
                                            break;
                                        case "StartJump":
                                            Model.Events.StartJumpEvent StartJump = JsonSerializer.Deserialize<Model.Events.StartJumpEvent>(line);
                                            //Journal.Add(StartJump);
                                            OnEventRaised(new EventRaisedEventArgs(StartJump, StartJump.Event));
                                            break;
                                        //case "LaunchSRV":
                                        //    Model.Events.LaunchSRVEvent LauchSRV = JsonSerializer.Deserialize<Model.Events.LaunchSRVEvent>(line);
                                        //    OnEventRaised(new EventRaisedEventArgs(LauchSRV, LauchSRV.Event));
                                        //    break;
                                        //case "DockSRV":
                                        //    Model.Events.DockSRVEvent DockSRV = JsonSerializer.Deserialize<Model.Events.DockSRVEvent>(line);
                                        //    OnEventRaised(new EventRaisedEventArgs(DockSRV, DockSRV.Event));
                                        //    break;
                                        //case "LaunchFighter":
                                        //case "ShipTargeted":
                                        //case "Bounty":
                                        //case "UnderAttack":
                                        //case "Scanned":
                                        //case "Scan":
                                        case "JetConeBoost":
                                            Model.Events.JetConeBoostEvent JetConeBoost = JsonSerializer.Deserialize<Model.Events.JetConeBoostEvent>(line);
                                            OnEventRaised(new EventRaisedEventArgs(JetConeBoost, JetConeBoost.Event));
                                            break;
                                        //case "Friends":
                                        //case "Shutdown":
                                        //    break;
                                        //case "Undocked":
                                        //case "SquadronStartup":
                                        //case "ModuleInfo":
                                        //case "FighterDestroyed":
                                        //case "FighterRebuilt":
                                        //case "DockFighter":
                                        //case "SAASignalsFound":
                                        case "RepairAll":
                                            Model.Events.RepairAllEvent RepairAll = JsonSerializer.Deserialize<Model.Events.RepairAllEvent>(line);
                                            OnEventRaised(new EventRaisedEventArgs(RepairAll, RepairAll.Event));
                                            break;
                                        case "AfmuRepairs":
                                            Model.Events.AfmuRepairsEvent AfmuRepairs = JsonSerializer.Deserialize<Model.Events.AfmuRepairsEvent>(line);
                                            OnEventRaised(new EventRaisedEventArgs(AfmuRepairs, AfmuRepairs.Event));
                                            break;
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
                                        //case "ApproachBody":
                                        //case "Touchdown":
                                        //case "BackpackChange":
                                        //case "Embark":
                                        //case "Liftoff":
                                        //case "TradeMicroResources":
                                        //case "SellMicroResources":

                                        //    break;
                                        case "LoadGame":

                                            Model.Events.LoadGameEvent LoadGame = JsonSerializer.Deserialize<Model.Events.LoadGameEvent>(line);

                                            //if (!LoadGame.IsEmpty)
                                            //{
                                                OnEventRaised(new EventRaisedEventArgs(LoadGame, LoadGame.Event));
                                            //}
                                            break;
                                        case "EngineerProgress":
                                            Model.Events.EngineerProgressEvent EngineerProgress = JsonSerializer.Deserialize<Model.Events.EngineerProgressEvent>(line);
                                            EngineerProgress.Engineers.Sort();
                                            break;
                                        case "Fileheader":
                                            Model.Events.FileHeaderEvent Fileheader = JsonSerializer.Deserialize<Model.Events.FileHeaderEvent>(line);
                                            OnEventRaised(new EventRaisedEventArgs(Fileheader, Fileheader.Event));
                                            break;
                                        case "Commander":
                                            Model.Events.CommanderEvent Commander = JsonSerializer.Deserialize<Model.Events.CommanderEvent>(line);
                                            CommanderProperty = Commander.Name;
                                            OnEventRaised(new EventRaisedEventArgs(Commander, Commander.Event));
                                            break;
                                        case "ShipLocker":
                                            //Model.Events.ShipLockerEvent ShipLocker = JsonSerializer.Deserialize<Model.Events.ShipLockerEvent>(line);
                                            //OnEventRaised(new EventRaisedEventArgs(ShipLocker, ShipLocker.Event));
                                            break;
                                        case "Materials":
                                            Model.Events.MaterialsEvent Materials = JsonSerializer.Deserialize<Model.Events.MaterialsEvent>(line);

                                            break;
                                        case "Rank":
                                            Model.Events.RankEvent Rank = JsonSerializer.Deserialize<Model.Events.RankEvent>(line);

                                            break;
                                        case "Progress":
                                            Model.Events.ProgressEvent Progress = JsonSerializer.Deserialize<Model.Events.ProgressEvent>(line);

                                            break;
                                        case "Reputation":
                                            Model.Events.ReputationEvent Reputation = JsonSerializer.Deserialize<Model.Events.ReputationEvent>(line);

                                            break;
                                        case "Location":
                                            Model.Events.LocationEvent Location = JsonSerializer.Deserialize<Model.Events.LocationEvent>(line);
                                            OnEventRaised(new EventRaisedEventArgs(Location, Location.Event));
                                            break;
                                        case "Statistics":
                                            Model.Events.StatisticsEvent Statistics = JsonSerializer.Deserialize<Model.Events.StatisticsEvent>(line);
                                            OnEventRaised(new EventRaisedEventArgs(Statistics, Statistics.Event));
                                            break;
                                        case "Loadout":
                                            Model.Events.LoadoutEvent Loadout = JsonSerializer.Deserialize<Model.Events.LoadoutEvent>(line);
                                            OnEventRaised(new EventRaisedEventArgs(Loadout, Loadout.Event));
                                            break;
                                        case "Shutdown":
                                            Model.Events.ShutdownEvent Shutdown = JsonSerializer.Deserialize<Model.Events.ShutdownEvent>(line);
                                            OnEventRaised(new EventRaisedEventArgs(Shutdown, Shutdown.Event));
                                            break;
                                        default:
                                            break;
                                    }

                                    Debug.Print("Last Log-Event:" + EventStrg + " [" + TimeStampMax.ToString() +"]");
                                }
                                
                                Index++;
                            }

                            TimeStampMax = EventTest.timestamp;
                        }
                    }
                }
            }
        }

        private void GetLastTimeStamp(string path)
        {
            if (File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    if (fs.CanRead)
                    {
                        using (var sr = new StreamReader(fs))
                        {
                            string line;

                            while ((line = sr.ReadLine()) != null)
                            {
                                string EventStrg = line.Substring(47, line.IndexOf('\"', 47) - 47);

                                Events.EventBase EventTest = JsonSerializer.Deserialize<Model.Events.EventBase>(line);

                                if (EventTest.timestamp > TimeStampMax)
                                {
                                    TimeStampMax = EventTest.timestamp;
                                }
                            }
                        }
                    }
                }
                
                Debug.Print("Last Journal Timestamp:" + TimeStampMax.ToString());
            }
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

    public class EventRaisedEventArgs : EventArgs
    {
        public string EventName { get; set; }
        public object EventObject { get; set; }

        public EventRaisedEventArgs(object Event,string name) 
        {
            EventObject = Event;
            EventName = name;
        }
    }
}
