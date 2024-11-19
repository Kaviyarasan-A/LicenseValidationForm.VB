Imports System
Imports System.IO
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports System.Xml.Linq
Imports System.Linq

Namespace LicenseVerification
    Partial Public Class CompanyDetailsForm
        Inherits Form

        ' Private members to store company details
        Private _companyId As Integer
        Private _companyName As String

        ' Local path for storing the XML file for company details
        Private Const LocalXmlPath As String = "C:\Users\aruch\OneDrive\Documents\CompanyDetails.xml"

        ' Constructor to initialize the form with both companyId and companyName
        Public Sub New(companyId As Integer, companyName As String)
            InitializeComponent()  ' This should only be called once in your constructor
            _companyId = companyId
            _companyName = companyName
        End Sub

        ' Form load event handler
        Private Async Sub CompanyDetailsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Try
                ' Try to fetch company details from local XML first
                Dim companyDetails = FetchCompanyDetailsFromXml(_companyName)

                If companyDetails IsNot Nothing Then
                    ' Display company details if found in local XML
                    DisplayCompanyDetails(companyDetails)
                Else
                    ' Fetch from API if not found locally
                    companyDetails = Await FetchCompanyDetailsAsync(_companyName)

                    If companyDetails IsNot Nothing Then
                        ' Display fetched details
                        DisplayCompanyDetails(companyDetails)

                        ' Save to XML for future use
                        SaveCompanyDetailsToXml(companyDetails)
                        SaveConnectionStringsToXml(companyDetails)
                    Else
                        ' Display a friendly message when company details are not found
                        MessageBox.Show("Company details could not be found.")
                    End If
                End If
            Catch ex As Exception
                ' Display only a general error message, suppressing exception details
                MessageBox.Show("An error occurred while processing the request. Please try again.")
            End Try
        End Sub

        ' Method to display company details on the form
        Private Sub DisplayCompanyDetails(companyDetails As CompanyDetails)
            lblCompanyName.Text = $"Company Name: {companyDetails.CompanyName}"
            lblCompanyId.Text = $"Company Id: {companyDetails.CompanyId}"
            lblConnectionStringOnline.Text = $"Connection String (Online): {companyDetails.ConnectionStringOnline}"
            lblConnectionStringOffline.Text = $"Connection String (Offline): {companyDetails.ConnectionStringOffline}"
        End Sub

        ' Fetch company details from a remote API asynchronously
        Private Async Function FetchCompanyDetailsAsync(companyName As String) As Task(Of CompanyDetails)
            Dim apiUrl = $"https://localhost:44368/api/Customer/GetCompanyDetailsByName/{companyName}"

            Using client = New HttpClient()
                Try
                    ' Perform HTTP GET request to fetch company details
                    Dim response = Await client.GetStringAsync(apiUrl)
                    Return JsonConvert.DeserializeObject(Of CompanyDetails)(response)
                Catch httpEx As HttpRequestException
                    ' Display a user-friendly message if the HTTP request fails
                    MessageBox.Show("Error fetching company details from the server. Please check your connection.")
                    Return Nothing
                End Try
            End Using
        End Function

        ' Fetch company details from a local XML file
        Private Function FetchCompanyDetailsFromXml(companyName As String) As CompanyDetails
            If Not File.Exists(LocalXmlPath) Then
                Return Nothing
            End If

            Try
                ' Load XML and search for the company by name
                Dim doc = XDocument.Load(LocalXmlPath)
                Dim companyElement = doc.Descendants("Company").FirstOrDefault(Function(c) c.Element("CompanyName")?.Value = companyName)

                If companyElement Is Nothing Then
                    Return Nothing
                End If

                ' Create CompanyDetails object from XML data
                Dim companyDetails = New CompanyDetails With {
                    .CustomerId = Integer.Parse(companyElement.Element("CustomerId")?.Value),
                    .CompanyId = Integer.Parse(companyElement.Element("CompanyId")?.Value),
                    .CompanyName = companyElement.Element("CompanyName")?.Value,
                    .ConnectionStringOnline = companyElement.Element("ConnectionStringOnline")?.Value,
                    .ConnectionStringOffline = companyElement.Element("ConnectionStringOffline")?.Value,
                    .LicenseKey = companyElement.Element("LicenseKey")?.Value
                }
                Return companyDetails
            Catch ex As Exception
                ' Display a friendly message if an error occurs while reading the XML
                MessageBox.Show("An error occurred while reading the local company details.")
                Return Nothing
            End Try
        End Function

        ' Save company details to the local XML file
        Private Sub SaveCompanyDetailsToXml(companyDetails As CompanyDetails)
            Try
                ' Load or create XML document
                Dim doc As XDocument = If(File.Exists(LocalXmlPath), XDocument.Load(LocalXmlPath), New XDocument(New XElement("Companies")))

                ' Check if the company already exists in the XML
                Dim existingCompany = doc.Descendants("Company").FirstOrDefault(Function(c) c.Element("CompanyName")?.Value = companyDetails.CompanyName)

                If existingCompany IsNot Nothing Then
                    MessageBox.Show("Company details already exist in the local file.")
                    Return
                End If

                ' Create new XElement for the company
                Dim newCompanyElement As XElement = New XElement("Company",
                    New XElement("CustomerId", companyDetails.CustomerId),
                    New XElement("CompanyId", companyDetails.CompanyId),
                    New XElement("CompanyName", companyDetails.CompanyName),
                    New XElement("ConnectionStringOnline", companyDetails.ConnectionStringOnline),
                    New XElement("ConnectionStringOffline", companyDetails.ConnectionStringOffline),
                    New XElement("LicenseKey", companyDetails.LicenseKey))

                ' Add the new company element and save to file
                doc.Root?.Add(newCompanyElement)
                doc.Save(LocalXmlPath)
                MessageBox.Show("Company details saved locally.")
            Catch ex As Exception
                ' Display a friendly message if an error occurs while saving the XML
                MessageBox.Show("An error occurred while saving company details.")
            End Try
        End Sub

        ' Save connection strings to a separate XML file
        Private Sub SaveConnectionStringsToXml(companyDetails As CompanyDetails)
            Try
                ' Define the path for saving the connection strings
                Dim documentsFolder As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                Dim connectionStringXmlPath As String = Path.Combine(documentsFolder, "ConnectionStrings.xml")

                ' Create an XML document for the connection strings
                Dim doc As XDocument = New XDocument(New XElement("configuration",
                    New XElement("connectionStrings",
                        New XElement("add", New XAttribute("name", "ERP.My.MySettings.ERPConnectionString"),
                                         New XAttribute("connectionString", companyDetails.ConnectionStringOnline),
                                         New XAttribute("providerName", "System.Data.SqlClient"))),
                    New XElement("add", New XAttribute("name", "ERP.My.MySettings.IMSdbConnectionString"),
                                         New XAttribute("connectionString", companyDetails.ConnectionStringOffline),
                                         New XAttribute("providerName", "System.Data.SqlClient"))
                ))

                ' Save the connection strings XML to the documents folder
                doc.Save(connectionStringXmlPath)
                MessageBox.Show("Connection strings saved successfully.")
            Catch ex As Exception
                ' Display a friendly message if an error occurs while saving connection strings
                MessageBox.Show("An error occurred while saving connection strings.")
            End Try
        End Sub

        ' Close button event handler to close the form
        Private Sub btnClose_Click(sender As Object, e As EventArgs)
            Me.Close()
        End Sub
    End Class

    ' CompanyDetails class to hold company information
    Public Class CompanyDetails
        Public Property CustomerId As Integer
        Public Property CompanyId As Integer
        Public Property CompanyName As String
        Public Property ConnectionStringOnline As String
        Public Property ConnectionStringOffline As String
        Public Property LicenseKey As String
    End Class
End Namespace
