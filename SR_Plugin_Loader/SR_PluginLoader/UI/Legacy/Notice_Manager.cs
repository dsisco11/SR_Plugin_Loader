
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SR_PluginLoader
{

    public class Notice_Manager : Singleton<Notice_Manager>
    {
        private static List<UI_Notification> notifications = new List<UI_Notification>();
        private static Rect area;
        /// <summary>
        /// Are notifications visible?
        /// </summary>
        private static bool visible = true;
        private static bool needs_layout = true;
        private int id = 0;


        public void Add_Notice(UI_Notification notice)
        {
            if (notice == null) return;

            notifications.Add(notice);
            needs_layout = true;
        }

        private void Start()
        {
        }

        private void Update()
        {
            List<UI_Notification> trash = new List<UI_Notification>();
            foreach (var notice in notifications)
            {
                notice.Update();
                if (notice.should_die) trash.Add(notice);
            }

            foreach (var notice in trash)
            {
                notifications.Remove(notice);
            }

            if (trash.Count > 0) this.doLayout();
        }


        private bool handleEvents()
        {
            id = GUIUtility.GetControlID(0, FocusType.Passive, Notice_Manager.area);
            var evt = Event.current.GetTypeForControl(id);

            switch (evt)
            {
                case EventType.Layout:
                    this.doLayout();
                    return false;
                case EventType.Ignore:
                case EventType.Used:
                    return false;
            }

            return true;
        }

        private void doLayout()
        {
            Notice_Manager.needs_layout = false;

            const float notice_pad = 5f;
            Notice_Manager.area = new Rect(Screen.width - (UI_Notification.notification_width + notice_pad), notice_pad, UI_Notification.notification_width + notice_pad, (Screen.height - notice_pad));
            

            float yPos = 0f;
            foreach (var notice in Notice_Manager.notifications)
            {
                notice.doLayout();
                notice.Set_Pos(0f, yPos);
                yPos += (notice.height + 3f);
            }
        }


        private void OnGUI()
        {
            if (Event.current.GetTypeForControl(id) == EventType.Layout || needs_layout)
            {
                this.doLayout();
                return;
            }
            // Abort drawing the panel IF we are not currently visible
            //if (!this.gameObject.activeSelf || !this.visible) return;
            if (!this.handleEvents()) return;

            GUI.BeginGroup(Notice_Manager.area);
            foreach (var notice in notifications)
            {
                try
                {
                    if (notice.Display())
                    {
                        //remove the notification.
                        notice.should_die = true;
                    }
                }
                catch(Exception ex)
                {
                    DebugHud.Log(ex);
                }
            }
            GUI.EndGroup();
        }
    }
}
