Imports System.IO

Namespace Data
  Public Class ValidateFileResult
#Region "Private Variables"
    Private _valid As Boolean
    Private _exception As Exception
    Private _message As String
    Private _filePath As String
#End Region

#Region "Constructor"
    Public Sub New()
      _valid = False
      _exception = Nothing
      _filePath = Nothing
      _message = Nothing
    End Sub
#End Region

#Region "Properties"
    Public Property IsValid() As Boolean
      Get
        Return _valid
      End Get
      Set(value As Boolean)
        _valid = value
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

    Public Property FilePath() As String
      Get
        Return _filePath
      End Get
      Set(value As String)
        _filePath = value
      End Set
    End Property
#End Region
  End Class
End Namespace