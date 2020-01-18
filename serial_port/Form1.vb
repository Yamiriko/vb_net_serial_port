Imports System.IO

Public Class Form1
    'Jean Riko Kurniawan Putra
    'Telegram/WA/CALL 082386944596

    Dim readBuffer As String

    Private Sub baca_port()
        Dim portName() As String
        Dim i As Integer

        Try
            portName = System.IO.Ports.SerialPort.GetPortNames()
            For i = 0 To i >= (portName.Length)
                Comlist.Items.Add(portName(i))
            Next i

            Comlist.SelectedText = Comlist.Items(0)
            disBtn.Enabled = False
        Catch ex As Exception
            MsgBox("Belum Ada Serial Port Aktif, Silahkan Cek Koneksi Serial Port Anda")
        End Try
    End Sub

    Private Sub ConBtn_Click(sender As Object, e As EventArgs) Handles ConBtn.Click
        If SerialPort1.IsOpen Then
            SerialPort1.Close()
        End If

        Try
            With SerialPort1
                .PortName = Comlist.Text
                .BaudRate = 9600
                .Parity = IO.Ports.Parity.None
                .DataBits = 8
                .StopBits = IO.Ports.StopBits.One
                .Open()
            End With
            Label1.Text = Comlist.Text + " Connected"
            ConBtn.Enabled = False
            disBtn.Enabled = True

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub disBtn_Click(sender As Object, e As EventArgs) Handles disBtn.Click
        Try
            SerialPort1.DiscardInBuffer() 'mengosongkan input buffer
            SerialPort1.Close()
            ConBtn.Enabled = True
            disBtn.Enabled = False
            Label1.Text = "  Disconnected"
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub SerialPort1_DataReceived(sender As Object, e As Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        If SerialPort1.IsOpen Then
            Try
                readBuffer = SerialPort1.ReadLine()
                'data to UI thread
                Me.Invoke(New EventHandler(AddressOf DoUpdate))
            Catch ex As Exception
                MsgBox("read " & ex.Message)
            End Try
        End If
    End Sub

    Public Sub DoUpdate(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim temp As Decimal
        temp = CInt(readBuffer) / 1024 * 5 * 100
        TextBox2.Text = Int(temp)
        TextBox4.Text = CInt(readBuffer)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        baca_port()
    End Sub
End Class
