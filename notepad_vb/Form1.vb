Imports System.IO
Imports System.Security.Cryptography
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class notepad
    Private Sub notepad_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CutToolStripMenuItem.Enabled = False
        CopyToolStripMenuItem.Enabled = False
        If Clipboard.ContainsText() Then
            PasteToolStripMenuItem.Enabled = True
        Else
            PasteToolStripMenuItem.Enabled = False
        End If
    End Sub
    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        Dim myStream As Stream = Nothing
        Dim fileExplorer As New OpenFileDialog With {
            .Title = "Please select a Text file",
            .DefaultExt = "txt",
            .FileName = "new_text_doc",
            .InitialDirectory = "Documents",
            .Filter = "All files|*.*|Text files|*.txt",
            .FilterIndex = 2,
            .RestoreDirectory = True
        }


        If fileExplorer.ShowDialog() = DialogResult.OK Then
            Try
                myStream = fileExplorer.OpenFile()
                If (myStream IsNot Nothing) Then
                    ' Insert code to read the stream here.
                    Try
                        textPlayground.Text = File.ReadAllText(fileExplorer.FileName)
                    Catch ex As Exception
                        MessageBox.Show("Cannot read file from disk. Original error: " & ex.Message)
                    End Try
                End If
            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally
                ' Check this again, since we need to make sure we didn't throw an exception on open.
                If (myStream IsNot Nothing) Then
                    myStream.Close()
                End If
            End Try
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        Dim fileExplorer As New SaveFileDialog With {
            .Title = "Save your file As",
            .DefaultExt = "txt",
            .FileName = "new_text_doc",
            .InitialDirectory = "Documents",
            .Filter = "All files|*.*|Text files|*.txt",
            .FilterIndex = 2,
            .RestoreDirectory = True
        }
        If fileExplorer.ShowDialog() = DialogResult.OK Then
            Dim sw As StreamWriter = New StreamWriter(fileExplorer.OpenFile())
            If (sw IsNot Nothing) Then
                sw.WriteLine(textPlayground.Text)
                sw.Close()
            End If
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Dim D As String
        D = MsgBox("Are you sure you want to exit?", vbYesNo + vbQuestion, "Thanking You")
        If D = vbYes Then
            Application.Exit()
        Else
            Exit Sub
        End If
    End Sub

    Private Sub textPlayground_TextChanged(sender As Object, e As EventArgs) Handles textPlayground.TextChanged
        If textPlayground.Text = String.Empty Then
            CutToolStripMenuItem.Enabled = False
            CopyToolStripMenuItem.Enabled = False
        Else
            ' Check if any text is selected
            If textPlayground.SelectionLength > 0 Then
                ' Enable the Cut menu item
                CutToolStripMenuItem.Enabled = True
                CopyToolStripMenuItem.Enabled = True
            Else
                ' Disable the Cut menu item
                CutToolStripMenuItem.Enabled = False
                CopyToolStripMenuItem.Enabled = False
            End If
        End If
    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click
        If Clipboard.ContainsText() Then
            textPlayground.Paste()
        End If
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectAllToolStripMenuItem.Click
        textPlayground.SelectAll()
    End Sub

    Private Sub ZoomInToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZoomInToolStripMenuItem.Click
        textPlayground.ZoomFactor += 0.1
    End Sub

    Private Sub ZoomOutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZoomOutToolStripMenuItem.Click
        textPlayground.ZoomFactor -= 0.1
    End Sub

    Private Sub RestoreDefaultToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestoreDefaultToolStripMenuItem.Click
        textPlayground.ZoomFactor -= 1
    End Sub

    Private Sub WordWrapToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WordWrapToolStripMenuItem.Click
        textPlayground.WordWrap = Not textPlayground.WordWrap
    End Sub
End Class
