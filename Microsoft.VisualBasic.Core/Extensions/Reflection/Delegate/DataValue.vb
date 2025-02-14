﻿#Region "Microsoft.VisualBasic::1ac8e0b02fcb33404236bb7513e0b43f, Microsoft.VisualBasic.Core\Extensions\Reflection\Delegate\DataValue.vb"

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

    '     Class DataValue
    ' 
    '         Properties: PropertyNames
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: GetProperty, inspectType, ToString
    ' 
    '         Sub: TestDEMO
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.DataFramework
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Microsoft.VisualBasic.Scripting.Runtime

Namespace Emit.Delegates

    ''' <summary>
    ''' .NET object collection data property value ``get/set`` helper.
    ''' (将属性的<see cref="PropertyInfo.SetValue(Object, Object)"/>编译为方法调用)
    ''' </summary>
    Public Class DataValue(Of T)

        ReadOnly type As Type = GetType(T)
        ReadOnly data As T()
        ''' <summary>
        ''' Using for expression tree compile to delegate by using <see cref="BindProperty(Of T)"/>, 
        ''' to makes the get/set invoke faster
        ''' </summary>
        ReadOnly properties As Dictionary(Of String, PropertyInfo)

        Public ReadOnly Property PropertyNames As String()
            Get
                Return properties.Values _
                    .Select(Function(x) x.Name) _
                    .ToArray
            End Get
        End Property

        Public Function GetProperty(property$) As PropertyInfo
            Return properties([property])
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="name$">The property name, using the ``nameof`` operator to get the property name!</param>
        ''' <returns></returns>
        Default Public Property Evaluate(name$) As Object
            Get
                Dim [property] As New BindProperty(Of DataFrameColumnAttribute)(properties(name))
                Dim vector As Array = Array.CreateInstance([property].Type, data.Length)

                For i As Integer = 0 To data.Length - 1
                    Call vector.SetValue([property].handleGetValue(data(i)), i)
                Next

                Return vector
            End Get
            Set(value As Object)
                Dim [property] As New BindProperty(Of DataFrameColumnAttribute)(properties(name))
                Dim array As IEnumerable

                If [property].Type Is GetType(String) AndAlso value.GetType Is GetType(String) Then
                    array = Nothing
                Else
                    array = TryCast(value, IEnumerable)
                End If

                If value Is Nothing Then
                    For Each x In data
                        Call [property].handleSetValue(x, Nothing)
                    Next
                ElseIf array Is Nothing Then  ' 不是一个集合
                    Dim v As Object = value

                    For Each x As T In data
                        Call [property].handleSetValue(x, v)
                    Next
                Else
                    Dim vector = array.As(Of Object).ToArray

                    If vector.Length <> data.Length Then
                        Throw New InvalidExpressionException(DimNotAgree$)
                    End If
                    For i As Integer = 0 To data.Length - 1
                        Call [property].handleSetValue(data(i), vector(i))
                    Next
                End If
            End Set
        End Property

        Const DimNotAgree$ = "Value array should have the same length as the target data array"

        'Public Property Evaluate(Of V)(name$) As V()
        '    Get
        '        Dim [property] As New BindProperty(Of DataFrameColumnAttribute)(properties(name))
        '        Return data _
        '            .Select(Function(x) DirectCast([property].__getValue(x), V)) _
        '            .ToArray
        '    End Get
        '    Set(ParamArray value As V())
        '        Dim [property] As New BindProperty(Of DataFrameColumnAttribute)(properties(name))

        '        If value.IsNullorEmpty Then  
        '            ' value array is nothing or have no data, 
        '            ' then means set all property value to nothing 
        '            For Each x In data
        '                Call [property].__setValue(x, Nothing)
        '            Next
        '        ElseIf value.Length = 1 Then 
        '            ' value array only have one element, 
        '            ' then means set all property value to a specific value
        '            Dim v As Object = value(Scan0)
        '            For Each x In data
        '                Call [property].__setValue(x, v)
        '            Next
        '        Else
        '            If value.Length <> data.Length Then
        '                Throw New InvalidExpressionException(DimNotAgree$)
        '            End If

        '            For i As Integer = 0 To data.Length - 1
        '                Call [property].__setValue(data(i), value(i))
        '            Next
        '        End If
        '    End Set
        'End Property

        Sub New(src As IEnumerable(Of T))
            data = src.ToArray
            properties = inspectType(type)
        End Sub

        Shared ReadOnly typeCache As New Dictionary(Of Type, Dictionary(Of String, PropertyInfo))

        Private Shared Function inspectType(type As Type) As Dictionary(Of String, PropertyInfo)
            If Not typeCache.ContainsKey(type) Then
                typeCache(type) = type.Schema(PropertyAccess.NotSure, PublicProperty, True)
            End If

            Return typeCache(type)
        End Function

        Public Overrides Function ToString() As String
            Return type.FullName
        End Function

        Private Shared Sub TestDEMO()
            Dim vector As NamedValue(Of String)() = {}
            Dim previousData = Linq.DATA(vector).Evaluate("Value")

            Linq.DATA(vector).Evaluate("Value") = {}    ' set all value property to nothing
            Linq.DATA(vector).Evaluate("Value") = {"1"} ' set all value property to a specifc value "1"
            Linq.DATA(vector).Evaluate("Value") = {"1", "2", "3"}
        End Sub
    End Class
End Namespace
