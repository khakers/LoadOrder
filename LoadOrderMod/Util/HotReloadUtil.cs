namespace LoadOrderMod.Util {
    using ColossalFramework;
    using ColossalFramework.UI;
    using KianCommons;
    using KianCommons.Plugins;
    using System;
    using System.Collections.Generic;
    using static ColossalFramework.Plugins.PluginManager;
    using static KianCommons.ReflectionHelpers;
    using UnityEngine;
    using ICities;
    using System.Reflection;
    using HarmonyLib;
    using System.Reflection.Emit;
    using ColossalFramework.Plugins;
    using System.Linq;

    public static class HotReloadUtil {
        public static List<UIComponent> m_Dummies(this OptionsMainPanel optionsMainPanel) =>
            GetFieldValue(optionsMainPanel, "m_Dummies") as List<UIComponent>;
        public static UIListBox m_Categories(this OptionsMainPanel optionsMainPanel) =>
            GetFieldValue(optionsMainPanel, "m_Categories") as UIListBox;
        public static UITabContainer m_CategoriesContainer(this OptionsMainPanel optionsMainPanel) =>
            GetFieldValue(optionsMainPanel, "m_CategoriesContainer") as UITabContainer;


        //static TDelegate CreateDelegate<TDelegate>(object instance, string name) where TDelegate : Delegate {
        //    var method = GetMethod(instance.GetType(), name);
        //    return (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), instance, method);
        //}

        //public static PluginsChangedHandler RefreshPlugins =>
        //    CreateDelegate<PluginsChangedHandler>(optionsMainPanel, "RefreshPlugins");

        //public static PropertyChangedEventHandler<int> OnCategoryChanged =>
        //    CreateDelegate<PropertyChangedEventHandler<int>>(optionsMainPanel, "OnCategoryChanged");


        public static void DropCategory(this OptionsMainPanel optionsMainPanel, string name) {
            try {
                LogCalled();
                if (name == null) throw new ArgumentNullException("name");
                var categories = optionsMainPanel.m_Categories();
                int index = categories.items.IndexOf(name);
                if (index < 0) return;
                int selectedIndex = categories.selectedIndex;

                Log.Info("Dropping category :" + name);
                var dummies = optionsMainPanel.m_Dummies();
                var category = dummies.Find(c => c.name == name);
                dummies.Remove(category);
                GameObject.DestroyImmediate(category);
                categories.items = categories.items.RemoveAt(index);

                categories.selectedIndex = selectedIndex;
            } catch(Exception ex) {
                Log.Exception(ex);
            }
        }


        public static void AddCategory(this OptionsMainPanel optionsMainPanel, PluginInfo p) {
            try { 
                LogCalled();
                if (!p.isEnabled)
                    return;
                if (p?.GetUserModInstance() is not IUserMod userMod)
                    return;

                string name = p.name;
                MethodInfo mOnSettingsUI = userMod.GetType().GetMethod("OnSettingsUI", BindingFlags.Instance | BindingFlags.Public);
                if (mOnSettingsUI != null) {
                    Log.Info("Adding category :" + name);
                    UIComponent category = optionsMainPanel.m_CategoriesContainer().AttachUIComponent(UITemplateManager.GetAsGameObject("OptionsScrollPanelTemplate"));
                    category.name = userMod.Name;
                    optionsMainPanel.m_Dummies().Add(category);

                    mOnSettingsUI.Invoke(userMod, new object[] { new UIHelper(category.Find("ScrollContent")) });
                    var categories = optionsMainPanel.m_Categories();
                    categories.items = categories.items.AddToArray(name);
                    category.zOrder = categories.items.Length - 1;
                }
            } catch (Exception ex) {
                Log.Exception(ex);
                
            }
        }


    }
}
