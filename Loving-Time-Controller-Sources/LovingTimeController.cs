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
        /// 游戏原本逻辑中社交窗口出现的回合数
        /// </summary>
        public const int ORIGIN_LOVING_OPEN_ROUNDS = 25;
        /// <summary>
        /// 游戏原本逻辑中王胜男转学的回合数
        /// </summary>
        public const int ORIGIN_WANG_SHENG_NAN_TRANSFER_ROUNDS = 27;
        /// <summary>
        /// 游戏原本逻辑中李若放转学的回合数
        /// </summary>
        public const int ORIGIN_LI_RUO_FANG_TRANSFER_ROUNDS = 28;
        /// <summary>
        /// 游戏原本逻辑中汤金娜转学的回合数
        /// </summary>
        public const int ORIGIN_TANG_JIN_NA_TRANSFER_ROUNDS = 28;
        /// <summary>
        /// 游戏原本逻辑中苏芳允转学的回合数
        /// </summary>
        public const int ORIGIN_SU_FANG_YUN_TRANSFER_ROUNDS = 29;
        /// <summary>
        /// 游戏原本逻辑中牧唯转学的回合数
        /// </summary>
        public const int ORIGIN_MU_WEI_TRANSFER_ROUNDS = 29;

        /// <summary>
        /// 社交按钮开启回合数
        /// </summary>
        [Draw("社交按钮开启回合数 - Loving Button Open Rounds")]
        public int LovingOpenRounds = ORIGIN_LOVING_OPEN_ROUNDS;
        /// <summary>
        /// 王胜男转学回合数
        /// </summary>
        [Draw("王胜男转学回合数 - WangShegnNan Transfer Rounds")]
        public int WangShegnNanTransferRounds = ORIGIN_WANG_SHENG_NAN_TRANSFER_ROUNDS;
        /// <summary>
        /// 李若放转学回合数
        /// </summary>
        [Draw("李若放转学回合数 - LiRuoFang Transfer Rounds")]
        public int LiRuoFangTransferRounds = ORIGIN_LI_RUO_FANG_TRANSFER_ROUNDS;
        /// <summary>
        /// 汤金娜转学回合数
        /// </summary>
        [Draw("汤金娜转学回合数 - TangJinNa Transfer Rounds")]
        public int TangJinNaTransferRounds = ORIGIN_TANG_JIN_NA_TRANSFER_ROUNDS;
        /// <summary>
        /// 苏芳允转学回合数
        /// </summary>
        [Draw("苏芳允转学回合数 - SuFangYun Transfer Rounds")]
        public int SuFangYunTransferRounds = ORIGIN_SU_FANG_YUN_TRANSFER_ROUNDS;
        /// <summary>
        /// 牧唯转学回合数
        /// </summary>
        [Draw("牧唯转学回合数 - MuWei Transfer Rounds")]
        public int MuWeiTransferRounds = ORIGIN_MU_WEI_TRANSFER_ROUNDS;

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

            // 返回 true 表示这个 Mod 切换到 Mod Manager 切换的状态，返回 false 表示 Mod 依然保持原来的状态
            return true;
        }

        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }
    }
}
