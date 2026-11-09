using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Hamdoku {
    public sealed class SaveModule {
        public PlayerSaveModel Load(GameConfigSO config, string directory) {
            string path = GetPath(config, directory);
            if (!File.Exists(path)) {
                return null; // A first installation has no progress yet.
            }
            PlayerSaveModel data;
            var serializer = new DataContractJsonSerializer(typeof(PlayerSaveModel));
            try {
                using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(File.ReadAllText(path)))) {
                    data = (PlayerSaveModel)serializer.ReadObject(stream);
                }
            } catch (SerializationException error) {
                throw new InvalidDataException("Invalid Hamdoku progress JSON: " + path, error);
            }
            if (data == null || (data.version != config.saveVersion &&
                !(config.saveVersion == 2 && data.version == 1) &&
                !(config.saveVersion == 3 && (data.version == 1 || data.version == 2)) &&
                !(config.saveVersion == 4 && data.version >= 1 && data.version <= 3) &&
                !(config.saveVersion == 5 && data.version >= 1 && data.version <= 4) &&
                !(config.saveVersion == 6 && data.version >= 1 && data.version <= 5) &&
                !(config.saveVersion == 7 && data.version >= 1 && data.version <= 6) &&
                !(config.saveVersion == 8 && data.version >= 1 && data.version <= 7) &&
                !(config.saveVersion == 9 && data.version >= 1 && data.version <= 8) &&
                !(config.saveVersion == 10 && data.version >= 1 && data.version <= 9) &&
                !(config.saveVersion == 11 && data.version >= 1 && data.version <= 10) &&
                !(config.saveVersion == 12 && data.version >= 1 && data.version <= 11) &&
                !(config.saveVersion == 13 && data.version >= 1 && data.version <= 12)) ||
                data.campaignClears < 0 || data.dailyStreak < 0 || data.previousPuzzleId < 0) {
                throw new InvalidDataException("Invalid or unsupported Hamdoku progress: " + path);
            }
            if (data.version >= 13) {
                ValidateEndless(data.endless);
            }
            if (data.version >= 10) {
                ValidateDaily(data.daily);
            }
            if (data.version >= 3) {
                ValidateStreak(data.streak);
            }
            if (data.version >= 8) {
                ValidateCampaign(data.campaign);
                if (data.version >= 12) {
                    ValidateCampaignCycles(data.campaign);
                }
                if (data.campaign.claimedRewardLevel > data.campaignClears || data.campaign.previousLevel > data.campaignClears) {
                    throw new InvalidDataException("Campaign reward or difficulty history exceeds completed progress.");
                }
            }
            if (data.version >= 6) {
                ValidateSettings(data.settings);
            }
            if (data.version >= 5) {
                ValidateProfile(data.profile);
                if (data.profile.trackedThroughLevel != data.campaignClears || data.profile.retriedLevel > data.campaignClears + 1) {
                    throw new InvalidDataException("Profile and campaign progress disagree.");
                }
            }
            if (data.version >= 4) {
                ValidateRanking(data.ranking, data.campaignClears);
            }
            return data;
        }

        public void Save(GameConfigSO config, string directory, PlayerSaveModel data) {
            string path = GetPath(config, directory);
            if (data.version != config.saveVersion || data.campaignClears < 0 || data.dailyStreak < 0 || data.previousPuzzleId < 0) {
                throw new InvalidDataException("Invalid Hamdoku progress.");
            }
            if (data.version >= 8) {
                ValidateCampaign(data.campaign);
                if (data.version >= 12) {
                    ValidateCampaignCycles(data.campaign);
                }
                if (data.campaign.claimedRewardLevel > data.campaignClears || data.campaign.previousLevel > data.campaignClears) {
                    throw new InvalidDataException("Campaign reward or difficulty history exceeds completed progress.");
                }
            }
            if (data.version >= 13) {
                ValidateEndless(data.endless);
            }
            if (data.version >= 10) {
                ValidateDaily(data.daily);
            }
            if (data.version >= 3) {
                ValidateStreak(data.streak);
            }
            if (data.version >= 6) {
                ValidateSettings(data.settings);
            }
            if (data.version >= 5) {
                ValidateProfile(data.profile);
                if (data.profile.trackedThroughLevel != data.campaignClears || data.profile.retriedLevel > data.campaignClears + 1) {
                    throw new InvalidDataException("Profile and campaign progress disagree.");
                }
            }
            if (data.version >= 4) {
                ValidateRanking(data.ranking, data.campaignClears);
            }
            Directory.CreateDirectory(directory);
            string temporary = path + ".tmp";
            var serializer = new DataContractJsonSerializer(typeof(PlayerSaveModel));
            using (var stream = File.Create(temporary)) {
                serializer.WriteObject(stream, data);
            }
            if (File.Exists(path)) {
                File.Replace(temporary, path, null);
            } else {
                File.Move(temporary, path);
            }
        }

        public static void ValidateEndless(EndlessSaveModel data) {
            if (data == null || data.bestFound < 0) {
                throw new InvalidDataException("Missing or invalid endless record.");
            }
        }
        public static void ValidateCampaign(CampaignSaveModel data) {
            if (data == null || data.skill < 1 || data.claimedRewardLevel < 0 || data.cleanStreak < 0 || data.sessionDay < 0 || data.sessionDay > 3652059 ||
                data.cursorPositions == null || data.recentPuzzleIds == null) {
                throw new InvalidDataException("Invalid campaign selection progress.");
            }
        }

        public static void ValidateCampaignCycles(CampaignSaveModel data) {
            if (data.cursorCycles == null || data.cursorCycles.Length != data.cursorPositions.Length) {
                throw new InvalidDataException("Missing campaign transform cycles.");
            }
            foreach (int cycle in data.cursorCycles) {
                if (cycle < 0 || cycle > 7) {
                    throw new InvalidDataException("Invalid campaign transform cycle.");
                }
            }
        }

        public static void ValidateSettings(SettingsSaveModel data) {
            if (data == null || data.voice < ComboVoiceType.None || data.voice > ComboVoiceType.Male) {
                throw new InvalidDataException("Invalid settings progress.");
            }
        }
        public static void ValidateDaily(DailySaveModel data) {
            if (data == null || data.records == null || data.claimedCostumes == null) {
                throw new InvalidDataException("Missing daily progress.");
            }
            int previous = 0;
            foreach (var record in data.records) {
                if (record == null || record.day <= previous || record.day >= 3652058 || record.attempts < 1 ||
                    record.bestSeconds < 0 || float.IsNaN(record.bestSeconds) || float.IsInfinity(record.bestSeconds) ||
                    (record.perfect && !record.cleared) || (!record.cleared && record.bestSeconds != 0)) {
                    throw new InvalidDataException("Invalid dated daily record.");
                }
                previous = record.day;
            }
            previous = -1;
            foreach (int id in data.claimedCostumes) {
                if (id <= previous) {
                    throw new InvalidDataException("Daily rewards must be unique and sorted.");
                }
                previous = id;
            }
        }

        public static void ValidateProfile(ProfileSaveModel data) {
            if (data == null || string.IsNullOrWhiteSpace(data.nickname) || data.avatarId < 0 || data.frameId < 0 ||
                data.costumeId < -1 || data.mottoId < 0 || data.firstRecordedDay < 1 || data.firstRecordedDay > 3652059 ||
                data.consecutiveClears < 0 || data.bestConsecutiveClears < data.consecutiveClears ||
                data.firstTryClears < 0 || data.threeSeedClears < 0 || data.trackedThroughLevel < 0 ||
                data.firstTryClears > data.trackedThroughLevel || data.threeSeedClears > data.trackedThroughLevel ||
                data.retriedLevel < 0 || data.lastLoginDay < 0 || data.lastLoginDay > 3652059 ||
                data.loginStreak < 0 || data.bestLoginStreak < data.loginStreak) {
                throw new InvalidDataException("Invalid profile progress.");
            }
        }

        void ValidateStreak(StreakSaveModel data) {
            if (data == null || data.winDays == null || data.runDays < 0 || data.bestDays < data.runDays ||
                data.lastWinDay < 0 || data.lastWinDay > 3652059 || data.pendingHints < 0 || data.pendingGuides < 0 ||
                data.bonusHints < 0 || data.bonusGuides < 0 || data.pendingSlot < 0 || data.pendingSlot > 7 ||
                data.recoveryDismissedDay < 0 || data.recoveryDismissedDay > 3652059 ||
                ((data.pendingHints > 0 || data.pendingGuides > 0) != (data.pendingSlot > 0))) {
                throw new InvalidDataException("Invalid streak progress.");
            }
            int previous = 0;
            foreach (int day in data.winDays) {
                if (day <= previous || day > 3652059) {
                    throw new InvalidDataException("Streak win dates must be valid, unique and sorted.");
                }
                previous = day;
            }
            if ((data.winDays.Count == 0 && (data.lastWinDay != 0 || data.runDays != 0)) ||
                (data.winDays.Count > 0 && (previous != data.lastWinDay || data.runDays == 0))) {
                throw new InvalidDataException("Streak counter and dated wins disagree.");
            }
            int run = 0;
            int next = data.lastWinDay;
            for (int i = data.winDays.Count - 1; i >= 0 && data.winDays[i] == next; i--) {
                run++;
                next--;
            }
            if (run != data.runDays) {
                throw new InvalidDataException("Streak length does not match consecutive dated wins.");
            }
        }

        void ValidateRanking(RankingSaveModel data, int clears) {
            if (data == null || data.version != 1 || !System.Guid.TryParseExact(data.clientId, "N", out _) || data.nextEventId <= 0 ||
                data.lastRecordedLevel < 0 || data.lastRecordedLevel > clears || data.pendingClears == null || data.claimSeasonId < 0 ||
                (data.claimSeasonId > 0 && !System.Guid.TryParseExact(data.claimRequestToken, "N", out _))) {
                throw new InvalidDataException("Invalid ranking progress.");
            }
            long previousId = 0;
            int previousLevel = 0;
            foreach (var item in data.pendingClears) {
                if (item == null || item.clientId != data.clientId || item.eventId <= previousId || item.eventId >= data.nextEventId ||
                    item.level <= previousLevel || item.level > data.lastRecordedLevel || item.puzzleId <= 0 || item.heartsRemaining < 0 ||
                    item.gameScore < 0 || item.completedAtUtc <= 0 || item.seasonId < 0) {
                    throw new InvalidDataException("Invalid pending ranking clear event.");
                }
                previousId = item.eventId;
                previousLevel = item.level;
            }
        }

        public string GetPath(GameConfigSO config, string directory) {
            if (config.saveVersion <= 0 || string.IsNullOrWhiteSpace(config.saveFileName) ||
                Path.GetFileName(config.saveFileName) != config.saveFileName) {
                throw new InvalidDataException("Invalid save configuration.");
            }
            return Path.Combine(directory, config.saveFileName);
        }
    }
}
