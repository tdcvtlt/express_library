Imports Microsoft.VisualBasic
Imports ZXing

Public Class barcodes
    Public Function Code_128(code As String, Optional height As Integer = 200, Optional width As Integer = 600) As Drawing.Bitmap
        Dim writer As BarcodeWriter = New BarcodeWriter()
        writer.Format = BarcodeFormat.CODE_128
        writer.Options.Height = height
        writer.Options.Width = width
        Return writer.Write(code)
    End Function


End Class
