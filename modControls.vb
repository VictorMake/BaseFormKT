'Imports System.Reflection
'Imports System.Windows.Forms


'Module modControls
'    Friend Function CloneControl(ByVal c As Control) As Control
'        ' instantiate another instance of the given control and
'        '   clone the common properties
'        Dim newControl As Control = CType(NewAs(c), Control)
'        CloneProperties(newControl, c, "Visible", "Size", "Font", "Text", "Location", _
'                                       "BackColor", "ForeColor", "Enabled", "BackgroundImage")

'        ' clone properties unique to specific controls
'        If TypeOf newControl Is ButtonBase Then
'            CloneProperties(newControl, c, "DialogResult", "BackgroundImage", "FlatStyle", _
'                                           "TextAlign", "Image", "ImageAlign", "ImageIndex", _
'                                           "ImageList")
'        ElseIf TypeOf newControl Is LinkLabel Then
'            CloneProperties(newControl, c, "VisitedLinkColor", "LinkVisited", "LinkColor", _
'                                           "LinkBehavior", "LinkArea", "FlatStyle", "BorderStyle", _
'                                           "DisabledLinkColor", "ActiveLinkColor", _
'                                           "Image", "ImageAlign", "ImageIndex", "ImageList")
'        End If

'        Return newControl
'    End Function

'End Module

'' <summary>
'' Set of functions for creating new instances of a type and cloning properties of one
'' type to another.
'' </summary>
'Public Module ObjectFactory
'    ' <summary>
'    ' Instantiate a new instance of the given type using its default constructor.
'    ' </summary>
'    ' <param name="t">The type to instantiate</param>
'    ' <returns>The new instance</returns>
'    Public Function NewAs(ByVal t As Type) As Object
'        Return t.Assembly.CreateInstance(t.FullName)
'    End Function

'    ' <summary>
'    ' Instantiate another instance of an object using its default constructor.
'    ' </summary>
'    ' <param name="x">An object for which another instance is to be created.</param>
'    ' <returns>The new instance</returns>
'    Public Function NewAs(ByVal x As Object) As Object
'        Return ObjectFactory.NewAs(x.GetType())
'    End Function

'    ' <summary>
'    ' Set the value of a list of properties on one object to match the values on another.
'    ' </summary>
'    ' <param name="target">The object whose properties are to be set.</param>
'    ' <param name="source">The object whose properties are to be queried for setting on parameter target.</param>
'    ' <param name="propertyNames">The list of property names to set.</param>
'    ' <remarks>For each property name N, the value of the property named N on parameter source is queried
'    ' and then set as the value for property N on parameter target.</returns>
'    Public Sub CloneProperties( _
'        ByVal target As Object, _
'        ByVal source As Object, _
'        ByVal ParamArray propertyNames() As String)

'        Dim sourceProperties As New PropertyAccessor(source)
'        Dim targetProperties As New PropertyAccessor(target)
'        Dim p As String
'        For Each p In propertyNames
'            targetProperties(p) = sourceProperties(p)
'        Next
'    End Sub
'End Module

'' <summary>
'' A helper class that sets and gets the values of properties on any object.  The properties
'' to get and set are given by string name.
'' </summary>
'' <example>
''   Dim props as new PropertyAccessor(someObject)
''   props("Visible") = false
'' </example>
'Public Class PropertyAccessor

'    ' <summary>
'    ' Construct a new accessor for the given object.
'    ' </summary>
'    ' <param name="target">The object whose properties are to be accessed</param>
'    Public Sub New(ByVal target As Object)
'        Me.target_ = target
'    End Sub

'    ' <summary>
'    ' The object whose properties are to be accessed.  Me.Item(n) accesses the property
'    ' named n on Me.Target.
'    ' </summary>
'    Public ReadOnly Property Target() As Object
'        Get
'            Return Me.target_
'        End Get
'    End Property

'    ' <summary>
'    ' The default property for this class.  It is used to get and set properties by name
'    ' on Me.Target.  Me.Item(n) can be used to get or set the property named n on Me.Target.
'    ' </summary>
'    ' <param name="propertyName">The name of the property to be accessed on Me.Target</param>
'    ' <remarks>Me(n) is equivalent to Me.Item(n).</remarks>
'    Default Public Property Item(ByVal propertyName As String) As Object
'        Get
'            Dim prop As PropertyInfo = Me.Target.GetType().GetProperty(propertyName)
'            Return prop.GetValue(Me.Target, Nothing)
'        End Get
'        Set(ByVal value As Object)
'            Dim prop As PropertyInfo = Me.Target.GetType().GetProperty(propertyName)
'            prop.SetValue(Me.Target, value, Nothing)
'        End Set
'    End Property

'#Region " Private State "
'    ' target_ is the object whose properties are to accessed
'    Private target_ As Object
'#End Region


'    '*************************************************
'    'где-то в форме следующий код для манипуляцмй контролами

'    'Private Sub TextBox_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
'    '    RaiseEvent TextBoxTextChanged(Me, e)
'    'End Sub

'    ''Клонирование свойств
'    'Friend MyArrayOfControls() As Windows.Forms.Control

'    'Private Sub ОткудаТоСкопировать()
'    '    ReDim_MyArrayOfControls(Me.Controls.Count - 1)
'    '    Me.Controls.CopyTo(MyArrayOfControls, 0)
'    'End Sub

'    'Private Sub КудаТоВставить()
'    '    'Private Sub Parameter_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'    '    Dim ctNew As Windows.Forms.Control
'    '    Dim i As Integer

'    '    If MyArrayOfControls.GetUpperBound(0) > 0 Then
'    '        For i = 0 To MyArrayOfControls.GetUpperBound(0)
'    '            ctNew = CloneControl(MyArrayOfControls(i))
'    '            If ctNew.GetType.ToString <> "System.Windows.Forms.Button" Then
'    '                Me.Controls.Add(ctNew)
'    '            End If
'    '        Next
'    '    End If
'    'End Sub

'    '' Create class variables to use during the form.
'    'Private controlCount As Integer = 0
'    'Private controlLocation As New Point(10, 50)

'    '' This subroutine handles the btnTightlyBoundControls.Click event and creates
'    '' two tightly bound controls. It uses the event handlers that have been 
'    '' previously defined to handle the events. These event handlers are 
'    '' have to be defined beforehand, unless Reflection.Emit is used.
'    '' The two controls are a Button and a TextBox. When the Button is pressed, the
'    '' text in the TextBox is displayed in a MsgBox.  In order to ensure that
'    '' we know which TextBox is being used, it is added to the Tag property of
'    '' the Button. (We don't know the name of the TextBox while creating the 
'    '' event handler, since the TextBox will be created dynamically.
'    'Private Sub btnTightlyBoundControls_Click() '(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTightlyBoundControls.Click

'    '    ' Increment the number of controls (only by one, even though two 
'    '    ' will be added.
'    '    controlCount += 1

'    '    ' Only allow 5 buttons, just to simplify drawing of the user interface.
'    '    If controlCount <= 5 Then

'    '        ' Create the TextBox that will contain the text to speak.
'    '        Dim txtSpeakText As New Windows.Forms.TextBox

'    '        ' Set up some properties for the TextBox.
'    '        txtSpeakText.Text = "Hello, World"
'    '        txtSpeakText.Name = "txtSpeakText"
'    '        txtSpeakText.Location = New Point(controlLocation.X + 250, controlLocation.Y)
'    '        txtSpeakText.Size = New Size(200, txtSpeakText.Height)

'    '        ' Add the TextBox to the controls collection.
'    '        Controls.Add(txtSpeakText)

'    '        ' Increment the m_LocationY so the next control won't overwrite it.
'    '        controlLocation.Y += txtSpeakText.Height + 5

'    '        ' Create a button that will be used to react to clicks
'    '        ' Since this button is tightly coupled to the TextBox which will
'    '        ' provide it with the text to display, we'll add the TextBox created
'    '        ' above as the Tag of this Button. 
'    '        Dim btnSpeakText As New Button

'    '        ' Set up some properties for the TextBox.
'    '        btnSpeakText.Text = "Speak Text"
'    '        btnSpeakText.Name = "btnSpeakText"
'    '        btnSpeakText.Location = New Point(controlLocation.X + 250, controlLocation.Y)
'    '        btnSpeakText.Size = New Size(100, btnSpeakText.Height)

'    '        ' Add the previously created TextBox to this button.
'    '        btnSpeakText.Tag = txtSpeakText

'    '        ' Add the TextBox to the controls collection.
'    '        Controls.Add(btnSpeakText)

'    '        ' Increment the m_LocationY so the next control won't overwrite it.
'    '        controlLocation.Y += btnSpeakText.Height + 5

'    '        ' Add the event handler that will handle the event when the
'    '        ' button is pressed.
'    '        AddHandler btnSpeakText.Click, AddressOf SpeakTextClickHandler
'    '    Else
'    '        ' Just allow 5 controls to simplify UI.
'    '        MsgBox("You've reached 5 controls. Clear controls to start again.", _
'    '            MsgBoxStyle.OkOnly, Me.Text)
'    '    End If

'    'End Sub


'    '' This subroutine handles the Click event of the Button created in the
'    '' tightly bound controls example. It displays in a MsgBox the text that
'    '' is inside of the Tag of the Button (which is provided in the 'sender' 
'    '' parameter.  Although those event handlers are sophisticated, 
'    '' they do have to be defined beforehand, unless Reflection.Emit is used.
'    'Private Sub SpeakTextClickHandler(ByVal sender As System.Object, _
'    '    ByVal e As System.EventArgs)

'    '    ' Check to see if the sender is a button, and that it has
'    '    ' a valid, tightly-coupled TextBox object attached as its
'    '    ' Tag property.
'    '    If TypeOf sender Is Button Then
'    '        ' Create a button object to use in its place.
'    '        Dim myButton As Button = CType(sender, Button)
'    '        ' Check to see if the Button has a TextBox in its Tag property.
'    '        If TypeOf myButton.Tag Is TextBox Then
'    '            ' Display the text to the user.
'    '            MsgBox(CType(myButton.Tag, TextBox).Text, MsgBoxStyle.OkOnly, _
'    '                Me.Text)
'    '        End If
'    '    End If
'    'End Sub

'End Class