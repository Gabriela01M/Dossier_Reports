using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using DevExpress.DataAccess.DataFederation;
using DevExpress.DataAccess.Json;
using DevExpress.XtraEditors.Filtering.Templates;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using static DevExpress.XtraEditors.Mask.MaskSettings;

namespace DXApplication3
{
  public partial class Form1 : DevExpress.XtraEditors.XtraForm
  {
    public Form1()
    {
      InitializeComponent();
      this.Load += Form1_Load; 
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      InitializeReportDesigner();
    }

    private void InitializeReportDesigner()
    {
      
      XRDesignForm designForm = new XRDesignForm();

     
      XRDesignMdiController mdiController = designForm.DesignMdiController;

      
      string reportsFolder = Path.Combine(Application.StartupPath, "Reports");

      if (!Directory.Exists(reportsFolder))
      {
        MessageBox.Show($"File not found:\n{reportsFolder}");
        return;
      }

      
      var repxFiles = Directory.GetFiles(reportsFolder, "*.repx");

      foreach (string filePath in repxFiles)
      {
        try
        {
          XtraReport report = XtraReport.FromFile(filePath, true);
          string originalDataMember = report.DataMember;
          report.DataSource = null;
          report.DataMember = null;
          report.ComponentStorage.Clear();
          report.DisplayName = Path.GetFileNameWithoutExtension(filePath);

          string jsonPath = Path.Combine(Application.StartupPath, "medastest.json");

          JsonDataSource jsonDataSource = new JsonDataSource();
          jsonDataSource.JsonSource = new UriJsonSource(new Uri(jsonPath, UriKind.Absolute));
          jsonDataSource.Fill();

          report.DataSource = jsonDataSource;
          report.DataMember = originalDataMember;

          mdiController.OpenReport(report);
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Error al abrir el reporte {filePath}:\n{ex.Message}");
        }
      }
      designForm.ShowDialog();
    }
  }
}
