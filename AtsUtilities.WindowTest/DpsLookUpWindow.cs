using AtsUtilities.DpsLookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtsUtilities.WindowTest
{
    public partial class DpsLookUpWindow : Form
    {
        private readonly String _userName;
        private readonly String _passWord;
        private readonly String _searchBy;
        private readonly String _query;

        private String _htmlResult;

        private DpsLookUpWindow()
        {
            //Default is not allowed
        }

        public DpsLookUpWindow(String[] arguments)
        {
            this.InitializeComponent();

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(
                Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height
            );

            this._userName = arguments[0];
            this._passWord = arguments[1];
            this._searchBy = arguments[2];
            this._query = arguments[3];

            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.RunWorkerAsync();

            
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var currentBackgroundWorker = sender as BackgroundWorker;

            _htmlResult = DpsLookUpParser.GetDpsLookupHtmlResult(
                _userName,
                _passWord,
                _searchBy,
                _query,
                (String stepChangedMessage, Int32 progressChangedValue) =>
                {
                    currentBackgroundWorker.ReportProgress(progressChangedValue, stepChangedMessage);
                },
                (String htmlResult) =>
                {
                    e.Result = htmlResult;
                }
            );
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBarLoading.Value = e.ProgressPercentage;
            this.labelMessage.Text = e.UserState as String;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            { 
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "DpsLookUpResult.html";
                saveFileDialog.Filter = "HTML File | *.html";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var streamWriter = new StreamWriter(saveFileDialog.OpenFile());
                    streamWriter.Write((e.Result as String));
                    streamWriter.Dispose();
                    streamWriter.Close();
                }
            }
            Application.Exit();
        }
    }
}
