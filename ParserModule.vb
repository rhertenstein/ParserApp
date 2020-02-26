Imports System.IO
Imports ParserApp.Common

Module ParserModule

#Region "Private Variables"
  Private _parser As Business.Parser
  Private _validator As Business.Validator
  Private _logger As Business.Logger
#End Region

  Sub Main()
    ' initialize locals to default values 
    Dim filePath As String = Nothing
    Dim fieldCount As Integer = 0
    Dim delimiter As Enums.DelimiterType = Enums.DelimiterType.Undefined
    Dim result As Data.ParserResult = Nothing
    Dim exitApp As Boolean = False

    ' initialize the stateless business objects so they can be used during operation.
    _parser = New Business.Parser()
    _validator = New Business.Validator()
    _logger = New Business.Logger()

    Console.WriteLine("Welcome to the name parser application")

    ' Operate in a loop in case the user wants to parse multiple documents. Keeps user from entering/exiting the app multiple times.
    While Not exitApp
      filePath = GetFilePath()
      delimiter = GetDelimiterType()
      fieldCount = GetFieldCount()
      result = ParseFile(filePath, delimiter, fieldCount)
      If result.IsSuccessful Then LogResults(result.ColumnNames, result.ValidRecords, result.InvalidRecords, delimiter)

      'check for user input to exit the application
      exitApp = GetExitResponse()
    End While
  End Sub

#Region "Helper Methods"

  ''' <summary>
  '''   Polls the user to see if they want to exit the application.
  ''' </summary>
  ''' <returns>Boolean value indication whether to exit the application or not.</returns>
  Private Function GetExitResponse() As Boolean
    ' use a nullable boolean for the purpose of determining invalid input from the user versus the expected Y/N value.
    Dim exitApp As Boolean? = Nothing
    Dim input As String = Nothing

    ' operate in a loop so that application can respond to invalid input
    While Not exitApp.HasValue
      Console.WriteLine($"{Environment.NewLine}Would you like to parse another file? Y / N")

      ' capture and validate input
      input = Console.ReadLine()
      exitApp = _validator.ValidateExitInput(input)

      ' notify the user if their input was invalid
      If Not exitApp.HasValue Then Console.WriteLine("Invalid input! Only ""Y"" or ""N"" are valid inputs")
    End While

    Return exitApp.Value
  End Function

  ''' <summary>
  '''   Polls the user for the file path value.
  ''' </summary>
  ''' <returns>The file path value that they entered as a string.</returns>
  Private Function GetFilePath() As String
    Dim filePath As String = Nothing
    Dim result As Data.ValidateFileResult = Nothing

    ' operate in a loop in case invalid input is entered
    While result Is Nothing OrElse Not result.IsValid
      Console.WriteLine($"{Environment.NewLine}Please enter the full file path to your list of names for parsing")

      ' capture and validate input
      filePath = Console.ReadLine()
      result = _validator.ValidateFilePath(filePath)

      ' notify the user if their input was invalid
      If Not result.IsValid Then Console.WriteLine(result.ErrorMessage)
    End While

    Return result.FilePath
  End Function

  ''' <summary>
  '''   Polls the user for the delimiter type.
  ''' </summary>
  ''' <returns>DelimiterType enum that represents the input from the user.</returns>
  Private Function GetDelimiterType() As Common.Enums.DelimiterType
    Dim delimiterType As Common.Enums.DelimiterType = Common.Enums.DelimiterType.Undefined
    Dim input As String = Nothing

    ' operate in a loop in case invalid input is entered
    While delimiterType = Common.Enums.DelimiterType.Undefined
      Console.WriteLine($"{Environment.NewLine}Please enter ""CSV"" or ""TSV"" to determine the delimiter used in the file")

      ' capture and validate input
      input = Console.ReadLine()
      delimiterType = _validator.ValidateDelimiterInput(input)

      ' notify the user if their input was invalid
      If delimiterType = Common.Enums.DelimiterType.Undefined Then Console.WriteLine("The input was invalid!")
    End While

    Return delimiterType
  End Function

  ''' <summary>
  '''   Polls the user for the field count in the file they want to parse.
  ''' </summary>
  ''' <returns>Integer value representing the expected number of fields for each record in the file.</returns>
  Private Function GetFieldCount() As Integer
    Dim fieldCount As Integer = 0
    Dim input As String

    ' operate in a loop in case invalid input is entered
    While fieldCount <= 0
      Console.WriteLine($"{Environment.NewLine}How many fields will each record have?")

      ' capture and validate input
      input = Console.ReadLine()
      fieldCount = _validator.ValidateFieldCountInput(input)

      ' notify the user if their input was invalid
      If fieldCount <= 0 Then Console.WriteLine("The input was not a valid number!")
    End While

    Return fieldCount
  End Function

  ''' <summary>
  '''   Parse the file related to the file path entered by the user. Also uses the delimiter and field count for parser operation.
  ''' </summary>
  ''' <returns>ParserResult data object that contains values related to the parser operation. Holds values to determin if the operation was successful and error details if applicable.</returns>
  Private Function ParseFile(ByVal filePath As String, ByVal delimiter As Common.Enums.DelimiterType, ByVal fieldCount As Integer) As Data.ParserResult
    Dim result As New Data.ParserResult

    ' notify the user that their file is being parsed in case operation takes time
    Console.WriteLine($"{Environment.NewLine}Please wait while your file is parsed...")

    ' use the stateless parser business object to parse the file and get the result details
    result = _parser.ParseFile(filePath, delimiter, fieldCount)

    ' check result for success and notify the user appropriately
    If result.IsSuccessful Then
      Console.WriteLine($"{Environment.NewLine}Parsing is complete!")
    Else
      Console.WriteLine($"{Environment.NewLine}Parsing did not complete successfully. {result.ErrorMessage}")
    End If

    Return result
  End Function

  ''' <summary>
  '''   Log the parser results for the user.
  ''' </summary>
  Private Sub LogResults(ByVal columnNames As List(Of String), ByVal validRecords As List(Of String), ByVal invalidRecords As List(Of String), ByVal delimiter As Enums.DelimiterType)
    Dim result As New Data.LoggerResult

    ' verify that logs exist to parse. If no records exist then notify the user and exit the method
    If validRecords.Count = 0 AndAlso invalidRecords.Count = 0 Then
      Console.WriteLine($"{Environment.NewLine}No records were parsed. Logs will not be generated.")
      Return
    End If

    ' notify the user that their logs are being generated.
    Console.WriteLine($"{Environment.NewLine}Please wait while your logs are generated...")

    ' use the stateless logger business object to log the valid and invalid records
    result = _logger.LogRecords(columnNames, validRecords, invalidRecords, delimiter)

    ' check the result for success and notify the user appropriately
    If result.IsSuccessful Then
      Console.WriteLine($"{Environment.NewLine}The logs have been generated and can be found at {result.LogFolder}")
    Else
      Console.WriteLine($"{Environment.NewLine}{result.ErrorMessage}")
    End If
  End Sub
#End Region
End Module
