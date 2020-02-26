Imports System.IO
Imports ParserApp.Common

Namespace Business

  ''' <summary>
  '''   Simple class used to log valid and invalid entries. Log entries are placed in corresponding folders of the relative path that the application is running in.
  ''' </summary>
  Public Class Logger

    Private _relativePath As String = Nothing

    Public Sub New()
      Initialize()
    End Sub

    ''' <summary>
    '''   Initializes the log folders. Will create new folders if they do not exist.
    ''' </summary>
    Private Sub Initialize()
      _relativePath = Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\", String.Empty)

      If Not Directory.Exists($"{_relativePath}\Logs") Then
        My.Computer.FileSystem.CreateDirectory($"{_relativePath}\Logs")
      End If

      If Not Directory.Exists($"{_relativePath}\Logs\Valid") Then
        My.Computer.FileSystem.CreateDirectory($"{_relativePath}\Logs\Valid")
      End If

      If Not Directory.Exists($"{_relativePath}\Logs\Invalid") Then
        My.Computer.FileSystem.CreateDirectory($"{_relativePath}\Logs\Invalid")
      End If
    End Sub

    ''' <summary>
    '''   Logs the valid and invalid records that are passed into it. Uses the given column names and delimiter type to format the data in the log and determine the log name.
    ''' </summary>
    ''' <returns>LoggerResult object with the success status and error message if applicable.</returns>
    Public Function LogRecords(ByVal columnNames As List(Of String), ByVal validRecords As List(Of String), ByVal invalidRecords As List(Of String), ByVal delimiterType As Enums.DelimiterType) As Data.LoggerResult
      Dim result As New Data.LoggerResult

      Try
        LogValidRecords(columnNames, validRecords, delimiterType)
        LogInvalidRecords(columnNames, invalidRecords, delimiterType)
        result.LogFolder = $"{_relativePath}\Logs"
        result.IsSuccessful = True

      Catch e As Exception
        result.IsSuccessful = False
        result.Exception = e
        result.ErrorMessage = $"There was an error while generating the logs. Exception details: {e.Message}"
      End Try

      Return result
    End Function

    ''' <summary>
    '''   Gets the delimiter in a string format
    ''' </summary>
    ''' <returns>String representation of the delimiter based on the given delimiter type enum value</returns>
    Private Function GetDelimiter(ByVal type As Enums.DelimiterType) As String
      Select Case type
        Case Enums.DelimiterType.CSV
          Return ","
        Case Enums.DelimiterType.TSV
          Return Convert.ToChar(9)
        Case Else
          Return Nothing
      End Select
    End Function

    ''' <summary>
    '''   Logs the given valid records to a new log file in the valid log directory
    ''' </summary>
    Private Sub LogValidRecords(ByVal columnNames As List(Of String), ByVal validRecords As List(Of String), ByVal delimiterType As Enums.DelimiterType)
      Dim filePath As String = $"{_relativePath}\Logs\Valid\{DateTime.UtcNow.ToString("MMddyyyy_HHmmssfff")}_{delimiterType.ToString}.txt"
      Dim writer As StreamWriter = Nothing

      Try
        ' write the log details
        writer = New StreamWriter(filePath)

        writer.WriteLine("Expected fields: " & String.Join(GetDelimiter(delimiterType), columnNames))
        writer.WriteLine(String.Empty)
        writer.WriteLine("Correctly formatted records: ")
        writer.WriteLine(String.Empty)

        For Each record As String In validRecords
          writer.WriteLine(record)
        Next

      Catch ex As Exception
        Throw ex
      Finally
        ' always close the writer if it has been initialized
        If writer IsNot Nothing Then writer.Close()
      End Try
    End Sub

    ''' <summary>
    '''   Logs the given invalid records to a new log file in the invalid log directory
    ''' </summary>
    Private Sub LogInvalidRecords(ByVal columnNames As List(Of String), ByVal invalidRecords As List(Of String), ByVal delimiterType As Enums.DelimiterType)
      Dim filePath As String = $"{_relativePath}\Logs\Invalid\{DateTime.UtcNow.ToString("MMddyyyy_HHmmssfff")}_{delimiterType.ToString}.txt"
      Dim writer As StreamWriter = Nothing

      Try
        ' write the log details
        writer = New StreamWriter(filePath)

        writer.WriteLine("Expected fields: " & String.Join(GetDelimiter(delimiterType), columnNames))
        writer.WriteLine(String.Empty)
        writer.WriteLine("Incorrectly formatted records: ")
        writer.WriteLine(String.Empty)

        For Each record As String In invalidRecords
          writer.WriteLine(record)
        Next

      Catch ex As Exception
        Throw ex
      Finally
        ' always close the writer if it has been initialized
        If writer IsNot Nothing Then writer.Close()
      End Try
    End Sub
  End Class
End Namespace
