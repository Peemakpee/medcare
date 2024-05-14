Imports MySql.Data.MySqlClient

Public Class Form7
    ' Define your MySQL connection string
    Private connectionString As String = "server=localhost;user id=root;password=;database=medcare"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Retrieve data from the form fields
        Dim name As String = TextBox2.Text
        Dim specialty As String = TextBox3.Text
        Dim position As String = TextBox6.Text
        Dim contactNo As String = TextBox4.Text
        Dim email As String = TextBox5.Text
        Dim username As String = TextBox7.Text
        Dim password As String = TextBox1.Text ' Password not encrypted

        ' Create a connection to the MySQL database
        Using connection As New MySqlConnection(connectionString)
            Try
                ' Open the connection
                connection.Open()

                ' Define the INSERT query
                Dim query As String = "INSERT INTO doctors (name, specialty, position, contact, email, username, password) VALUES (@name, @specialty, @position, @contactNo, @email, @username, @password)"

                ' Create a command to execute the query
                Using command As New MySqlCommand(query, connection)
                    ' Add parameters to the command
                    command.Parameters.AddWithValue("@name", name)
                    command.Parameters.AddWithValue("@specialty", specialty)
                    command.Parameters.AddWithValue("@position", position)
                    command.Parameters.AddWithValue("@contactNo", contactNo)
                    command.Parameters.AddWithValue("@email", email)
                    command.Parameters.AddWithValue("@username", username)
                    command.Parameters.AddWithValue("@password", password)

                    ' Execute the query
                    command.ExecuteNonQuery()

                    ' Display a success message
                    MessageBox.Show("Data inserted successfully.")

                    ' Clear the form fields
                    ClearFormFields()

                    ' Redirect to Form3
                    Dim form3 As New Form3()
                    form3.Show()
                    Me.Hide()
                End Using
            Catch ex As Exception
                ' Display an error message if any exception occurs
                MessageBox.Show("An error occurred: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Function to clear the form fields
    Private Sub ClearFormFields()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox6.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox7.Clear()
        TextBox1.Clear()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        ' Set the password textbox to hide characters
        TextBox1.UseSystemPasswordChar = True
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
