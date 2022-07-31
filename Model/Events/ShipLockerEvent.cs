using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class ShipLockerEvent:Model.Events.EventBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Model.Material.MaterialOdyssey> _Items;
        public List<Model.Material.MaterialOdyssey> Items 
        {
            get { return _Items; }
            set { _Items = value; if (value != null) { CleanUp(Items); SetItemsType(); RPCEvent(nameof(Items)); } }
        
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Model.Material.MaterialOdyssey> _Components;
        public List<Model.Material.MaterialOdyssey> Components 
        {
            get { return _Components; }
            set { _Components = value; if (value != null) { CleanUp(Components); SetCompnentType(Components); RPCEvent(nameof(Components)); } } 
        }

        private List<Model.Material.MaterialOdyssey> _Consumables;
        public List<Model.Material.MaterialOdyssey> Consumables
        {
            get { return _Consumables; }
            set { _Consumables = value; CleanUp(Consumables); SetConsumablesType(); RPCEvent(nameof(Consumables)); }
        }


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Model.Material.MaterialOdyssey> _Data;
        public List<Model.Material.MaterialOdyssey> Data
        {
            get { return _Data; }
            set { _Data = value; CleanUp(Data); SetDataType(); RPCEvent(nameof(Data)); } 
        }

        public Model.Material.MaterialOdyssey this[string Name] => AllDataItems.First(x => x.Name.Contains(Name));

        public ShipLockerEvent()
        {
            Event = "ShipLocker";
        }

        public List<Model.Material.MaterialOdyssey> AllDataItems 
        { 
            get 
            {
                List<Model.Material.MaterialOdyssey> AllItems = new List<Model.Material.MaterialOdyssey>();
                AllItems.AddRange(Items);
                AllItems.AddRange(Components);
                AllItems.AddRange(Consumables);
                AllItems.AddRange(Data);

                AllItems.Sort();

                return AllItems;
            } 
        
        }

        private static void CleanUp(List<Model.Material.MaterialOdyssey> ItemList)
        {

            foreach (var item in ItemList)
            {
                if (item.Name_Localised != null)
                {
                    if (item.Name_Localised.Contains("-<br>"))
                    {
                        item.Name_Localised = item.Name_Localised.Replace("-<br>", string.Empty);
                    }
                }
                else
                {
                    item.Name_Localised = item.Name[0].ToString().ToUpper() + item.Name[1..];
                }
            }

            MergeItemDoublettes(ItemList);


        }

        private static void SetCompnentType(List<Model.Material.MaterialOdyssey> ItemList)
        {
            foreach (var item in ItemList)
            {
                item.Type = GetComponentType(item.Name);
            }
        }

        private void SetItemsType()
        {
            foreach (var item in Items)
            {
                item.Type = "ITEMS";
            }
        }
        private void SetConsumablesType()
        {
            foreach (var item in Consumables)
            {
                item.Type = "CONSUMABLES";
            }
        }
        private void SetDataType()
        {
            foreach (var item in Data)
            {
                item.Type = "DATA";
            }
        }


        private static void MergeItemDoublettes(List<Model.Material.MaterialOdyssey> ItemList)
        {
            var querydouble = ItemList.GroupBy(x => x.Name)
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();

            if (querydouble.Count > 0)
            {
                for (int q = 0; q < querydouble.Count; q++)
                {
                
                    List<Model.Material.MaterialOdyssey> Doublettes = ItemList.FindAll(x => x.Name.Equals(querydouble[q]));

                    for (int i = 1; i < Doublettes.Count; i++)
                    {
                        Doublettes[0].Count += Doublettes[i].Count;

                        ItemList.Remove(Doublettes[i]);
                    }
                }
            }

            //ItemList.Sort();
        }

        private static string GetComponentType(string name)
        {
            string Type = "COMPONENTS";

            switch (name)
            {
                case "graphene":
                case "aerogel":
                case "chemicalcatalyst":
                case "chemicalsuperbase":
                case "epinephrine":
                case "epoxyadhesive":
                case "oxygenicbacteria":
                case "phneutraliser":
                case "rdx":
                case "viscoelasticpolymer":
                    Type += ".CHEMICALS";
                    break;

                case "circuitboard":
                case "circuitswitch":
                case "electricalfuse":
                case "electricalwiring":
                case "electromagnet":
                case "ionbattery":
                case "metalcoil":
                case "microsupercapacitor":
                case "microtransformer":
                case "microelectrode":
                case "motor":
                case "opticalfibre":
                    Type += ".CIRCUITS";
                    break;
                case "carbonfibreplating":
                case "encryptedmemorychip":
                case "memorychip":
                case "microhydraulics":
                case "microthrusters":
                case "opticallens":
                case "scrambler":
                case "titaniumplating":
                case "transmitter":
                case "tungstencarbide":
                case "weaponcomponent":
                    Type += ".TECH";
                    break;

                default:
                    Debug.Print("found Unknown Component");
                    break;
            }

            return Type;
        }

        //{ "timestamp":"2021-11-30T14:14:59Z", "event":"ShipLocker", "Items":[ { "Name":"largecapacitypowerregulator", "Name_Localised":"Energieregulator", "OwnerID":0, "MissionID":824120133, "Count":1 }, { "Name":"largecapacitypowerregulator", "Name_Localised":"Energieregulator", "OwnerID":0, "MissionID":824162866, "Count":1 }, { "Name":"largecapacitypowerregulator", "Name_Localised":"Energieregulator", "OwnerID":0, "MissionID":824162878, "Count":1 }, { "Name":"largecapacitypowerregulator", "Name_Localised":"Energieregulator", "OwnerID":0, "MissionID":824162918, "Count":1 }, { "Name":"largecapacitypowerregulator", "Name_Localised":"Energieregulator", "OwnerID":0, "MissionID":824163009, "Count":1 }, { "Name":"largecapacitypowerregulator", "Name_Localised":"Energieregulator", "OwnerID":0, "MissionID":824163049, "Count":1 }, { "Name":"largecapacitypowerregulator", "Name_Localised":"Energieregulator", "OwnerID":0, "MissionID":824163064, "Count":1 }, { "Name":"largecapacitypowerregulator", "Name_Localised":"Energieregulator", "OwnerID":0, "MissionID":824163120, "Count":1 }, { "Name":"biochemicalagent", "Name_Localised":"Biochemischer Wirkstoff", "OwnerID":0, "Count":10 }, { "Name":"geneticsample", "Name_Localised":"Biologische Probe", "OwnerID":0, "Count":7 }, { "Name":"gmeds", "Name_Localised":"G-Medizin", "OwnerID":0, "Count":32 }, { "Name":"healthmonitor", "Name_Localised":"Gesundheitsmonitor", "OwnerID":0, "Count":3 }, { "Name":"inertiacanister", "Name_Localised":"Trägheits-Container", "OwnerID":0, "Count":1 }, { "Name":"ionisedgas", "Name_Localised":"Ionisiertes Gas", "OwnerID":0, "Count":34 }, { "Name":"geneticrepairmeds", "Name_Localised":"Gen-Reparatursets", "OwnerID":0, "Count":12 }, { "Name":"hush", "Name_Localised":"Schnurr", "OwnerID":0, "Count":21 }, { "Name":"insightentertainmentsuite", "Name_Localised":"Insight-Entertainment-Bibliothek", "OwnerID":0, "Count":14 }, { "Name":"lazarus", "OwnerID":0, "Count":40 }, { "Name":"push", "Name_Localised":"Schub", "OwnerID":0, "Count":26 }, { "Name":"shipschematic", "Name_Localised":"Schiffsplan", "OwnerID":0, "Count":9 }, { "Name":"surveillanceequipment", "Name_Localised":"Überwachungsausrüstung", "OwnerID":0, "Count":25 }, { "Name":"syntheticpathogen", "Name_Localised":"Synthetischer Erreger", "OwnerID":0, "Count":7 }, { "Name":"weaponschematic", "Name_Localised":"Waffenplan", "OwnerID":0, "Count":153 }, { "Name":"compressionliquefiedgas", "Name_Localised":"Komprimiertes Flüssiggas", "OwnerID":0, "Count":56 }, { "Name":"degradedpowerregulator", "Name_Localised":"Ausrangierter Energieregulator", "OwnerID":0, "Count":1 } ], "Components":[ { "Name":"graphene", "Name_Localised":"Graphen", "OwnerID":0, "Count":1 }, { "Name":"aerogel", "OwnerID":0, "Count":1 }, { "Name":"carbonfibreplating", "Name_Localised":"Carbonfaserplatten", "OwnerID":0, "Count":14 }, { "Name":"chemicalcatalyst", "Name_Localised":"Chemischer Katalysator", "OwnerID":0, "Count":25 }, { "Name":"chemicalsuperbase", "Name_Localised":"Chemische Superbase", "OwnerID":0, "Count":27 }, { "Name":"circuitboard", "Name_Localised":"Schaltplatte", "OwnerID":0, "Count":23 }, { "Name":"circuitswitch", "Name_Localised":"Elektroschalter", "OwnerID":0, "Count":22 }, { "Name":"electricalfuse", "Name_Localised":"Elektrosicherung", "OwnerID":0, "Count":20 }, { "Name":"electricalwiring", "Name_Localised":"Elektroverkabelung", "OwnerID":0, "Count":21 }, { "Name":"encryptedmemorychip", "Name_Localised":"Verschlüsselter Speicherchip", "OwnerID":0, "Count":10 }, { "Name":"epoxyadhesive", "Name_Localised":"Epoxy-Klebstoff", "OwnerID":0, "Count":45 }, { "Name":"memorychip", "Name_Localised":"Speicherchip", "OwnerID":0, "Count":10 }, { "Name":"metalcoil", "Name_Localised":"Metallspule", "OwnerID":0, "Count":20 }, { "Name":"microhydraulics", "Name_Localised":"Mikrohydraulikgeräte", "OwnerID":0, "Count":10 }, { "Name":"microsupercapacitor", "Name_Localised":"Mikro-Superkondensatoren", "OwnerID":0, "Count":21 }, { "Name":"microthrusters", "Name_Localised":"Mikro-Schubdüsen", "OwnerID":0, "Count":10 }, { "Name":"microtransformer", "Name_Localised":"Mikro-Transformator", "OwnerID":0, "Count":20 }, { "Name":"motor", "OwnerID":0, "Count":20 }, { "Name":"opticalfibre", "Name_Localised":"Glasfaser", "OwnerID":0, "Count":23 }, { "Name":"opticallens", "Name_Localised":"Optische Linse", "OwnerID":0, "Count":9 }, { "Name":"scrambler", "Name_Localised":"Störsender", "OwnerID":0, "Count":10 }, { "Name":"titaniumplating", "Name_Localised":"Titanplatten", "OwnerID":0, "Count":13 }, { "Name":"transmitter", "Name_Localised":"Sender", "OwnerID":0, "Count":6 }, { "Name":"tungstencarbide", "Name_Localised":"Wolframcarbid", "OwnerID":0, "Count":10 }, { "Name":"viscoelasticpolymer", "Name_Localised":"Viskoelastisches Polymer", "OwnerID":0, "Count":31 }, { "Name":"rdx", "Name_Localised":"Hexogen", "OwnerID":0, "Count":25 }, { "Name":"electromagnet", "Name_Localised":"Elektromagnet", "OwnerID":0, "Count":21 }, { "Name":"oxygenicbacteria", "Name_Localised":"Sauerstoffbakterien", "OwnerID":0, "Count":20 }, { "Name":"epinephrine", "Name_Localised":"Epinephrin", "OwnerID":0, "Count":20 }, { "Name":"phneutraliser", "Name_Localised":"pH-Neutralisator", "OwnerID":0, "Count":24 }, { "Name":"microelectrode", "Name_Localised":"Mikroelektrode", "OwnerID":0, "Count":132 }, { "Name":"ionbattery", "Name_Localised":"Ionen-Batterie", "OwnerID":0, "Count":20 }, { "Name":"weaponcomponent", "Name_Localised":"Waffenkomponente", "OwnerID":0, "Count":14 } ], "Consumables":[ { "Name":"healthpack", "Name_Localised":"Medikit", "OwnerID":0, "Count":100 }, { "Name":"energycell", "Name_Localised":"Energie-<br>zelle", "OwnerID":0, "Count":100 }, { "Name":"amm_grenade_emp", "Name_Localised":"Schild-<br>unterbrecher", "OwnerID":0, "Count":100 }, { "Name":"amm_grenade_frag", "Name_Localised":"Splitter-<br>granate", "OwnerID":0, "Count":100 }, { "Name":"amm_grenade_shield", "Name_Localised":"Schild-<br>generator", "OwnerID":0, "Count":100 }, { "Name":"bypass", "Name_Localised":"E-Breach", "OwnerID":0, "Count":82 } ], "Data":[ { "Name":"biometricdata", "Name_Localised":"Biometrische Daten", "OwnerID":0, "Count":4 }, { "Name":"nocdata", "Name_Localised":"Geheimoperation-Daten", "OwnerID":0, "Count":17 }, { "Name":"axcombatlogs", "Name_Localised":"AX-Kampf-Logs", "OwnerID":0, "Count":1 }, { "Name":"airqualityreports", "Name_Localised":"Berichte zur Luftqualität", "OwnerID":0, "Count":18 }, { "Name":"atmosphericdata", "Name_Localised":"Atmosphärische Daten", "OwnerID":0, "Count":19 }, { "Name":"audiologs", "Name_Localised":"Audio-Logs", "OwnerID":0, "Count":3 }, { "Name":"ballisticsdata", "Name_Localised":"Ballistische Daten", "OwnerID":0, "Count":15 }, { "Name":"bloodtestresults", "Name_Localised":"Bluttest-Ergebnisse", "OwnerID":0, "Count":4 }, { "Name":"campaignplans", "Name_Localised":"Kampagnenpläne", "OwnerID":0, "Count":11 }, { "Name":"catmedia", "Name_Localised":"Katzen-Medien", "OwnerID":0, "Count":6 }, { "Name":"censusdata", "Name_Localised":"Zensusdaten", "OwnerID":0, "Count":14 }, { "Name":"chemicalexperimentdata", "Name_Localised":"Chemieexperiment-Daten", "OwnerID":0, "Count":10 }, { "Name":"chemicalformulae", "Name_Localised":"Chemische Formeln", "OwnerID":0, "Count":6 }, { "Name":"chemicalinventory", "Name_Localised":"Chemische Bestandsliste", "OwnerID":0, "Count":11 }, { "Name":"chemicalpatents", "Name_Localised":"Chemische Patente", "OwnerID":0, "Count":22 }, { "Name":"classicentertainment", "Name_Localised":"Klassische Unterhaltung", "OwnerID":0, "Count":8 }, { "Name":"cocktailrecipes", "Name_Localised":"Cocktail-Rezepte", "OwnerID":0, "Count":2 }, { "Name":"combattrainingmaterial", "Name_Localised":"Kampftraining-Materialien", "OwnerID":0, "Count":7 }, { "Name":"combatantperformance", "Name_Localised":"Kampfleistung", "OwnerID":0, "Count":19 }, { "Name":"conflicthistory", "Name_Localised":"Konflikthistorie", "OwnerID":0, "Count":2 }, { "Name":"cropyieldanalysis", "Name_Localised":"Ernteertrag-Analyse", "OwnerID":0, "Count":2 }, { "Name":"culinaryrecipes", "Name_Localised":"Kulinarische Rezepte", "OwnerID":0, "Count":2 }, { "Name":"digitaldesigns", "Name_Localised":"Digitale Konzepte", "OwnerID":0, "Count":11 }, { "Name":"dutyrota", "Name_Localised":"Dienstpläne", "OwnerID":0, "Count":5 }, { "Name":"employeeexpenses", "Name_Localised":"Aufwendungen für Mitarbeiter", "OwnerID":0, "Count":7 }, { "Name":"employeegeneticdata", "Name_Localised":"Gen-Daten von Mitarbeitern", "OwnerID":0, "Count":1 }, { "Name":"employmenthistory", "Name_Localised":"Beschäftigungshistorie", "OwnerID":0, "Count":3 }, { "Name":"evacuationprotocols", "Name_Localised":"Evakuierungsprotokolle", "OwnerID":0, "Count":34 }, { "Name":"explorationjournals", "Name_Localised":"Erkundungstagebücher", "OwnerID":0, "Count":1 }, { "Name":"extractionyielddata", "Name_Localised":"Daten zur Förderungsausbeute", "OwnerID":0, "Count":6 }, { "Name":"factionassociates", "Name_Localised":"Fraktionspartner", "OwnerID":0, "Count":5 }, { "Name":"financialprojections", "Name_Localised":"Finanzprognosen", "OwnerID":0, "Count":6 }, { "Name":"fleetregistry", "Name_Localised":"Flottenliste", "OwnerID":0, "Count":4 }, { "Name":"genesequencingdata", "Name_Localised":"Gen-Sequenzierungsdaten", "OwnerID":0, "Count":4 }, { "Name":"geneticresearch", "Name_Localised":"Genforschung", "OwnerID":0, "Count":6 }, { "Name":"hydroponicdata", "Name_Localised":"Hydrokultur-Daten", "OwnerID":0, "Count":1 }, { "Name":"influenceprojections", "Name_Localised":"Einfluss-Projektionen", "OwnerID":0, "Count":2 }, { "Name":"interrogationrecordings", "Name_Localised":"Vernehmungsaufzeichnungen", "OwnerID":0, "Count":2 }, { "Name":"jobapplications", "Name_Localised":"Bewerbungen", "OwnerID":0, "Count":3 }, { "Name":"literaryfiction", "Name_Localised":"Belletristik", "OwnerID":0, "Count":6 }, { "Name":"maintenancelogs", "Name_Localised":"Wartungs-Logs", "OwnerID":0, "Count":33 }, { "Name":"manufacturinginstructions", "Name_Localised":"Herstellungsanweisungen", "OwnerID":0, "Count":3 }, { "Name":"medicaltrialrecords", "Name_Localised":"Klinische Prüfberichte", "OwnerID":0, "Count":2 }, { "Name":"meetingminutes", "Name_Localised":"Besprechungsprotokolle", "OwnerID":0, "Count":5 }, { "Name":"mineralsurvey", "Name_Localised":"Mineralienübersicht", "OwnerID":0, "Count":8 }, { "Name":"mininganalytics", "Name_Localised":"Abbau-Analytik", "OwnerID":0, "Count":11 }, { "Name":"multimediaentertainment", "Name_Localised":"Multimedia-Unterhaltung", "OwnerID":0, "Count":3 }, { "Name":"networkaccesshistory", "Name_Localised":"Netzwerkzugriffshistorie", "OwnerID":0, "Count":3 }, { "Name":"networksecurityprotocols", "Name_Localised":"Netzwerksicherheitsprotokolle", "OwnerID":0, "Count":1 }, { "Name":"nextofkinrecords", "Name_Localised":"Angehörigen-Aufzeichnungen", "OwnerID":0, "Count":6 }, { "Name":"operationalmanual", "Name_Localised":"Bedienungsanleitung", "OwnerID":0, "Count":32 }, { "Name":"opinionpolls", "Name_Localised":"Umfrageergebnisse", "OwnerID":0, "Count":3 }, { "Name":"patrolroutes", "Name_Localised":"Patrouillenrouten", "OwnerID":3028142, "Count":1 }, { "Name":"patrolroutes", "Name_Localised":"Patrouillenrouten", "OwnerID":0, "Count":12 }, { "Name":"personallogs", "Name_Localised":"Persönliche Logs", "OwnerID":0, "Count":2 }, { "Name":"photoalbums", "Name_Localised":"Fotoalben", "OwnerID":0, "Count":1 }, { "Name":"politicalaffiliations", "Name_Localised":"Politische Beziehungen", "OwnerID":0, "Count":2 }, { "Name":"productionreports", "Name_Localised":"Produktionsberichte", "OwnerID":0, "Count":6 }, { "Name":"productionschedule", "Name_Localised":"Produktionszeitplan", "OwnerID":0, "Count":7 }, { "Name":"purchaserecords", "Name_Localised":"Kaufaufzeichnungen", "OwnerID":0, "Count":1 }, { "Name":"purchaserequests", "Name_Localised":"Kaufanfragen", "OwnerID":0, "Count":7 }, { "Name":"radioactivitydata", "Name_Localised":"Daten zur Radioaktivität", "OwnerID":0, "Count":31 }, { "Name":"reactoroutputreview", "Name_Localised":"Evaluierung der Reaktorleistung", "OwnerID":0, "Count":17 }, { "Name":"recyclinglogs", "Name_Localised":"Recycling-Logs", "OwnerID":0, "Count":22 }, { "Name":"residentialdirectory", "Name_Localised":"Einwohnerverzeichnis", "OwnerID":0, "Count":7 }, { "Name":"riskassessments", "Name_Localised":"Gefährdungsbeurteilungen", "OwnerID":0, "Count":13 }, { "Name":"salesrecords", "Name_Localised":"Verkaufszahlen", "OwnerID":0, "Count":12 }, { "Name":"securityexpenses", "Name_Localised":"Aufwendungen für Sicherheit", "OwnerID":0, "Count":5 }, { "Name":"settlementassaultplans", "Name_Localised":"Siedlungsangriffspläne", "OwnerID":0, "Count":5 }, { "Name":"settlementdefenceplans", "Name_Localised":"Siedlungsverteidigungspläne", "OwnerID":0, "Count":2 }, { "Name":"stellaractivitylogs", "Name_Localised":"Sternenaktivitäts-Logs", "OwnerID":0, "Count":3 }, { "Name":"surveilleancelogs", "Name_Localised":"Überwachungs-Logs", "OwnerID":0, "Count":24 }, { "Name":"tacticalplans", "Name_Localised":"Taktikpläne", "OwnerID":0, "Count":7 }, { "Name":"taxrecords", "Name_Localised":"Steuerunterlagen", "OwnerID":0, "Count":6 }, { "Name":"topographicalsurveys", "Name_Localised":"Topographische Erhebung", "OwnerID":0, "Count":1 }, { "Name":"travelpermits", "Name_Localised":"Reiseerlaubnisse", "OwnerID":0, "Count":4 }, { "Name":"troopdeploymentrecords", "Name_Localised":"Truppeneinsatzprotokolle", "OwnerID":0, "Count":1 }, { "Name":"unionmembership", "Name_Localised":"Gewerkschaftsmitgliedschaft", "OwnerID":0, "Count":7 }, { "Name":"vaccineresearch", "Name_Localised":"Impfstoffforschung", "OwnerID":0, "Count":1 }, { "Name":"vipsecuritydetail", "Name_Localised":"VIP-Sicherheitsdienst", "OwnerID":0, "Count":2 }, { "Name":"virologydata", "Name_Localised":"Virologische Daten", "OwnerID":0, "Count":7 }, { "Name":"visitorregister", "Name_Localised":"Besucherliste", "OwnerID":0, "Count":4 }, { "Name":"weaponinventory", "Name_Localised":"Waffeninventarliste", "OwnerID":0, "Count":10 }, { "Name":"weapontestdata", "Name_Localised":"Waffentestdaten", "OwnerID":0, "Count":8 }, { "Name":"xenodefenceprotocols", "Name_Localised":"Xeno-Abwehr-Protokolle", "OwnerID":0, "Count":2 }, { "Name":"payrollinformation", "Name_Localised":"Gehaltsinformationen", "OwnerID":0, "Count":4 }, { "Name":"geologicaldata", "Name_Localised":"Geologische Daten", "OwnerID":0, "Count":1 }, { "Name":"factiondonatorlist", "Name_Localised":"Spenderliste einer Fraktion", "OwnerID":0, "Count":6 }, { "Name":"pharmaceuticalpatents", "Name_Localised":"Pharmazeutische Patente", "OwnerID":0, "Count":6 } ] }
    }
}