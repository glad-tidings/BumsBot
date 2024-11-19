using System.Text.Json.Serialization;

namespace BumsBot
{
    public class BumsBotQuery
    {
        [JsonPropertyName("Index")]
        public int Index { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("API_ID")]
        public string API_ID { get; set; } = string.Empty;
        [JsonPropertyName("API_HASH")]
        public string API_HASH { get; set; } = string.Empty;
        [JsonPropertyName("Phone")]
        public string Phone { get; set; } = string.Empty;
        public string Auth { get; set; } = string.Empty;
        [JsonPropertyName("Active")]
        public bool Active { get; set; }
        [JsonPropertyName("DailyReward")]
        public bool DailyReward { get; set; }
        [JsonPropertyName("FriendBonus")]
        public bool FriendBonus { get; set; }
        [JsonPropertyName("Tap")]
        public bool Tap { get; set; }
        [JsonPropertyName("TapSleep")]
        public int[] TapSleep { get; set; } = [];
        [JsonPropertyName("Task")]
        public bool Task { get; set; }
        [JsonPropertyName("TaskSleep")]
        public int[] TaskSleep { get; set; } = [];
        [JsonPropertyName("Lottery")]
        public bool Lottery { get; set; }
        [JsonPropertyName("Spin")]
        public bool Spin { get; set; }
        [JsonPropertyName("SpinSleep")]
        public int[] SpinSleep { get; set; } = [];
        [JsonPropertyName("UpgradeLevel")]
        public bool UpgradeLevel { get; set; }
        [JsonPropertyName("UpgradeLevelSleep")]
        public int[] UpgradeLevelSleep { get; set; } = [];
        [JsonPropertyName("UpgradeMine")]
        public bool UpgradeMine { get; set; }
        [JsonPropertyName("UpgradeMineSleep")]
        public int[] UpgradeMineSleep { get; set; } = [];
        [JsonPropertyName("UpgradeLevels")]
        public string[] UpgradeLevels { get; set; } = [];
        [JsonPropertyName("DaySleep")]
        public int[] DaySleep { get; set; } = [];
        [JsonPropertyName("NightSleep")]
        public int[] NightSleep { get; set; } = [];
    }

    public class BumsLoginResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public BumsLoginData Data { get; set; } = new();
    }

    public class BumsLoginData
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
    }

    public class BumsGameInfoResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public BumsGameInfoData Data { get; set; } = new();
    }

    public class BumsGameInfoData
    {
        [JsonPropertyName("userInfo")]
        public BumsGameInfoDataUserInfo UserInfo { get; set; } = new();
        [JsonPropertyName("gameInfo")]
        public BumsGameInfoDataGameInfo GameInfo { get; set; } = new();
        [JsonPropertyName("tapInfo")]
        public BumsGameInfoDataTapInfo TapInfo { get; set; } = new();
        [JsonPropertyName("mineInfo")]
        public BumsGameInfoDataMineInfo MineInfo { get; set; } = new();
        [JsonPropertyName("propInfo")]
        public List<BumsGameInfoDataPropInfo> PropInfo { get; set; } = new();
    }

    public class BumsGameInfoDataUserInfo
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; } = string.Empty;
        [JsonPropertyName("avatarId")]
        public int AvatarId { get; set; }
        [JsonPropertyName("nickName")]
        public string NickName { get; set; } = string.Empty;
        [JsonPropertyName("daysInGame")]
        public int DaysInGame { get; set; }
        [JsonPropertyName("invitedFriendsCount")]
        public int InvitedFriendsCount { get; set; }
        [JsonPropertyName("animalInvitedFriendsCount")]
        public int AnimalInvitedFriendsCount { get; set; }
        [JsonPropertyName("imprpvesCount")]
        public int ImprpvesCount { get; set; }
    }

    public class BumsGameInfoDataGameInfo
    {
        [JsonPropertyName("skinId")]
        public long SkinId { get; set; }
        [JsonPropertyName("experience")]
        public string Experience { get; set; } = string.Empty;
        [JsonPropertyName("coin")]
        public int Coin { get; set; }
        [JsonPropertyName("energySurplus")]
        public int EnergySurplus { get; set; }
        [JsonPropertyName("level")]
        public int Level { get; set; }
        [JsonPropertyName("levelExperience")]
        public string LevelExperience { get; set; } = string.Empty;
        [JsonPropertyName("nextUpgradeLevel")]
        public int NextUpgradeLevel { get; set; }
        [JsonPropertyName("nextExperience")]
        public string NextExperience { get; set; } = string.Empty;
        [JsonPropertyName("todayCollegeCoin")]
        public int TodayCollegeCoin { get; set; }
        [JsonPropertyName("todayMaxCollegeCoin")]
        public int TodayMaxCollegeCoin { get; set; }
    }

    public class BumsGameInfoDataTapInfo
    {
        [JsonPropertyName("energy")]
        public BumsGameInfoDataTapInfoSub Energy { get; set; } = new();
        [JsonPropertyName("recovery")]
        public BumsGameInfoDataTapInfoSub Recovery { get; set; } = new();
        [JsonPropertyName("tap")]
        public BumsGameInfoDataTapInfoSub Tap { get; set; }
        [JsonPropertyName("bonusChance")]
        public BumsGameInfoDataTapInfoSub BonusChance { get; set; } = new();
        [JsonPropertyName("bonusRatio")]
        public BumsGameInfoDataTapInfoSub BonusRatio { get; set; } = new();
        [JsonPropertyName("collectInfo")]
        public BumsGameInfoDataTapInfoCollectInfo CollectInfo { get; set; } = new();
        [JsonPropertyName("autoCollectCoin")]
        public string AutoCollectCoin { get; set; } = string.Empty;
    }

    public class BumsGameInfoDataTapInfoSub
    {
        [JsonPropertyName("level")]
        public int Level { get; set; }
        [JsonPropertyName("value")]
        public int Value { get; set; }
        [JsonPropertyName("nextCostCoin")]
        public int NextCostCoin { get; set; }
        [JsonPropertyName("nextIncrease")]
        public int NextIncrease { get; set; }
    }

    public class BumsGameInfoDataTapInfoCollectInfo
    {
        [JsonPropertyName("collectSeqNo")]
        public int CollectSeqNo { get; set; }
        [JsonPropertyName("collectTime")]
        public int CollectTime { get; set; }
    }

    public class BumsGameInfoDataMineInfo
    {
        [JsonPropertyName("minePower")]
        public int MinePower { get; set; }
        [JsonPropertyName("mineOfflineCoin")]
        public int MineOfflineCoin { get; set; }
    }

    public class BumsGameInfoDataPropInfo
    {
        [JsonPropertyName("prop_id")]
        public long PropId { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("affect")]
        public string Affect { get; set; } = string.Empty;
        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;
        [JsonPropertyName("ratio")]
        public double Ratio { get; set; }
        [JsonPropertyName("times")]
        public long Times { get; set; }
    }

    public class BumsPubResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;
    }

    public class BumsTaskResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public BumsTaskData Data { get; set; } = new();
    }

    public class BumsTaskData
    {
        [JsonPropertyName("lists")]
        public List<BumsTaskDataList> Lists { get; set; } = [];
        [JsonPropertyName("newCount")]
        public int NewCount { get; set; }
        [JsonPropertyName("notFinishCount")]
        public int NotFinishCount { get; set; }
    }

    public class BumsTaskDataList
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;
        [JsonPropertyName("taskType")]
        public string TaskType { get; set; } = string.Empty;
        [JsonPropertyName("qualify")]
        public int Qualify { get; set; }
        [JsonPropertyName("isFinish")]
        public int IsFinish { get; set; }
    }

    public class BumsAnswersResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("pwd")]
        public string Pwd { get; set; } = string.Empty;
    }

    public class BumsMineResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public BumsMineData Data { get; set; } = new();
    }

    public class BumsMineData
    {
        [JsonPropertyName("lists")]
        public List<BumsMineDataList> Lists { get; set; } = [];
        [JsonPropertyName("invite")]
        public int Invite { get; set; }
    }

    public class BumsMineDataList
    {
        [JsonPropertyName("mineId")]
        public int MineId { get; set; }
        [JsonPropertyName("level")]
        public int Level { get; set; }
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("nextLevelCost")]
        public long NextLevelCost { get; set; }
        [JsonPropertyName("nextPerHourReward")]
        public string NextPerHourReward { get; set; } = string.Empty;
        [JsonPropertyName("limitText")]
        public string LimitText { get; set; } = string.Empty;
    }

    public class BumsSpinResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public List<BumsSpinData> Data { get; set; } = [];
    }

    public class BumsSpinData
    {
        [JsonPropertyName("propId")]
        public long PropId { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("isBuy")]
        public bool IsBuy { get; set; }
        [JsonPropertyName("toDayUse")]
        public bool ToDayUse { get; set; }
        [JsonPropertyName("isAllowBuy")]
        public bool IsAllowBuy { get; set; }
    }

    public class BumsLotteryAnswerResponse
    {
        [JsonPropertyName("expire")]
        public DateTime Expire { get; set; }
        [JsonPropertyName("cardIdStr")]
        public string CardIdStr { get; set; } = string.Empty;
    }

    public class BumsLotteryResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public BumsLotteryData Data { get; set; } = new();
    }

    public class BumsLotteryData
    {
        [JsonPropertyName("downTime")]
        public int DownTime { get; set; }
        [JsonPropertyName("perHourPower")]
        public string PerHourPower { get; set; } = string.Empty;
        [JsonPropertyName("totalEarn")]
        public string TotalEarn { get; set; } = string.Empty;
        [JsonPropertyName("rewardNum")]
        public string RewardNum { get; set; } = string.Empty;
        [JsonPropertyName("cardNumber")]
        public string CardNumber { get; set; } = string.Empty;
        [JsonPropertyName("resultNum")]
        public int ResultNum { get; set; }
    }

    public class BumsJoinLotteryResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public BumsJoinLotteryData Data { get; set; } = new();
    }

    public class BumsJoinLotteryData
    {
        [JsonPropertyName("cardIdStr")]
        public string CardIdStr { get; set; } = string.Empty;
        [JsonPropertyName("resultNum")]
        public int ResultNum { get; set; }
        [JsonPropertyName("status")]
        public int Status { get; set; }
    }

    public class BumsBalanceResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("msg")]
        public string Msg { get; set; } = string.Empty;
        [JsonPropertyName("data")]
        public BumsBalanceData Data { get; set; } = new();
    }

    public class BumsBalanceData
    {
        [JsonPropertyName("lists")]
        public List<BumsBalanceDataList> Lists { get; set; } = [];
    }

    public class BumsBalanceDataList
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        [JsonPropertyName("freezeAmount")]
        public int FreezeAmount { get; set; }
        [JsonPropertyName("availableAmount")]
        public int AvailableAmount { get; set; }
    }
}

public class BumsGangResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("msg")]
    public string Msg { get; set; } = string.Empty;
    [JsonPropertyName("data")]
    public BumsGangData? Data { get; set; }
}

public class BumsGangData
{
    [JsonPropertyName("myGang")]
    public BumsGangDataMyGang? MyGang { get; set; }
}

public class BumsGangDataMyGang
{
    [JsonPropertyName("gangId")]
    public string GangId { get; set; } = string.Empty;
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("userCount")]
    public int UserCount { get; set; }
    [JsonPropertyName("power")]
    public string Power { get; set; } = string.Empty;
    [JsonPropertyName("boost")]
    public int Boost { get; set; }
    [JsonPropertyName("rank")]
    public int Rank { get; set; }
}

public class BumsMysteryBoxResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("msg")]
    public string Msg { get; set; }
    [JsonPropertyName("data")]
    public List<BumsMysteryBoxData>? Data { get; set; }
}

public class BumsMysteryBoxData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    [JsonPropertyName("propId")]
    public int PropId { get; set; }
    [JsonPropertyName("sellLists")]
    public List<BumsMysteryBoxDataSellLists>? SellLists { get; set; }
    [JsonPropertyName("isBuy")]
    public bool IsBuy { get; set; }
    [JsonPropertyName("toDayUse")]
    public bool ToDayUse { get; set; }
    [JsonPropertyName("toDayMaxUseNum")]
    public int ToDayMaxUseNum { get; set; }
    [JsonPropertyName("toDayNowUseNum")]
    public int ToDayNowUseNum { get; set; }
    [JsonPropertyName("isAllowBuy")]
    public bool IsAllowBuy { get; set; }
}

public class BumsMysteryBoxDataSellLists
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("payMethod")]
    public int PayMethod { get; set; }
    [JsonPropertyName("oldAmount")]
    public double OldAmount { get; set; }
    [JsonPropertyName("newAmount")]
    public double NewAmount { get; set; }
}

public class ProxyType
{
    [JsonPropertyName("Index")]
    public int Index { get; set; }
    [JsonPropertyName("Proxy")]
    public string Proxy { get; set; } = string.Empty;
}

public class Httpbin
{
    [JsonPropertyName("origin")]
    public string Origin { get; set; } = string.Empty;
}