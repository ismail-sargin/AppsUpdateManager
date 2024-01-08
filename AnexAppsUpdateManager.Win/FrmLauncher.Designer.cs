namespace AnexAppsUpdateManager.Win
{
    partial class FrmLauncher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLauncher));
            this.toastNotificationsManager1 = new DevExpress.XtraBars.ToastNotifications.ToastNotificationsManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.toastNotificationsManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // toastNotificationsManager1
            // 
            this.toastNotificationsManager1.ApplicationId = " ";
            this.toastNotificationsManager1.Notifications.AddRange(new DevExpress.XtraBars.ToastNotifications.IToastNotificationProperties[] {
            new DevExpress.XtraBars.ToastNotifications.ToastNotification("6bb7d998-1859-490c-ae68-d63b3be7c6d2", ((System.Drawing.Image)(resources.GetObject("toastNotificationsManager1.Notifications"))), "Information", "You have the latest version of the launcher", " ", DevExpress.XtraBars.ToastNotifications.ToastNotificationTemplate.ImageAndText03),
            new DevExpress.XtraBars.ToastNotifications.ToastNotification("def7da51-e504-4c1d-bb4f-0cef49150202", ((System.Drawing.Image)(resources.GetObject("toastNotificationsManager1.Notifications1"))), "Information", "Launcher will be updated in the background", "It will restart automatically when the launcher is updated", DevExpress.XtraBars.ToastNotifications.ToastNotificationTemplate.ImageAndText03),
            new DevExpress.XtraBars.ToastNotifications.ToastNotification("a35a9886-06bd-480b-a63f-3935e3d26b45", ((System.Drawing.Image)(resources.GetObject("toastNotificationsManager1.Notifications2"))), "Info", "Anex Apps  Latest Version installed", "", DevExpress.XtraBars.ToastNotifications.ToastNotificationTemplate.ImageAndText03)});
            // 
            // FrmLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 268);
            this.Name = "FrmLauncher";
            this.Text = "FrmLauncher";
            ((System.ComponentModel.ISupportInitialize)(this.toastNotificationsManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.ToastNotifications.ToastNotificationsManager toastNotificationsManager1;
    }
}