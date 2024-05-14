Imports MySql.Data.MySqlClient

Public Class Form2

    ' Connection string to your MySQL database
    Dim connectionString As String = "server=localhost;user id=root;password=;database=medcare"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim username As String = txtname.Text
        Dim password As String = txtpass.Text

        ' Check if username and password are provided
        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then
            MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Create connection to MySQL database
        Using connection As New MySqlConnection(connectionString)
            Try
                connection.Open()

                ' Query to check if user exists in the database
                Dim query As String = "SELECT COUNT(*) FROM doctors WHERE username = @username AND password = @password"
                Using cmd As New MySqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@username", username)
                    cmd.Parameters.AddWithValue("@password", password)

                    ' Execute the query
                    Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                    ' If count is greater than 0, user exists, login successful
                    If count > 0 Then
                        MessageBox.Show("Login Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        ' Open Form3
                        Dim form3 As New Form3()
                        form3.Show()
                        Me.Hide() ' Hide the current form
                    Else
                        MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error connecting to database: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' Optional: Handle the Load event of the form to set the password field as masked by default
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtpass.UseSystemPasswordChar = True
    End Sub
End Class
