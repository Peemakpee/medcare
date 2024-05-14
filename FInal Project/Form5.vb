Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient

Public Class Form5
    ' Connection string to your MySQL database
    Dim connectionString As String = "server=localhost;user id=root;password=;database=medcare"

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DoctorDGV.Columns.AddRange(New DataGridViewColumn() {
            New DataGridViewTextBoxColumn() With {.HeaderText = "Doctor ID", .DataPropertyName = "Id", .Visible = False},
            New DataGridViewTextBoxColumn() With {.HeaderText = "Name", .DataPropertyName = "Name"},
            New DataGridViewTextBoxColumn() With {.HeaderText = "Position", .DataPropertyName = "Position"},
            New DataGridViewTextBoxColumn() With {.HeaderText = "Specialty", .DataPropertyName = "Specialty"},
            New DataGridViewTextBoxColumn() With {.HeaderText = "Contact No.", .DataPropertyName = "Contact"},
            New DataGridViewTextBoxColumn() With {.HeaderText = "Email", .DataPropertyName = "Email"}
        })

        ' Load data into DataGridView when the form is loaded
        LoadData()
    End Sub

    ' Function to load data into DataGridView
    Private Sub LoadData()
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                ' Query to select all records from the 'doctors' table
                Dim query As String = "SELECT id, name, position, specialty, contact, email FROM doctors"
                Using adapter As New MySqlDataAdapter(query, connection)
                    Dim dataSet As New DataSet()
                    adapter.Fill(dataSet, "Doctors")

                    ' Set DataGridView data source to the DataSet
                    DoctorDGV.DataSource = dataSet.Tables("Doctors")
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Function to execute non-query SQL commands (INSERT, UPDATE, DELETE)
    Private Function ExecuteNonQuery(query As String, Optional parameters As MySqlParameter() = Nothing) As Boolean
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                Using cmd As New MySqlCommand(query, connection)
                    ' Add parameters if any
                    If parameters IsNot Nothing Then
                        cmd.Parameters.AddRange(parameters)
                    End If

                    ' Execute the command
                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                    ' Return true if rows affected > 0, indicating success
                    Return rowsAffected > 0
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Create a new record
        Dim query As String = "INSERT INTO doctors (name, position, contact, email) VALUES (@name, @position, @contactNo, @email)"
        Dim parameters As MySqlParameter() = {
            New MySqlParameter("@name", TextBox2.Text),
            New MySqlParameter("@position", TextBox3.Text),
            New MySqlParameter("@contactNo", TextBox4.Text),
            New MySqlParameter("@email", TextBox5.Text)
        }

        Try
            If ExecuteNonQuery(query, parameters) Then
                MessageBox.Show("Doctor information added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadData() ' Reload data into DataGridView
            End If
        Catch ex As Exception
            MessageBox.Show("Error adding doctor information: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ' Update an existing record
        If DoctorDGV.SelectedCells.Count = 0 Then
            MessageBox.Show("Please select a doctor from the list to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim selectedRowIndex As Integer = DoctorDGV.SelectedCells(0).RowIndex
        Dim selectedRow As DataGridViewRow = DoctorDGV.Rows(selectedRowIndex)

        Dim idValue As Integer = Convert.ToInt32(selectedRow.Cells(0).Value) ' Assuming the Id column is the first column

        Dim query As String = "UPDATE doctors SET name = @name, position = @position, contact = @contactNo, email = @email WHERE id = @id"
        Dim parameters As MySqlParameter() = {
        New MySqlParameter("@name", TextBox2.Text),
        New MySqlParameter("@position", TextBox3.Text),
        New MySqlParameter("@contactNo", TextBox4.Text),
        New MySqlParameter("@email", TextBox5.Text),
        New MySqlParameter("@id", idValue)
    }

        If ExecuteNonQuery(query, parameters) Then
            MessageBox.Show("Doctor information updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            LoadData() ' Reload data into DataGridView
        End If
    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ' Delete a record
        If DoctorDGV.SelectedRows.Count = 0 AndAlso DoctorDGV.SelectedCells.Count = 0 Then
            MessageBox.Show("Please select a doctor from the list to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow

        If DoctorDGV.SelectedRows.Count > 0 Then
            selectedRow = DoctorDGV.SelectedRows(0)
        ElseIf DoctorDGV.SelectedCells.Count > 0 Then
            Dim rowIndex As Integer = DoctorDGV.SelectedCells(0).RowIndex
            selectedRow = DoctorDGV.Rows(rowIndex)
        Else
            MessageBox.Show("Please select a valid doctor from the list to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim confirmResult As DialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If confirmResult = DialogResult.Yes Then
            Dim query As String = "DELETE FROM doctors WHERE `id` = @id"
            Dim delIdValue As Integer = Convert.ToInt32(selectedRow.Cells(0).Value)
            Dim parameters As MySqlParameter() = {
            New MySqlParameter("@id", delIdValue)
        }
            If ExecuteNonQuery(query, parameters) Then
                MessageBox.Show("Doctor information deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadData() ' Reload data into DataGridView
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ' Search for a record based on the name
        Dim searchTerm As String = TextBox6.Text.Trim()
        If Not String.IsNullOrEmpty(searchTerm) Then
            Try
                Using connection As New MySqlConnection(connectionString)
                    connection.Open()

                    ' Query to search for records containing the search term in the name field
                    Dim query As String = "SELECT * FROM doctors WHERE name LIKE @searchTerm"
                    Using adapter As New MySqlDataAdapter(query, connection)
                        adapter.SelectCommand.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%")

                        Dim dataSet As New DataSet()
                        adapter.Fill(dataSet, "SearchResults")

                        ' Set DataGridView data source to the DataSet
                        DoctorDGV.DataSource = dataSet.Tables("SearchResults")
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error searching data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            LoadData() ' If search term is empty, reload all data
        End If
    End Sub

    Private Sub DoctorDGV_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DoctorDGV.CellClick
        ' Check if a valid row index and not the header row
        If e.RowIndex >= 0 AndAlso e.RowIndex < DoctorDGV.Rows.Count - 1 Then
            ' Get the selected row
            Dim selectedRow As DataGridViewRow = DoctorDGV.Rows(e.RowIndex)

            ' Populate textboxes with selected doctor information
            'TextBox1.Text = selectedRow.Cells(0).Value.ToString() ' First column
            TextBox2.Text = selectedRow.Cells(1).Value.ToString() ' Second column
            TextBox3.Text = selectedRow.Cells(2).Value.ToString() ' Third column
            TextBox4.Text = selectedRow.Cells(4).Value.ToString() ' Fourth column
            TextBox5.Text = selectedRow.Cells(5).Value.ToString()
        End If
    End Sub

    Private Sub reset_Click(sender As Object, e As EventArgs) Handles reset.Click
        ' Clear the text in all TextBox controls
        'TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Me.Close()

        ' Show Form3
        Form3.Show()
    End Sub
End Class
