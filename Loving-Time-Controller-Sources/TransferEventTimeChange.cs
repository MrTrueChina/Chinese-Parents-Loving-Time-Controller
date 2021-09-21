using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MtC.Mod.ChineseParents.EventControlLib;

namespace MtC.Mod.ChineseParents.LovingTimeController
{
    /// <summary>
    /// 这个类用于修改转学事件的发生时间
    /// </summary>
    public static class TransferEventTimeChange
    {
        /// <summary>
        /// 所有添加的事件
        /// </summary>
        private static List<EventControl.ChatEventControlParam> addCharEventParams = new List<EventControl.ChatEventControlParam>();
        /// <summary>
        /// 所有阻断的事件
        /// </summary>
        private static List<EventControl.ChatEventControlParam> blockCharEventParams = new List<EventControl.ChatEventControlParam>();

        /// <summary>
        /// 向事件控制前置 Mod 添加修改转学事件的事件
        /// </summary>
        public static void ChangeTransferEvents() 
        {
            // 每个女同学的 原有转学回合、设置转学回合、转学事件 ID
            List<int[]> transferRoundParams = new List<int[]>();
            // 王胜男
            transferRoundParams.Add(new int[] { Main.VANILLA_WANG_SHENG_NAN_TRANSFER_ROUNDS, Main.settings.wangShegnNanTransferRounds, Main.WANG_SHENG_NAN_TRANSFER_DATA_ID });
            // 李若放
            transferRoundParams.Add(new int[] { Main.VANILLA_LI_RUO_FANG_TRANSFER_ROUNDS, Main.settings.liRuoFangTransferRounds, Main.LI_RUO_FANG_TRANSFER_DATA_ID });
            // 汤金娜
            transferRoundParams.Add(new int[] { Main.VANILLA_TANG_JIN_NA_TRANSFER_ROUNDS, Main.settings.tangJinNaTransferRounds, Main.TANG_JIN_NA_TRANSFER_DATA_ID });
            // 苏芳允
            transferRoundParams.Add(new int[] { Main.VANILLA_SU_FANG_YUN_TRANSFER_ROUNDS, Main.settings.suFangYunTransferRounds, Main.SU_FANG_YUN_TRANSFER_DATA_ID });
            // 牧唯
            transferRoundParams.Add(new int[] { Main.VANILLA_MU_WEI_TRANSFER_ROUNDS, Main.settings.muWeiTransferRounds, Main.MU_WEI_TRANSFER_DATA_ID });

            // 遍历所有女同学，修改转学事件发出回合数
            transferRoundParams.ForEach(param =>
            {
                // 如果设置的转学回合数和原本的转学回合数不同则阻断原来回合数的转学事件并在设置的天数增加转学事件
                if (param[1] != param[0])
                {
                    // 如果是在原本的转学时间添加了转学对话 ID 的事件，那么这个事件就是应该阻断的事件
                    EventControl.ChatEventControlParam blockParam = EventControl.BlockChatEvent(
                        (id) => id == param[2] && player_data.Instance.round_current == param[0],
                        (id) => { });
                    // 记录阻断的事件
                    blockCharEventParams.Add(blockParam);

                    // 在设置的回合数添加转学事件
                    EventControl.ChatEventControlParam addParam = EventControl.AddChatEvent(
                        param[2],
                        (id) => player_data.Instance.round_current == param[1],
                        (id) => { });
                    // 记录添加的事件
                    addCharEventParams.Add(addParam);
                }
            });
        }

        /// <summary>
        /// 从事件控制前置 Mod 移除修改转学事件的事件
        /// </summary>
        public static void UnchangeTransferEvents()
        {
            // 移除所有添加的事件
            addCharEventParams.ForEach(param =>
            {
                EventControl.RemoveAddChatEvent(param);
            });

            // 取消所有阻断的事件
            blockCharEventParams.ForEach(param =>
            {
                EventControl.RemoveBlockChatEvent(param);
            });
        }
    }
}
