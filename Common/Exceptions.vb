
Namespace Common.Exceptions

  ''' <summary>
  '''   Empty file exception that will denote when a file contains no valid data.
  ''' </summary>
  Public Class EmptyFileException
    Inherits Exception

    Public Sub New()
      MyBase.New("The file provided does not contain any valid data")
    End Sub

  End Class

  ''' <summary>
  '''   Invalid header exception that will denote when the header field count does not match the expected field count.
  ''' </summary>
  Public Class InvalidHeaderException
    Inherits Exception

    Public Sub New()
      MyBase.New("The header row's field count does not match the specified field count")
    End Sub
  End Class
End Namespace

