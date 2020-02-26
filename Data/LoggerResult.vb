

Namespace Data

  ''' <summary>
  '''   Logger result data object. Stores basic result information for logger operation.
  ''' </summary>
  Public Class LoggerResult

#Region "Private Variables"
    Private _logFolder As String
    Private _successful As Boolean
    Private _exception As Exception
    Private _message As String
#End Region

#Region "Constructor"
    Public Sub New()
      _logFolder = Nothing
      _successful = False
      _exception = Nothing
      _message = Nothing
    End Sub
#End Region

#Region "Properties"
    Public Property LogFolder() As String
      Get
        Return _logFolder
      End Get
      Set(value As String)
        _logFolder = value
      End Set
    End Property

    Public Property IsSuccessful() As Boolean
      Get
        Return _successful
      End Get
      Set(value As Boolean)
        _successful = value
      End Set
    End Property

    Public Property Exception As Exception
      Get
        Return _exception
      End Get
      Set(value As Exception)
        _exception = value
      End Set
    End Property

    Public Property ErrorMessage As String
      Get
        Return _message
      End Get
      Set(value As String)
        _message = value
      End Set
    End Property
  End Class
#End Region
End Namespace