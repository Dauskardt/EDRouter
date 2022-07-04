using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class FSDJumpEvent:EventBase
    {
        //{ "timestamp":"2021-12-25T18:59:13Z", "event":"FSDJump", "Taxi":false, "Multicrew":false, "StarSystem":"G 113-20", "SystemAddress":2870246188449, "StarPos":[18.78125,10.09375,-20.75000], "SystemAllegiance":"Federation", "SystemEconomy":"$economy_Colony;", "SystemEconomy_Localised":"Kolonie", "SystemSecondEconomy":"$economy_Industrial;", "SystemSecondEconomy_Localised":"Industrie", "SystemGovernment":"$government_Democracy;", "SystemGovernment_Localised":"Demokratie", "SystemSecurity":"$SYSTEM_SECURITY_low;", "SystemSecurity_Localised":"Geringe Sicherheit",
        //"Population":85670, "Body":"G 113-20", "BodyID":0, "BodyType":"Star", "JumpDist":32.600, "FuelUsed":1.541689, "FuelLevel":14.458311, "Factions":[ { "Name":"G 113-20 Silver Electronics", "FactionState":"None", "Government":"Corporate", "Influence":0.042424, "Allegiance":"Federation", "Happiness":"$Faction_HappinessBand2;", "Happiness_Localised":"Glücklich", "MyReputation":0.000000 }, { "Name":"Abrogo Partnership", "FactionState":"None", "Government":"Confederacy", "Influence":0.093939, "Allegiance":"Federation", "Happiness":"$Faction_HappinessBand2;", "Happiness_Localised":"Glücklich", "MyReputation":-3.750000, "RecoveringStates":[ { "State":"Expansion", "Trend":0 }, { "State":"War", "Trend":0 } ] }, { "Name":"Green Party of Luyten's Star", "FactionState":"InfrastructureFailure", "Government":"Democracy", "Influence":0.267677, "Allegiance":"Federation", "Happiness":"$Faction_HappinessBand2;", "Happiness_Localised":"Glücklich", "MyReputation":25.559999, "RecoveringStates":[ { "State":"War", "Trend":0 } ], "ActiveStates":[ { "State":"InfrastructureFailure" } ] }, { "Name":"LHS 2065 Gold Allied Industries", "FactionState":"None", "Government":"Corporate", "Influence":0.171717, "Allegiance":"Federation", "Happiness":"$Faction_HappinessBand2;", "Happiness_Localised":"Glücklich", "MyReputation":5.681130, "RecoveringStates":[ { "State":"War", "Trend":0 } ] }, { "Name":"Confederacy of G 113-20", "FactionState":"None", "Government":"Confederacy", "Influence":0.107071, "Allegiance":"Federation", "Happiness":"$Faction_HappinessBand2;", "Happiness_Localised":"Glücklich", "MyReputation":0.000000 }, { "Name":"G 113-20 Nobles", "FactionState":"CivilUnrest", "Government":"Feudal", "Influence":0.051515, "Allegiance":"Independent", "Happiness":"$Faction_HappinessBand2;", "Happiness_Localised":"Glücklich", "MyReputation":0.000000, "RecoveringStates":[ { "State":"InfrastructureFailure", "Trend":0 } ], "ActiveStates":[ { "State":"CivilUnrest" } ] }, { "Name":"LHS 1918 Major Exchange", "FactionState":"None", "Government":"Corporate", "Influence":0.193939, "Allegiance":"Federation", "Happiness":"$Faction_HappinessBand2;", "Happiness_Localised":"Glücklich", "MyReputation":0.000000, "RecoveringStates":[ { "State":"War", "Trend":0 } ] }, { "Name":"Adnovi Security Holdings", "FactionState":"None", "Government":"Corporate", "Influence":0.071717, "Allegiance":"Federation", "Happiness":"$Faction_HappinessBand2;", "Happiness_Localised":"Glücklich", "MyReputation":0.000000 } ], "SystemFaction":{ "Name":"Green Party of Luyten's Star", "FactionState":"InfrastructureFailure" }, "Conflicts":[ { "WarType":"war", "Status":"", "Faction1":{ "Name":"Abrogo Partnership", "Stake":"", "WonDays":1 }, "Faction2":{ "Name":"LHS 2065 Gold Allied Industries", "Stake":"McHugh Installation", "WonDays":4 } }, { "WarType":"war", "Status":"", "Faction1":{ "Name":"Green Party of Luyten's Star", "Stake":"Cernan Lab", "WonDays":4 }, "Faction2":{ "Name":"LHS 1918 Major Exchange", "Stake":"Ansari Hangar", "WonDays":1 } } ] }

        public FSDJumpEvent() 
        {
            this.Event = "FSDJump";
        }

        public bool Taxi { get; set; }
        public bool Multicrew { get; set; }
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
        public double[] StarPos { get; set; }
        public string SystemAllegiance { get; set; }
        public string SystemEconomy { get; set; }
        public string SystemEconomy_Localised { get; set; }
        public string SystemGovernment { get; set; }
        public string SystemGovernment_Localised { get; set; }
        public string SystemSecurity { get; set; }
        public string SystemSecurity_Localised { get; set; }
        public long Population { get; set; }
        public string Body { get; set; }
        public int BodyID { get; set; }
        public string BodyType { get; set; }
        public double JumpDist { get; set; }
        public double FuelUsed { get; set; }
        public double FuelLevel { get; set; }
    }
}
