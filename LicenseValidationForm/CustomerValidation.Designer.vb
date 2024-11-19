Imports System.Windows.Forms
Imports System

Namespace LicenseVerification
    Partial Class CustomerValidation
        Private components As System.ComponentModel.IContainer = Nothing

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub InitializeComponent()
            Me.txtLicenseKey = New System.Windows.Forms.TextBox()
            Me.rbOnline = New System.Windows.Forms.RadioButton()
            Me.rbOffline = New System.Windows.Forms.RadioButton()
            Me.btnValidate = New System.Windows.Forms.Button()
            Me.cmbCompanies = New System.Windows.Forms.ComboBox()
            Me.btnNext = New System.Windows.Forms.Button()
            Me.lblLicenseKey = New System.Windows.Forms.Label()
            Me.lblSelectConnection = New System.Windows.Forms.Label()
            Me.lblSelectCompany = New System.Windows.Forms.Label()
            Me.SuspendLayout()

            ' 
            ' lblLicenseKey
            ' 
            Me.lblLicenseKey.AutoSize = True
            Me.lblLicenseKey.Font = New System.Drawing.Font("Segoe UI", 12.0F, System.Drawing.FontStyle.Bold)
            Me.lblLicenseKey.ForeColor = System.Drawing.Color.DarkSlateGray
            Me.lblLicenseKey.Location = New System.Drawing.Point(50, 40)
            Me.lblLicenseKey.Name = "lblLicenseKey"
            Me.lblLicenseKey.Size = New System.Drawing.Size(126, 21)
            Me.lblLicenseKey.TabIndex = 0
            Me.lblLicenseKey.Text = "Enter License Key:"
            ' 
            ' txtLicenseKey
            ' 
            Me.txtLicenseKey.Font = New System.Drawing.Font("Segoe UI", 12.0F)
            Me.txtLicenseKey.Location = New System.Drawing.Point(50, 70)
            Me.txtLicenseKey.Name = "txtLicenseKey"
            Me.txtLicenseKey.Size = New System.Drawing.Size(250, 29)
            Me.txtLicenseKey.TabIndex = 1
            ' 
            ' lblSelectConnection
            ' 
            Me.lblSelectConnection.AutoSize = True
            Me.lblSelectConnection.Font = New System.Drawing.Font("Segoe UI", 12.0F, System.Drawing.FontStyle.Bold)
            Me.lblSelectConnection.ForeColor = System.Drawing.Color.DarkSlateGray
            Me.lblSelectConnection.Location = New System.Drawing.Point(50, 110)
            Me.lblSelectConnection.Name = "lblSelectConnection"
            Me.lblSelectConnection.Size = New System.Drawing.Size(220, 21)
            Me.lblSelectConnection.TabIndex = 2
            Me.lblSelectConnection.Text = "Select Connection Type:"
            ' 
            ' rbOnline
            ' 
            Me.rbOnline.AutoSize = True
            Me.rbOnline.Font = New System.Drawing.Font("Segoe UI", 12.0F)
            Me.rbOnline.Location = New System.Drawing.Point(50, 140)
            Me.rbOnline.Name = "rbOnline"
            Me.rbOnline.Size = New System.Drawing.Size(76, 25)
            Me.rbOnline.TabIndex = 3
            Me.rbOnline.TabStop = True
            Me.rbOnline.Text = "Online"
            Me.rbOnline.UseVisualStyleBackColor = True
            ' 
            ' rbOffline
            ' 
            Me.rbOffline.AutoSize = True
            Me.rbOffline.Font = New System.Drawing.Font("Segoe UI", 12.0F)
            Me.rbOffline.Location = New System.Drawing.Point(150, 140)
            Me.rbOffline.Name = "rbOffline"
            Me.rbOffline.Size = New System.Drawing.Size(79, 25)
            Me.rbOffline.TabIndex = 4
            Me.rbOffline.TabStop = True
            Me.rbOffline.Text = "Offline"
            Me.rbOffline.UseVisualStyleBackColor = True
            ' 
            ' btnValidate
            ' 
            Me.btnValidate.Font = New System.Drawing.Font("Segoe UI", 12.0F, System.Drawing.FontStyle.Bold)
            Me.btnValidate.BackColor = System.Drawing.Color.Teal
            Me.btnValidate.ForeColor = System.Drawing.Color.White
            Me.btnValidate.Location = New System.Drawing.Point(50, 180)
            Me.btnValidate.Name = "btnValidate"
            Me.btnValidate.Size = New System.Drawing.Size(250, 40)
            Me.btnValidate.TabIndex = 5
            Me.btnValidate.Text = "Validate License Key"
            Me.btnValidate.FlatStyle = FlatStyle.Flat
            ' Event handlers for mouse events
            AddHandler Me.btnValidate.MouseEnter, AddressOf Me.btnValidate_MouseEnter
            AddHandler Me.btnValidate.MouseLeave, AddressOf Me.btnValidate_MouseLeave
            AddHandler Me.btnValidate.Click, AddressOf Me.btnValidate_Click

            ' 
            ' lblSelectCompany
            ' 
            Me.lblSelectCompany.AutoSize = True
            Me.lblSelectCompany.Font = New System.Drawing.Font("Segoe UI", 12.0F, System.Drawing.FontStyle.Bold)
            Me.lblSelectCompany.ForeColor = System.Drawing.Color.DarkSlateGray
            Me.lblSelectCompany.Location = New System.Drawing.Point(50, 220)
            Me.lblSelectCompany.Name = "lblSelectCompany"
            Me.lblSelectCompany.Size = New System.Drawing.Size(140, 21)
            Me.lblSelectCompany.TabIndex = 6
            Me.lblSelectCompany.Text = "Select Your Company:"
            ' 
            ' cmbCompanies
            ' 
            Me.cmbCompanies.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCompanies.Enabled = False
            Me.cmbCompanies.Font = New System.Drawing.Font("Segoe UI", 12.0F)
            Me.cmbCompanies.FormattingEnabled = True
            Me.cmbCompanies.Location = New System.Drawing.Point(50, 250)
            Me.cmbCompanies.Name = "cmbCompanies"
            Me.cmbCompanies.Size = New System.Drawing.Size(250, 29)
            Me.cmbCompanies.TabIndex = 7
            ' 
            ' btnNext
            ' 
            Me.btnNext.Font = New System.Drawing.Font("Segoe UI", 12.0F, System.Drawing.FontStyle.Bold)
            Me.btnNext.BackColor = System.Drawing.Color.Teal
            Me.btnNext.ForeColor = System.Drawing.Color.White
            Me.btnNext.Location = New System.Drawing.Point(50, 290)
            Me.btnNext.Name = "btnNext"
            Me.btnNext.Size = New System.Drawing.Size(250, 40)
            Me.btnNext.TabIndex = 8
            Me.btnNext.Text = "Show Company Details"
            Me.btnNext.FlatStyle = FlatStyle.Flat
            ' Event handlers for mouse events
            AddHandler Me.btnNext.MouseEnter, AddressOf Me.btnNext_MouseEnter
            AddHandler Me.btnNext.MouseLeave, AddressOf Me.btnNext_MouseLeave
            AddHandler Me.btnNext.Click, AddressOf Me.btnNext_Click

            Me.ClientSize = New System.Drawing.Size(350, 370)
            Me.Controls.Add(Me.btnNext)
            Me.Controls.Add(Me.cmbCompanies)
            Me.Controls.Add(Me.lblSelectCompany)
            Me.Controls.Add(Me.btnValidate)
            Me.Controls.Add(Me.rbOffline)
            Me.Controls.Add(Me.rbOnline)
            Me.Controls.Add(Me.lblSelectConnection)
            Me.Controls.Add(Me.txtLicenseKey)
            Me.Controls.Add(Me.lblLicenseKey)
            Me.Name = "CustomerValidation"
            Me.Text = "Customer Validation"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.BackColor = System.Drawing.Color.WhiteSmoke
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub

        ' Mouse Enter / Leave event handlers
        Private Sub btnValidate_MouseEnter(ByVal sender As Object, ByVal e As EventArgs)
            Dim btn As Button = CType(sender, Button)
            btn.BackColor = System.Drawing.Color.DarkSlateGray
        End Sub

        Private Sub btnValidate_MouseLeave(ByVal sender As Object, ByVal e As EventArgs)
            Dim btn As Button = CType(sender, Button)
            btn.BackColor = System.Drawing.Color.Teal
        End Sub

        Private Sub btnNext_MouseEnter(ByVal sender As Object, ByVal e As EventArgs)
            Dim btn As Button = CType(sender, Button)
            btn.BackColor = System.Drawing.Color.DarkSlateGray
        End Sub

        Private Sub btnNext_MouseLeave(ByVal sender As Object, ByVal e As EventArgs)
            Dim btn As Button = CType(sender, Button)
            btn.BackColor = System.Drawing.Color.Teal
        End Sub

        ' Declarations for controls
        Private txtLicenseKey As System.Windows.Forms.TextBox
        Private rbOnline As System.Windows.Forms.RadioButton
        Private rbOffline As System.Windows.Forms.RadioButton
        Private btnValidate As System.Windows.Forms.Button
        Private cmbCompanies As System.Windows.Forms.ComboBox
        Private btnNext As System.Windows.Forms.Button
        Private lblLicenseKey As System.Windows.Forms.Label
        Private lblSelectConnection As System.Windows.Forms.Label
        Private lblSelectCompany As System.Windows.Forms.Label
    End Class
End Namespace
