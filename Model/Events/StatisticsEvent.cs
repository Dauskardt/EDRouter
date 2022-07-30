using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class StatisticsEvent:EventBase
    {
        public BankAccountValues Bank_Account { get; set; }
        public CombatValues Combat { get; set; }

        public StatisticsEvent()
        {
            this.Event = "Statistics";
        }
    }

    [Serializable]
    public class BankAccountValues
    {
        public long Current_Wealth { get; set; }
        public long Spent_On_Ships { get; set; }
        public long Spent_On_Outfitting { get; set; }
        public long Spent_On_Repairs { get; set; }
        public long Spent_On_Fuel { get; set; }
        public long Spent_On_Ammo_Consumables { get; set; }
        public int Insurance_Claims { get; set; }
        public long Spent_On_Insurance { get; set; }
        public int Owned_Ship_Count { get; set; }
        public long Spent_On_Suits { get; set; }
        public long Spent_On_Weapons { get; set; }
        public long Spent_On_Suit_Consumables { get; set; }
        public int Suits_Owned { get; set; }
        public int Weapons_Owned { get; set; }
        public long Spent_On_Premium_Stock { get; set; }
        public int Premium_Stock_Bought { get; set; }

    }

    [Serializable]
    public class CombatValues
    {
        public long Bounties_Claimed { get; set; }
        public long Bounty_Hunting_Profit { get; set; }
        public long Combat_Bonds { get; set; }
        public long Combat_Bond_Profits { get; set; }
        public long Assassinations { get; set; }
        public long Assassination_Profits { get; set; }
        public long Highest_Single_Reward { get; set; }
        public long Skimmers_Killed { get; set; }
        public long OnFoot_Combat_Bonds { get; set; }
        public long OnFoot_Combat_Bonds_Profits { get; set; }
        public long OnFoot_Vehicles_Destroyed { get; set; }
        public long OnFoot_Ships_Destroyed { get; set; }
        public long Dropships_Taken { get; set; }
        public long Dropships_Booked { get; set; }
        public long Dropships_Cancelled { get; set; }
        public long ConflictZone_High { get; set; }

        public long ConflictZone_Medium { get; set; }
        public long ConflictZone_Low { get; set; }
        public long ConflictZone_Total { get; set; }

        public long ConflictZone_High_Wins { get; set; }
        public long ConflictZone_Medium_Wins { get; set; }
        public long ConflictZone_Low_Wins { get; set; }
        public long ConflictZone_Total_Wins { get; set; }
        public long Settlement_Defended { get; set; }
        public long Settlement_Conquered { get; set; }
        public long OnFoot_Skimmers_Killed { get; set; }
        public long OnFoot_Scavs_Killed { get; set; }
    }
}
