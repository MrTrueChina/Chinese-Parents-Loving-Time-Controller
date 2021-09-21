using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityModManagerNet;

namespace MtC.Mod.ChineseParents.LovingTimeController
{
    /// <summary>
    /// 这个 Mod 的设置
    /// </summary>
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        /// <summary>
        /// 社交按钮开启回合数
        /// </summary>
        [Draw("社交按钮开启回合数（原版是 25） - Loving Button Open Rounds (Vanilla is 25)")]
        public int lovingOpenRounds = Main.VANILLA_LOVING_OPEN_ROUNDS;

        /// <summary>
        /// 社交按钮开启对话是否覆盖原有的事件
        /// </summary>
        [Draw("如果社交按钮开启的日期有其他对话则覆盖对话 - Overlay Other Chat On Loving Open Chat")]
        public bool overlayChat = true;

        /// <summary>
        /// 王胜男转学回合数
        /// </summary>
        [Draw("王胜男转学回合数（原版是 27） - WangShegnNan Transfer Rounds (Vanilla is 27)")]
        public int wangShegnNanTransferRounds = Main.VANILLA_WANG_SHENG_NAN_TRANSFER_ROUNDS;
        /// <summary>
        /// 李若放转学回合数
        /// </summary>
        [Draw("李若放转学回合数（原版是 28） - LiRuoFang Transfer Rounds (Vanilla is 28)")]
        public int liRuoFangTransferRounds = Main.VANILLA_LI_RUO_FANG_TRANSFER_ROUNDS;
        /// <summary>
        /// 汤金娜转学回合数
        /// </summary>
        [Draw("汤金娜转学回合数（原版是 28） - TangJinNa Transfer Rounds (Vanilla is 28)")]
        public int tangJinNaTransferRounds = Main.VANILLA_TANG_JIN_NA_TRANSFER_ROUNDS;
        /// <summary>
        /// 苏芳允转学回合数
        /// </summary>
        [Draw("苏芳允转学回合数（原版是 29） - SuFangYun Transfer Rounds (Vanilla is 29)")]
        public int suFangYunTransferRounds = Main.VANILLA_SU_FANG_YUN_TRANSFER_ROUNDS;
        /// <summary>
        /// 牧唯转学回合数
        /// </summary>
        [Draw("牧唯转学回合数（原版是 29） - MuWei Transfer Rounds (Vanilla is 29)")]
        public int muWeiTransferRounds = Main.VANILLA_MU_WEI_TRANSFER_ROUNDS;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        public void OnChange()
        {
        }
    }

    public static class Main
    {
        /// <summary>
        /// 游戏原本逻辑中社交窗口出现的回合数
        /// </summary>
        public const int VANILLA_LOVING_OPEN_ROUNDS = 25;
        /// <summary>
        /// 游戏原本逻辑中社交窗口出现的对话的 ID
        /// </summary>
        public const int VANILLA_LOVING_OPEN_CHAT_ID = 21001;
        /// <summary>
        /// 游戏原本逻辑中王胜男转学的回合数
        /// </summary>
        public const int VANILLA_WANG_SHENG_NAN_TRANSFER_ROUNDS = 27;
        /// <summary>
        /// 游戏原本逻辑中李若放转学的回合数
        /// </summary>
        public const int VANILLA_LI_RUO_FANG_TRANSFER_ROUNDS = 28;
        /// <summary>
        /// 游戏原本逻辑中汤金娜转学的回合数
        /// </summary>
        public const int VANILLA_TANG_JIN_NA_TRANSFER_ROUNDS = 28;
        /// <summary>
        /// 游戏原本逻辑中苏芳允转学的回合数
        /// </summary>
        public const int VANILLA_SU_FANG_YUN_TRANSFER_ROUNDS = 29;
        /// <summary>
        /// 游戏原本逻辑中牧唯转学的回合数
        /// </summary>
        public const int VANILLA_MU_WEI_TRANSFER_ROUNDS = 29;
        /// <summary>
        /// 王胜男转学事件对话数据 ID
        /// </summary>
        public const int WANG_SHENG_NAN_TRANSFER_DATA_ID = 7000001;
        /// <summary>
        /// 李若放转学事件对话数据 ID
        /// </summary>
        public const int LI_RUO_FANG_TRANSFER_DATA_ID = 7000002;
        /// <summary>
        /// 汤金娜转学事件对话数据 ID
        /// </summary>
        public const int TANG_JIN_NA_TRANSFER_DATA_ID = 7000003;
        /// <summary>
        /// 苏芳允转学事件对话数据 ID
        /// </summary>
        public const int SU_FANG_YUN_TRANSFER_DATA_ID = 7000004;
        /// <summary>
        /// 牧唯转学事件对话数据 ID
        /// </summary>
        public const int MU_WEI_TRANSFER_DATA_ID = 7000005;

        /// <summary>
        /// Mod 对象
        /// </summary>
        public static UnityModManager.ModEntry ModEntry { get; set; }

        /// <summary>
        /// 这个 Mod 是否启动
        /// </summary>
        public static bool enabled;

        /// <summary>
        /// 这个 Mod 的设置
        /// </summary>
        public static Settings settings;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            // 读取设置
            settings = Settings.Load<Settings>(modEntry);

            // 保存 Mod 对象并绑定事件
            ModEntry = modEntry;
            ModEntry.OnToggle = OnToggle;
            ModEntry.OnGUI = OnGUI;
            ModEntry.OnSaveGUI = OnSaveGUI;

            // 加载 Harmony
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();

            modEntry.Logger.Log("事件控制前置 Mod 加载完成");

            // 返回加载成功
            return true;
        }

        /// <summary>
        /// Mod Manager 对 Mod 进行控制的时候会调用这个方法
        /// </summary>
        /// <param name="modEntry"></param>
        /// <param name="value">这个 Mod 是否激活</param>
        /// <returns></returns>
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            // 将 Mod Manager 切换的状态保存下来
            enabled = value;

            // 如果是激活 Mod 则修改转校事件，否则取消修改
            if (enabled)
            {
                TransferEventTimeChange.ChangeTransferEvents();
            }
            else
            {
                TransferEventTimeChange.UnchangeTransferEvents();
            }

            // 返回 true 表示这个 Mod 切换到 Mod Manager 切换的状态，返回 false 表示 Mod 依然保持原来的状态
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            // 保存设置
            settings.Save(modEntry);

            // 取消对转校事件的修改，并用新的配置再次修改
            TransferEventTimeChange.UnchangeTransferEvents();
            TransferEventTimeChange.ChangeTransferEvents();
        }
    }
}
