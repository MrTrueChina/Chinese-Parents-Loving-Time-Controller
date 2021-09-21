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

        ///// <summary>
        ///// 来自反编译的代码，这是刷新女生面板的方法，这个方法放在这里留作参考，不会进行调用
        ///// </summary>
        ///// <param name="__instance">panel_girls 对象</param>
        ///// <param name="___list">所有得女生按钮列表</param>
        ///// <param name="___girl">女生按钮的预制</param>
        ///// <param name="___girlList">所有女生的数据</param>
        ///// <param name="___scroll">社交面板的滚动条组件</param>
        ///// <param name="___infoPanel"></param>
        ///// <param name="___bgButtons">背景列表，所有按钮是背景+角色画像组合的</param>
        ///// <param name="___close"></param>
        //private static bool Prefix(panel_girls __instance, List<GameObject> ___list, GameObject ___girl, XmlList ___girlList, ScrollRect ___scroll, GameObject ___infoPanel, Image[] ___bgButtons, Button ___close)
        //{
        //    // 如果 Mod 未启动则直接按照游戏原本的逻辑进行调用
        //    if (!Main.enabled)
        //    {
        //        return true;
        //    }

        //    Main.ModEntry.Logger.Log("panel_girls.refresh 即将调用");

        //    // 测试代码输出女生列表
        //    foreach (KeyValuePair<int, int> pair in girlmanager.InstanceGirlmanager.GirlsDictionary)
        //    {
        //        Main.ModEntry.Logger.Log("遍历女生列表：key = " + pair.Key + ", value = " + pair.Value);
        //    }

        //    // 输出女生数据
        //    foreach (KeyValuePair<int, XmlData> girl in ___girlList.value)
        //    {
        //        int id = girl.Key;
        //        string name = girl.Value.GetStringLanguage("name");
        //        string image = "UI/girls/" + girl.Value.GetString("head");

        //        Main.ModEntry.Logger.Log("遍历女生数据，id = " + id + ", 名称 = " + name + ", 图片 = " + image);
        //    }

        //    // 以下是原始代码

        //    // 获取编译器生成的私有内部类 <refresh>c__AnonStorey1，这个类在反编译器中一般是默认隐藏的，需要开启编译器的显示自动生成的类功能
        //    Type type_c__AnonStorey1 = typeof(panel_girls).GetNestedType("<refresh>c__AnonStorey1", BindingFlags.NonPublic);

        //    // 这里应该是清除已有的选项，就是所有的女同学
        //    for (int i = 0; i < ___list.Count; i++)
        //    {
        //        UnityEngine.Object.Destroy(___list[i]);
        //    }
        //    ___list.Clear();

        //    // 这个循环的功能是给面板里添加女同学
        //    int num = 0;
        //    using (Dictionary<int, int>.Enumerator enumerator = girlmanager.InstanceGirlmanager.GirlsDictionary.GetEnumerator())
        //    {
        //        while (enumerator.MoveNext())
        //        {
        //            // 发现这个对象在代码里几乎就是个数据中转站，而且都是从这个方法里中转的，把那些中转去掉后就剩下这一行创建对象了，不知道是为什么
        //            object c__AnonStorey = Activator.CreateInstance(type_c__AnonStorey1, new object[] { });

        //            // 这里是实例化女生选项，之后有初始化，就是图片和文本的显示
        //            GameObject item = UnityEngine.Object.Instantiate<GameObject>(___girl, ___bgButtons[num].transform);
        //            ___list.Add(item);
        //            item.name = enumerator.Current.Key.ToString();
        //            GameObject gameObject = item.transform.Find("Button_Name").gameObject;
        //            Text component = gameObject.transform.Find("Text_Name").GetComponent<Text>();
        //            Image component2 = item.transform.Find("head").GetComponent<Image>();
        //            Text component3 = item.transform.Find("loving").GetComponent<Text>();
        //            SkeletonGraphic talkSpine = item.transform.Find("TalkBubble").GetComponent<SkeletonGraphic>();
        //            SkeletonGraphic heartSpine = item.transform.Find("Heart").GetComponent<SkeletonGraphic>();
        //            if (enumerator.Current.Value >= 30)
        //            {
        //                heartSpine.gameObject.SetActive(true);
        //                item.transform.Find("NormalHeart").gameObject.SetActive(false);
        //            }
        //            XmlData xmlData = ___girlList.Get(enumerator.Current.Key);
        //            // 女生名字
        //            component.text = xmlData.GetStringLanguage("name");
        //            // 设置图片
        //            component2.sprite = (Resources.Load("UI/girls/" + xmlData.GetString("head"), typeof(Sprite)) as Sprite);
        //            component2.SetNativeSize();
        //            // 好感度
        //            component3.text = enumerator.Current.Value.ToString();
        //            XmlData itemData = ___girlList.Get(int.Parse(item.name));

        //            // 测试输出
        //            Main.ModEntry.Logger.Log("原代码循环内，id = " + enumerator.Current.Key + ", 名称 = " + xmlData.GetStringLanguage("name") + ", 图片 = " + xmlData.GetString("head"));

        //            // 这里应该是和滚动条有关的
        //            ScrollRectListener gameObject2 = ScrollRectListener.GetGameObject(___bgButtons[num].gameObject);
        //            gameObject2.SetScrollRect(___scroll);

        //            // 绑定点击效果
        //            EventTriggerListener.Get(___bgButtons[num].gameObject).onClick = delegate (GameObject go)
        //            {
        //                MonoBehaviour.print(" ScrollRectListener.Get(bgButtons[index].gameObject).onClick");
        //                if (!__instance.is_talk(int.Parse(item.name)))
        //                {
        //                    chat_manager.InstanceChatManager.start_chat(itemData.GetInt("hello_bad"), 0, 0, null, null, null, string.Empty, false, false);
        //                }
        //                else if (player_data.Instance.Potentiality >= (int)typeof(panel_girls).GetField("need_potential", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null))
        //                {
        //                    player_data.Instance.Potentiality -= (int)typeof(panel_girls).GetField("need_potential", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

        //                    __instance.chatPanel(int.Parse(item.name));
        //                    MessageCenter.sendMessage("refresh_ui_data", null);
        //                }
        //                else
        //                {
        //                    TipsManager.instance.AddTips(ReadXml.GetString("PotentialityNotEnough"), 1);
        //                }
        //            };

        //            // 绑定鼠标进入效果，猜测是鼠标悬浮时的高光效果
        //            EventTriggerListener.Get(___bgButtons[num].gameObject).onEnter = delegate (GameObject go)
        //            {
        //                talkSpine.gameObject.SetActive(true);
        //                lzhspine.change_anim_ui(talkSpine, talkSpine.SkeletonDataAsset, "animation", true, null);
        //                int num2 = 0;
        //                if (enumerator.Current.Value >= 30)
        //                {
        //                    num2 = 1;
        //                }
        //                else if (enumerator.Current.Value >= 50)
        //                {
        //                    num2 = 2;
        //                }
        //                else if (enumerator.Current.Value >= 80)
        //                {
        //                    num2 = 3;
        //                }
        //                if (num2 > 0)
        //                {
        //                    heartSpine.timeScale = (float)num2;
        //                    lzhspine.change_anim_ui2(heartSpine, heartSpine.SkeletonDataAsset, "play", true);
        //                }
        //                go.transform.Find("Image_Light").GetComponent<Image>().DOFade(0.3f, 0.5f);
        //            };

        //            // 绑定鼠标离开效果，猜测是鼠标离开后取消高光效果
        //            EventTriggerListener.Get(___bgButtons[num].gameObject).onExit = delegate (GameObject go)
        //            {
        //                talkSpine.gameObject.SetActive(false);
        //                if (enumerator.Current.Value >= 30)
        //                {
        //                    heartSpine.timeScale = 1f;
        //                    lzhspine.change_anim_ui2(heartSpine, heartSpine.SkeletonDataAsset, "idle", true);
        //                }
        //                go.transform.Find("Image_Light").GetComponent<Image>().DOFade(0f, 0.5f);
        //            };

        //            ScrollRectListener gameObject3 = ScrollRectListener.GetGameObject(gameObject);
        //            gameObject3.SetScrollRect(___scroll);
        //            EventTriggerListener.Get(gameObject).onClick = delegate (GameObject go)
        //            {


        //                ___close.gameObject.SetActive(false);
        //                GameObject info = UnityEngine.Object.Instantiate<GameObject>(___infoPanel, __instance.transform);
        //                info.GetComponent<Button>().onClick.AddListener(delegate ()
        //                {


        //                    ___close.gameObject.SetActive(true);
        //                    UnityEngine.Object.DestroyObject(info);
        //                });
        //                info.transform.Find("head").GetComponent<Image>().sprite = (Resources.Load("UI/girls/" + itemData.GetString("image_info"), typeof(Sprite)) as Sprite);
        //                info.transform.Find("name").GetComponent<Text>().text = itemData.GetStringLanguage("name");
        //                info.transform.Find("desc").GetComponent<Text>().text = itemData.GetStringLanguage("desc");
        //            };
        //            num++;
        //        }
        //    }

        //    // 这里是根据回合数隐藏同学的功能，包括第 9 个同学出现时开启滚动条的功能
        //    int[] array = new int[]
        //    {
        //        27,
        //        28,
        //        28,
        //        29,
        //        29
        //    };
        //    for (int j = 4; j <= 8; j++)
        //    {
        //        if (___list[j] == null)
        //        {
        //            return false;
        //        }
        //        if (player_data.Instance.Round_current >= array[j - 4])
        //        {
        //            ___list[j].gameObject.SetActive(true);
        //            if (j == 8)
        //            {
        //                // 索引 8，这是第 9 个按钮，这个按钮出现后就要启动滚动范围，之前这个按钮是不在显示范围里的
        //                ___bgButtons[8].gameObject.SetActive(true);
        //                ___scroll.vertical = true;
        //            }
        //        }
        //        else
        //        {
        //            // 这里是隐藏还没到显示时间的按钮，可以看出来除了禁用外还关闭了接收射线的功能
        //            ___bgButtons[j].raycastTarget = false;
        //            ___list[j].gameObject.SetActive(false);
        //        }
        //    }

        //    return false;
        //}
    }
}
