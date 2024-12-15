using System.Text.Json;

namespace BumsBot
{
    static class Program
    {
        private static ProxyType[]? proxies;
        static List<BumsBotQuery>? LoadQuery()
        {
            try
            {
                var contents = File.ReadAllText(@"data.txt");
                return JsonSerializer.Deserialize<List<BumsBotQuery>>(contents);
            }
            catch { }

            return null;
        }

        static ProxyType[]? LoadProxy()
        {
            try
            {
                var contents = File.ReadAllText(@"proxy.txt");
                return JsonSerializer.Deserialize<ProxyType[]>(contents);
            }
            catch { }

            return null;
        }

        static void Main()
        {
            Console.WriteLine("  ____                      ____   ___ _____ \r\n | __ ) _   _ _ __ ___  ___| __ ) / _ \\_   _|\r\n |  _ \\| | | | '_ ` _ \\/ __|  _ \\| | | || |  \r\n | |_) | |_| | | | | | \\__ \\ |_) | |_| || |  \r\n |____/ \\__,_|_| |_| |_|___/____/ \\___/ |_|  \r\n                                             ");
            Console.WriteLine();
            Console.WriteLine("Github: https://github.com/glad-tidings/BumsBot");
            Console.WriteLine();
            Console.Write("Select an option:\n1. Run bot\n2. Create session\n> ");
            string? opt = Console.ReadLine();

            var BumsQueries = LoadQuery();
            proxies = LoadProxy();

            if (opt != null)
            {
                if (opt == "1")
                {
                    foreach (var Query in BumsQueries ?? [])
                    {
                        var BotThread = new Thread(() => BumsThread(Query)); BotThread.Start();
                        Thread.Sleep(60000);
                    }
                }
                else
                {
                    foreach (var Query in BumsQueries ?? [])
                    {
                        if (!File.Exists(@$"sessions\{Query.Name}.session"))
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Create session for account {Query.Name} ({Query.Phone})");
                            TelegramMiniApp.WebView vw = new(Query.API_ID, Query.API_HASH, Query.Name, Query.Phone, "", "");
                            if (vw.Save_Session().Result)
                                Console.WriteLine("Session created");
                            else
                                Console.WriteLine("Create session failed");
                        }
                    }

                    Environment.Exit(0);
                }
            }

            Console.ReadLine();
        }

        public async static void BumsThread(BumsBotQuery Query)
        {
            while (true)
            {
                var RND = new Random();

                try
                {
                    var Bot = new BumsBots(Query, proxies ?? []);
                    if (!Bot.HasError)
                    {
                        Log.Show("Bums", Query.Name, $"my ip '{Bot.IPAddress}'", ConsoleColor.White);
                        Log.Show("Bums", Query.Name, $"login successfully.", ConsoleColor.Green);
                        var Sync = await Bot.BumsGameInfo();
                        if (Sync != null)
                        {
                            Log.Show("Bums", Query.Name, $"synced successfully. B<{Sync.Data.GameInfo.Coin}> L<{Sync.Data.GameInfo.Level}> E<{Sync.Data.GameInfo.EnergySurplus}> P<{Sync.Data.MineInfo.MinePower}> T<{Sync.Data.GameInfo.Experience}>", ConsoleColor.Blue);

                            var gang = await Bot.BumsGangs();
                            if (gang != null)
                            {
                                if (gang.Data?.MyGang?.GangId != "1854507053120028672")
                                {
                                    if (!string.IsNullOrEmpty(gang.Data?.MyGang?.GangId))
                                    {
                                        bool leaveGang = await Bot.BumsLeaveGang();
                                        if (leaveGang)
                                        {
                                            bool joinGang = await Bot.BumsJoinGang();
                                            if (joinGang)
                                                Log.Show("Bums", Query.Name, $"join gang successfully", ConsoleColor.Green);
                                            else
                                                Log.Show("Bums", Query.Name, $"join gang failed", ConsoleColor.Red);
                                        }
                                    }
                                    else
                                    {
                                        bool joinGang = await Bot.BumsJoinGang();
                                        if (joinGang)
                                            Log.Show("Bums", Query.Name, $"join gang successfully", ConsoleColor.Green);
                                        else
                                            Log.Show("Bums", Query.Name, $"join gang failed", ConsoleColor.Red);
                                    }

                                    Thread.Sleep(3000);
                                }
                            }

                            if (Query.DailyReward)
                            {
                                bool reward = await Bot.BumsDailyReward();
                                if (reward)
                                    Log.Show("Bums", Query.Name, $"daily reward claimed", ConsoleColor.Green);

                                Thread.Sleep(3000);
                            }

                            if (Query.FriendBonus)
                            {
                                var friends = await Bot.BumsBalance();
                                if (friends != null)
                                {
                                    var w7001 = friends.Data.Lists.Where(x => x.Id == 70001);
                                    if (w7001.Count() != 0)
                                    {
                                        if (w7001.ElementAtOrDefault(0)?.AvailableAmount > 0)
                                        {
                                            bool claimFriend = await Bot.BumsW70001To80001();
                                            if (claimFriend)
                                                Log.Show("Bums", Query.Name, $"friends bonus claimed", ConsoleColor.Green);
                                            else
                                                Log.Show("Bums", Query.Name, $"claim friends bonus failed", ConsoleColor.Red);
                                        }
                                    }
                                }

                                Thread.Sleep(3000);
                            }

                            if (Query.Tap)
                            {
                                while (Sync?.Data.GameInfo.EnergySurplus > Sync?.Data.TapInfo.Energy.Value / 10d)
                                {
                                    int taps = RND.Next(20, 50);
                                    if (taps > Sync.Data.GameInfo.EnergySurplus / Sync.Data.TapInfo.Tap.Value)
                                        taps = (int)Math.Round((decimal)Sync.Data.GameInfo.EnergySurplus / (decimal)Sync.Data.TapInfo.Tap.Value);

                                    bool tap = await Bot.BumsCollectCoin(Sync.Data.TapInfo.CollectInfo.CollectSeqNo + 1, taps * Sync.Data.TapInfo.Tap.Value);
                                    Sync = await Bot.BumsGameInfo();
                                    if (tap)
                                        Log.Show("Bums", Query.Name, $"'{taps}' taps completed. '{Sync.Data.GameInfo.EnergySurplus}' energy remaining", ConsoleColor.Green);
                                    else
                                        Log.Show("Bums", Query.Name, $"tap failed", ConsoleColor.Red);

                                    int eachtapRND = RND.Next(Query.TapSleep[0], Query.TapSleep[1]);
                                    Thread.Sleep(eachtapRND * 1000);
                                }
                            }

                            if (Query.Task)
                            {
                                var tasks = await Bot.BumsTasks();
                                if (tasks != null)
                                {
                                    foreach (var task in tasks.Data.Lists.Where(x => x.TaskType == "normal" & x.IsFinish == 0 & x.Id != 38 & x.Name != "Boost channel" & x.Name != "Score $BALLS in PiggyBank" & x.Name != "Dunk and earn an Airdrop!"))
                                    {
                                        bool finishTask = await Bot.BumsFinishTask(task.Id, "");
                                        if (finishTask)
                                            Log.Show("Bums", Query.Name, $"task '{task.Name}' finished", ConsoleColor.Green);
                                        else
                                            Log.Show("Bums", Query.Name, $"finish task '{task.Name}' failed", ConsoleColor.Red);

                                        int eachtaskRND = RND.Next(Query.TaskSleep[0], Query.TaskSleep[1]);
                                        Thread.Sleep(eachtaskRND * 1000);
                                    }

                                    var taskAnswers = await Bot.BumsAnswers();
                                    if (taskAnswers != null)
                                    {
                                        foreach (var task in tasks.Data.Lists.Where(x => x.TaskType == "pwd" & x.IsFinish == 0))
                                        {
                                            var answer = taskAnswers.Where(x => (x.Name ?? "") == (task.Name ?? ""));
                                            if (answer.Count() != 0)
                                            {
                                                bool finishTask = await Bot.BumsFinishTask(task.Id, answer.ElementAtOrDefault(0)?.Pwd ?? string.Empty);
                                                if (finishTask)
                                                    Log.Show("Bums", Query.Name, $"task '{task.Name}' finished", ConsoleColor.Green);
                                                else
                                                    Log.Show("Bums", Query.Name, $"finish task '{task.Name}' failed", ConsoleColor.Red);

                                                int eachtaskRND = RND.Next(Query.TaskSleep[0], Query.TaskSleep[1]);
                                                Thread.Sleep(eachtaskRND * 1000);
                                            }
                                        }
                                    }
                                }
                            }

                            if (Query.Lottery)
                            {
                                var lottery = await Bot.BumsLottery();
                                if (lottery != null)
                                {
                                    if (lottery.Data.ResultNum > 0)
                                    {
                                        var lotteryAnswer = await Bot.BumsLotteryAnswer();
                                        if (lotteryAnswer != null)
                                        {
                                            if (lotteryAnswer.Expire.ToLocalTime() > DateTime.Now)
                                            {
                                                bool joinLottery = await Bot.BumsJoinLottery(lotteryAnswer.CardIdStr);
                                                if (joinLottery)
                                                    Log.Show("Bums", Query.Name, $"daily lottery claimed", ConsoleColor.Green);
                                                else
                                                    Log.Show("Bums", Query.Name, $"claim daily lottery failed", ConsoleColor.Red);
                                            }
                                        }
                                    }
                                }

                                Thread.Sleep(3000);
                            }

                            if (Query.Spin)
                            {
                                var spins = await Bot.BumsPropShop("spin");
                                if (spins != null)
                                {
                                    foreach (var spin in spins.Data.Where(x => x.PropId == 500010001 & x.IsBuy == false & x.IsAllowBuy == true & x.ToDayUse == false & x.ToDayMaxUseNum > x.ToDayNowUseNum))
                                    {
                                        bool startSpin = await Bot.BumsStartSpin(spin.PropId);
                                        if (startSpin)
                                            Log.Show("Bums", Query.Name, $"spin '{spin.Title}' started", ConsoleColor.Green);
                                        else
                                            Log.Show("Bums", Query.Name, $"start spin '{spin.Title}' failed", ConsoleColor.Red);

                                        int eachspinRND = RND.Next(Query.SpinSleep[0], Query.SpinSleep[1]);
                                        Thread.Sleep(eachspinRND * 1000);
                                    }
                                }

                                Thread.Sleep(3000);

                                var expedition = await Bot.BumsPropShop("expedition");
                                if (expedition is not null)
                                {
                                    foreach (var spin in spins.Data.Where(x => x.PropId == 200010001 & x.IsBuy == false & x.IsAllowBuy == true & x.ToDayUse == false & x.ToDayMaxUseNum > x.ToDayNowUseNum))
                                    {
                                        bool startSpin = await Bot.BumsStartSpin(spin.PropId);
                                        if (startSpin)
                                            Log.Show("Bums", Query.Name, $"expedition '{spin.Title}' started", ConsoleColor.Green);
                                        else
                                            Log.Show("Bums", Query.Name, $"start expedition '{spin.Title}' failed", ConsoleColor.Red);

                                        int eachspinRND = RND.Next(Query.SpinSleep[0], Query.SpinSleep[1]);
                                        Thread.Sleep(eachspinRND * 1000);
                                    }
                                }
                            }

                            if (Query.UpgradeLevel)
                            {
                                foreach (var level in Query.UpgradeLevels)
                                {
                                    Sync = await Bot.BumsGameInfo();
                                    bool canUpg = false;
                                    switch (level ?? "")
                                    {
                                        case "bonusChance":
                                            {
                                                if (Sync?.Data.GameInfo.Coin > Sync?.Data.TapInfo.BonusChance.NextCostCoin)
                                                    canUpg = true;
                                                break;
                                            }
                                        case "bonusRatio":
                                            {
                                                if (Sync?.Data.GameInfo.Coin > Sync?.Data.TapInfo.BonusRatio.NextCostCoin)
                                                    canUpg = true;
                                                break;
                                            }
                                        case "energy":
                                            {
                                                if (Sync?.Data.GameInfo.Coin > Sync?.Data.TapInfo.Energy.NextCostCoin)
                                                    canUpg = true;
                                                break;
                                            }
                                        case "tap":
                                            {
                                                if (Sync?.Data.GameInfo.Coin > Sync?.Data.TapInfo.Tap.NextCostCoin)
                                                    canUpg = true;
                                                break;
                                            }
                                        case "recovery":
                                            {
                                                if (Sync?.Data.GameInfo.Coin > Sync?.Data.TapInfo.Recovery.NextCostCoin)
                                                    canUpg = true;
                                                break;
                                            }
                                    }
                                    if (canUpg)
                                    {
                                        bool upgradeLevel = await Bot.BumsUpgradeLevel(level ?? string.Empty);
                                        if (upgradeLevel)
                                            Log.Show("Bums", Query.Name, $"'{level}' upgraded", ConsoleColor.Green);
                                        else
                                            Log.Show("Bums", Query.Name, $"upgrade '{level}' failed", ConsoleColor.Red);

                                        int eachupgradeRND = RND.Next(Query.UpgradeLevelSleep[0], Query.UpgradeLevelSleep[1]);
                                        Thread.Sleep(eachupgradeRND * 1000);
                                    }
                                }
                            }

                            if (Query.UpgradeMine)
                            {
                                var mines = await Bot.BumsMines();
                                if (mines != null)
                                {
                                    foreach (var mine in mines.Data.Lists.Where(x => x.Status == 1 & x.NextLevelCost < Sync?.Data.GameInfo.Coin / 50d).OrderBy(x => x.NextLevelCost))
                                    {
                                        Sync = await Bot.BumsGameInfo();
                                        if (Sync?.Data.GameInfo.Coin > mine.NextLevelCost)
                                        {
                                            bool upgradeMine = await Bot.BumsUpgradeMine(mine.MineId);
                                            if (upgradeMine)
                                                Log.Show("Bums", Query.Name, $"miner '{mine.MineId}' upgraded", ConsoleColor.Green);

                                            int eachupgradeRND = RND.Next(Query.UpgradeMineSleep[0], Query.UpgradeMineSleep[1]);
                                            Thread.Sleep(eachupgradeRND * 1000);
                                        }
                                        else
                                            break;
                                    }
                                }
                            }
                        }
                        else
                            Log.Show("Bums", Query.Name, $"synced failed", ConsoleColor.Red);

                        Sync = await Bot.BumsGameInfo();
                        if (Sync != null)
                            Log.Show("Bums", Query.Name, $"B<{Sync.Data.GameInfo.Coin}> L<{Sync.Data.GameInfo.Level}> E<{Sync.Data.GameInfo.EnergySurplus}> P<{Sync.Data.MineInfo.MinePower}> T<{Sync.Data.GameInfo.Experience}>", ConsoleColor.Blue);
                    }
                    else
                        Log.Show("Bums", Query.Name, $"{Bot.ErrorMessage}", ConsoleColor.Red);
                }
                catch (Exception ex)
                {
                    Log.Show("Bums", Query.Name, $"Error: {ex.Message}", ConsoleColor.Red);
                }

                int syncRND = 0;
                if (DateTime.Now.Hour < 8)
                    syncRND = RND.Next(Query.NightSleep[0], Query.NightSleep[1]);
                else
                    syncRND = RND.Next(Query.DaySleep[0], Query.DaySleep[1]);
                Log.Show("Bums", Query.Name, $"sync sleep '{Convert.ToInt32(syncRND / 3600d)}h {Convert.ToInt32(syncRND % 3600 / 60d)}m {syncRND % 60}s'", ConsoleColor.Yellow);
                Thread.Sleep(syncRND * 1000);
            }
        }
    }
}