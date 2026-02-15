<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.btn_Record = New System.Windows.Forms.Button()
        Me.btn_play = New System.Windows.Forms.Button()
        Me.btn_save = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.btn_open = New System.Windows.Forms.Button()
        Me.lbl_Status = New System.Windows.Forms.Label()
        Me.cb_Record_Keyboard = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cb_loop = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'btn_Record
        '
        Me.btn_Record.Location = New System.Drawing.Point(174, 61)
        Me.btn_Record.Name = "btn_Record"
        Me.btn_Record.Size = New System.Drawing.Size(75, 23)
        Me.btn_Record.TabIndex = 1
        Me.btn_Record.Text = "aufnehmen"
        Me.btn_Record.UseVisualStyleBackColor = True
        '
        'btn_play
        '
        Me.btn_play.Location = New System.Drawing.Point(255, 61)
        Me.btn_play.Name = "btn_play"
        Me.btn_play.Size = New System.Drawing.Size(75, 23)
        Me.btn_play.TabIndex = 2
        Me.btn_play.Text = "abspielen"
        Me.btn_play.UseVisualStyleBackColor = True
        '
        'btn_save
        '
        Me.btn_save.Location = New System.Drawing.Point(93, 61)
        Me.btn_save.Name = "btn_save"
        Me.btn_save.Size = New System.Drawing.Size(75, 23)
        Me.btn_save.TabIndex = 3
        Me.btn_save.Text = "speichern"
        Me.btn_save.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 38)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "stoppen:"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(66, 35)
        Me.TextBox1.MaxLength = 1
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(28, 20)
        Me.TextBox1.TabIndex = 5
        '
        'btn_open
        '
        Me.btn_open.Location = New System.Drawing.Point(12, 61)
        Me.btn_open.Name = "btn_open"
        Me.btn_open.Size = New System.Drawing.Size(75, 23)
        Me.btn_open.TabIndex = 6
        Me.btn_open.Text = "öffnen"
        Me.btn_open.UseVisualStyleBackColor = True
        '
        'lbl_Status
        '
        Me.lbl_Status.AutoSize = True
        Me.lbl_Status.Location = New System.Drawing.Point(12, 89)
        Me.lbl_Status.Name = "lbl_Status"
        Me.lbl_Status.Size = New System.Drawing.Size(37, 13)
        Me.lbl_Status.TabIndex = 7
        Me.lbl_Status.Text = "Status"
        '
        'cb_Record_Keyboard
        '
        Me.cb_Record_Keyboard.AutoSize = True
        Me.cb_Record_Keyboard.Location = New System.Drawing.Point(12, 12)
        Me.cb_Record_Keyboard.Name = "cb_Record_Keyboard"
        Me.cb_Record_Keyboard.Size = New System.Drawing.Size(175, 17)
        Me.cb_Record_Keyboard.TabIndex = 9
        Me.cb_Record_Keyboard.Text = "Tastaturanschläge aufzeichnen"
        Me.cb_Record_Keyboard.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label2.Location = New System.Drawing.Point(255, 89)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(92, 13)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "0, 0"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cb_loop
        '
        Me.cb_loop.AutoSize = True
        Me.cb_loop.Location = New System.Drawing.Point(193, 12)
        Me.cb_loop.Name = "cb_loop"
        Me.cb_loop.Size = New System.Drawing.Size(83, 17)
        Me.cb_loop.TabIndex = 11
        Me.cb_loop.Text = "wiederholen"
        Me.cb_loop.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(359, 111)
        Me.Controls.Add(Me.cb_loop)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cb_Record_Keyboard)
        Me.Controls.Add(Me.lbl_Status)
        Me.Controls.Add(Me.btn_open)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btn_save)
        Me.Controls.Add(Me.btn_play)
        Me.Controls.Add(Me.btn_Record)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Mouse Action Repeater"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btn_Record As Button
    Friend WithEvents btn_play As Button
    Friend WithEvents btn_save As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents btn_open As Button
    Friend WithEvents lbl_Status As Label
    Friend WithEvents cb_Record_Keyboard As CheckBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cb_loop As CheckBox
End Class
