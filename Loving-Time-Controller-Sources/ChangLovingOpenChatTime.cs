using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;

namespace MtC.Mod.ChineseParents.LovingTimeController
{
    /// <summary>
    /// 读取数据的方法，
    /// </summary>
    [HarmonyPatch(typeof(ReadXml), "GetData")]
    public static class ReadXml_GetData
    {
        /// <summary>
        /// 读取数据方法使用的参数
        /// </summary>
        private class ReadXmlGetDataPrams
        {
            /// <summary>
            /// 文件名，也就是 Key
            /// </summary>
            public string fileName;
            /// <summary>
            /// id，相当于第二个 Key，在不同的文件名中有不同的意义，在 fileName = week_data 时这个 id 的意思是"第几个回合"
            /// </summary>
            public int id;

            public ReadXmlGetDataPrams(string fileName,int id)
            {
                this.fileName = fileName;
                this.id = id;
            }
        }

        private static void Prefix(out ReadXmlGetDataPrams __state, string fileName, int id)
        {
            // 将参数传给后缀，虽然后缀也能获取参数，但是 GetData 是一个调用庞杂的方法，为了防止参数在过程中被修改在这里备份一份给后缀
            __state = new ReadXmlGetDataPrams(fileName, id);
        }

        private static void Postfix(ReadXmlGetDataPrams __state, ref XmlData __result)
        {
            // 如果 Mod 未启动则直接按照游戏原本的逻辑进行调用
            if (!Main.enabled)
            {
                return;
            }

            // 这个方法调用次数过多，先对是否是查询每回合事件进行过滤，否则后续 Log 会污染 Log 文件还可能影响运行速度
            if (!("week_data".Equals(__state.fileName)))
            {
                return;
            }

            Main.ModEntry.Logger.Log("读取每回合事件数据方法调用完毕");

            if (Main.VANILLA_LOVING_OPEN_ROUNDS == Main.settings.lovingOpenRounds)
            {
                Main.ModEntry.Logger.Log("设置的社交按钮开启时间和原版时间相同，不作处理");
                return;
            }

            // 在原版的开启社交回合获取数据，也就是原版的开启社交对话
            if ("week_data".Equals(__state.fileName) && __state.id == Main.VANILLA_LOVING_OPEN_ROUNDS) 
            {
                Main.ModEntry.Logger.Log("检测到原版逻辑获取开启社交对话数据，进行阻断");

                // 设为没有对话数据
                __result.value["chat"] = "0";
                return;
            }

            // 在设置的开启社交回合获取数据，这个时候就是显示开启社交对话的时候
            if ("week_data".Equals(__state.fileName) && __state.id == Main.settings.lovingOpenRounds)
            {
                // 如果这个回合本来有对话，而且设置里是不覆盖的，不作处理返回
                if(!("0".Equals(__result.GetInt("chat", true))) && !Main.settings.overlayChat)
                {
                    Main.ModEntry.Logger.Log("检测到应当显示社交对话的回合已有其它对话，根据设置放弃显示社交对话");
                    return;
                }

                Main.ModEntry.Logger.Log("到达社交按钮显示回合，显示社交对话");

                // 替换对话为社交按钮第一次出现的对话
                __result.value["chat"] = Main.VANILLA_LOVING_OPEN_CHAT_ID.ToString();
            }
        }
    }
}
