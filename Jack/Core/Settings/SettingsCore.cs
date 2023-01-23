using Jack.Core.ThreadUtils;
using Jack.Pages;
using Jack.Tools.StringTLS;
using Newtonsoft.Json.Linq;
using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Jack.Core
{
    class SettingsCore
    {
        public static Boolean LoadSettings()
        {
            return true;
        }

        protected static Boolean ChangeLabelState(String toggleButtonName, Boolean isChecked, UInt16 removeNameLen, String addToName)
        {
            if (String.IsNullOrEmpty(toggleButtonName) ||
                String.IsNullOrEmpty(addToName))
            {
                return false;
            }

            var settingsPage = SettingsPage.GetInstance();

            if (settingsPage is null)
            {
                return false;
            }

            var targetLabelButton = Invokes.GetLabelByName(
                SettingsPage.GetInstance(),
                $"{StringTools.RemoveCharacter(toggleButtonName, removeNameLen)}{addToName}");
            var context = String.Empty;

            if (isChecked)
            {
                context = "Вкл.";
            }
            else
            {
                context = "Откл.";
            }

            return Invokes.UpdateLabelContext(SettingsPage._synchronizationContext, targetLabelButton, context);
        }
    }
}