Imports System.Data.Odbc
Imports System.Data
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Xml
Imports System.Drawing.Text
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Linq
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports Microsoft.Office.Core
Imports ExcelAutoFormat = Microsoft.Office.Interop.Excel.XlRangeAutoFormat
Imports Microsoft.Office.Interop
Imports System.Xml.XPath
Imports System.CodeDom
Imports System.Data.Common

Public Class Form1
    Dim Conn As OdbcConnection
    Dim Cmd As OdbcCommand
    Dim Ds As DataSet
    Dim Da As OdbcDataAdapter
    Dim Rd As OdbcDataReader
    Dim MyDB As String

    'Membuat koneksi
    Sub Koneksi()
        'Memanggil database
        MyDB = "Driver={MySQL ODBC 8.1 ANSI DRIVER};Database=inventoryapp;Server=localhost;uid=root"
        Conn = New OdbcConnection(MyDB)
        If Conn.State = ConnectionState.Closed Then Conn.Open()
    End Sub

    'Memanggil kondisi awal
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""

        'TextBox 1,2,3,4 formnya dimatikan
        TextBox1.Enabled = False
        TextBox2.Enabled = False
        TextBox3.Enabled = False
        TextBox4.Enabled = False

        'Button 1,2,3,4 tambahkan text masing-masing
        Button1.Text = "INPUT"
        Button2.Text = "EDIT"
        Button3.Text = "HAPUS"
        Button4.Text = "TUTUP"

        'Button 1,2,3,4 diaktifkan dengan menggunakan perintah True
        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button4.Enabled = True

        'Panggil koneksi
        Call Koneksi()

        'Memanggil tabel yang sudah dibuat
        Da = New OdbcDataAdapter("Select * From barang", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "barang")
        DataGridView1.DataSource = Ds.Tables("barang")
    End Sub

    Sub FieldAktif()
        'Untuk mengaktifkan form
        TextBox1.Enabled = True
        TextBox2.Enabled = True
        TextBox3.Enabled = True
        TextBox4.Enabled = True
        TextBox1.Focus()
    End Sub

    'Proses Input
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "INPUT" Then
            Button1.Text = "SIMPAN"
            'Button 2 dan 3 tidak aktif. button 4 ubah jadi "BATAL"
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Text = "BATAL"

            Call FieldAktif()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
                MsgBox("Pastikan semua field terisi")
            Else
                Call Koneksi()
                Dim InputData As String = "Insert into barang values('" & TextBox1.Text & "','" & TextBox2.Text & "', '" & TextBox3.Text & "', '" & TextBox4.Text & "')"
                Cmd = New OdbcCommand(InputData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Simpan Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub

    'Proses Edit
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.Text = "EDIT" Then
            Button2.Text = "UPDATE"
            Button1.Enabled = False
            Button3.Enabled = False
            Button4.Text = "BATAL"

            Call FieldAktif()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
                MsgBox("Pastikan semua field terisi")
            Else
                Call Koneksi()
                Dim EditData As String = "Update barang set nama_barang='" & TextBox2.Text & "',quantity='" & TextBox3.Text & "',min_stock='" & TextBox4.Text & "'where kode_barang='" & TextBox1.Text & "'"
                Cmd = New OdbcCommand(EditData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Edit data berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub

    'Proses Hapus 
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Text = "HAPUS" Then
            Button3.Text = "DELETE"
            Button1.Enabled = False
            Button2.Enabled = False
            Button4.Text = "BATAL"

            Call FieldAktif()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
                MsgBox("Pastikan data yang akan dihapus terisi")
            Else
                Call Koneksi()
                Dim HapusData As String = "delete from barang where kode_barang='" & TextBox1.Text & "'"
                Cmd = New OdbcCommand(HapusData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Hapus data berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub

    'Keypress
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            Call Koneksi()
            Cmd = New OdbcCommand("Select * from barang where kode_barang='" & TextBox1.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()

            If Rd.HasRows Then
                TextBox2.Text = Rd.Item("nama_barang")
                TextBox3.Text = Rd.Item("quantity")
                TextBox4.Text = Rd.Item("min_stock")
            Else
                MsgBox("Data Tidak Ada")
            End If
        End If
    End Sub

    'Proses Tutup 
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Button4.Text = "TUTUP" Then
            End
        Else
            Call KondisiAwal()
        End If
    End Sub

    'Proses Export to PDF
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim pdfTable As PdfPTable = New PdfPTable(DataGridView1.ColumnCount)
        Dim time As DateTime = DateTimePicker1.Value
        Dim format As String = "yyMMdd"
        Dim fname As String = Convert.ToString(time.ToString(format))
        Dim fnamelast As String
        pdfTable.DefaultCell.Padding = 5
        pdfTable.WidthPercentage = 100
        pdfTable.HorizontalAlignment = Element.ALIGN_LEFT
        pdfTable.DefaultCell.BorderWidth = 2

        Dim flag As Integer = 0
        For Each column As DataGridViewColumn In DataGridView1.Columns
            Dim cell As PdfPCell = New PdfPCell(New Phrase(column.HeaderText))
            cell.BackgroundColor = New iTextSharp.text.BaseColor(172, 209, 233)
            pdfTable.AddCell(cell)
        Next

        For Each row As DataGridViewRow In DataGridView1.Rows
            If flag Mod 2 = 0 Then
                For Each cell As DataGridViewCell In row.Cells
                    If cell.Value Is Nothing Then
                    Else
                        Dim pdfCell As PdfPCell = New PdfPCell(New Phrase(cell.Value.ToString()))
                        pdfCell.BackgroundColor = iTextSharp.text.BaseColor.WHITE
                        pdfTable.AddCell(pdfCell)
                    End If
                Next
            Else
                For Each cell As DataGridViewCell In row.Cells

                    If cell.Value Is Nothing Then
                    Else
                        Dim pdfCell As PdfPCell = New PdfPCell(New Phrase(cell.Value.ToString()))
                        pdfCell.BackgroundColor = New iTextSharp.text.BaseColor(172, 209, 233)
                        pdfTable.AddCell(pdfCell)
                    End If
                Next
            End If
            flag += 1
        Next

        fnamelast = "Inventory-" & fname
        Dim folderPath As String = "F:\PKL TASYRIKA\Inventory\"
        If IsNothing(Directory.Exists(folderPath)) Then
            Directory.CreateDirectory(folderPath)
        End If
        Using Stream As FileStream = New FileStream(folderPath & fnamelast & ".pdf", FileMode.Create)
            Dim document As Document = New Document(PageSize.A4, 10.0F, 10.0F, 10.0F, 0)
            PdfWriter.GetInstance(document, Stream)

            document.Open()
            document.Add(pdfTable)
            document.Close()
            Stream.Close()
            MessageBox.Show("Pdf File Exported")
        End Using
    End Sub

    'Proses Export to Excel 
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim xlApp As Microsoft.Office.Interop.Excel.Application
        Dim xlWorkBook As Microsoft.Office.Interop.Excel.Workbook
        Dim xlWorkSheet As Microsoft.Office.Interop.Excel.Worksheet
        Dim misValue As Object = System.Reflection.Missing.Value
        Dim time As DateTime = DateTimePicker1.Value
        Dim format As String = "yyMMdd"
        Dim fname As String = Convert.ToString(time.ToString(format))
        Dim fnamelast As String
        Dim i As Integer
        Dim j As Integer

        xlApp = New Microsoft.Office.Interop.Excel.Application
        xlWorkBook = xlApp.Workbooks.Add(misValue)
        xlWorkSheet = xlWorkBook.Sheets("sheet1")

        For i = 0 To DataGridView1.RowCount - 2
            For j = 0 To DataGridView1.ColumnCount - 1
                For k As Integer = 1 To DataGridView1.Columns.Count
                    xlWorkSheet.Cells(1, k) = DataGridView1.Columns(k - 1).HeaderText
                    xlWorkSheet.Cells(i + 2, j + 1) = DataGridView1(j, i).Value.ToString()
                Next
            Next
        Next

        fnamelast = "Inventory-" & fname
        xlWorkSheet.SaveAs("F:\PKL TASYRIKA\Inventory\" & fnamelast & ".xlsx")
        xlWorkBook.Close()
        xlApp.Quit()

        releaseObject(xlApp)
        releaseObject(xlWorkBook)
        releaseObject(xlWorkSheet)
        MsgBox("Excel File Exported")
    End Sub

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub
End Class
