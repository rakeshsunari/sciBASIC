﻿#Region "Microsoft.VisualBasic::908c9cbdd97c267de749a744b89a5281, Data\BinaryData\DataStorage\netCDF\netCDFReader.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:

    '     Class netCDFReader
    ' 
    '         Properties: dimensions, globalAttributes, recordDimension, variables, version
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: attributeExists, dataVariableExists, (+2 Overloads) getDataVariable, getDataVariableAsString, getDataVariableEntry
    '                   Open, ToString
    ' 
    '         Sub: (+2 Overloads) Dispose, Print
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Data.IO.netCDF.Components
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Text

Namespace netCDF

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' https://github.com/cheminfo-js/netcdfjs
    ''' </remarks>
    Public Class netCDFReader : Implements IDisposable

        Dim buffer As BinaryDataReader
        Dim header As Header
        Dim globalAttributeTable As Dictionary(Of String, attribute)
        Dim variableTable As Dictionary(Of String, variable)

        Public Const Magic$ = "CDF"

        ''' <summary>
        ''' Version for the NetCDF format
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property version As String
            Get
                If (header.version = 1) Then
                    Return "classic format"
                Else
                    Return "64-bit offset format"
                End If
            End Get
        End Property

        ''' <summary>
        ''' Metadata for the record dimension
        ''' 
        '''  + `length`: Number of elements in the record dimension
        '''  + `id`: Id number In the list Of dimensions For the record dimension
        '''  + `name`: String with the name of the record dimension
        '''  + `recordStep`: Number with the record variables step size
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property recordDimension As recordDimension
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return header.recordDimension
            End Get
        End Property

        ''' <summary>
        ''' List of dimensions with:
        ''' 
        '''  + `name`: String with the name of the dimension
        '''  + `size`: Number with the size of the dimension
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' 一个cdf文件之中只能够有一种<see cref="Dimension"/>可以是矩阵类型的么？
        ''' </remarks>
        Public ReadOnly Property dimensions As Dimension()
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return header.dimensions
            End Get
        End Property

        ''' <summary>
        ''' List of global attributes with:
        ''' 
        '''  + `name`: String with the name of the attribute
        '''  + `type`: String with the type of the attribute
        '''  + `value`: A number Or String With the value Of the attribute
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property globalAttributes As attribute()
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return header.globalAttributes
            End Get
        End Property

        ''' <summary>
        ''' Returns the value of an global attribute
        ''' </summary>
        ''' <param name="attributeName">attributeName</param>
        ''' <returns>Value of the attributeName Or undefined</returns>
        Default Public ReadOnly Property getAttribute(attributeName As String) As Object
            Get
                With globalAttributeTable.TryGetValue(attributeName)
                    If .IsNothing Then
                        Return Nothing
                    Else
                        Select Case .type
                            Case CDFDataTypes.BYTE : Return Byte.Parse(.value)
                            Case CDFDataTypes.CHAR : Return .value
                            Case CDFDataTypes.DOUBLE : Return Double.Parse(.value)
                            Case CDFDataTypes.FLOAT : Return Single.Parse(.value)
                            Case CDFDataTypes.INT : Return Integer.Parse(.value)
                            Case CDFDataTypes.SHORT : Return Short.Parse(.value)
                            Case CDFDataTypes.LONG : Return Long.Parse(.value)

                            Case Else
                                Throw New NotSupportedException
                        End Select
                    End If
                End With
            End Get
        End Property

        ''' <summary>
        ''' List of variables with:
        ''' 
        '''  + `name`: String with the name of the variable
        '''  + `dimensions`: Array with the dimension IDs of the variable
        '''  + `attributes`: Array with the attributes of the variable
        '''  + `type`: String with the type of the variable
        '''  + `size`: Number with the size of the variable
        '''  + `offset`: Number with the offset where of the variable begins
        '''  + `record`: True if Is a record variable, false otherwise
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property variables As variable()
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return header.variables
            End Get
        End Property

        Sub New(buffer As BinaryDataReader)
            Dim version As Value(Of Byte) = Scan0

            buffer.ByteOrder = ByteOrder.BigEndian
            ' Test if file in support format
            Utils.notNetcdf(buffer.ReadString(3) <> Magic, $"should start with {Magic}")
            Utils.notNetcdf((version = buffer.ReadByte) > 2, "unknown version")

            Me.header = New Header(buffer, version)
            Me.buffer = buffer
            Me.globalAttributeTable = header _
                .globalAttributes _
                .ToDictionary(Function(att) att.name)
            Me.variableTable = header _
                .variables _
                .ToDictionary(Function(var) var.name)
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Sub New(path$, Optional encoding As Encodings = Encodings.UTF8)
            Call Me.New(path.OpenBinaryReader(encoding))
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function Open(filePath$, Optional encoding As Encodings = Encodings.UTF8) As netCDFReader
            Return New netCDFReader(filePath, encoding)
        End Function

        ''' <summary>
        ''' Returns the value of a variable as a string
        ''' </summary>
        ''' <param name="variableName">variableName</param>
        ''' <returns>Value of the variable as a string Or undefined</returns>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function getDataVariableAsString(variableName As String) As String
            Return getDataVariable(variableName).ToString
        End Function

        ''' <summary>
        ''' Retrieves the data for a given variable
        ''' </summary>
        ''' <returns>List with the variable values</returns>
        Public Function getDataVariable(variable As variable) As CDFData
            Dim values As Object()

            ' go to the offset position
            Call buffer.Seek(variable.offset, SeekOrigin.Begin)

            If (variable.record) Then
                ' record variable case
                values = DataReader.record(buffer, variable, header.recordDimension)
            Else
                ' non-record variable case
                values = DataReader.nonRecord(buffer, variable)
            End If

            Return (values, variable.type)
        End Function

        ''' <summary>
        ''' Retrieves the data for a given variable
        ''' </summary>
        ''' <param name="variableName">Name of the variable to search Or variable object</param>
        ''' <returns>List with the variable values</returns>
        Public Function getDataVariable(variableName As String) As CDFData
            ' search the variable
            Dim variable As variable = variableTable.TryGetValue(variableName)
            ' throws if variable Not found
            Utils.notNetcdf(variable Is Nothing, $"variable Not found: {variableName}")

            Return getDataVariable(variable)
        End Function

        Public Function getDataVariableEntry(variableName As String) As variable
            Return variableTable.TryGetValue(variableName)
        End Function

        ''' <summary>
        ''' Check if a dataVariable exists
        ''' </summary>
        ''' <param name="variableName">Name of the variable to find</param>
        ''' <returns></returns>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function dataVariableExists(variableName As String) As Boolean
            Return variableTable.ContainsKey(variableName)
        End Function

        ''' <summary>
        ''' Check if an attribute exists
        ''' </summary>
        ''' <param name="attributeName">Name of the attribute to find</param>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function attributeExists(attributeName As String) As Boolean
            Return globalAttributeTable.ContainsKey(attributeName)
        End Function

        ''' <summary>
        ''' CDF file data summary
        ''' </summary>
        ''' <returns></returns>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            With New StringBuilder
                Call netCDF.toString(Me, New System.IO.StringWriter(.ByRef))
                Return .ToString
            End With
        End Function

        ''' <summary>
        ''' Print CDF file data summary on console screen std_output
        ''' </summary>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub Print()
            Call Me.toString(New StreamWriter(Console.OpenStandardOutput))
        End Sub

#Region "IDisposable Support"
        Private disposedValue As Boolean ' 要检测冗余调用

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: 释放托管状态(托管对象)。
                    Call buffer.Dispose()
                End If

                ' TODO: 释放未托管资源(未托管对象)并在以下内容中替代 Finalize()。
                ' TODO: 将大型字段设置为 null。
            End If
            disposedValue = True
        End Sub

        ' TODO: 仅当以上 Dispose(disposing As Boolean)拥有用于释放未托管资源的代码时才替代 Finalize()。
        'Protected Overrides Sub Finalize()
        '    ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' Visual Basic 添加此代码以正确实现可释放模式。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' 请勿更改此代码。将清理代码放入以上 Dispose(disposing As Boolean)中。
            Dispose(True)
            ' TODO: 如果在以上内容中替代了 Finalize()，则取消注释以下行。
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace
