namespace UrlCheckerService
{
    partial class Service1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._timer = new System.Timers.Timer();
            this._eventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this._timer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._eventLog)).BeginInit();
            // 
            // _timer
            // 
            this._timer.Enabled = true;
            this._timer.Interval = 5000D;
            this._timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
            // 
            // Service1
            // 
            this.ServiceName = "PlaceberryUrlChecker";
            ((System.ComponentModel.ISupportInitialize)(this._timer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._eventLog)).EndInit();

        }

        #endregion

        private System.Timers.Timer _timer;
        private System.Diagnostics.EventLog _eventLog;
    }
}
