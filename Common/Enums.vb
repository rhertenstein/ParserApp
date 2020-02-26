
Namespace Common.Enums

  ''' <summary>
  '''   File type enum to determine the valid types for the application versus the user input.
  ''' </summary>
  Public Enum FileType As Integer
    Invalid = -1
    TXT = 0
    CSV = 1
    TSV = 2
  End Enum

  ''' <summary>
  '''   Delimiter type enum to determine the valid types for the application versus the user input.
  ''' </summary>
  Public Enum DelimiterType
    Undefined = -1
    CSV = 0
    TSV = 1
  End Enum
End Namespace
