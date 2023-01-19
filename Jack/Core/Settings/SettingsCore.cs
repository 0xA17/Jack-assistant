using Jack.Pages;
using Jack.Tools.StringTLS;
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

            var targetLabelButton = (Label)settingsPage.FindName(
                $"{StringTools.RemoveCharacter(toggleButtonName, removeNameLen)}{addToName}");

            if (targetLabelButton is null ||
                targetLabelButton.Content is null)
            {
                return false;
            }

            if (isChecked)
            {
                targetLabelButton.Content = "Вкл.";
            }
            else
            {
                targetLabelButton.Content = "Откл.";
            }

            return true;
        }
    }
}