'Imports System.ComponentModel
'Imports System.Reflection
'Imports System.Globalization
'Imports System.Data

'Public Class CustomerManager


'    ''' <summary>
'    ''' TypeConverter для Enum, преобразовывающий Enum к строке с
'    ''' учетом атрибута Description
'    ''' </summary>
'    Public Class EnumTypeConverter
'        Inherits EnumConverter
'        Private _EnumType As Type
'        ''' <summary>
'        ''' Инициализирует экземпляр
'        ''' </summary>
'        ''' <param name="type">тип Enum</param>
'        Public Sub New(ByVal type As Type)
'            MyBase.New(type)
'            _EnumType = type
'        End Sub
'        Public Overloads Overrides Function CanConvertTo(ByVal context As ITypeDescriptorContext, ByVal destType As Type) As Boolean
'            Return destType Is GetType(String)
'        End Function
'        Public Overloads Overrides Function ConvertTo(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object, ByVal destType As Type) As Object
'            Dim fi As FieldInfo = _EnumType.GetField([Enum].GetName(_EnumType, value))
'            Dim dna As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)
'            If dna IsNot Nothing Then
'                Return dna.Description
'            Else
'                Return value.ToString()
'            End If
'        End Function
'        Public Overloads Overrides Function CanConvertFrom(ByVal context As ITypeDescriptorContext, ByVal srcType As Type) As Boolean
'            Return srcType Is GetType(String)
'        End Function
'        Public Overloads Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
'            For Each fi As FieldInfo In _EnumType.GetFields()
'                Dim dna As DescriptionAttribute = DirectCast(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)
'                If (dna IsNot Nothing) AndAlso (DirectCast(value, String) = dna.Description) Then
'                    Return [Enum].Parse(_EnumType, fi.Name)
'                End If
'            Next
'            Return [Enum].Parse(_EnumType, DirectCast(value, String))
'        End Function
'    End Class

'    Public Class MyUserControl
'        Public Enum EnumTypeOfControls
'            <Description("Используется TextBox")> _
'            ParameterTuning
'            <Description("Используется ListBox")> _
'            StageTuning
'        End Enum

'        Public Shared Function GetDescriptionAttribute(ByVal value As EnumTypeOfControls) As String
'            Dim _EnumType As Type = GetType(EnumTypeOfControls)
'            'Dim strDescription As String
'            Dim fi As FieldInfo = _EnumType.GetField([Enum].GetName(_EnumType, value))
'            Dim dna As DescriptionAttribute = CType(Attribute.GetCustomAttribute(fi, GetType(DescriptionAttribute)), DescriptionAttribute)
'            If dna IsNot Nothing Then
'                Return dna.Description
'            Else
'                Return value.ToString()
'            End If
'        End Function
'    End Class


'    Public Interface IUserControl
'        'Event SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
'        Property Name() As String
'        Property Text() As String
'        ReadOnly Property GetDescriptionAttribute() As String
'#Region "Control Properties"
'        ReadOnly Property TypeOfControls() As Type

'        <TypeConverter(GetType(EnumTypeConverter))> _
'       ReadOnly Property EnumOfType() As MyUserControl.EnumTypeOfControls

'        Property МестоНаПанели() As Integer
'        Property Описание() As String
'        Property InputOrOutput() As Boolean
'        Property Value() As String
'        Property Query() As String
'        Property ЛогическоеЗначение() As Boolean
'        Property ЗначениеПользователя() As String
'        Sub EraseValue()
'        Function ValidatedUserValue() As Boolean
'        Property keyКонтролДляУровня() As Integer
'        Property Row() As Integer
'        Property Col() As Integer

'#End Region
'    End Interface

'****************************************
'Пример Obgect Binding
'Public Shared Function GetAllCustomers() As Customers
'    Dim customerList As New Customers()
'    customerList.Add(New Customer("BOTTM", "Bottom-Dollar Markets", "Tsawassen", "BC", "Canada"))
'    customerList(0).Orders.Add(New Order(1, "BOTTM", #1/4/2004#, #1/11/2004#))
'    customerList(0).Orders.Add(New Order(2, "BOTTM", #2/9/2004#, #2/16/2004#))
'    customerList(0).Orders.Add(New Order(3, "BOTTM", #3/2/2004#, #3/7/2004#))

'    customerList.Add(New Customer("GOURL", "Gourmet Lanchonetes", "Campinas", "SP", "Brazil"))
'    customerList(1).Orders.Add(New Order(4, "GOURL", #4/7/2004#, #4/14/2004#))
'    customerList(1).Orders.Add(New Order(5, "GOURL", #5/1/2004#, #5/14/2004#))
'    customerList(1).Orders.Add(New Order(6, "GOURL", #6/20/2004#, #6/23/2004#))


'    customerList.Add(New Customer("GREAL", "Great Lakes Food Market", "Eugene", "OR", "USA"))
'    customerList(2).Orders.Add(New Order(7, "GREAL", #7/7/2004#, #7/14/2004#))
'    customerList(2).Orders.Add(New Order(8, "GREAL", #8/1/2004#, #8/14/2004#))
'    customerList(2).Orders.Add(New Order(9, "GREAL", #9/20/2004#, #9/23/2004#))

'    Return customerList
'End Function

'Public Shared Function GetNewCustomer() As Customer
'    Return New Customer("CustID", "CompanyName", "City", "Region", "Country")
'End Function

'создан в модуле класс

'' Copyright (c) Microsoft Corporation. All rights reserved.
'''' <summary>
'''' A single customer
'''' </summary>
'Public Class Customer

'    ''' <summary>
'    ''' Creates a new customer
'    ''' </summary>
'    ''' <param name="customerId">The ID that uniquely identifies the customer</param>
'    ''' <param name="companyName">The name of this customer</param>
'    ''' <param name="city">The city where this customer is located</param>
'    ''' <param name="region">The region where this customer is located</param>
'    ''' <param name="country">The country where this customer is located</param>
'    Public Sub New(ByVal customerId As String, ByVal companyName As String, ByVal city As String, ByVal region As String, ByVal country As String)
'        Initialize()
'        customerIDValue = customerId
'        companyNameValue = companyName
'        cityValue = city
'        regionValue = region
'        countryValue = country
'    End Sub

'    Private Sub Initialize()
'        ordersValue = New Orders()
'    End Sub

'    Private customerIDValue As String
'    ''' <summary>
'    ''' Identifier for the customer
'    ''' </summary>
'    Public Property CustomerID() As String
'        Get
'            Return customerIDValue
'        End Get
'        Set(ByVal value As String)
'            customerIDValue = value
'        End Set
'    End Property

'    Private companyNameValue As String
'    ''' <summary>
'    ''' The name of this customer
'    ''' </summary>
'    Public Property CompanyName() As String
'        Get
'            Return companyNameValue
'        End Get
'        Set(ByVal Value As String)
'            companyNameValue = Value
'        End Set
'    End Property


'    Private cityValue As String
'    ''' <summary>
'    ''' The city where this customer is located
'    ''' </summary>
'    Public Property City() As String
'        Get
'            Return cityValue
'        End Get
'        Set(ByVal Value As String)
'            cityValue = Value
'        End Set
'    End Property

'    Private regionValue As String
'    ''' <summary>
'    ''' The region where this customer is located
'    ''' </summary>
'    Public Property Region() As String
'        Get
'            Return regionValue
'        End Get
'        Set(ByVal Value As String)
'            regionValue = Value
'        End Set
'    End Property

'    Private countryValue As String
'    ''' <summary>
'    ''' The country where this customer is located
'    ''' </summary>
'    Public Property Country() As String
'        Get
'            Return countryValue
'        End Get
'        Set(ByVal Value As String)
'            countryValue = Value
'        End Set
'    End Property

'    Private WithEvents ordersValue As Orders
'    ''' <summary>
'    ''' The orders for this customer
'    ''' </summary>
'    Public Property Orders() As Orders
'        Get
'            Return ordersValue
'        End Get
'        Set(ByVal value As Orders)
'            ordersValue = value
'        End Set
'    End Property

'    Private Sub ordersValue_AddingNew(ByVal sender As Object, ByVal e As System.ComponentModel.AddingNewEventArgs) Handles ordersValue.AddingNew
'        e.NewObject = New Order(999, Me.CustomerID, Date.Today, DateAdd(DateInterval.Day, 30, Date.Today))
'    End Sub
'End Class

'''' <summary>
'''' A collection of Customers
'''' </summary>
'Public Class Customers
'    Inherits System.ComponentModel.BindingList(Of Customer)
'End Class

'где-то  в модуле класс Orders

'' Copyright (c) Microsoft Corporation. All rights reserved.
'''' <summary>
'''' A single order
'''' </summary>
'Public Class Order

'    Public Sub New()
'    End Sub

'    ''' <summary>
'    ''' Creates a new order
'    ''' </summary>
'    ''' <param name="orderid">The identifier for this order</param>
'    ''' <param name="customerID">The customer who placed this order</param>
'    ''' <param name="orderDate">The date the order was placed</param>
'    ''' <param name="shippedDate">The date the order was shipped</param>
'    Public Sub New(ByVal orderid As Integer, ByVal customerID As String, ByVal orderDate As DateTime, ByVal shippedDate As DateTime)
'        orderIDValue = orderid
'        customerIDValue = customerID
'        orderDateValue = orderDate
'        shippedDateValue = shippedDate
'    End Sub

'    Private orderIDValue As Integer
'    ''' <summary>
'    ''' Identifier for the order
'    ''' </summary>
'    Public ReadOnly Property OrderID() As Integer
'        Get
'            Return orderIDValue
'        End Get
'    End Property

'    Private customerIDValue As String
'    ''' <summary>
'    ''' The customer who placed this order
'    ''' </summary>
'    Public Property CustomerID() As String
'        Get
'            Return customerIDValue
'        End Get
'        Set(ByVal Value As String)
'            customerIDValue = Value
'        End Set
'    End Property

'    Private orderDateValue As DateTime
'    ''' <summary>
'    ''' The date the order was placed
'    ''' </summary>
'    Public Property OrderDate() As DateTime
'        Get
'            Return orderDateValue
'        End Get
'        Set(ByVal Value As DateTime)
'            orderDateValue = Value
'        End Set
'    End Property

'    Private shippedDateValue As DateTime
'    ''' <summary>
'    ''' The date the order was shipped
'    ''' </summary>
'    Public Property ShippedDate() As DateTime
'        Get
'            Return shippedDateValue
'        End Get
'        Set(ByVal Value As DateTime)
'            shippedDateValue = Value
'        End Set
'    End Property
'End Class

'''' <summary>
'''' A collection of Orders
'''' </summary>
'Public Class Orders
'    Inherits System.ComponentModel.BindingList(Of Order)
'End Class

'End Class



'*******************************************************
'Пример связывания элементов с таблицей посредством DataBindings
'Public Class Employee
'    ' Property variables       
'    Private employeeIdValue As Integer
'    Private firstNameValue As String
'    Private lastNameValue As String
'    Private titleValue As String
'    Private birthDateValue As DateTime
'    Private hireDateValue As DateTime
'    Private maritalStatusValue As String

'    ' Property accessors
'    Public ReadOnly Property EmployeeId() As Integer
'        Get
'            Return employeeIdValue
'        End Get
'    End Property

'    Public Property FirstName() As String
'        Get
'            Return firstNameValue
'        End Get
'        Set(ByVal value As String)
'            firstNameValue = value
'        End Set
'    End Property

'    Public Property LastName() As String
'        Get
'            Return lastNameValue
'        End Get
'        Set(ByVal value As String)
'            lastNameValue = value
'        End Set
'    End Property

'    Public Property Title() As String
'        Get
'            Return titleValue
'        End Get
'        Set(ByVal value As String)
'            titleValue = value
'        End Set
'    End Property

'    Public Property BirthDate() As DateTime
'        Get
'            Return birthDateValue
'        End Get
'        Set(ByVal value As DateTime)
'            birthDateValue = value
'        End Set
'    End Property

'    Public Property HireDate() As DateTime
'        Get
'            Return hireDateValue
'        End Get
'        Set(ByVal value As DateTime)
'            hireDateValue = value
'        End Set
'    End Property

'    Public Property MaritalStatus() As String
'        Get
'            Return maritalStatusValue
'        End Get
'        Set(ByVal value As String)
'            maritalStatusValue = value
'        End Set
'    End Property

'    Public Sub New()

'        ' Provide default values
'        employeeIdValue = 0
'        FirstName = "Enter first name."
'        LastName = "Enter last name."
'        Title = "Enter title."
'        BirthDate = DateTime.Today
'        HireDate = DateTime.Today
'        MaritalStatus = ""
'    End Sub

'    Public Sub New(ByVal employeeData As DataRow)

'        employeeIdValue = CInt(employeeData("EmployeeId"))
'        FirstName = CStr(employeeData("FirstName"))
'        LastName = CStr(employeeData("LastName"))
'        Title = CStr(employeeData("Title"))
'        BirthDate = CDate(employeeData("BirthDate"))
'        HireDate = CDate(employeeData("HireDate"))
'        MaritalStatus = CStr(employeeData("MaritalStatus"))
'    End Sub

'    Public Shared Function LoadEmployees(ByVal employeesData As DataTable) As System.Collections.Generic.List(Of Employee)

'        Dim employees As New List(Of Employee)()
'        For i As Integer = 0 To employeesData.Rows.Count - 1
'            Dim employeeData As DataRow = employeesData.Rows(i)
'            Dim newEmployee As New Employee(employeeData)
'            employees.Add(newEmployee)
'        Next

'        Return employees
'    End Function




''где-то в форме производится связывание элементов управления с employeesList
'' A DataSet of Employee data, used for the 2nd and 3rd tabs
'Private employeeDataSet As DataSet

'' A List of Employees, for use as a DataSource for binding on the 3rd tab
'Private employeesList As System.Collections.Generic.List(Of Employee)

'Private Sub SetupGrid3()
'    employeeDataSet = New DataSet
'    employeeDataSet.Tables.Add(New DataTable("Employee"))


'    ' Wrap the employee class around the employee data
'    Me.employeesList = Employee.LoadEmployees(Me.employeeDataSet.Tables("Employee"))

'    ' Create a BindingSource of employees
'    Dim classBindingSource As New BindingSource()
'    classBindingSource.DataSource = Me.employeesList
'    classBindingSource.AllowNew = False

'    ' Create a BindingNavigator for navigating through the Employees
'    Dim classBindingNavigator As New BindingNavigator(True)
'    classBindingNavigator.BindingSource = classBindingSource

'    ' Bind the controls to the BindingSource
'    Me.classBirthDateDateTimePicker.DataBindings.Add( _
'        New System.Windows.Forms.Binding("Text", classBindingSource, "BirthDate", True))
'    Me.classEmployeeIdTextBox.DataBindings.Add( _
'       New System.Windows.Forms.Binding("Text", classBindingSource, "EmployeeId", True))
'    Me.classFirstNameTextBox.DataBindings.Add( _
'       New System.Windows.Forms.Binding("Text", classBindingSource, "FirstName", True))
'    Me.classHireDateDateTimePicker.DataBindings.Add( _
'       New System.Windows.Forms.Binding("Text", classBindingSource, "HireDate", True))
'    Me.classLastNameTextBox.DataBindings.Add( _
'      New System.Windows.Forms.Binding("Text", classBindingSource, "LastName", True))
'    Me.classMaritalStatusTextBox.DataBindings.Add( _
'       New System.Windows.Forms.Binding("Text", classBindingSource, "MaritalStatus", True))
'    Me.classTitleTextBox.DataBindings.Add( _
'       New System.Windows.Forms.Binding("Text", classBindingSource, "Title", True))

'    ' Place navigator on 3rd tab
'    Me.Panel2.Controls.Add(classBindingNavigator)
'    classBindingNavigator.Dock = DockStyle.Top
'End Sub

'End Class
