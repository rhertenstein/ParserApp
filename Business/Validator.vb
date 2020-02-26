Imports ParserApp.Data
Imports ParserApp.Common
Imports System.IO

Namespace Business

  ''' <summary>
  '''   Simple validation class that will validate the user input for the application.
  ''' </summary>
  Public Class Validator

    ''' <summary>
    '''   Checks to see if a valid extension exists for the file path.
    ''' </summary>
    ''' <returns>Boolean value that indicates if the file has a valid CSV, TXT, or TSV extension.</returns>
    Private Function IsExtensionValid(ByVal extension As String) As Boolean
      Select Case extension.ToUpper.Replace(".", String.Empty)
        Case Enums.FileType.CSV.ToString, Enums.FileType.TSV.ToString, Enums.FileType.TXT.ToString
          Return True
        Case Else
          Return False
      End Select
    End Function

    ''' <summary>
    '''   Checks to see if an extension exists in the user input for the file path.
    ''' </summary>
    ''' <returns>Boolean value that indicates if an extension exists in the file path.</returns>
    Private Function IsExtensionProvided(ByVal filePath As String) As Boolean
      Dim nameParts As String() = filePath.Split("\"c)

      ' this is a crude check that simply looks for at least one period in the last part of the file path as an indicator that a file extension may have been given
      Return nameParts(nameParts.Count - 1).Contains(".")
    End Function

    ''' <summary>
    '''   Checks the given file path for existence, extension validity, valid characters, valid length, and accessibility. 
    ''' </summary>
    ''' <returns>ValidateFileResult object that contains the results of the validation check, including the file path, any exception details, and an error message if applicable.</returns>
    Public Function ValidateFilePath(ByVal filePath As String) As ValidateFileResult
      Dim result As New ValidateFileResult With {.IsValid = True}

      ' perform the most likely fail checks immediately to reduce complexity and wasted processing. Exit immediately if the checks show a fail case.

      ' check the file path's length to verify that it contains text before further execution
      If String.IsNullOrEmpty(filePath) Then
        result.IsValid = False
        result.ErrorMessage = "The file path does not contain any characters!"

        Return result
      End If

      ' check for invalid characters
      If filePath.IndexOfAny(Path.GetInvalidPathChars()) <> -1 Then
        result.IsValid = False
        result.ErrorMessage = "The file path contains invalid characters!"

        Return result
      End If

      ' check for a file extension
      If Not IsExtensionProvided(filePath) Then
        result.IsValid = False
        result.ErrorMessage = "The file path must include the file extension!"

        Return result
      End If

      Try
        ' Exceptions from FileInfo Constructor
        '   System.Security.SecurityException:
        '     The caller does Not have the required permission.
        '
        '   System.IO.PathTooLongException:
        '     The specified path, file name, Or both exceed the system-defined maximum
        '     length. For example, on Windows-based platforms, paths must be less than
        '     248 characters, And file names must be less than 260 characters.
        '
        '   System.NotSupportedException:
        '     file contains a colon () in the middle of the string.
        Dim file As New FileInfo(filePath)

        ' Exceptions using FileInfo.Length
        '   System.IO.IOException
        '     System.IO.FileSystemInfo.Refresh() cannot update the state of the file Or
        '     directory.
        '
        '   System.IO.FileNotFoundException:
        '     The file does Not exist.-Or- The Length property Is called for a directory.
        Dim throwEx As Boolean = file.Length = -1

        ' Exceptions using FileInfo.IsReadOnly
        '   System.UnauthorizedAccessException
        '     Access to file Is denied.
        '     The file described by the current System.IO.FileInfo object Is read-only.-Or-
        '     This operation Is Not supported on the current platform.-Or- The caller does
        '     Not have the required permission.
        throwEx = file.IsReadOnly

        ' Validate the Extension of the file.
        If Not IsExtensionValid(file.Extension) Then
          result.IsValid = False
          result.ErrorMessage = "An invalid file type was used! Valie types: '.tsv', '.csv', and '.txt'"
        End If

        result.FilePath = filePath

      Catch e As System.Security.SecurityException

        ' The caller does Not have the required permission.
        result.IsValid = False
        result.Exception = e
        result.ErrorMessage = "You do not have the permissions required to access the file!"

      Catch e As UnauthorizedAccessException

        ' Access to file Is denied.
        result.IsValid = False
        result.Exception = e
        result.ErrorMessage = "Access to the file has been denied!"

      Catch e As PathTooLongException

        ' The specified path, file name, Or both exceed the system-defined maximum
        ' length. For example, on Windows-based platforms, paths must be less than
        ' 248 characters, And file names must be less than 260 characters.
        result.IsValid = False
        result.Exception = e
        result.ErrorMessage = "The file path exceeds the maximum length and must be less than 248 characters!"

      Catch e As NotSupportedException

        ' file contains a colon or some other unsopported character in the middle of the string.
        result.IsValid = False
        result.Exception = e
        result.ErrorMessage = "There was an unsupported character in the file path!"

      Catch e As FileNotFoundException

        ' The exception that Is thrown when an attempt to access a file that does Not
        ' exist on disk fails.
        result.IsValid = False
        result.Exception = e
        result.ErrorMessage = "The file associated to the given file path does not exist!"

      Catch e As IOException

        ' An I/O error occurred while opening the file.
        result.IsValid = False
        result.Exception = e
        result.ErrorMessage = "There was an error while attempting to open the file!"

      Catch e As Exception

        ' Unexpected Exception
        result.IsValid = False
        result.Exception = e
        result.ErrorMessage = "An unexpected error occurred. Error details: " & e.Message

      End Try

      Return result
    End Function

    ''' <summary>
    '''   Validates the field count input from the user.
    ''' </summary>
    ''' <returns>Integer value that will be more than 0 if the input was valid. Invalid input returns a -1.</returns>
    Public Function ValidateFieldCountInput(ByVal input As String) As Integer
      Dim fieldCount As Integer = -1

      ' use try parse to protect from unwanted exceptions
      Integer.TryParse(input, fieldCount)

      Return fieldCount
    End Function

    ''' <summary>
    '''   Validates the delimiter input value from the user.
    ''' </summary>
    ''' <returns>DelmiterType enum value corresponding to the given delimiter. Invalid input returns the Undefined DelimiterType enum value.</returns>
    Public Function ValidateDelimiterInput(ByVal input As String) As Enums.DelimiterType
      Dim seperator As Enums.DelimiterType = Enums.DelimiterType.Undefined

      Select Case input.ToUpper
        Case "CSV"
          seperator = Enums.DelimiterType.CSV
        Case "TSV"
          seperator = Enums.DelimiterType.TSV
      End Select

      Return seperator
    End Function

    ''' <summary>
    '''   Validates the exit app input from the user.
    ''' </summary>
    ''' <returns>Nullable Boolean value that determines whether to exit the app or not. If the input was not recognized, a null value will be returned.</returns>
    Public Function ValidateExitInput(ByVal input As String) As Boolean?
      Select Case input.ToUpper.Trim
        Case "Y"
          Return False
        Case "N"
          Return True
        Case Else
          Return Nothing
      End Select
    End Function
  End Class
End Namespace