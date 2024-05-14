Imports MySql.Data.MySqlClient

Public Class Form8
    ' Define your MySQL connection string
    Dim connectionString As String = "server=localhost;user=root;password=;database=medcare"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Get the values from the textboxes
        Dim name As String = TextBox2.Text
        Dim gender As String = TextBox3.Text
        Dim address As String = TextBox4.Text
        Dim contactNo As String = TextBox5.Text
        Dim email As String = TextBox1.Text
        Dim birthDate As Date = DateTimePicker1.Value
        Dim emergencyContactName As String = TextBox7.Text
        Dim emergencyContactNo As String = TextBox6.Text

        ' Define the MySQL INSERT query
        Dim query As String = "INSERT INTO patients (Name, Gender, Address, ContactNo, Email, BirthDate, EmergencyContactName, EmergencyContactNo) VALUES (@Name, @Gender, @Address, @ContactNo, @Email, @BirthDate, @EmergencyContactName, @EmergencyContactNo)"

        ' Create a new MySqlConnection object
        Using connection As New MySqlConnection(connectionString)
            ' Create a new MySqlCommand object with the INSERT query and connection
            Using command As New MySqlCommand(query, connection)
                ' Add parameters to the MySqlCommand object
                command.Parameters.AddWithValue("@Name", name)
                command.Parameters.AddWithValue("@Gender", gender)
                command.Parameters.AddWithValue("@Address", address)
                command.Parameters.AddWithValue("@ContactNo", contactNo)
                command.Parameters.AddWithValue("@Email", email)
                command.Parameters.AddWithValue("@BirthDate", birthDate)
                command.Parameters.AddWithValue("@EmergencyContactName", emergencyContactName)
                command.Parameters.AddWithValue("@EmergencyContactNo", emergencyContactNo)

                ' Open the connection
                connection.Open()

                ' Execute the INSERT query
                Dim rowsAffected As Integer = command.ExecuteNonQuery()

                ' Close the connection
                connection.Close()

                ' Check if the query was successful
                If rowsAffected > 0 Then
                    ' Show success message
                    MessageBox.Show("Patient information added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ' Clear the form
                    ClearForm()
                Else
                    ' Show error message
                    MessageBox.Show("Failed to add patient information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
        End Using
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ' Close the current form
        Me.Close()

        ' Show Form3
        Form3.Show()
    End Sub

    Private Sub ClearForm()
        ' Clear textboxes
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox1.Clear()
        TextBox7.Clear()
        TextBox6.Clear()

        ' Reset date picker
        DateTimePicker1.Value = Date.Now
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
