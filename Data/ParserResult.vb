Namespace Data

  ''' <summary>
  '''   Parser result data object. Stores basic result information for parser operation.
  ''' </summary>
  Public Class ParserResult

#Region "Private Variables"
    Private _validRecords As List(Of String)
    Private _invalidRecords As List(Of String)
    Private _columnNames As New List(Of String)
    Private _successful As Boolean
    Private _exception As Exception
    Private _message As String
#End Region

#Region "Constructor"
    Public Sub New()
      _validRecords = New List(Of String)
      _invalidRecords = New List(Of String)
      _columnNames = New List(Of String)
      _successful = False
      _exception = Nothing
      _message = Nothing
    End Sub
#End Region

#Region "Properties"
    Public Property ValidRecords() As List(Of String)
      Get
        Return _validRecords
      End Get
      Set(value As List(Of String))
        _validRecords = value
      End Set
    End Property

    Public Property InvalidRecords() As List(Of String)
      Get
        Return _invalidRecords
      End Get
      Set(value As List(Of String))
        _invalidRecords = value
      End Set
    End Property

    Public Property ColumnNames() As List(Of String)
      Get
        Return _columnNames
      End Get
      Set(value As List(Of String))
        _columnNames = value
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
#End Region
  End Class
End Namespace