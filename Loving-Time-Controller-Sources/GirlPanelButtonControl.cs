using UnityEngine;
using System.Collections;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine.UI;

namespace MtC.Mod.ChineseParents.LovingTimeController
{
    /// <summary>
    /// 女生面板刷新的方法，在这里控制各个女生是否显示
    /// </summary>
    [HarmonyPatch(typeof(panel_girls), "refresh")]
    public static class action_manager_AddAnniversaryButton
    {
        private static void Postfix(panel_girls __instance, List<GameObject> ___list, Image[] ___bgButtons, ScrollRect ___scroll)
		{
            // 如果 Mod 未启动则直接按照游戏原本的逻辑进行调用
            if (!Main.enabled)
            {
                return;
            }

            Main.ModEntry.Logger.Log("刷新女生面板方法调用完毕");

            // 获取当前回合数
            int currentRound = player_data.Instance.Round_current;

            // 记录总共有多少个按钮，一开始是 4 个，对应四个初始同学
            int buttonNumber = 4;

            // 按顺序将需要转学的同学的转学事件放入数组，准备后续操作
            int[] transferRounds = new int[] {
                Main.settings.WangShegnNanTransferRounds,
                Main.settings.LiRuoFangTransferRounds,
                Main.settings.TangJinNaTransferRounds,
                Main.settings.SuFangYunTransferRounds,
                Main.settings.MuWeiTransferRounds
            };

            for(int i = 4; i < 9; i++)
            {
                // 获取当前女生设置的转学回合数
                int transferRound = transferRounds[i - 4];

                // 当前回合大于转学回合则激活按钮，否则禁用按钮
                if(currentRound >= transferRound)
                {
                    // 启用背景色块
                    ___bgButtons[i].gameObject.SetActive(true);
                    // 启用背景色块的射线接受，也就是让背景的色块能够响应鼠标事件
                    ___bgButtons[i].raycastTarget = true;
                    // 启用人物图片
                    ___list[i].gameObject.SetActive(true);
                    // 记录按钮数 +1
                    buttonNumber++;
                }
                else
                {
                    // 禁用背景色块的射线接收
                    ___bgButtons[i].raycastTarget = false;
                    // 禁用人物图片
                    ___list[i].gameObject.SetActive(false);
                }
            }

            // 如果牧唯转学了，则社交面板显示不到牧唯，开启垂直滚动，否则禁用滚动
            if(currentRound >= Main.settings.MuWeiTransferRounds)
            {
                ___scroll.vertical = true;
            }
            else
            {
                ___scroll.vertical = false;
            }
        }
    }
}
