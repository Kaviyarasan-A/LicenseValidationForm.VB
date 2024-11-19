Imports System
Imports System.ComponentModel
Imports System.Windows.Forms

Namespace LicenseVerification
    Partial Class CompanyDetailsForm
        Inherits Form

        ' Controls for the form
        Private components As IContainer = Nothing
        Private lblCompanyName As Label
        Private lblCompanyId As Label
        Private lblCustomerId As Label
        Private lblConnectionStringOnline As Label
        Private lblConnectionStringOffline As Label
        Private btnClose As Button

        ' Constructor to initialize the form
        Public Sub New(ByVal companyName As String)
            InitializeComponent()
            _companyName = companyName
        End Sub

        ' Dispose method to clean up resources
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        ' Initialize the components (controls)
        Private Sub InitializeComponent()
            Me.lblCompanyName = New Label()
            Me.lblCompanyId = New Label()
            Me.lblCustomerId = New Label()
            Me.lblConnectionStringOnline = New Label()
            Me.lblConnectionStringOffline = New Label()
            Me.btnClose = New Button()

            ' Set properties for lblCompanyName
            Me.lblCompanyName.AutoSize = True
            Me.lblCompanyName.Font = New Font("Segoe UI", 20.0F, FontStyle.Bold)
            Me.lblCompanyName.ForeColor = Color.DarkSlateGray
            Me.lblCompanyName.Location = New Point(50, 80)
            Me.lblCompanyName.Name = "lblCompanyName"
            Me.lblCompanyName.Size = New Size(300, 37)
            Me.lblCompanyName.TabIndex = 0
            Me.lblCompanyName.Text = "Company Name: ABC Ltd"
            Me.lblCompanyName.TextAlign = ContentAlignment.MiddleLeft

            ' Set properties for lblCompanyId
            Me.lblCompanyId.AutoSize = True
            Me.lblCompanyId.Font = New Font("Segoe UI", 18.0F, FontStyle.Regular)
            Me.lblCompanyId.ForeColor = Color.DarkSlateGray
            Me.lblCompanyId.Location = New Point(50, 140)
            Me.lblCompanyId.Name = "lblCompanyId"
            Me.lblCompanyId.Size = New Size(250, 32)
            Me.lblCompanyId.TabIndex = 1
            Me.lblCompanyId.Text = "Company Id: 12345"
            Me.lblCompanyId.TextAlign = ContentAlignment.MiddleLeft

            ' Set properties for lblConnectionStringOnline
            Me.lblConnectionStringOnline.AutoSize = True
            Me.lblConnectionStringOnline.Font = New Font("Segoe UI", 16.0F, FontStyle.Regular)
            Me.lblConnectionStringOnline.ForeColor = Color.DarkSlateGray
            Me.lblConnectionStringOnline.Location = New Point(50, 210)
            Me.lblConnectionStringOnline.Name = "lblConnectionStringOnline"
            Me.lblConnectionStringOnline.Size = New Size(500, 30)
            Me.lblConnectionStringOnline.TabIndex = 3
            Me.lblConnectionStringOnline.Text = "Connection String (Online): online_12345"
            Me.lblConnectionStringOnline.TextAlign = ContentAlignment.MiddleLeft

            ' Set properties for lblConnectionStringOffline
            Me.lblConnectionStringOffline.AutoSize = True
            Me.lblConnectionStringOffline.Font = New Font("Segoe UI", 16.0F, FontStyle.Regular)
            Me.lblConnectionStringOffline.ForeColor = Color.DarkSlateGray
            Me.lblConnectionStringOffline.Location = New Point(50, 270)
            Me.lblConnectionStringOffline.Name = "lblConnectionStringOffline"
            Me.lblConnectionStringOffline.Size = New Size(500, 30)
            Me.lblConnectionStringOffline.TabIndex = 4
            Me.lblConnectionStringOffline.Text = "Connection String (Offline): offline_67890"
            Me.lblConnectionStringOffline.TextAlign = ContentAlignment.MiddleLeft

            ' Set properties for btnClose
            Me.btnClose.Font = New Font("Segoe UI", 16.0F, FontStyle.Bold)
            Me.btnClose.BackColor = Color.DarkSlateGray
            Me.btnClose.ForeColor = Color.White
            Me.btnClose.FlatStyle = FlatStyle.Flat
            Me.btnClose.Location = New Point(380, 350)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New Size(140, 60)
            Me.btnClose.TabIndex = 6
            Me.btnClose.Text = "Close"
            Me.btnClose.UseVisualStyleBackColor = False

            ' Wire up event handlers using AddressOf
            AddHandler Me.Load, AddressOf Me.CompanyDetailsForm_Load
            AddHandler Me.btnClose.Click, AddressOf Me.btnClose_Click

            ' Set form properties
            Me.ClientSize = New Size(1000, 600)
            Me.Controls.Add(Me.btnClose)
            Me.Controls.Add(Me.lblConnectionStringOffline)
            Me.Controls.Add(Me.lblConnectionStringOnline)
            Me.Controls.Add(Me.lblCompanyId)
            Me.Controls.Add(Me.lblCompanyName)
            Me.Name = "CompanyDetailsForm"
            Me.Text = "Company Details"
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.BackColor = Color.FromArgb(255, 255, 240)
            Me.WindowState = FormWindowState.Maximized
            Me.FormBorderStyle = FormBorderStyle.None

            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub
    End Class
End Namespace
