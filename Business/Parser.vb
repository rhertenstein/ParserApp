Imports ParserApp.Common
Imports System.IO

Namespace Business

  ''' <summary>
  '''   Simple parser class for parsing TSV and CSV files.
  ''' </summary>
  Public Class Parser

    ''' <summary>
    '''   Parses a file with the given path, delimiter, and field count.
    ''' </summary>
    ''' <returns>ParserResult object that contains success status, valid/invalid records, and error details if applicable.</returns>
    Public Function ParseFile(ByVal filePath As String, ByVal delimiterType As Enums.DelimiterType, ByVal fieldCount As Integer) As Data.ParserResult
      Dim result As New Data.ParserResult
      Dim delimiter As Char = GetDelimiterFromType(delimiterType)
      Dim reader As StreamReader = Nothing
      Dim fHeaderChecked As Boolean = False

      Try
        ' start parsing the file
        reader = New StreamReader(filePath)

        While reader.Peek() >= 0
          Dim record As String = reader.ReadLine()
          Dim fields As String() = record.Split(delimiter)

          ' get the header row and validate the field count before attempting to parse further
          If Not fHeaderChecked Then
            If fields.Count <> fieldCount Then Throw New Exceptions.InvalidHeaderException()

            result.ColumnNames = New List(Of String)(fields)
            fHeaderChecked = True
          Else
            If fields.Count <> fieldCount Then
              result.InvalidRecords.Add(record)
            Else
              result.ValidRecords.Add(record)
            End If
          End If
        End While

        ' if the header checked bool was never toggled then no valid data is in the file. Throw empty file exception
        If Not fHeaderChecked Then Throw New Exceptions.EmptyFileException()

        result.IsSuccessful = True

      Catch e As Exceptions.InvalidHeaderException
        result.IsSuccessful = False
        result.Exception = e
        result.ErrorMessage = $"Invalid Parser Settings! {e.Message}"

      Catch e As Exceptions.EmptyFileException
        result.IsSuccessful = False
        result.Exception = e
        result.ErrorMessage = $"Invalid File! {e.Message}"

      Catch e As Exception
        result.IsSuccessful = False
        result.Exception = e
        result.ErrorMessage = $"The file could not be read: {e.Message}"

      Finally
        'always close the reader if it has been initialized
        If reader IsNot Nothing Then reader.Close()
      End Try

      Return result
    End Function

    ''' <summary>
    '''   Gets the delimiter char from the given delimiter type enum.
    ''' </summary>
    ''' <returns>The char representation of the delimiter type value.</returns>
    Private Function GetDelimiterFromType(ByVal delimiterType As Enums.DelimiterType) As Char
      Select Case delimiterType
        Case Enums.DelimiterType.CSV
          Return ","c
        Case Enums.DelimiterType.TSV
          Return Convert.ToChar(9)
        Case Else
          Return Nothing
      End Select
    End Function
  End Class
End Namespace
