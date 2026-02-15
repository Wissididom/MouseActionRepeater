Imports System.ComponentModel
Imports System.Runtime.InteropServices
Public Class Form1
    Private Declare Sub mouse_event Lib "user32" (ByVal dwFlags As Long, ByVal dx As Long, ByVal dy As Long, ByVal dwData As Long, ByVal dwExtraInfo As Long)
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Long) As Integer
    Private Const MOUSEEVENTF_LEFTDOWN As Integer = &H2
    Private Const MOUSEEVENTF_LEFTUP As Integer = &H4
    Private Const MOUSEEVENTF_MIDDLEDOWN As Integer = &H20
    Private Const MOUSEEVENTF_MIDDLEUP As Integer = &H40
    Private Const MOUSEEVENTF_RIGHTDOWN As Integer = &H8
    Private Const MOUSEEVENTF_RIGHTUP As Integer = &H10
    Private Const MOUSEEVENTF_XDOWN As Integer = &H80
    Private Const MOUSEEVENTF_XUP As Integer = &H100
    Private Const XBUTTON1 As Integer = &H1
    Private Const XBUTTON2 As Integer = &H2
    Private WithEvents tmr As New Timer With {.Interval = 10}
    Private WithEvents tmr_getcurpos As New Timer With {.Interval = 10}
    Dim globi As Integer = 0
    Dim MouseData As String = ""
    Dim KeyboardData As String = ""
    Dim PressedKey As Char = Nothing
    Dim Action As ActionEnum = ActionEnum.Nothing
    Dim LEFTDOWN As Boolean = False
    Dim MIDDLEDOWN As Boolean = False
    Dim RIGHTDOWN As Boolean = False
    Dim XBUTTON1DOWN As Boolean = False
    Dim XBUTTON2DOWN As Boolean = False

#Region "HOOK"
    Private Declare Unicode Function GetModuleHandleW Lib "kernel32.dll" (ByVal lpModuleName As IntPtr) As IntPtr

    Private Delegate Function HOOKPROCDelegate(ByVal nCode As Integer, ByVal wParam As IntPtr, ByRef lParam As KBDLLHOOKSTRUCT) As IntPtr

    Private Declare Unicode Function SetWindowsHookExW Lib "user32.dll" (ByVal idHook As Integer, ByVal lpfn As HOOKPROCDelegate, ByVal hMod As IntPtr, ByVal dwThreadId As UInteger) As IntPtr

    Private HookProc As New HOOKPROCDelegate(AddressOf KeyboardHookProc) ' dauerhafte Delegaten-Variable erzeugen

    Private Declare Unicode Function UnhookWindowsHookEx Lib "user32.dll" (ByVal hhk As IntPtr) As UInteger

    Private Declare Unicode Function CallNextHookEx Lib "user32.dll" (ByVal hhk As IntPtr, ByVal nCode As Integer, ByVal wParam As IntPtr, ByRef lParam As KBDLLHOOKSTRUCT) As IntPtr

    Private Const WM_KEYDOWN As Int32 = &H100
    Private Const WM_KEYUP As Int32 = &H101
    Private Const WH_KEYBOARD_LL As Integer = 13
    Private Const HC_ACTION As Integer = 0

    Private mHandle As IntPtr

    Public PrevWndProc As Integer

    <StructLayout(LayoutKind.Sequential)>
    Public Structure KBDLLHOOKSTRUCT

        Public vkCode As Keys
        Public scanCode, flags, time, dwExtraInfo As UInteger

        Public Sub New(ByVal key As Keys, ByVal scancod As UInteger, ByVal flagss As _
            UInteger, ByVal zeit As UInteger, ByVal extra As UInteger)

            vkCode = key
            scanCode = scancod
            flags = flagss
            time = zeit
            dwExtraInfo = extra

        End Sub

    End Structure

    Public Property KeyHookEnable() As Boolean
        Get
            Return mHandle <> IntPtr.Zero
        End Get
        Set(ByVal value As Boolean)
            If KeyHookEnable = value Then Return
            If value Then
                mHandle = SetWindowsHookExW(WH_KEYBOARD_LL, HookProc, GetModuleHandleW(IntPtr.Zero), 0)
            Else
                UnhookWindowsHookEx(mHandle)
                mHandle = IntPtr.Zero
            End If
        End Set
    End Property

    Private Function KeyboardHookProc(ByVal nCode As Integer, ByVal wParam As IntPtr, ByRef _
        lParam As KBDLLHOOKSTRUCT) As IntPtr
        Dim fEatKeyStroke As Boolean
        If nCode = HC_ACTION Then
            If lParam.vkCode = My.Settings.KeyCode Then
                stopAction(True)
                fEatKeyStroke = True
            Else
                PressedKey = If(GetAsyncKeyState(Keys.LShiftKey) Or GetAsyncKeyState(Keys.RShiftKey) Or GetAsyncKeyState(Keys.Shift) Or GetAsyncKeyState(Keys.ShiftKey), Chr(lParam.vkCode), Chr(lParam.vkCode).ToString().ToLower().ToCharArray()(0))
            End If

            '' noch ein Bsp, bei Druck von X wird eine Nachricht ausgegeben und der
            '' Tastendruck verschluckt
            'If lParam.vkCode = Keys.X Then
            '    MessageBox.Show("Ein X wurde gerückt" & vbNewLine & wParam.ToString, "Globaler KeyHook", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '    fEatKeyStroke = True
            'End If
        End If
        If fEatKeyStroke Then
            Return New IntPtr(1)
            Exit Function
        End If
        Return CallNextHookEx(mHandle, nCode, wParam, lParam)
    End Function


#End Region

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lbl_Status.Text = "Bereit"
        TextBox1.Text = My.Settings.ShortcutKeyToStop
        tmr_getcurpos.Start()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        KeyHookEnable = False
        tmr_getcurpos.Stop()
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        TextBox1.Text = Chr(e.KeyCode)
        My.Settings.ShortcutKeyToStop = TextBox1.Text
        My.Settings.KeyCode = e.KeyCode
        My.Settings.Save()
        e.SuppressKeyPress = True
        e.Handled = True
    End Sub

    Private Sub btn_open_Click(sender As Object, e As EventArgs) Handles btn_open.Click
        Action = ActionEnum.Open
        lbl_Status.Text = "Datei öffnen..."
        Dim FileContents As String = ""
        Dim OFD As New OpenFileDialog
        With OFD
            .Multiselect = False
            .Title = "Aufnahme öffnen"
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            .FileName = "MouseActionRepeater"
            If Globalization.CultureInfo.CurrentCulture.ThreeLetterWindowsLanguageName = "DEU" Then
                .Filter = "MouseActionRepeater-Dateien|*.mar|Textdokumente|*.txt|Alle Dateien|*.*"
            Else
                .Filter = "MouseActionRepeater-Files|*.mar|Textdocuments|*.txt|All Files|*.*"
            End If
            If .ShowDialog() = DialogResult.OK Then
                FileContents = IO.File.ReadAllText(.FileName)
                lbl_Status.Text = "Datei erfolgreich geladen"
            Else
                lbl_Status.Text = "Öffnen abgebrochen"
            End If
        End With
        MouseData = FileContents.Replace("MouseData:" & vbCrLf, "").Split({vbCrLf & "KeyboardData:"}, StringSplitOptions.None)(0)
        KeyboardData = FileContents.Replace("MouseData:" & vbCrLf & MouseData & vbCrLf & "KeyboardData:" & vbCrLf, "")
        Action = ActionEnum.Nothing
    End Sub

    Private Sub btn_save_Click(sender As Object, e As EventArgs) Handles btn_save.Click
        Action = ActionEnum.Save
        lbl_Status.Text = "Datei speichern..."
        Dim FileContents As String = "MouseData:" & vbCrLf & MouseData
        If Not KeyboardData.Replace("<NOKEY>", "") = "" Then
            FileContents &= vbCrLf & "KeyboardData:" & vbCrLf & KeyboardData
        End If
        Dim SFD As New SaveFileDialog
        With SFD
            .Title = "Aufnahme speichern"
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            .FileName = "MouseActionRepeater"
            If Globalization.CultureInfo.CurrentCulture.ThreeLetterWindowsLanguageName = "DEU" Then
                .Filter = "MouseActionRepeater-Dateien|*.mar|Textdokumente|*.txt|Alle Dateien|*.*"
            Else
                .Filter = "MouseActionRepeater-Files|*.mar|Textdocuments|*.txt|All Files|*.*"
            End If
            If .ShowDialog() = DialogResult.OK Then
                IO.File.WriteAllText(.FileName, FileContents)
                lbl_Status.Text = "Datei erfolgreich gespeichert"
            Else
                lbl_Status.Text = "Speichern abgebrochen"
            End If
        End With
        Action = ActionEnum.Nothing
    End Sub

    Private Sub btn_Record_Click(sender As Object, e As EventArgs) Handles btn_Record.Click
        KeyHookEnable = True
        Action = ActionEnum.Record
        MouseData = ""
        KeyboardData = ""
        lbl_Status.Text = "aufnehmen..."
        tmr.Start()
    End Sub

    Private Sub btn_play_Click(sender As Object, e As EventArgs) Handles btn_play.Click
        If Not MouseData = "" Then
            lbl_Status.Text = "abspielen..."
            KeyHookEnable = True
            Action = ActionEnum.Play
            tmr.Start()
        End If
    End Sub

    Private Sub tmr_getcurpos_Tick(sender As Object, e As EventArgs) Handles tmr_getcurpos.Tick
        Label2.Text = CStr(Cursor.Position.X) & ", " & CStr(Cursor.Position.Y)
    End Sub

    Private Sub tmr_Tick(sender As Object, e As EventArgs) Handles tmr.Tick
        Select Case Action
            Case ActionEnum.Record
                If MouseData = "" Then
                    If GetAsyncKeyState(1) Then
                        MouseData = CStr(MousePosition.X) & "," & CStr(MousePosition.Y) & ",LB" 'LBUTTON
                    ElseIf GetAsyncKeyState(2) Then
                        MouseData = CStr(MousePosition.X) & "," & CStr(MousePosition.Y) & ",RB" 'RBUTTON
                    ElseIf GetAsyncKeyState(4) Then
                        MouseData = CStr(MousePosition.X) & "," & CStr(MousePosition.Y) & ",MB" 'MBUTTON
                    ElseIf GetAsyncKeyState(5) Then
                        MouseData = CStr(MousePosition.X) & "," & CStr(MousePosition.Y) & ",X1" 'XBUTTON1
                    ElseIf GetAsyncKeyState(6) Then
                        MouseData = CStr(MousePosition.X) & "," & CStr(MousePosition.Y) & ",X2" 'XBUTTON2
                    Else
                        MouseData = CStr(MousePosition.X) & "," & CStr(MousePosition.Y) 'no button pressed
                    End If
                Else
                    If GetAsyncKeyState(1) Then 'VK_LBUTTON
                        MouseData &= vbCrLf & CStr(MousePosition.X) & "," & CStr(MousePosition.Y) & ",LB"
                    ElseIf GetAsyncKeyState(2) Then 'VK_RBUTTON
                        MouseData &= vbCrLf & CStr(MousePosition.X) & "," & CStr(MousePosition.Y) & ",RB"
                    ElseIf GetAsyncKeyState(4) Then 'VK_MBUTTON
                        MouseData &= vbCrLf & CStr(MousePosition.X) & "," & CStr(MousePosition.Y) & ",MB"
                    ElseIf GetAsyncKeyState(5) Then 'VK_XBUTTON1
                        MouseData &= vbCrLf & CStr(MousePosition.X) & "," & CStr(MousePosition.Y) & ",X1"
                    ElseIf GetAsyncKeyState(6) Then 'VK_XBUTTON2
                        MouseData &= vbCrLf & CStr(MousePosition.X) & "," & CStr(MousePosition.Y) & ",X2"
                    Else
                        MouseData &= vbCrLf & CStr(MousePosition.X) & "," & CStr(MousePosition.Y) 'no button pressed
                    End If
                End If
                If cb_Record_Keyboard.Checked Then
                    If KeyboardData = "" Then
                        If PressedKey = Nothing Then
                            KeyboardData = "<NOKEY>"
                        Else
                            KeyboardData = PressedKey
                            PressedKey = Nothing
                        End If
                    Else
                        If PressedKey = Nothing Then
                            KeyboardData &= vbCrLf & "<NOKEY>"
                        Else
                            KeyboardData &= vbCrLf & PressedKey
                            PressedKey = Nothing
                        End If
                    End If
                    KeyboardData = KeyboardData.Replace("ÿ", "<NOKEY>").Replace("¡", "<NOKEY>")
                End If
            Case ActionEnum.Play
                Dim tempMouseData As String = MouseData.Replace(vbCrLf, "|")
                If globi = tempMouseData.Split("|"c).Length - 1 Then stopAction()
                Dim x As Integer = Integer.Parse(tempMouseData.Split("|"c)(globi).Split(","c)(0))
                Dim y As Integer = Integer.Parse(tempMouseData.Split("|"c)(globi).Split(","c)(1))
                If tempMouseData.Split("|"c)(globi).EndsWith("LB") Then
                    ResetBooleans("LB")
                    Left_Down(x, y)
                    LEFTDOWN = True
                ElseIf tempMouseData.Split("|"c)(globi).EndsWith("MB") Then
                    ResetBooleans("MB")
                    Middle_Down(x, y)
                    MIDDLEDOWN = True
                ElseIf tempMouseData.Split("|"c)(globi).EndsWith("RB") Then
                    ResetBooleans("RB")
                    Right_Down(x, y)
                    RIGHTDOWN = True
                ElseIf tempMouseData.Split("|"c)(globi).EndsWith("X1") Then
                    ResetBooleans("X1")
                    XButton1_Down(x, y)
                    XBUTTON1DOWN = True
                ElseIf tempMouseData.Split("|"c)(globi).EndsWith("X2") Then
                    ResetBooleans("X2")
                    XButton2_Down(x, y)
                    XBUTTON2DOWN = True
                Else
                    ResetBooleans()
                    Cursor.Position = New Point(x, y)
                End If
                If cb_Record_Keyboard.Checked Then
                    Dim tempKeyboardData As String = KeyboardData.Replace(vbCrLf, "|")
                    If Not tempKeyboardData.Split("|"c)(globi) = "<NOKEY>" Then SendKeys.Send(tempKeyboardData.Split("|"c)(globi))
                End If
                globi += 1
        End Select
    End Sub

    Private Sub ResetBooleans(ByVal Optional Now As String = "")
        If LEFTDOWN And Not Now = "LB" Then
            Left_Up()
            LEFTDOWN = False
        End If
        If MIDDLEDOWN And Not Now = "MB" Then
            Middle_Up()
            MIDDLEDOWN = False
        End If
        If RIGHTDOWN And Not Now = "RB" Then
            Right_Up()
            RIGHTDOWN = False
        End If
        If XBUTTON1DOWN And Not Now = "X1" Then
            XButton1_Up()
            XBUTTON1DOWN = False
        End If
        If XBUTTON2DOWN And Not Now = "X2" Then
            XButton2_Up()
            XBUTTON2DOWN = False
        End If
    End Sub

    Private Sub stopAction(ByVal Optional force As Boolean = False)
        KeyHookEnable = False
        If Action = ActionEnum.Record Then
            tmr.Stop()
            lbl_Status.Text = "Bereit"
            globi = 0
            Action = ActionEnum.Nothing
        ElseIf Action = ActionEnum.Play Then
            If cb_loop.Checked AndAlso Not force Then
                globi = 0
            Else
                tmr.Stop()
                lbl_Status.Text = "Bereit"
                globi = 0
                Action = ActionEnum.Nothing
            End If
        End If
    End Sub

    Public Shared Sub Left_Click(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        Left_Down(x, y)
        Left_Up(x, y)
    End Sub

    Public Shared Sub Left_Down(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        If x = -1 Then x = Cursor.Position.X
        If y = -1 Then y = Cursor.Position.Y
        mouse_event(MOUSEEVENTF_LEFTDOWN, x, y, 0, 0)
    End Sub

    Public Shared Sub Left_Up(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        If x = -1 Then x = Cursor.Position.X
        If y = -1 Then y = Cursor.Position.Y
        mouse_event(MOUSEEVENTF_LEFTUP, x, y, 0, 0)
    End Sub

    Public Shared Sub Middle_Click(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        Middle_Down(x, y)
        Middle_Up(x, y)
    End Sub

    Public Shared Sub Middle_Down(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        If x = -1 Then x = Cursor.Position.X
        If y = -1 Then y = Cursor.Position.Y
        mouse_event(MOUSEEVENTF_MIDDLEDOWN, x, y, 0, 0)
    End Sub

    Public Shared Sub Middle_Up(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        If x = -1 Then x = Cursor.Position.X
        If y = -1 Then y = Cursor.Position.Y
        mouse_event(MOUSEEVENTF_MIDDLEUP, x, y, 0, 0)
    End Sub

    Public Shared Sub Right_Click(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        Right_Down(x, y)
        Right_Up(x, y)
    End Sub

    Public Shared Sub Right_Down(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        If x = -1 Then x = Cursor.Position.X
        If y = -1 Then y = Cursor.Position.Y
        mouse_event(MOUSEEVENTF_RIGHTDOWN, x, y, 0, 0)
    End Sub

    Public Shared Sub Right_Up(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        If x = -1 Then x = Cursor.Position.X
        If y = -1 Then y = Cursor.Position.Y
        mouse_event(MOUSEEVENTF_RIGHTUP, x, y, 0, 0)
    End Sub

    Public Shared Sub XButton1_Click(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        XButton1_Down(x, y)
        XButton1_Up(x, y)
    End Sub

    Public Shared Sub XButton1_Down(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        If x = -1 Then x = Cursor.Position.X
        If y = -1 Then y = Cursor.Position.Y
        mouse_event(MOUSEEVENTF_XDOWN, x, y, XBUTTON1, 0)
    End Sub

    Public Shared Sub XButton1_Up(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        If x = -1 Then x = Cursor.Position.X
        If y = -1 Then y = Cursor.Position.Y
        mouse_event(MOUSEEVENTF_XUP, x, y, XBUTTON1, 0)
    End Sub

    Public Shared Sub XButton2_Click(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        XButton2_Down(x, y)
        XButton2_Up(x, y)
    End Sub

    Public Shared Sub XButton2_Down(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        If x = -1 Then x = Cursor.Position.X
        If y = -1 Then y = Cursor.Position.Y
        mouse_event(MOUSEEVENTF_XDOWN, x, y, XBUTTON2, 0)
    End Sub

    Public Shared Sub XButton2_Up(ByVal Optional x As Integer = -1, ByVal Optional y As Integer = -1)
        If x = -1 Then x = Cursor.Position.X
        If y = -1 Then y = Cursor.Position.Y
        mouse_event(MOUSEEVENTF_XUP, x, y, XBUTTON2, 0)
    End Sub
End Class

Public Enum ActionEnum
    [Nothing] = 0
    Record = 1
    Play = 2
    Open = 3
    Save = 4
End Enum