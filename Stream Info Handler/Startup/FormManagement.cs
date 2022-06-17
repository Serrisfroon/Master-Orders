using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler.Startup
{
    public static class FormManagement
    {

        /// <summary>
        /// Contains a list of all open forms. Use TryGetValue to check if a form is open.
        /// </summary>
        public static Dictionary<FormNames, Form> openForms = new Dictionary<FormNames, Form>();

        /// <summary>
        /// The names that forms can use.
        /// </summary>
        public enum  FormNames
        {
            StreamAssistant,
            BracketAssistant,
            GeneralSettings,
            PlayerManager,
            Top8Generator,
            ClipToTweet,
            Settings,
            None
        }

        /// <summary>
        /// The startup form.
        /// </summary>
        public static Form baseForm { get; set; }

        /// <summary>
        /// Closes the form associated with a specific name. Accepted
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public static bool CloseForm(FormNames formName)
        {
            baseForm.Visible = true;
            if (openForms.ContainsKey(formName))
            {
                openForms.Remove(formName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Form OpenForm(FormNames formName)
        {
            switch(formName)
            {
                case FormNames.StreamAssistant:
                    return new StreamAssistant.StreamAssistantForm();
                case FormNames.BracketAssistant:
                    return new frm_bracket_assistant();
                case FormNames.GeneralSettings:
                    return new AppSettings.GeneralSettings.GeneralSettingsForm(FormNames.None);
                case FormNames.PlayerManager:
                    return new frm_playermanager();
                case FormNames.Top8Generator:
                    return new frm_othertools();
                case FormNames.ClipToTweet:
                    return new ClipToTwitter.ClipToTwitterForm();
            }
            return null;
        }
    }
}
