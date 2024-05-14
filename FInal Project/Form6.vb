Imports MySql.Data.MySqlClient

Public Class Form6
    ' Define your MySQL connection string
    Dim connectionString As String = "server=localhost;user=root;password=;database=medcare"

    ' Define a stack to store appointments
    Dim appointmentsStack As New Stack(Of Appointment)()

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()

        ' Show Form3
        Form3.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        CreateAppointment()
    End Sub
    Private Sub ClearAppointmentFields()
        ' Clear ComboBoxes
        ComboBox1.SelectedIndex = -1
        ComboBox2.SelectedIndex = -1

        ' Clear DateTimePicker
        DateTimePicker1.Value = DateTime.Now

        ' Clear TextBoxes
        TextBox3.Clear()
        TextBox5.Clear()
        TextBox1.Clear()
    End Sub

    Private Sub CreateAppointment()
        ' Extract data from form controls
        Dim doctorName As String = ComboBox1.Text
        Dim patientName As String = ComboBox2.Text
        Dim appointmentDate As Date = DateTimePicker1.Value.Date
        Dim appointmentTime As String = TextBox3.Text
        Dim contactNo As String = TextBox5.Text
        Dim email As String = TextBox1.Text

        ' Define the MySQL INSERT query
        Dim query As String = "INSERT INTO appointments (docName, patientName, date, time, contactNo, email) VALUES (@DoctorName, @PatientName, @AppointmentDate, @AppointmentTime, @ContactNo, @Email)"

        ' Create a new MySqlConnection object
        Using connection As New MySqlConnection(connectionString)
            ' Create a new MySqlCommand object with the INSERT query and connection
            Using command As New MySqlCommand(query, connection)
                ' Add parameters to the command
                command.Parameters.AddWithValue("@DoctorName", doctorName)
                command.Parameters.AddWithValue("@PatientName", patientName)
                command.Parameters.AddWithValue("@AppointmentDate", appointmentDate)
                command.Parameters.AddWithValue("@AppointmentTime", appointmentTime)
                command.Parameters.AddWithValue("@ContactNo", contactNo)
                command.Parameters.AddWithValue("@Email", email)

                ' Open the connection
                connection.Open()

                ' Execute the command
                command.ExecuteNonQuery()

                ' Show prompt
                MessageBox.Show("Appointment created successfully.")
                ClearAppointmentFields()
            End Using
        End Using

        ' Reload appointments data into DataGridView
        LoadAppointmentsDataFromDatabase()
    End Sub

    Private Sub LoadAppointmentsDataFromDatabase()
        ' Clear appointments stack
        appointmentsStack.Clear()

        ' Define the base MySQL SELECT query
        Dim query As String = "SELECT id, docName, patientName, date, time, contactNo, email FROM appointments"

        ' Check if search term is provided
        If Not String.IsNullOrEmpty(TextBox6.Text) Then
            ' Append WHERE clause to filter by doctorName or patientName containing the search term
            query &= " WHERE docName LIKE @SearchTerm OR patientName LIKE @SearchTerm"
        End If

        ' Create a new MySqlConnection object
        Using connection As New MySqlConnection(connectionString)
            ' Create a new MySqlCommand object with the SELECT query and connection
            Using command As New MySqlCommand(query, connection)
                ' Add parameter for search term
                command.Parameters.AddWithValue("@SearchTerm", "%" & TextBox6.Text & "%")

                ' Create a new DataTable to hold the data
                Dim dataTable As New DataTable()

                ' Create a new MySqlDataAdapter to fill the DataTable
                Using adapter As New MySqlDataAdapter(command)
                    ' Fill the DataTable with data from the database
                    adapter.Fill(dataTable)

                    ' Add appointments to the stack
                    For Each row As DataRow In dataTable.Rows
                        Dim id As Integer = Integer.Parse(row("id").ToString())
                        Dim doctorName As String = row("docName").ToString()
                        Dim patientName As String = row("patientName").ToString()
                        Dim appointmentDate As Date = Date.Parse(row("date").ToString())
                        Dim appointmentTime As String = row("time").ToString()
                        Dim contactNo As String = row("contactNo").ToString()
                        Dim email As String = row("email").ToString()

                        Dim appointment As New Appointment(id, doctorName, patientName, appointmentDate, appointmentTime, contactNo, email)
                        appointmentsStack.Push(appointment)
                    Next
                End Using
            End Using
        End Using

        ' Bind the appointmentsStack to the DataGridView
        PatientDGV.DataSource = New BindingSource(appointmentsStack, Nothing)
    End Sub


    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load appointments data into DataGridView
        LoadAppointmentsDataFromDatabase()

        PatientDGV.Columns("ID").Visible = False

        ' Load doctor names into ComboBox1
        LoadDoctorNames()

        ' Load patient names into ComboBox2
        LoadPatientNames()
    End Sub

    ' Load doctor names into ComboBox1
    Private Sub LoadDoctorNames()
        ' Define the MySQL SELECT query for doctors
        Dim query As String = "SELECT name FROM doctors"

        ' Create a new MySqlConnection object
        Using connection As New MySqlConnection(connectionString)
            ' Create a new MySqlCommand object with the SELECT query and connection
            Using command As New MySqlCommand(query, connection)
                ' Open the connection
                connection.Open()

                ' Execute the command and read data
                Using reader As MySqlDataReader = command.ExecuteReader()
                    ' Clear existing items in ComboBox1
                    ComboBox1.Items.Clear()

                    ' Read doctor names and add them to ComboBox1
                    While reader.Read()
                        ComboBox1.Items.Add(reader("name").ToString())
                    End While
                End Using
            End Using
        End Using
    End Sub

    ' Load patient names into ComboBox2
    Private Sub LoadPatientNames()
        ' Define the MySQL SELECT query for patients
        Dim query As String = "SELECT name FROM patients"

        ' Create a new MySqlConnection object
        Using connection As New MySqlConnection(connectionString)
            ' Create a new MySqlCommand object with the SELECT query and connection
            Using command As New MySqlCommand(query, connection)
                ' Open the connection
                connection.Open()

                ' Execute the command and read data
                Using reader As MySqlDataReader = command.ExecuteReader()
                    ' Clear existing items in ComboBox2
                    ComboBox2.Items.Clear()

                    ' Read patient names and add them to ComboBox2
                    While reader.Read()
                        ComboBox2.Items.Add(reader("name").ToString())
                    End While
                End Using
            End Using
        End Using
    End Sub

    ' Define a class to represent an appointment
    Private Class Appointment
        Public Property ID As Integer
        Public Property DoctorName As String
        Public Property PatientName As String
        Public Property AppointmentDate As Date
        Public Property AppointmentTime As String
        Public Property ContactNo As String
        Public Property Email As String

        Public Sub New(id As Integer, doctorName As String, patientName As String, appointmentDate As Date, appointmentTime As String, contactNo As String, email As String)
            Me.ID = id
            Me.DoctorName = doctorName
            Me.PatientName = patientName
            Me.AppointmentDate = appointmentDate
            Me.AppointmentTime = appointmentTime
            Me.ContactNo = contactNo
            Me.Email = email
        End Sub
    End Class

    Private Sub PatientDGV_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles PatientDGV.CellClick
        If e.RowIndex >= 0 AndAlso e.RowIndex < PatientDGV.Rows.Count Then
            Dim selectedRow As DataGridViewRow = PatientDGV.Rows(e.RowIndex)

            ' Get the values directly from the cells by column index
            ComboBox1.Text = selectedRow.Cells("DoctorName").Value.ToString()
            ComboBox2.Text = selectedRow.Cells("PatientName").Value.ToString()
            DateTimePicker1.Value = Date.Parse(selectedRow.Cells("AppointmentDate").Value.ToString())
            TextBox3.Text = selectedRow.Cells("AppointmentTime").Value.ToString()
            TextBox5.Text = selectedRow.Cells("ContactNo").Value.ToString()
            TextBox1.Text = selectedRow.Cells("Email").Value.ToString()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        UpdateAppointment()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        DeleteAppointment()
    End Sub

    Private Sub UpdateAppointment()
        ' Extract data from form controls
        Dim id As Integer = DirectCast(PatientDGV.CurrentRow.DataBoundItem, Appointment).ID
        Dim doctorName As String = ComboBox1.Text
        Dim patientName As String = ComboBox2.Text
        Dim appointmentDate As Date = DateTimePicker1.Value.Date
        Dim appointmentTime As String = TextBox3.Text
        Dim contactNo As String = TextBox5.Text
        Dim email As String = TextBox1.Text

        ' Define the MySQL UPDATE query
        Dim query As String = "UPDATE appointments SET docName = @DoctorName, patientName = @PatientName, date = @AppointmentDate, time = @AppointmentTime, contactNo = @ContactNo, email = @Email WHERE id = @ID"

        ' Create a new MySqlConnection object
        Using connection As New MySqlConnection(connectionString)
            ' Create a new MySqlCommand object with the UPDATE query and connection
            Using command As New MySqlCommand(query, connection)
                ' Add parameters to the command
                command.Parameters.AddWithValue("@ID", id)
                command.Parameters.AddWithValue("@DoctorName", doctorName)
                command.Parameters.AddWithValue("@PatientName", patientName)
                command.Parameters.AddWithValue("@AppointmentDate", appointmentDate)
                command.Parameters.AddWithValue("@AppointmentTime", appointmentTime)
                command.Parameters.AddWithValue("@ContactNo", contactNo)
                command.Parameters.AddWithValue("@Email", email)

                ' Open the connection
                connection.Open()

                ' Execute the command
                command.ExecuteNonQuery()

                ' Show prompt
                MessageBox.Show("Appointment updated successfully.")
            End Using
        End Using

        ' Reload appointments data into DataGridView
        LoadAppointmentsDataFromDatabase()
    End Sub

    Private Sub DeleteAppointment()
        ' Extract the appointment ID
        Dim id As Integer = DirectCast(PatientDGV.CurrentRow.DataBoundItem, Appointment).ID

        ' Define the MySQL DELETE query
        Dim query As String = "DELETE FROM appointments WHERE id = @ID"

        ' Create a new MySqlConnection object
        Using connection As New MySqlConnection(connectionString)
            ' Create a new MySqlCommand object with the DELETE query and connection
            Using command As New MySqlCommand(query, connection)
                ' Add parameters to the command
                command.Parameters.AddWithValue("@ID", id)

                ' Open the connection
                connection.Open()

                ' Execute the command
                command.ExecuteNonQuery()

                ' Show prompt
                MessageBox.Show("Appointment deleted successfully.")
            End Using
        End Using

        ' Reload appointments data into DataGridView
        LoadAppointmentsDataFromDatabase()
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        LoadAppointmentsDataFromDatabase()
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        ' Check if a patient is selected in ComboBox2
        If ComboBox2.SelectedIndex <> -1 Then
            ' Get the selected patient name
            Dim selectedPatient As String = ComboBox2.SelectedItem.ToString()

            ' Fetch contact number and email of the selected patient from the database
            Dim contactNo As String = ""
            Dim email As String = ""

            ' Define the MySQL SELECT query to fetch contact number and email of the selected patient
            Dim query As String = "SELECT contactNo, email FROM patients WHERE name = @PatientName"

            ' Create a new MySqlConnection object
            Using connection As New MySqlConnection(connectionString)
                ' Create a new MySqlCommand object with the SELECT query and connection
                Using command As New MySqlCommand(query, connection)
                    ' Add parameter for patient name
                    command.Parameters.AddWithValue("@PatientName", selectedPatient)

                    ' Open the connection
                    connection.Open()

                    ' Execute the command and read data
                    Using reader As MySqlDataReader = command.ExecuteReader()
                        If reader.Read() Then
                            ' Get contact number and email from the reader
                            contactNo = reader("contactNo").ToString()
                            email = reader("email").ToString()
                        End If
                    End Using
                End Using
            End Using

            ' Update TextBoxes with contact number and email
            TextBox5.Text = contactNo
            TextBox1.Text = email
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        ClearAppointmentFields()
    End Sub
End Class
