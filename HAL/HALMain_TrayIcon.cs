using System;
using System.Windows;

namespace HAL {
    public partial class HALMain {

        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        private WindowState m_storedWindowState = WindowState.Normal;

        private void InitTrayIcon(bool minimize) {
            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.BalloonTipText = "The app has been minimised. Click the tray icon to show.";
            m_notifyIcon.BalloonTipTitle = "Home Assistant Listener";
            m_notifyIcon.Text = "HAL";
            m_notifyIcon.Icon = new System.Drawing.Icon("hal_icon.ico");
            m_notifyIcon.Click += new EventHandler(m_notifyIcon_Click);

            if (minimize) MinimizeWindow();
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            m_notifyIcon.Dispose();
            m_notifyIcon = null;
        }

        private void Window_StateChanged(object sender, EventArgs e) {
            MinimizeWindow();
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
            CheckTrayIcon();
        }

        void m_notifyIcon_Click(object sender, EventArgs e) {
            Show();
            WindowState = m_storedWindowState;
            CenterWindowOnScreen();
        }

        void CheckTrayIcon() {
            ShowTrayIcon(!IsVisible);
        }

        void ShowTrayIcon(bool show) {
            if (m_notifyIcon != null)
                m_notifyIcon.Visible = show;
        }

        void MinimizeWindow() {
            if (WindowState == WindowState.Minimized) {
                Hide();
                if (m_notifyIcon != null) {
                    m_notifyIcon.Visible = true;
                    m_notifyIcon.ShowBalloonTip(2000);
                }
            } else
                m_storedWindowState = WindowState;
        }

        private void CenterWindowOnScreen() {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
