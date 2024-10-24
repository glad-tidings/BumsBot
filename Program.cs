﻿using System.Text.Json;

namespace BumsBot
{
    static class Program
    {
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

        static void Main(string[] args)
        {
            Console.WriteLine("----------------------- Bums Bot Starting -----------------------");
            Console.WriteLine();

            var BumsQueries = LoadQuery();

            foreach (var Query in BumsQueries ?? [])
            {
                var BotThread = new Thread(() => BumsThread(Query)); BotThread.Start();
                Thread.Sleep(60000);
            }
        }

        public async static void BumsThread(BumsBotQuery Query)
        {
            while (true)
            {
                var RND = new Random();

                var Bot = new BumsBots(Query);
                if (!Bot.HasError)
                {
                    Query = Bot.PubQuery;
                    Log.Show("Bums", Query.Name, $"login successfully.", ConsoleColor.Green);
                    var Sync = await Bot.BumsGameInfo();
                    if (Sync is not null)
                    {
                        Log.Show("Bums", Query.Name, $"synced successfully. B<{Convert.ToInt32(Sync.Data.GameInfo.Coin)}> L<{Convert.ToInt32(Sync.Data.GameInfo.Level)}> E<{Convert.ToInt32(Sync.Data.GameInfo.EnergySurplus)}> P<{Convert.ToInt32(Sync.Data.MineInfo.MinePower)}> T<{Convert.ToInt32(Sync.Data.GameInfo.Experience)}>", ConsoleColor.Blue);
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
                            if (friends is not null)
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
                                    taps = Sync.Data.GameInfo.EnergySurplus / Sync.Data.TapInfo.Tap.Value;

                                bool tap = await Bot.BumsCollectCoin(Sync.Data.TapInfo.CollectInfo.CollectSeqNo + 1, taps * Sync.Data.TapInfo.Tap.Value);
                                Sync = await Bot.BumsGameInfo();
                                if (tap)
                                    Log.Show("Bums", Query.Name, $"'{taps}' taps completed. '{Sync?.Data.GameInfo.EnergySurplus}' energy remaining", ConsoleColor.Green);
                                else
                                    Log.Show("Bums", Query.Name, $"tap failed", ConsoleColor.Red);

                                int eachtapRND = RND.Next(Query.TapSleep[0], Query.TapSleep[1]);
                                Thread.Sleep(eachtapRND * 1000);
                            }
                        }

                        if (Query.Task)
                        {
                            var tasks = await Bot.BumsTasks();
                            if (tasks is not null)
                            {
                                foreach (var task in tasks.Data.Lists.Where(x => x.TaskType == "normal" & x.IsFinish == 0 & x.Id != 38 & x.Name != "Boost channel"))
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
                                if (taskAnswers is not null)
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
                            if (lottery is not null)
                            {
                                if (lottery.Data.ResultNum > 0)
                                {
                                    var lotteryAnswer = await Bot.BumsLotteryAnswer();
                                    if (lotteryAnswer is not null)
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
                            var spins = await Bot.BumsSpins();
                            if (spins is not null)
                            {
                                foreach (var spin in spins.Data.Where(x => x.PropId == 500010001L & x.IsAllowBuy == true & x.ToDayUse == false))
                                {
                                    bool startSpin = await Bot.BumsStartSpin((int)spin.PropId);
                                    if (startSpin)
                                        Log.Show("Bums", Query.Name, $"spin '{spin.Title}' started", ConsoleColor.Green);
                                    else
                                        Log.Show("Bums", Query.Name, $"start spin '{spin.Title}' failed", ConsoleColor.Red);

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
                            if (mines is not null)
                            {
                                foreach (var mine in mines.Data.Lists.Where(x => x.Status == 1).OrderBy(x => x.NextLevelCost))
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
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                        Log.Show("Bums", Query.Name, $"synced failed", ConsoleColor.Red);
                }
                else
                    Log.Show("Bums", Query.Name, $"{Bot.ErrorMessage}", ConsoleColor.Red);

                int syncRND = 0;
                if (DateTime.Now.Hour < 8)
                    syncRND = RND.Next(Query.NightSleep[0], Query.NightSleep[1]);
                else
                    syncRND = RND.Next(Query.DaySleep[0], Query.DaySleep[1]);
                Log.Show("Blum", Query.Name, $"sync sleep '{Convert.ToInt32(syncRND / 3600d)}h {Convert.ToInt32(syncRND % 3600 / 60d)}m {syncRND % 60}s'", ConsoleColor.Yellow);
                Thread.Sleep(syncRND * 1000);
            }
        }
    }
}