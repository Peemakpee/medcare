Imports MySql.Data.MySqlClient

Public Class Form4
    ' Define your MySQL connection string
    Dim connectionString As String = "server=localhost;user=root;password=;database=medcare"

    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load data into DataGridView
        LoadPatientData()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Close the current form (Form4)
        Me.Close()

        ' Show Form3
        Form3.Show()
    End Sub

    Private Sub LoadPatientData()
        ' Define the MySQL SELECT query
        Dim query As String = "SELECT Name, Gender, BirthDate, Address, ContactNo, Email, EmergencyContactName, EmergencyContactNo FROM patients"

        ' Create a new MySqlConnection object
        Using connection As New MySqlConnection(connectionString)
            ' Create a new MySqlCommand object with the SELECT query and connection
            Using command As New MySqlCommand(query, connection)
                ' Create a new DataTable to hold the data
                Dim dataTable As New DataTable()

                ' Create a new MySqlDataAdapter to fill the DataTable
                Using adapter As New MySqlDataAdapter(command)
                    ' Fill the DataTable with data from the database
                    adapter.Fill(dataTable)
                End Using

                ' Bind the DataTable to the DataGridView
                PatientDGV.DataSource = dataTable
            End Using
        End Using

        ' Customize column headers
        With PatientDGV
            .Columns("Name").HeaderText = "Name"
            .Columns("Gender").HeaderText = "Gender"
            .Columns("BirthDate").HeaderText = "Birth Date"
            .Columns("Address").HeaderText = "Address"
            .Columns("ContactNo").HeaderText = "Contact No"
            .Columns("Email").HeaderText = "Email"
            .Columns("EmergencyContactName").HeaderText = "Emergency Contact Name"
            .Columns("EmergencyContactNo").HeaderText = "Emergency Contact No"
        End With
    End Sub

    Private Sub PatientDGV_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles PatientDGV.CellClick
        ' Get the index of the selected row
        Dim rowIndex As Integer = e.RowIndex

        ' Make sure the row index is valid
        If rowIndex >= 0 AndAlso rowIndex < PatientDGV.Rows.Count Then
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = PatientDGV.Rows(rowIndex)

            ' Populate TextBoxes with selected row's data
            TextBox2.Text = selectedRow.Cells("Name").Value.ToString()
            TextBox3.Text = selectedRow.Cells("Address").Value.ToString()
            TextBox4.Text = selectedRow.Cells("ContactNo").Value.ToString()
            TextBox5.Text = selectedRow.Cells("Email").Value.ToString()
            TextBox6.Text = selectedRow.Cells("EmergencyContactName").Value.ToString()
            TextBox7.Text = selectedRow.Cells("EmergencyContactNo").Value.ToString()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Define the MySQL UPDATE query
        Dim query As String = "UPDATE patients SET Name = @Name, Address = @Address, ContactNo = @ContactNo, Email = @Email, EmergencyContactName = @EmergencyContactName, EmergencyContactNo = @EmergencyContactNo WHERE Name = @OriginalName"

        ' Create a new MySqlConnection object
        Using connection As New MySqlConnection(connectionString)
            ' Create a new MySqlCommand object with the UPDATE query and connection
            Using command As New MySqlCommand(query, connection)
                ' Add parameters to the command
                command.Parameters.AddWithValue("@Name", TextBox2.Text)
                command.Parameters.AddWithValue("@Address", TextBox3.Text)
                command.Parameters.AddWithValue("@ContactNo", TextBox4.Text)
                command.Parameters.AddWithValue("@Email", TextBox5.Text)
                command.Parameters.AddWithValue("@EmergencyContactName", TextBox6.Text)
                command.Parameters.AddWithValue("@EmergencyContactNo", TextBox7.Text)
                command.Parameters.AddWithValue("@OriginalName", TextBox2.Text)

                ' Open the connection
                connection.Open()

                ' Execute the command
                command.ExecuteNonQuery()

                ' Show prompt
                MessageBox.Show("Record updated successfully.")
            End Using
        End Using

        ' Reload data into DataGridView
        LoadPatientData()

        ' Clear TextBoxes
        ClearTextBoxes()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' Define the MySQL DELETE query
        Dim query As String = "DELETE FROM patients WHERE Name = @Name"

        ' Create a new MySqlConnection object
        Using connection As New MySqlConnection(connectionString)
            ' Create a new MySqlCommand object with the DELETE query and connection
            Using command As New MySqlCommand(query, connection)
                ' Add parameters to the command
                command.Parameters.AddWithValue("@Name", TextBox2.Text)

                ' Open the connection
                connection.Open()

                ' Execute the command
                command.ExecuteNonQuery()

                ' Show prompt
                MessageBox.Show("Record deleted successfully.")
            End Using
        End Using

        ' Reload data into DataGridView
        LoadPatientData()

        ' Clear TextBoxes
        ClearTextBoxes()
    End Sub


    Private Sub ClearTextBoxes()
        ' Clear all TextBoxes
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Get the search term from TextBox1
        Dim searchTerm As String = TextBox1.Text.Trim()

        ' Define the base MySQL SELECT query
        Dim query As String = "SELECT Name, Gender, BirthDate, Address, ContactNo, Email, EmergencyContactName, EmergencyContactNo FROM patients"

        ' Check if search term is provided
        If Not String.IsNullOrEmpty(searchTerm) Then
            ' Append WHERE clause to filter by name containing the search term
            query &= " WHERE Name LIKE @SearchTerm"
        End If

        ' Create a new MySqlConnection object
        Using connection As New MySqlConnection(connectionString)
            ' Create a new MySqlCommand object with the SELECT query and connection
            Using command As New MySqlCommand(query, connection)
                ' Add parameter for search term
                command.Parameters.AddWithValue("@SearchTerm", "%" & searchTerm & "%")

                ' Create a new DataTable to hold the data
                Dim dataTable As New DataTable()

                ' Create a new MySqlDataAdapter to fill the DataTable
                Using adapter As New MySqlDataAdapter(command)
                    ' Fill the DataTable with data from the database
                    adapter.Fill(dataTable)
                End Using

                ' Bind the DataTable to the DataGridView
                PatientDGV.DataSource = dataTable
            End Using
        End Using

        ' Customize column headers
        With PatientDGV
            .Columns("Name").HeaderText = "Name"
            .Columns("Gender").HeaderText = "Gender"
            .Columns("BirthDate").HeaderText = "Birth Date"
            .Columns("Address").HeaderText = "Address"
            .Columns("ContactNo").HeaderText = "Contact No"
            .Columns("Email").HeaderText = "Email"
            .Columns("EmergencyContactName").HeaderText = "Emergency Contact Name"
            .Columns("EmergencyContactNo").HeaderText = "Emergency Contact No"
        End With
    End Sub

End Class
