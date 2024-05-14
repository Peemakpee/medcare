Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim f2 As New Form2
        f2.Show()
        Visible = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Navigate to Form7
        Dim f7 As New Form7
        f7.Show()
        Me.Hide() ' Hide Form1
    End Sub
End Class
