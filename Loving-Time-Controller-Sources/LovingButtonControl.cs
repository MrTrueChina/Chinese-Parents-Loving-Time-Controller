using UnityEngine;
using System.Collections;
using HarmonyLib;

namespace MtC.Mod.ChineseParents.LovingTimeController
{
    /// <summary>
    /// 读取玩家数据方法，可能是读档的时候用到的方法，这里有开启按钮功能。在这里控制显隐社交按钮
    /// </summary>
    [HarmonyPatch(typeof(player_data), "read_data")]
    public static class player_data_read_data
    {
        private static void Postfix()
        {
            // 如果 Mod 未启动则直接按照游戏原本的逻辑进行调用
            if (!Main.enabled)
            {
                return;
            }

            Main.ModEntry.Logger.Log("读取玩家数据方法调用完毕");

            // 如果到了约会出现回合数，则显示社交面板，否则禁用社交面板
            if(player_data.Instance.Round_current >= Main.settings.lovingOpenRounds)
            {
                open_system.InstanceOpenSystem.opensystem(open_system.InstanceOpenSystem.GirlGameObject);
            }
            else
            {
                open_system.InstanceOpenSystem.GirlGameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 每周开始时开启按钮的方法。在这里控制显隐社交按钮
    /// </summary>
    [HarmonyPatch(typeof(week_player), "system_open")]
    public static class week_player_system_open
    {
        private static void Postfix()
        {
            // 如果 Mod 未启动则直接按照游戏原本的逻辑进行调用
            if (!Main.enabled)
            {
                return;
            }

            Main.ModEntry.Logger.Log("每周开始时开启按钮的方法调用完毕");

            // 如果到了约会出现回合数，则显示社交面板，否则禁用社交面板
            if (player_data.Instance.Round_current >= Main.settings.lovingOpenRounds)
            {
                open_system.InstanceOpenSystem.opensystem(open_system.InstanceOpenSystem.GirlGameObject);
            }
            else
            {
                open_system.InstanceOpenSystem.GirlGameObject.SetActive(false);
            }
        }
    }
}