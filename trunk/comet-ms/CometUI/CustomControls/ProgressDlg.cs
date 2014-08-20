﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using CometUI.Properties;

namespace CometUI.CustomControls
{
    public partial class ProgressDlg : Form
    {
        readonly BackgroundWorker _backgroundWorker;

        public ProgressDlg(BackgroundWorker backgroundWorker)
        {
            InitializeComponent();
            _backgroundWorker = backgroundWorker;
            StatusText.Text = String.Empty;
            ProgressBar.Value = 1;
            ProgressBar.Visible = true;

            progressStatusMessageTimer.Start();
        }

        protected String StatusMessage { get; set; }

        public String TitleText
        {
            get { return Text; }
            set { Text = value; }
        }

        public void UpdateStatusText(String statusText)
        {
            StatusText.Text = statusText;
        }

        public void AllowCancel(bool allow)
        {
            CancelButton.Enabled = allow;
        }

        public virtual void Cancel()
        {
            _backgroundWorker.CancelAsync();
        }

        protected virtual void UpdateStatusText()
        {
            StatusText.Text = StatusMessage;
        }

        private void ProgressDlgClosing(object sender, FormClosingEventArgs e)
        {
            progressStatusMessageTimer.Stop();
            Cancel();
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            if (VerifyCancel())
            {
                Close();
            }
        }

        private bool VerifyCancel()
        {
            if (DialogResult.OK == MessageBox.Show(Resources.ProgressDlg_VerifyCancel_Are_you_sure_you_want_to_cancel_the_operation_, Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
            {
                return true;
            }

            return false;
        }

        private void StatusTextMouseHover(object sender, EventArgs e)
        {
            ProgressDlgToolTip.Show(StatusText.Text, StatusText);
        }

        private void ProgressStatusMessageTimerTick(object sender, EventArgs e)
        {
            UpdateStatusText();
        }
    }
}
