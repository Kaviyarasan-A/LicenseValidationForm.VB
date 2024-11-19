Imports System.IO
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports System.Xml.Linq

Namespace LicenseVerification
    Public Class CustomerValidation
        Inherits Form

        ' Constant for the local XML file path
        Private Const localXmlFilePath As String = "C:\Users\aruch\OneDrive\Documents\Customers.xml"

        ' Constructor
        Public Sub New()
            InitializeComponent()
        End Sub

        ' Validate the license key and fetch customer data
        Private Async Sub btnValidate_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim licenseKey = txtLicenseKey.Text.Trim()
            Dim connectionType = If(rbOnline.Checked, "Online", "Offline")

            If String.IsNullOrEmpty(licenseKey) OrElse String.IsNullOrEmpty(connectionType) Then
                ShowMessage("Please enter License Key and select a connection type.")
                LogValidationResult(licenseKey, connectionType, False, "License Key or connection type is empty.")
                Return
            End If

            Try
                ' Fetch customer data from the API or XML
                Dim customerData = Await GetCustomerAndCompaniesAsync(licenseKey, connectionType)
                cmbCompanies.DataSource = Nothing

                If customerData IsNot Nothing AndAlso customerData.Companies.Any() Then
                    ' If only one company, navigate to the next page
                    If customerData.Companies.Count = 1 Then
                        Dim selectedCompany = customerData.Companies.First()
                        NavigateToNextPage(selectedCompany)
                        LogValidationResult(licenseKey, connectionType, True, "Validation successful with one company.")
                    Else
                        ' If multiple companies, populate combo box
                        cmbCompanies.DataSource = customerData.Companies
                        cmbCompanies.DisplayMember = "CompanyName"
                        cmbCompanies.ValueMember = "CompanyId"
                        cmbCompanies.Enabled = True
                        LogValidationResult(licenseKey, connectionType, True, "Validation successful with multiple companies.")
                    End If
                Else
                    cmbCompanies.Enabled = False
                    LogValidationResult(licenseKey, connectionType, False, "No companies found for the customer.")
                End If

                ' If online, save the customer data locally
                If connectionType = "Online" Then
                    If IsCustomerDataExistsInXml(customerData.Customer.CustomerId) Then
                        ShowMessage("Customer data already exists locally. Please verify offline.")
                        LogValidationResult(licenseKey, connectionType, False, "Customer data already exists locally.")
                        rbOffline.Checked = True
                    Else
                        SaveCustomerDataToXml(customerData)
                        LogValidationResult(licenseKey, connectionType, True, "Customer data saved to XML.")
                    End If
                End If

            Catch ex As Exception
                LogError(ex)
                ShowMessage("An error occurred while processing your request. Please confirm that your internet is connected.")
                LogValidationResult(licenseKey, connectionType, False, $"Error during validation: {ex.Message}")
            End Try
        End Sub

        ' Fetch customer and company data based on the connection type (online or offline)
        Private Async Function GetCustomerAndCompaniesAsync(ByVal licenseKey As String, ByVal connectionType As String) As Task(Of CustomerWithCompanies)
            If connectionType = "Online" Then
                Return Await FetchCustomerDataFromApiAsync(licenseKey)
            Else
                Return FetchCustomerDataFromXml(licenseKey)
            End If
        End Function

        ' Fetch customer data from the API (online)
        Private Async Function FetchCustomerDataFromApiAsync(ByVal licenseKey As String) As Task(Of CustomerWithCompanies)
            Try
                Dim apiUrl = $"https://localhost:44368/api/Customer/GetCustomerWithCompanies/{licenseKey}"

                Using client = New HttpClient()
                    client.Timeout = TimeSpan.FromSeconds(30)
                    Dim response = Await client.GetAsync(apiUrl)

                    If response.IsSuccessStatusCode Then
                        Dim responseData = Await response.Content.ReadAsStringAsync()
                        Return JsonConvert.DeserializeObject(Of CustomerWithCompanies)(responseData)
                    Else
                        ShowMessage($"The LicenseKey has not been found. Please enter a valid license key. {response.StatusCode}")
                        Return Nothing
                    End If
                End Using

            Catch ex As Exception
                LogError(ex)
                ShowMessage("An error occurred while connecting to the server. Please check your internet connection and try again.")
                Return Nothing
            End Try
        End Function

        ' Fetch customer data from local XML (offline)
        Private Function FetchCustomerDataFromXml(ByVal licenseKey As String) As CustomerWithCompanies
            If Not File.Exists(localXmlFilePath) Then
                ShowMessage("No local data found.")
                Return Nothing
            End If

            Try
                Dim doc = LoadOrCreateXml()
                Dim customerElement = doc.Descendants("Customer").FirstOrDefault(Function(c) c.Element("LicenseKey")?.Value = licenseKey)

                If customerElement Is Nothing Then
                    ShowMessage("Customer not found in local data. Please connect online and fetch details.")
                    Return Nothing
                End If

                Dim customer = New CustomerHeader With {
                    .CustomerId = Long.Parse(customerElement.Element("CustomerId").Value),
                    .CustomerName = customerElement.Element("CustomerName").Value
                }
                Dim companies = customerElement.Elements("Companies").Elements("Company").[Select](Function(c) New CustomerDetails With {
                    .CompanyId = Integer.Parse(c.Element("CompanyId").Value),
                    .CompanyName = c.Element("CompanyName").Value
                }).ToList()

                If Not companies.Any() Then
                    ShowMessage("No companies found for this customer.")
                End If

                Return New CustomerWithCompanies With {
                    .Customer = customer,
                    .Companies = companies
                }
            Catch ex As Exception
                LogError(ex)
                ShowMessage("An error occurred while reading the local data. Please try again.")
                Return Nothing
            End Try
        End Function

        ' Load or create the XML file for customers
        Private Function LoadOrCreateXml() As XDocument
            Dim doc As XDocument

            If File.Exists(localXmlFilePath) Then
                doc = XDocument.Load(localXmlFilePath)
            Else
                doc = New XDocument(New XElement("Customers"))
                doc.Save(localXmlFilePath)
            End If

            Return doc
        End Function

        ' Save customer data to local XML file
        Private Sub SaveCustomerDataToXml(ByVal customerData As CustomerWithCompanies)
            Try
                Dim doc = LoadOrCreateXml()
                Dim existingCustomer = doc.Descendants("Customer").FirstOrDefault(Function(c) c.Element("CustomerId")?.Value = customerData.Customer.CustomerId.ToString())

                If existingCustomer IsNot Nothing Then
                    ShowMessage("Customer data already exists in the XML file.")
                    Return
                End If

                Dim customerElement As XElement = New XElement("Customer", New XElement("CustomerId", customerData.Customer.CustomerId), New XElement("CustomerName", customerData.Customer.CustomerName), New XElement("LicenseKey", customerData.Customer.LicenseKey), New XElement("Companies", customerData.Companies.[Select](Function(c) New XElement("Company", New XElement("CompanyId", c.CompanyId), New XElement("CompanyName", c.CompanyName)))))
                doc.Root?.Add(customerElement)
                doc.Save(localXmlFilePath)
                ShowMessage("Customer data saved locally.")
            Catch ex As Exception
                LogError(ex)
                ShowMessage("An error occurred while saving the data. Please try again.")
            End Try
        End Sub

        ' Check if the customer data exists in the local XML file
        Private Function IsCustomerDataExistsInXml(ByVal customerId As Long) As Boolean
            Try
                Dim doc = LoadOrCreateXml()
                Dim existingCustomer = doc.Descendants("Customer").FirstOrDefault(Function(c) c.Element("CustomerId")?.Value = customerId.ToString())
                Return existingCustomer IsNot Nothing
            Catch ex As Exception
                LogError(ex)
                ShowMessage("An error occurred while checking the customer data. Please try again.")
                Return False
            End Try
        End Function

        ' Show a message to the user
        Private Sub ShowMessage(ByVal message As String)
            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        ' Log validation results
        Private Sub LogValidationResult(ByVal licenseKey As String, ByVal connectionType As String, ByVal success As Boolean, ByVal message As String)
            Dim logDirectory As String = "C:\Users\aruch\OneDrive\Documents\Log\validation_log.txt"
            Dim logFilePath As String = Path.Combine(logDirectory, "validation_log.txt")

            Try
                If Not Directory.Exists(logDirectory) Then
                    Directory.CreateDirectory(logDirectory)
                End If

                Dim logMessage As String = $"[{DateTime.Now}] - LicenseKey: {licenseKey}, ConnectionType: {connectionType}, Success: {success}, Message: {message}\n"
                File.AppendAllText(logFilePath, logMessage)
            Catch ex As Exception
                LogError(ex)
            End Try
        End Sub

        ' Log errors
        Private Sub LogError(ByVal ex As Exception)
            Try
                Dim logDirectory As String = "C:\Users\aruch\OneDrive\Documents\Log\error_log.txt"
                Dim logFilePath As String = Path.Combine(logDirectory, "error_log.txt")

                If Not Directory.Exists(logDirectory) Then
                    Directory.CreateDirectory(logDirectory)
                End If

                Dim logMessage As String = $"[{DateTime.Now}] - Exception: {ex.Message}\n" & $"Stack Trace: {ex.StackTrace}\n" & $"Source: {ex.Source}\n" & $"Inner Exception: {ex.InnerException?.Message}\n" & New String("-"c, 50) & vbLf
                File.AppendAllText(logFilePath, logMessage)
            Catch loggingEx As Exception
                MessageBox.Show("An error occurred while logging the details. Please contact support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End Try
        End Sub

        ' Navigate to the next page (company details form)
        Private Sub NavigateToNextPage(ByVal selectedCompany As CustomerDetails)
            ' Pass both companyId and companyName to the CompanyDetailsForm constructor
            Dim companyDetailsForm As New CompanyDetailsForm(selectedCompany.CompanyId, selectedCompany.CompanyName)
            companyDetailsForm.Show()
        End Sub

        ' Handle Next button click
        Private Sub btnNext_Click(ByVal sender As Object, ByVal e As EventArgs)
            If cmbCompanies.SelectedItem IsNot Nothing Then
                Dim selectedCompany = CType(cmbCompanies.SelectedItem, CustomerDetails)
                ' Pass both companyId and companyName to the CompanyDetailsForm constructor
                Dim companyDetailsForm As New CompanyDetailsForm(selectedCompany.CompanyId, selectedCompany.CompanyName)
                companyDetailsForm.Show()
            Else
                ShowMessage("Please select a company.")
            End If
        End Sub
    End Class

    ' Data Models
    Public Class CustomerWithCompanies
        Public Property Customer As CustomerHeader
        Public Property Companies As List(Of CustomerDetails)
    End Class

    Public Class CustomerHeader
        Public Property CustomerId As Long
        Public Property CustomerName As String
        Public Property LicenseKey As String
    End Class

    Public Class CustomerDetails
        Public Property CompanyId As Integer
        Public Property CompanyName As String
    End Class
End Namespace
